<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CompanyParameter.aspx.cs" Inherits="MasterSetUp_CompanyParameter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/company_paremeter.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Company Parameter%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Company Parameter%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Time" Style="display: none;" runat="server" OnClick="btnTime_Click" Text="list" />
            <asp:Button ID="Btn_Hr" Style="display: none;" runat="server" OnClick="BtnHr_Click" Text="list" />
            <asp:Button ID="Btn_Help_New" Style="display: none;" runat="server" OnClick="btnHelp_Click" Text="list" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="li_help" runat="server"><a onclick="Li_Tab_Help()" href="#Help_New" data-toggle="tab">
                        <img src="../Images/help.png" style="width: 25px;" alt="" /><asp:Label ID="Label163" runat="server" Text="<%$ Resources:Attendance,Help %>"></asp:Label></a></li>
                    <li id="Li_HR" runat="server"><a onclick="Li_Tab_HR()" href="#ctl00_MainContent_HR_New" data-toggle="tab">
                        <img src="../Images/hr1.png" style="width: 25px;" alt="" />
                        <asp:Label ID="Label164" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label>
                    </a></li>
                    <li id="Li_TimeMan" runat="server"><a onclick="Li_Tab_TimeMan()" href="#ctl00_MainContent_TimeMan_New" data-toggle="tab">
                        <img src="../Images/device.png" style="width: 25px;" alt="" /><asp:Label ID="Label165" runat="server" Text="<%$ Resources:Attendance,TimeMan %>"></asp:Label></a></li>
                    <li id="Li_SMS_Email" class="active"><a onclick="Li_Tab_SMS_Email()" href="#Tab_SMS_Email" data-toggle="tab">
                        <asp:Label ID="Label171" runat="server" Text="<%$ Resources:Attendance,SMS/Email %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="Tab_SMS_Email">
                        <asp:UpdatePanel ID="Update_Tab_SMS_Email" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label71" runat="server" Text="<%$ Resources:Attendance,Email Setup %>"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label72" runat="server" Text="<%$ Resources:Attendance,Email Functionality %>"></asp:Label>
                                                    <asp:RadioButton ID="rbtnEmailEnable" GroupName="Email" Text="<%$ Resources:Attendance,Enable %>"
                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbtEmail_OnCheckedChanged" />
                                                    &nbsp;&nbsp;
                                                    <asp:RadioButton ID="rbtnEmailDisable" GroupName="Email" Text="<%$ Resources:Attendance,Disable %>"
                                                     runat="server" AutoPostBack="true" OnCheckedChanged="rbtEmail_OnCheckedChanged" />
                                                    <br />
                                                    <asp:Label ID="Label73" runat="server" Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmail" ErrorMessage="<%$ Resources:Attendance,Enter Email%>" />

                                                    <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label74" runat="server" Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                                                    <asp:TextBox ID="txtPasswordEmail" TextMode="Password" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label75" runat="server" Text="<%$ Resources:Attendance,SMTP %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSMTP" ErrorMessage="<%$ Resources:Attendance,Enter SMTP%>" />

                                                    <asp:TextBox ID="txtSMTP" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label61" runat="server" Text="<%$ Resources:Attendance,Port %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPort" ErrorMessage="<%$ Resources:Attendance,Enter Port%>" />

                                                    <asp:TextBox ID="txtPort" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label62" runat="server" Text="<%$ Resources:Attendance,EnableSSL %>"></asp:Label>
                                                    <asp:CheckBox ID="chkEnableSSL" runat="server" class="form-control"></asp:CheckBox>
                                                    <br />
                                                    <asp:Label ID="Label63" runat="server" Text="<%$ Resources:Attendance,POP3  %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator12" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPop3" ErrorMessage="<%$ Resources:Attendance,Enter POP3%>" />

                                                    <asp:TextBox ID="txtPop3" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label68" runat="server" Text="<%$ Resources:Attendance,Port %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator13" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtpopport" ErrorMessage="<%$ Resources:Attendance,Enter Port%>" />

                                                    <asp:TextBox ID="txtpopport" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label69" runat="server" Text="<%$ Resources:Attendance,Password Reminder( In Days) %>"></asp:Label>
                                                    <asp:TextBox ID="txtPasswordReminder" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="lblsendemail" runat="server" Text="<%$ Resources:Attendance,Send email on leave transaction ?%>"></asp:Label>
                                                    <asp:CheckBox ID="chkSendEmail" runat="server" class="form-control"></asp:CheckBox>
                                                    <br />
                                                </div>
                                            </div>

                                        </div>


                                        <div class="col-md-6">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label50" runat="server" Text="<%$ Resources:Attendance,SMS Setup %>"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:Attendance,SMS Functionality %>"></asp:Label>
                                                    <asp:RadioButton ID="rbtnSMSEnable" GroupName="SMS" Text="<%$ Resources:Attendance,Enable %>"
                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbtSMS_OnCheckedChanged" />
                                                    &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbtnSMSDisable" GroupName="SMS" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbtSMS_OnCheckedChanged" />
                                                    <br />
                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:Attendance,SMS_API %>"></asp:Label>
                                                    <asp:TextBox ID="txtSMSAPI" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label58" runat="server" Text="<%$ Resources:Attendance,Sender Id %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSenderId" ErrorMessage="<%$ Resources:Attendance,Enter Sender Id %>" />

                                                    <asp:TextBox ID="txtSenderId" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label59" runat="server" Text="<%$ Resources:Attendance,SMS User Id %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Sms_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtUserId" ErrorMessage="<%$ Resources:Attendance,Enter SMS User Id %>" />

                                                    <asp:TextBox ID="txtUserId" runat="server" class="form-control"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label60" runat="server" Text="<%$ Resources:Attendance,SMS_User_Password %>"></asp:Label>
                                                    <asp:TextBox ID="txtSmsPassword" TextMode="Password" runat="server" class="form-control"></asp:TextBox>
                                                    <br />

                                                    <asp:Label ID="lblprmUploadSize" Text="File Upload Size" runat="server"></asp:Label>
                                                    <asp:TextBox ID="txtprmUploadSize" type="number" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <br />
                                                </div>
                                            </div>

                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label195" runat="server" Text="POS & Pryce Integration"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>

                                                <div class="box-body">
                                                    <asp:CheckBox ID="chkIntegratedwithPOS" runat="server"
                                                        OnCheckedChanged="chkIntegratedwithPOS_CheckedChanged" AutoPostBack="true"
                                                        Text="POS Integrated with Pryce" />

                                                    <asp:Button ID="btnSynchPOSData" OnClick="btnSynchPOSData_Click" Visible="false" runat="server" Text="Synch All POS Data" />
                                                </div>

                                                <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                    <asp:Button ID="Button5" ValidationGroup="Sms_Save" runat="server" class="btn btn-success" OnClick="btnSaveSMSEmail_Click"
                                                        Text="<%$ Resources:Attendance,Save %>" />
                                                    &nbsp; &nbsp;
                                                    <asp:Button ID="Button6" runat="server" CausesValidation="False" class="btn btn-primary"
                                                        OnClick="btnCancelSMSEmail_Click" Text="<%$ Resources:Attendance,Close %>" />
                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="TimeMan_New" runat="server">
                        <%--<asp:UpdatePanel ID="Update_TimeMan_New" runat="server">
                            <ContentTemplate>--%>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="nav-tabs-custom">
                                    <ul class="nav nav-tabs pull-right bg-blue-gradient">
                                        <li id="Li_Report"><a onclick="Li_Tab_Report()" href="#Tab_Report" data-toggle="tab">
                                            <asp:Label ID="Label166" runat="server" Text="<%$ Resources:Attendance,Report %>"></asp:Label></a></li>
                                        <li id="Li_Penalty"><a onclick="Li_Tab_Penalty()" href="#Tab_Penalty" data-toggle="tab">
                                            <asp:Label ID="Label167" runat="server" Text="<%$ Resources:Attendance,Penalty%>"></asp:Label></a></li>
                                        <li id="Li_Color_Code"><a onclick="Li_Tab_Color_Code()" href="#Tab_Color_Code" data-toggle="tab">
                                            <asp:Label ID="Label168" runat="server" Text="<%$ Resources:Attendance,Color Code %>"></asp:Label></a></li>
                                        <li id="Li_Keys"><a onclick="Li_Tab_Keys()" href="#Tab_Keys" data-toggle="tab">
                                            <asp:Label ID="Label169" runat="server" Text="<%$ Resources:Attendance,Keys %>"></asp:Label></a></li>
                                        <li id="Li_OT_PL"><a onclick="Li_Tab_OT_PL()" href="#Tab_OT_PL" data-toggle="tab">
                                            <asp:Label ID="Label170" runat="server" Text="<%$ Resources:Attendance,OT/PL %>"></asp:Label></a></li>
                                        <li id="Li_Work_Calculation"><a onclick="Li_Tab_Work_Calculation()" href="#Tab_Work_Calculation" data-toggle="tab">
                                            <asp:Label ID="Label172" runat="server" Text="<%$ Resources:Attendance,Work Calculation %>"></asp:Label></a></li>
                                        <li class="active" id="Li_Time_Table"><a onclick="Li_Tab_Time_Table()" href="#Tab_Time_Table" data-toggle="tab">
                                            <asp:Label ID="Label173" runat="server" Text="<%$ Resources:Attendance,Time Table %>"></asp:Label></a></li>

                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane active" id="Tab_Time_Table">
                                            <asp:UpdatePanel ID="Update_Tab_Time_Table" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lblDeviceParameter" runat="server" Text="<%$ Resources:Attendance,Device Parameter%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Lbl_Employee_Device" runat="server" Text="<%$ Resources:Attendance,Employee Sychronization Device %>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:DropDownList ID="ddlEmpSync" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Location Level" Value="Location"></asp:ListItem>
                                                                            <asp:ListItem Text="Company Level" Value="Company"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="lblDownloadServicePath" runat="server" Text="<%$ Resources:Attendance,Log Service Interval(In Minutes) %>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:TextBox ID="txtDownloadServicePath" runat="server" class="form-control"></asp:TextBox>
                                                                        <br />
                                                                        <asp:Label ID="Label184" runat="server" Text="<%$ Resources:Attendance,Email for device disconnected alert%>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:TextBox ID="txtEmailAlertDevice" runat="server" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Leave Parameter%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Financial Year (Month)%>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" class="form-control">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,January%>" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,February%>" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,March%>" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,April%>" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,May%>" Value="5"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,June%>" Value="6"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,July%>" Value="7"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,August%>" Value="8"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,September%>" Value="9"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,October%>" Value="10"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,November%>" Value="11"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,December%>" Value="12"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Yearly Half Day Allow %>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Time_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtHalfDayCount" ErrorMessage="<%$ Resources:Attendance,Enter Yearly Half Day Allow %>" />


                                                                        <asp:TextBox ID="txtHalfDayCount" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilterHalfday" TargetControlID="txtHalfDayCount"
                                                                            runat="server" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Leave Calculation On Week Off%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkLeaveCountForWeekOff" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Leave Calculation On Holiday%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkLeaveCountForHoliday" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Leave Salary Calculation%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkIsLeaveSalary" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label177" runat="server" Text="<%$ Resources:Attendance,Is Manual Log allowed on leave?%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkIsManualLog" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label175" runat="server" Text="<%$ Resources:Attendance,Logs priority on leave?%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkLogpriorityonLeave" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label185" runat="server" Text="<%$ Resources:Attendance,Leave Salary Approval%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkLeaveApproval" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label186" runat="server" Visible="false" Text="<%$ Resources:Attendance,Leave transaction on shift uploading%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkLeaveValidation" Visible="false" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$Resources:Attendance,Leave and Indemnity Salary Calculation%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,WeekOff Leave Salary Calculation%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkWeekoffLeaveSal" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Holiday Leave Salary Calculation%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkHolidayLeaveSal" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Absent Leave Salary Calculation%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkAbsentLeaveSal" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label182" runat="server" Text="<%$ Resources:Attendance,Paid Leave For Leave Salary Calculation%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkPaidLeaveSal" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="lblUnpaidLeave" runat="server" Text="<%$ Resources:Attendance,Unpaid Leave For Leave Salary Calculation%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkUnPaidLeaveSal" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />



                                                                        <asp:Label ID="Label179" runat="server" Text="<%$ Resources:Attendance,Applicable Allowances For Leave Salary%>"></asp:Label>


                                                                        <asp:TextBox ID="txtLsApplicableAllowances" BackColor="#eeeeee" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtLsApplicableAllowances_OnTextChanged"></asp:TextBox>

                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=","
                                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="Get_Applicable_Allowance"
                                                                            ServicePath="" TargetControlID="txtLsApplicableAllowances" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>



                                                                        <br />
                                                                        <asp:Label ID="Label180" runat="server" Text="<%$ Resources:Attendance,Month Day Count For Leave Salary%>"></asp:Label>&nbsp;&nbsp;&nbsp;

                                                                        <asp:DropDownList ID="ddlLSDaysCount" runat="server">
                                                                            <asp:ListItem Text="Fixed Days" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Month Days" Value="Month Days"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtLsFixedDays" runat="server" placeholder="Enter Fixed Days" class="form-control"></asp:TextBox>


                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender72" runat="server" Enabled="True"
                                                                            TargetControlID="txtLsFixedDays" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label181" runat="server" Text="<%$ Resources:Attendance,Pay Scale For Leave Salary%>"></asp:Label>
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnCurrentSalary" runat="server" Text="<%$ Resources:Attendance,Current Salary%>" GroupName="Ps" Checked="true" />
                                                                        <asp:RadioButton ID="rbtnactualSalary" runat="server" GroupName="Ps" Text="<%$ Resources:Attendance,Actual Salary during Leave Salary Calculation%>" />
                                                                    </div>
                                                                </div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Shift Parameter%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,With Shift (Without Function Key)%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkShiftFunctionKey" runat="server" class="form-control"></asp:CheckBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Service Parameter%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Service Run Time %>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:TextBox ID="txtServiceRunTime" BackColor="#eeeeee" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                            Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtServiceRunTime">
                                                                        </cc1:MaskedEditExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Download New User%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkNewUserDownload" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance,Upload New User%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkNewUserUpload" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Time Table Parameter%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Exclude Day in Shift%>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:DropDownList ID="ddlExculeDay" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Absent" Value="Absent"></asp:ListItem>
                                                                            <asp:ListItem Text="IsOff" Value="IsOff"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Default Shift For Employee %>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:DropDownList ID="ddlDefaultShift" runat="server" class="form-control">
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Allow Shortest Time In Time Table(Minutes) %>"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Time_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShortestTime" ErrorMessage="<%$ Resources:Attendance,Enter Allow Shortest Time In Time Table(Minutes)%>" />

                                                                        <asp:TextBox ID="txtShortestTime" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                            TargetControlID="txtShortestTime" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label189" runat="server" Text="<%$ Resources:Attendance,Absent sandwich on week Off%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkweekoffsandwich" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label190" runat="server" Text="<%$ Resources:Attendance,Absent sandwich on holiday%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkholidaysandwich" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Default Parameter%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance,Week Off Days%>"></asp:Label>
                                                                        <asp:CheckBoxList ID="ChkWeekOffList" runat="server">
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Sunday%>" Value="Sunday"></asp:ListItem>
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Monday%>" Value="Monday"></asp:ListItem>
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Tuesday%>" Value="Tuesday"></asp:ListItem>
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Wednesday%>" Value="Wednesday"></asp:ListItem>
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Thursday%>" Value="Thursday"></asp:ListItem>
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Friday%>" Value="Friday"></asp:ListItem>
                                                                            <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Saturday%>" Value="Saturday"></asp:ListItem>
                                                                        </asp:CheckBoxList>
                                                                        <br />
                                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Work Day Minutes %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Time_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkDayMin" ErrorMessage="<%$ Resources:Attendance,Enter Work Day Minutes %>" />

                                                                        <asp:TextBox ID="txtWorkDayMin" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                            TargetControlID="txtWorkDayMin" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />


                                                                        <asp:Label ID="Label138" runat="server" Text="<%$ Resources:Attendance,Auto Generate Employee Code %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkAutoGenerateEmployeeCode" runat="server" class="form-control"></asp:CheckBox>

                                                                        <br />


                                                                        <asp:Label ID="Label178" runat="server" Text="<%$ Resources:Attendance,Send Full Month Repots on Month End (Report Notification)%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkNotification" runat="server" class="form-control"></asp:CheckBox>

                                                                        <br />

                                                                        <asp:Label ID="Label193" runat="server" Text="<%$ Resources:Attendance,Holiday Validity%>"></asp:Label>


                                                                        <asp:DropDownList ID="ddlHolidayValidity" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHolidayValidity_SelectedIndexChanged">
                                                                            <asp:ListItem Text="Financial Year" Value="0" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Text="Day" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Month" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="Year" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>

                                                                        <asp:TextBox ID="txtHolidayValue" runat="server" class="form-control" placeholder="Enter Value" Visible="false"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtHolidayValue" runat="server" Enabled="True"
                                                                            TargetControlID="txtHolidayValue" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>





                                                                        <br />


                                                                    </div>
                                                                    <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                                        <asp:Button ID="btnSaveTime" ValidationGroup="Time_Save" runat="server" class="btn btn-success" OnClick="btnSaveTime_Click"
                                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                                        &nbsp; &nbsp;
                                                    <asp:Button ID="btnCancelTime" runat="server" CausesValidation="False" class="btn btn-danger"
                                                        OnClick="btnCancelTime_Click" Text="<%$ Resources:Attendance,Close %>" />
                                                                        <asp:HiddenField ID="hdnCompanyId" runat="server" />
                                                                    </div>
                                                                </div>


                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane" id="Tab_Work_Calculation">
                                            <asp:UpdatePanel ID="Update_Tab_Work_Calculation" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Work Calculation%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Work Calculation Method %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlWorkCal" runat="server" class="form-control">
                                                                            <asp:ListItem Text="PairWise" Value="PairWise"></asp:ListItem>
                                                                            <asp:ListItem Text="InOut" Value="InOut"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label39" runat="server" Text="Salary Calculation According To"></asp:Label>
                                                                        <asp:DropDownList ID="ddlSalCal" AutoPostBack="true" OnSelectedIndexChanged="ddlSalCal_OnSelectedIndexChanged" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                                            <asp:ListItem Text="Fixed Days" Value="Fixed Days"></asp:ListItem>
                                                                        </asp:DropDownList>


                                                                        <div id="lblDay" runat="server" class="input-group">
                                                                            <asp:TextBox ID="txtFixedDays" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">
                                                                                <asp:Label ID="Lbl_Days_" runat="server" Text="<%$ Resources:Attendance,Days %>"></asp:Label></span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" Enabled="True"
                                                                                TargetControlID="txtSalIncrDuration" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>

                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label64" runat="server" Text="<%$ Resources:Attendance,Pay Salary According To %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlPaySal" AutoPostBack="true" OnSelectedIndexChanged="ddlPaySal_OnSelectedIndexChanged" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Work Hour" Value="Work Hour"></asp:ListItem>
                                                                            <asp:ListItem Text="Ref Hour" Value="Ref Hour"></asp:ListItem>
                                                                            <asp:ListItem Text="Work Calculation" Value="Work Calculation"></asp:ListItem>
                                                                        </asp:DropDownList>


                                                                        <div style="text-align: center;" runat="server" id="pnlWorkPercent">
                                                                            <br />
                                                                            <asp:Label ID="lblWorkpercent1" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                                            <asp:TextBox ID="txtWorkPercentFrom1" runat="server" CssClass="textComman" Enabled="False"
                                                                                Text="0" Width="10%"></asp:TextBox>

                                                                            <asp:Label ID="Label149" runat="server" CssClass="labelComman" Text="To"></asp:Label>
                                                                            <asp:TextBox ID="txtWorkPercentTo1" runat="server" AutoPostBack="True" CssClass="textComman"
                                                                                OnTextChanged="txtWorkPercentTo1_TextChanged" Width="10%"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                TargetControlID="txtWorkPercentTo1" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>

                                                                            <asp:Label ID="Label150" runat="server" CssClass="labelComman" Text="="></asp:Label>
                                                                            <asp:TextBox ID="txtValue1" runat="server" CssClass="textComman" Width="10%"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbtxtValue1" runat="server" Enabled="True" TargetControlID="txtValue1"
                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label151" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                                            <asp:TextBox ID="txtWorkPercentFrom2" runat="server" CssClass="textComman" Enabled="False"
                                                                                Width="10%"></asp:TextBox>

                                                                            <asp:Label ID="Label152" runat="server" CssClass="labelComman" Text=" To"></asp:Label>
                                                                            <asp:TextBox ID="txtWorkPercentTo2" runat="server" AutoPostBack="True" CssClass="textComman"
                                                                                OnTextChanged="txtWorkPercentTo2_TextChanged" Width="10%"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True"
                                                                                TargetControlID="txtWorkPercentTo2" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>

                                                                            <asp:Label ID="Label153" runat="server" CssClass="labelComman" Text="="></asp:Label>
                                                                            <asp:TextBox ID="txtValue2" runat="server" CssClass="textComman" Width="10%"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbtxtValue2" runat="server" Enabled="True" TargetControlID="txtValue2"
                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                            <asp:Label ID="lblWorkper1" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                                            <asp:TextBox ID="txtWorkPercentFrom3" runat="server" CssClass="textComman" Enabled="False"
                                                                                Width="10%"></asp:TextBox>

                                                                            <asp:Label ID="Label154" runat="server" CssClass="labelComman" Text="To"></asp:Label>
                                                                            <asp:TextBox ID="txtWorkPercentTo3" runat="server" CssClass="textComman" Enabled="False"
                                                                                Text="100" Width="10%"></asp:TextBox>
                                                                            <asp:Label ID="Label155" runat="server" CssClass="labelComman" Text="="></asp:Label>
                                                                            <asp:TextBox ID="txtValue3" runat="server" CssClass="textComman" Width="10%"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftbTxtValue3" runat="server" Enabled="True" TargetControlID="txtValue3"
                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>

                                                                        </div>

                                                                        <br />



                                                                        <asp:Label ID="Label65" runat="server" Text="<%$ Resources:Attendance,For Work Hour %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkForWorkHour" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label66" runat="server" Text="<%$ Resources:Attendance,Late Without Present %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkLateWithoutPresent" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label67" runat="server" Text="<%$ Resources:Attendance,Week Off In Shift %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkWeekOffInShift" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label183" runat="server" Text="<%$ Resources:Attendance,Shift Approval Functionality %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkShiftApproval" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />

                                                                    </div>
                                                                </div>

                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Salary Increment For Fresher%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Attendance,Salary Increment Functionality  %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbSalIncEnable" GroupName="SalInc" Text="<%$ Resources:Attendance,Enable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbSalInc_OnCheckedChanged" />
                                                                        <asp:RadioButton ID="rbSalIncDisable" GroupName="SalInc" Text="<%$ Resources:Attendance,Disable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbSalInc_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Duration of salary increment %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtSalIncrDuration" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">
                                                                                <asp:Label ID="Label54" runat="server" Text="<%$ Resources:Attendance,In Months %>"></asp:Label></span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True"
                                                                                TargetControlID="txtSalIncrDuration" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtFreshPerFrom" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True"
                                                                                TargetControlID="txtFreshPerFrom" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Attendance,To %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtFreshPerTo" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" Enabled="True"
                                                                                TargetControlID="txtFreshPerTo" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                    </div>
                                                                </div>



                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label57" runat="server" Text="<%$ Resources:Attendance,Salary Increment For Experience%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Attendance,Duration of salary increment %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtSalIncrDurationForExperience" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">
                                                                                <asp:Label ID="Label139" runat="server" Text="<%$ Resources:Attendance,In Months %>"></asp:Label></span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" Enabled="True"
                                                                                TargetControlID="txtSalIncrDurationForExperience" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtExpPerFrom" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                                                                TargetControlID="txtFreshPerFrom" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:Attendance,To %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtExpPerTo" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                                                                TargetControlID="txtExpPerTo" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="col-md-6">
                                                                <div id="dvPFEsic" runat="server"></div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label53" runat="server" Text="<%$ Resources:Attendance,PF%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">


                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,PF Applicable Salary%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator24" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpPF" ErrorMessage="<%$ Resources:Attendance,Enter Employee PF %>" />

                                                                        <asp:TextBox ID="txtPfApplicablesalary" runat="server" class="form-control"></asp:TextBox>

                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                            TargetControlID="txtPfApplicablesalary" ValidChars="1234567890.">
                                                                        </cc1:FilteredTextBoxExtender>

                                                                        <br />


                                                                        <asp:Label ID="Label55" runat="server" Text="<%$ Resources:Attendance,Employee PF %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpPF" ErrorMessage="<%$ Resources:Attendance,Enter Employee PF %>" />

                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEmpPF" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                                                                TargetControlID="txtEmpPF" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />

                                                                    </div>
                                                                </div>



                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Employer%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">



                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,PF %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployerPf" ErrorMessage="<%$ Resources:Attendance,Enter Employer PF %>" />
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEmployerPf" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtEmployerPf" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label176" runat="server" Text="<%$ Resources:Attendance,FPF%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator29" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployerFPF" ErrorMessage="Enter Employee FPF" />
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEmployerFPF" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" Enabled="True"
                                                                                TargetControlID="txtEmployerFPF" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Admin Charges%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator25" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpPF" ErrorMessage="<%$ Resources:Attendance,Enter Employee PF %>" />

                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtAdminCharges" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" Enabled="True"
                                                                                TargetControlID="txtAdminCharges" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,EDLI%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator26" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpPF" ErrorMessage="<%$ Resources:Attendance,Enter Employee PF %>" />

                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtPfEdLi" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" Enabled="True"
                                                                                TargetControlID="txtPfEdLi" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>

                                                                        <br />

                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Inspection Charges%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator27" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpPF" ErrorMessage="<%$ Resources:Attendance,Enter Employee PF %>" />

                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtpfInspectionCharges" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" Enabled="True"
                                                                                TargetControlID="txtpfInspectionCharges" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                    </div>

                                                                </div>

                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,ESIC%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">

                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Applicable Salary%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator28" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpPF" ErrorMessage="<%$ Resources:Attendance,Enter Employee PF %>" />

                                                                        <asp:TextBox ID="txtESicApplicablesalary" runat="server" class="form-control"></asp:TextBox>

                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" Enabled="True"
                                                                            TargetControlID="txtESicApplicablesalary" ValidChars="1234567890.">
                                                                        </cc1:FilteredTextBoxExtender>

                                                                        <br />

                                                                        <asp:Label ID="Label56" runat="server" Text="<%$ Resources:Attendance,Employee %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Work_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpESIC" ErrorMessage="<%$ Resources:Attendance,Enter Employee ESIC %>" />

                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEmpESIC" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True"
                                                                                TargetControlID="txtEmpESIC" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label46" runat="server" Text="<%$ Resources:Attendance,Employer%>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEmployerESIC" runat="server" class="form-control"></asp:TextBox>
                                                                            <span class="input-group-addon">%</span>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True"
                                                                                TargetControlID="txtEmployerESIC" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                        <br />
                                                                    </div>


                                                                    <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                                        <asp:Button ID="btnSaveWork" ValidationGroup="Work_Save" runat="server" class="btn btn-success" OnClick="btnSaveWork_Click"
                                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                                        &nbsp; &nbsp;
                                                    <asp:Button ID="Button4" runat="server" CausesValidation="False" class="btn btn-danger"
                                                        OnClick="btnCancelWork_Click" Text="<%$ Resources:Attendance,Close %>" />
                                                                    </div>
                                                                </div>
                                                            </div>



                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane" id="Tab_OT_PL">
                                            <asp:UpdatePanel ID="Update_Tab_OT_PL" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div id="dvPL" runat="server" class="col-md-12">
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label70" runat="server" Text="<%$ Resources:Attendance,Partial Leave %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label76" runat="server" Text="<%$ Resources:Attendance,Partial Leave Functionality %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbtnPartialEnable" GroupName="Partial" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartial_OnCheckedChanged" />
                                                                        &nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rbtnPartialDisable" GroupName="Partial" Text="<%$ Resources:Attendance,Disable %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartial_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:Label ID="Label77" runat="server" Text="<%$ Resources:Attendance,Total Minutes For Month %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="OTPL_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalMinutes" ErrorMessage="<%$ Resources:Attendance,Enter Total Minutes For Month %>" />

                                                                        <asp:TextBox ID="txtTotalMinutes" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txtTotalMinutes"
                                                                            FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label78" runat="server" Text="<%$ Resources:Attendance,Minute Use in a Day One Time %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator15" ValidationGroup="OTPL_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMinuteday" ErrorMessage="<%$ Resources:Attendance,Enter Minute Use in a Day One Time%>" />

                                                                        <asp:TextBox ID="txtMinuteday" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtMinuteday"
                                                                            FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>

                                                                        <asp:Label ID="Label79" runat="server" Text="<%$ Resources:Attendance,PL WithTimeWithOutTime %>" Visible="false"></asp:Label>
                                                                        <asp:CheckBox ID="chkPLWithTimeWithOutTime" runat="server" class="form-control" Visible="false"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label80" runat="server" Text="<%$ Resources:Attendance,PL Date Editable %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkPLDateEditable" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label81" runat="server" Text="<%$ Resources:Attendance,Over Time %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label82" runat="server" Text="<%$ Resources:Attendance,Over Time Functionality %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbtnOtEnable" GroupName="OT" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" CssClass="labelComman" AutoPostBack="true" OnCheckedChanged="rbtOT_OnCheckedChanged" />
                                                                        &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbtnOtDisable" GroupName="OT" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" CssClass="labelComman" AutoPostBack="true" OnCheckedChanged="rbtOT_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:Label ID="Label83" runat="server" Text="<%$ Resources:Attendance,Max Over Time Minutes in a Day %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator16" ValidationGroup="OTPL_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMaxOTMint" ErrorMessage="<%$ Resources:Attendance,Enter Max Over Time Minutes in a Day %>" />
                                                                        <asp:TextBox ID="txtMaxOTMint" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                            TargetControlID="txtMaxOTMint" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label84" runat="server" Text="<%$ Resources:Attendance,Min Over Time Minutes in a Day %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator17" ValidationGroup="OTPL_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMinOTMint" ErrorMessage="<%$ Resources:Attendance,Enter Min Over Time Minutes in a Day %>" />
                                                                        <asp:TextBox ID="txtMinOTMint" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                            TargetControlID="txtMinOTMint" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <asp:Label ID="Label85" runat="server" Text="<%$ Resources:Attendance,Calculation Method%>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlCalculationMethod" runat="server" class="form-control">
                                                                            <asp:ListItem Text="In" Value="In"></asp:ListItem>
                                                                            <asp:ListItem Text="Out" Value="Out"></asp:ListItem>
                                                                            <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                                                            <asp:ListItem Text="Work Hour" Value="Work Hour"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label86" runat="server" Text="<%$ Resources:Attendance,Week Off Over Time Functionality %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbnWeekOffOTEnable" Enabled="false" GroupName="OS" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" />
                                                                        &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbnWeekOffOTDisable" Enabled="false" GroupName="OS" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" />
                                                                        <br />
                                                                        <asp:Label ID="Label87" runat="server" Text="<%$ Resources:Attendance,Holiday Over Time Functionality %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbnHoliayOTEnable" Enabled="false" GroupName="OL" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" />
                                                                        &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbnHoliayOTDisable" Enabled="false" GroupName="OL" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" />
                                                                        <br />
                                                                        <asp:Label ID="Label88" runat="server" Text="<%$ Resources:Attendance, Over Time Approval Functionality %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbnApprovalOTEnable" GroupName="OTApp" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" />
                                                                        &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbnApprovalOTDisable" GroupName="OTApp" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" />
                                                                        <br />
                                                                    </div>

                                                                    <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                                        <asp:Button ID="Button8" ValidationGroup="OTPL_Save" runat="server" class="btn btn-success" OnClick="btnSavePartial_Click"
                                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                                        &nbsp; &nbsp;
                                                    <asp:Button ID="Button9" runat="server" CausesValidation="False" class="btn btn-danger"
                                                        OnClick="btnCancelPartial_Click" Text="<%$ Resources:Attendance,Close %>" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane" id="Tab_Keys">
                                            <asp:UpdatePanel ID="Update_Tab_Keys" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div id="Div2" runat="server" class="col-md-12">
                                                            <div class="col-md-12">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Keys %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="col-md-12" style="color: ">
                                                                            <asp:Label ID="Lbl_s" runat="server" Text="<%$ Resources:Attendance,Key Preference Functionality %>"></asp:Label>
                                                                            <asp:RadioButton ID="rbtnKeyEnable" GroupName="KeyPref" Text="<%$ Resources:Attendance,Enable %>"
                                                                                runat="server" AutoPostBack="true" OnCheckedChanged="rbtKeyPref_OnCheckedChanged" />
                                                                            &nbsp;&nbsp;
                                                    <asp:RadioButton ID="rbtnKeyDisable" GroupName="KeyPref" Text="<%$ Resources:Attendance,Disable %>"
                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbtKeyPref_OnCheckedChanged" />
                                                                            <br />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label89" runat="server" Text="<%$ Resources:Attendance,IN Key %>"></asp:Label>
                                                                            <asp:TextBox ID="txtInKey" class="form-control" runat="server"></asp:TextBox>
                                                                            <br />
                                                                            <asp:Label ID="Label90" runat="server" Text="<%$ Resources:Attendance,Out Key %>"></asp:Label>
                                                                            <asp:TextBox ID="txtOutKey" class="form-control" runat="server"></asp:TextBox>
                                                                            <br />
                                                                            <asp:Label ID="Label91" runat="server" Text="<%$ Resources:Attendance,Break In Key %>"></asp:Label>
                                                                            <asp:TextBox ID="txtBreakInKey" class="form-control" runat="server"></asp:TextBox>
                                                                            <br />
                                                                            <asp:Label ID="Label92" runat="server" Text="<%$ Resources:Attendance,Break Out Key %>"></asp:Label>
                                                                            <asp:TextBox ID="txtBreakOutKey" class="form-control" runat="server"></asp:TextBox>
                                                                            <br />

                                                                            <div id="Td1" visible="false" runat="server">
                                                                                <asp:Label ID="Label162" runat="server" Text="<%$ Resources:Attendance,Consider Next Day Log %>"></asp:Label>
                                                                            </div>
                                                                            <div id="Td2" runat="server" visible="false">
                                                                                <asp:CheckBox ID="ChkNextDayLog" runat="server" CssClass="form-control" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label93" runat="server" Text="<%$ Resources:Attendance,Partial Leave IN Key %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPartialInKey" class="form-control" runat="server"></asp:TextBox>
                                                                            <br />
                                                                            <asp:Label ID="Label94" runat="server" Text="<%$ Resources:Attendance,Partial Leave Out Key %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPartialOutKey" class="form-control" runat="server"></asp:TextBox>
                                                                            <br />
                                                                            <asp:Label ID="Label95" runat="server" Text="<%$ Resources:Attendance,Shift Range%>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlShiftRange" class="form-control" runat="server">
                                                                                <asp:ListItem Text="One Days" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Two Days" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                            <div style="text-align: center;">
                                                                                <asp:Button ID="Button10" runat="server" class="btn btn-success" OnClick="btnSaveKeyPreference_Click"
                                                                                    Text="<%$ Resources:Attendance,Save %>" />
                                                                                &nbsp; &nbsp;
                                                    <asp:Button ID="Button11" runat="server" CausesValidation="False" class="btn btn-danger"
                                                        OnClick="btnCancelKeyPreference_Click" Text="<%$ Resources:Attendance,Close %>" />
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
                                        <div class="tab-pane" id="Tab_Color_Code">
                                            <asp:UpdatePanel ID="Update_Tab_Color_Code" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div id="Div3" runat="server" class="col-md-12">
                                                            <div class="col-md-12">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Color Code %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Lbl_Col" runat="server" Text="<%$ Resources:Attendance,Present Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPresentColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender8" runat="server" Enabled="True"
                                                                                TargetControlID="txtPresentColorCode" SampleControlID="txtPresentColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label96" runat="server" Text="<%$ Resources:Attendance,Late Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtLateColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtLateColorCode" SampleControlID="txtLateColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label97" runat="server" Text="<%$ Resources:Attendance,Leave Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtLeaveColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender4" runat="server" Enabled="True"
                                                                                TargetControlID="txtLeaveColorCode" SampleControlID="txtLeaveColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label98" runat="server" Text="<%$ Resources:Attendance,Week Off Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtWeekOffColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender6" runat="server" Enabled="True"
                                                                                TargetControlID="txtWeekOffColorCode" SampleControlID="txtWeekOffColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label192" runat="server" Text="<%$ Resources:Attendance,Missed Check In%>"></asp:Label>
                                                                            <asp:TextBox ID="txtMIcolorcode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender10" runat="server" Enabled="True"
                                                                                TargetControlID="txtMIcolorcode" SampleControlID="txtMIcolorcode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />


                                                                            <asp:Label ID="Label194" runat="server" Text="Not Registered"></asp:Label>
                                                                            <asp:TextBox ID="txtNRColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender11" runat="server" Enabled="True"
                                                                                TargetControlID="txtNRColorCode" SampleControlID="txtNRColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <a href="http://html-color-codes.info/" target="_blank" cssclass="acc">Get Customized Color Form Here</a>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label99" runat="server" Text="<%$ Resources:Attendance,Absent Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtAbsentColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtAbsentColorCode" SampleControlID="txtAbsentColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label100" runat="server" Text="<%$ Resources:Attendance,Early Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEarlyColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender3" runat="server" Enabled="True"
                                                                                TargetControlID="txtEarlyColorCode" SampleControlID="txtEarlyColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label101" runat="server" Text="<%$ Resources:Attendance,Holiday Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtHolidayColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender5" runat="server" Enabled="True"
                                                                                TargetControlID="txtHolidayColorCode" SampleControlID="txtHolidayColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label102" runat="server" Text="<%$ Resources:Attendance,Temporary Shift Color Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtTempShiftColorCode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender7" runat="server" Enabled="True"
                                                                                TargetControlID="txtTempShiftColorCode" SampleControlID="txtTempShiftColorCode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />


                                                                            <asp:Label ID="Label191" runat="server" Text="<%$ Resources:Attendance,Missed Check Out%>"></asp:Label>
                                                                            <asp:TextBox ID="txtMOcolorcode" class="form-control" runat="server"></asp:TextBox>
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender9" runat="server" Enabled="True"
                                                                                TargetControlID="txtMOcolorcode" SampleControlID="txtMOcolorcode">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />





                                                                            <div style="text-align: center;">
                                                                                <asp:Button ID="btnSaveColorCode" runat="server" class="btn btn-success" OnClick="btnSaveColorCode_Click"
                                                                                    Text="<%$ Resources:Attendance,Save %>" />
                                                                                &nbsp; &nbsp;
                                                    <asp:Button ID="btnCancelColorCode" runat="server" CausesValidation="False" class="btn btn-danger"
                                                        OnClick="btnCancelColorCode_Click" Text="<%$ Resources:Attendance,Close %>" />
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
                                        <div class="tab-pane" id="Tab_Penalty">
                                            <asp:UpdatePanel ID="Update_Tab_Penalty" runat="server">
                                                <ContentTemplate>

                                                    <div class="row">

                                                        <div class="col-md-12">
                                                            <div class="box box-danger">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label187" runat="server" Text="<%$ Resources:Attendance,Penalty Calculation%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:Label ID="Label188" runat="server" Text="<%$ Resources:Attendance,Penalty Calculation based on%>"></asp:Label>

                                                                    <asp:CheckBox ID="chkWorkMinutePenalty" runat="server" Text="<%$ Resources:Attendance,Work Minute%>" Enabled="false" Checked="true" />
                                                                    <asp:CheckBox ID="chkbreakMinutePenalty" runat="server" Text="<%$ Resources:Attendance,Break Minute%>" />
                                                                    <asp:CheckBox ID="chkRelaxationMinutePenalty" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute%>" />

                                                                    <br />


                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>


                                                    <div class="row">
                                                        <div id="Div4" runat="server" class="col-md-12">
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label103" runat="server" Text="<%$ Resources:Attendance,Late In %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label104" runat="server" Text="<%$ Resources:Attendance,Late In Functionality for a Month %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbtLateInEnable" GroupName="Late" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtLateIn_OnCheckedChanged" />
                                                                        &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbtLateInDisable" GroupName="Late" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbtLateIn_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnLateSalary" GroupName="LateType" Text="<%$ Resources:Attendance,Salary Deduction by Fixed or % Value %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtLateType_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnLateMinutes" GroupName="LateType" Text="<%$ Resources:Attendance,Fix Minute's Salary Deduction by Time%>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtLateType_OnCheckedChanged" />
                                                                        <br />
                                                                        <div id="pnlLateSal" runat="server">
                                                                            <asp:Label ID="Label107" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute Deduction For Day %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlDeductionMinuteForDay" runat="server" class="form-control">
                                                                                <asp:ListItem Text="Fixed For Day" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Varient For Day" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                            <asp:Label ID="Label108" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute for a Month %>"></asp:Label>
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator18" ValidationGroup="Penalty_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLateRelaxMin" ErrorMessage="<%$ Resources:Attendance,Enter Relaxation Minute for a Month %>" />

                                                                            <asp:TextBox ID="txtLateRelaxMin" runat="server" class="form-control"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                                TargetControlID="txtLateRelaxMin" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label109" runat="server" Text="<%$ Resources:Attendance,Late Count for a Month %>"></asp:Label>
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator19" ValidationGroup="Penalty_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLateCount" ErrorMessage="<%$ Resources:Attendance,Enter Late Count for a Month%>" />
                                                                            <asp:TextBox ID="txtLateCount" runat="server" class="form-control"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                                                TargetControlID="txtLateCount" ValidChars="123456790.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label110" runat="server" Text="<%$ Resources:Attendance,Late In Salary Deduction Value %>"></asp:Label>
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator20" ValidationGroup="Penalty_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLateValue" ErrorMessage="<%$ Resources:Attendance,Enter Late In Salary Deduction Value%>" />
                                                                            <div style="width: 100%;" class="input-group">
                                                                                <asp:TextBox ID="txtLateValue" Style="width: 50%;" runat="server" class="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                                    TargetControlID="txtLateValue" ValidChars="123456790.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:DropDownList ID="ddlLateType" runat="server" Style="width: 50%;" class="form-control">
                                                                                    <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <br />
                                                                            <asp:Label ID="Label111" runat="server" Text="<%$ Resources:Attendance,Late In Count As Half Day %>"></asp:Label>
                                                                            <asp:CheckBox ID="chkLateInCountAsHalfDay" runat="server" class="form-control"></asp:CheckBox>
                                                                            <br />
                                                                        </div>
                                                                        <div id="pnlLateMin" runat="server">
                                                                            <asp:Label ID="Label105" runat="server" Text="<%$ Resources:Attendance,Fixed Minutes Deduction by Times %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlLateMinTime" runat="server" class="form-control">
                                                                                <asp:ListItem Text="1 Times" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="2 Times" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="3 Times" Value="3"></asp:ListItem>
                                                                                <asp:ListItem Text="4 Times" Value="4"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                            <asp:Label ID="Label120" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute for a Month %>"></asp:Label>
                                                                            <asp:TextBox ID="txtLateRelaxMinWithMTimes" runat="server" class="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                                TargetControlID="txtLateRelaxMinWithMTimes" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label112" runat="server" Text="<%$ Resources:Attendance,Absent%>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label106" runat="server" Text="<%$ Resources:Attendance,Deduction Value %>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator22" ValidationGroup="Penalty_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAbsentDeduc" ErrorMessage="<%$ Resources:Attendance,Enter Deduction Value %>" />

                                                                        <div style="width: 100%;" class="input-group">
                                                                            <asp:TextBox ID="txtAbsentDeduc" runat="server" Style="width: 50%;" CssClass="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                                TargetControlID="txtAbsentDeduc" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:DropDownList ID="ddlAbsentType" Width="70px" Style="width: 50%;" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="Fixed Per Absent" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label113" runat="server" Text="<%$ Resources:Attendance,No Clock In Count As Absent %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkNoClockIn" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label114" runat="server" Text="<%$ Resources:Attendance,No Clock Out Count As Absent %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkNoClockOut" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label115" runat="server" Text="<%$ Resources:Attendance,No Clock In Count As Late %>"></asp:Label>
                                                                        <div style="width: 100%;" class="input-group">
                                                                            <asp:CheckBox ID="ChkNoClockLate" Style="width: 50%;" class="form-control" runat="server"></asp:CheckBox>
                                                                            <asp:TextBox ID="txtNoClockLate" Style="width: 50%;" class="form-control" runat="server" MaxLength="3"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label129" runat="server" Text="<%$ Resources:Attendance,No Clock Out Count As Early %>"></asp:Label>
                                                                        <div style="width: 100%;" class="input-group">
                                                                            <asp:CheckBox ID="ChkNoClockEarly" runat="server" Style="width: 50%;" class="form-control" />
                                                                            <asp:TextBox ID="txtNoClockEarly" Style="width: 50%;" class="form-control" runat="server" MaxLength="3"></asp:TextBox>
                                                                        </div>
                                                                        <br />
                                                                        <asp:Label ID="Label130" runat="server" Text="<%$ Resources:Attendance,After EndingIn Count As Absent %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkAfterEInAbsent" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div id="Div5" runat="server" class="col-md-12">
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label118" runat="server" Text="<%$ Resources:Attendance,Early Out %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label119" runat="server" Text="<%$ Resources:Attendance,Early Out Functionality for a Month %>"></asp:Label>
                                                                        <asp:RadioButton ID="rbtEarlyOutEnable" GroupName="Early" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtEarlyOut_OnCheckedChanged" />
                                                                        &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbtEarlyOutDisable" GroupName="Early" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbtEarlyOut_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnEarlySalary" GroupName="EarlyType" Text="<%$ Resources:Attendance,Salary Deduction by Fixed or % Value  %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtEarlyType_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnEarlyMinutes" GroupName="EarlyType" Text="<%$ Resources:Attendance,Fix Minute's Salary Deduction by Time%>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtEarlyType_OnCheckedChanged" />
                                                                        <br />
                                                                        <div runat="server" id="pnlEarlySal">
                                                                            <asp:Label ID="Label140" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute Deduction For Day %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlOutDeductionMinuteForDay" runat="server" class="form-control">
                                                                                <asp:ListItem Text="Fixed For Day" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Varient For Day" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />

                                                                            <asp:Label ID="Label141" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute for a Month %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEarlyRelaxMin" runat="server" class="form-control"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                                                                TargetControlID="txtEarlyRelaxMin" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                            <asp:Label ID="Label146" runat="server" Text="<%$ Resources:Attendance,Early Count for a Month %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEarlyCount" runat="server" class="form-control"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                                TargetControlID="txtEarlyCount" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>

                                                                            <asp:Label ID="Label147" runat="server" Text="<%$ Resources:Attendance,Early Out Salary Deduction Value %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEarlyValue" Width="70px" runat="server" class="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                                                TargetControlID="txtEarlyValue" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:DropDownList ID="ddlEarlyType" Width="70px" runat="server" CssClass="DropdownSearch">
                                                                                <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div id="pnlEarlyMin" runat="server">
                                                                            <asp:Label ID="Label121" runat="server" Text="<%$ Resources:Attendance,Fixed Minutes Deduction by Times %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlEarlyMinTime" runat="server" class="form-control">
                                                                                <asp:ListItem Text="1 Times" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="2 Times" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="3 Times" Value="3"></asp:ListItem>
                                                                                <asp:ListItem Text="4 Times" Value="4"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                            <asp:Label ID="Label116" runat="server" Text="<%$ Resources:Attendance,Relaxation Minute for a Month %>"></asp:Label>
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator21" ValidationGroup="Penalty_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEarlyRelaxMinWithMinTimes" ErrorMessage="<%$ Resources:Attendance,Enter Relaxation Minute for a Month%>" />

                                                                            <asp:TextBox ID="txtEarlyRelaxMinWithMinTimes" Width="70px" runat="server" class="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                                                                TargetControlID="txtEarlyRelaxMinWithMinTimes" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label122" runat="server" Text="<%$ Resources:Attendance,Partial Leave Violation %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:RadioButton ID="rbtnPartialSalary" GroupName="PartialType" Text="<%$ Resources:Attendance,Salary Deduction by Fixed or % Value  %>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartialType_OnCheckedChanged" />
                                                                        <br />
                                                                        <asp:RadioButton ID="rbtnPartialMinutes" GroupName="PartialType" Text="<%$ Resources:Attendance,Fix Minute's Salary Deduction by Time%>"
                                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartialType_OnCheckedChanged" />
                                                                        <br />
                                                                        <div id="pnlPartialSal" runat="server">
                                                                            <asp:Label ID="Label148" runat="server" Text="<%$ Resources:Attendance,Value for Deduction %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPartialValue" Style="width: 50%;" runat="server" CssClass="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                TargetControlID="txtPartialValue" ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:DropDownList ID="ddlPartialType" Style="width: 50%;" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                        </div>
                                                                        <div id="pnlPartialMin" runat="server">
                                                                            <asp:Label ID="Label123" runat="server" Text="<%$ Resources:Attendance,Fixed Minutes Deduction by Times %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlPartialMinTime" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="1 Times" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="2 Times" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="3 Times" Value="3"></asp:ListItem>
                                                                                <asp:ListItem Text="4 Times" Value="4"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                        </div>
                                                                        <asp:Label ID="Label124" runat="server" Text="<%$ Resources:Attendance,Violation Minutes for a Month%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator23" ValidationGroup="Penalty_Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtViolation" ErrorMessage="<%$ Resources:Attendance,Enter Violation Minutes for a Month%>" />

                                                                        <asp:TextBox ID="txtViolation" runat="server" class="form-control"></asp:TextBox>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                                        <asp:Button ID="Button16" ValidationGroup="Penalty_Save" runat="server" CssClass="btn btn-success" OnClick="btnSavePanelty_Click"
                                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                                        &nbsp; &nbsp;
                                                    <asp:Button ID="Button17" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                        OnClick="btnCancelPanelty_Click" Text="<%$ Resources:Attendance,Close %>" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>



                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane" id="Tab_Report">
                                            <asp:UpdatePanel ID="Update_Tab_Report" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label117" runat="server" Text="<%$ Resources:Attendance,Salary Report %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label125" runat="server" Text="<%$ Resources:Attendance,Salary Report %>"></asp:Label>
                                                                        <asp:CheckBox ID="ChkWeekOff" runat="server" Text="Week Off" class="form-control"></asp:CheckBox>&nbsp;&nbsp;
                                                                <asp:CheckBox ID="ChkHoliday" runat="server" Text="Holiday" class="form-control"></asp:CheckBox>&nbsp;&nbsp;
                                                                <asp:CheckBox ID="ChkLeave" runat="server" Text="Leave" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label127" runat="server" Text="<%$ Resources:Attendance,Parameter Level %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label128" runat="server" Text="<%$ Resources:Attendance,Update Parameter Level %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlParameterLevel" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Company" Value="Company"></asp:ListItem>
                                                                            <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="box box-danger">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label135" runat="server" Text="<%$ Resources:Attendance,Page Parameter %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <asp:Label ID="Label136" runat="server" Text="<%$ Resources:Attendance,For Log Report %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlForLogReport" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Date Time" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="12 Hour Time" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="24 Hour Time" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label137" runat="server" Text="<%$ Resources:Attendance,Page Level %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlPageLevel" runat="server" class="form-control">
                                                                            <asp:ListItem Text="Company" Value="Company"></asp:ListItem>
                                                                            <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:Label ID="Label126" runat="server" Text="<%$ Resources:Attendance,Manual Attendance Verification%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkManualAttendanceVerified" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                        <asp:Label ID="Label131" runat="server" Text="<%$ Resources:Attendance,Holiday Assign On Week Off %>"></asp:Label>
                                                                        <asp:CheckBox ID="chkHolidayAssignOnWeekOff" runat="server" class="form-control"></asp:CheckBox>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                                        <asp:Button ID="btnReports" runat="server" CssClass="btn btn-success" OnClick="btnReports_Click"
                                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                                        &nbsp; &nbsp;
                                                    <asp:Button ID="btnRCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                        OnClick="btnRCancel_Click" Text="<%$ Resources:Attendance,Close %>" />
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
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                    <div class="tab-pane" id="HR_New" runat="server">
                        <asp:UpdatePanel ID="Update_HR_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div style="display: none;" runat="server" visible="false" class="col-md-6">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label142" runat="server" Text="<%$ Resources:Attendance,Indemnity %>"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label143" runat="server" Text="<%$ Resources:Attendance,Indemnity Functionality %>"></asp:Label>
                                                    <asp:RadioButton ID="rbnIndemnity1" GroupName="Indemnity" Text="<%$ Resources:Attendance,Enable %>"
                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbnIndemnity1_OnCheckedChanged" />
                                                    &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbnIndemnity2" GroupName="Indemnity" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbnIndemnity2_OnCheckedChanged" />
                                                    <br />
                                                    <asp:Label ID="Label144" runat="server" Text="<%$ Resources:Attendance,Indemnity Duration(Year)%>"></asp:Label>
                                                    <asp:TextBox ID="txtIndemnityYear" runat="server" class="form-control"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="txtIndemnityYear"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                    <asp:Label ID="Label145" runat="server" Text="<%$ Resources:Attendance,Given Leave Salary Days(Year)%>"></asp:Label>
                                                    <asp:TextBox ID="txtIndemnityDays" runat="server" CssClass="form-control" Width="120px"
                                                        MaxLength="4"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" Enabled="True"
                                                        TargetControlID="txtIndemnityDays" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>


                                            </div>
                                        </div>





                                        <div runat="server" class="col-md-6">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label196" runat="server" Text="<%$ Resources:Attendance,Indemnity %>"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label197" runat="server" Text="Allow All Employees on Gratuity"></asp:Label>
                                                    <asp:CheckBox ID="chkAllowAllEmployeesonGratuity" runat="server" class="form-control"></asp:CheckBox>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" class="col-md-6">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label198" runat="server" Text="Termination"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label199" runat="server" Text="Log Post Necessary On Termination"></asp:Label>
                                                    <asp:CheckBox ID="chkLogPostOnTermination" runat="server" class="form-control"></asp:CheckBox>
                                                    <br />
                                                    <asp:Label ID="Label200" runat="server" Text="Payroll Post Necessary On Termination"></asp:Label>
                                                    <asp:CheckBox ID="chkPayrollPostOnTermination" runat="server" class="form-control"></asp:CheckBox>
                                                    <br />
                                                    <asp:Label ID="Label201" runat="server" Text="Finance Configuration on Termination"></asp:Label>
                                                    <asp:CheckBox ID="chkFinanceAccountConfiguration" runat="server" class="form-control"></asp:CheckBox>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>












                                        <div class="col-md-12">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label132" runat="server" Text="<%$ Resources:Attendance,Probation Period %>"></asp:Label></h3>
                                                    <div class="box-tools pull-right">
                                                        <%--<button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>--%>
                                                    </div>

                                                </div>
                                                <div class="box-body">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label133" runat="server" Text="<%$ Resources:Attendance,Probation Period Functionality %>"></asp:Label>
                                                        <asp:RadioButton ID="rbnProbation1" Style="padding-left: 10px; padding-right: 10px;" GroupName="a" Text="<%$ Resources:Attendance,Enable %>"
                                                            runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="rbnProbation1_OnCheckedChanged" />

                                                        <asp:RadioButton ID="rbnProbation2" Style="padding-left: 10px; padding-right: 10px;" GroupName="a" Text="<%$ Resources:Attendance,Disable %>"
                                                            runat="server" AutoPostBack="true" OnCheckedChanged="rbnProbation2_OnCheckedChanged" />
                                                        <br />
                                                        <asp:Label ID="Label134" runat="server" Text="<%$ Resources:Attendance,Probation Period(Months) %>"></asp:Label>
                                                        <asp:TextBox ID="txtProbationPeriod" runat="server" class="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txtProbationPeriod"
                                                            FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3"></div>
                                                </div>
                                                <div class="col-md-12 form-control" style="height: 160px;">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label13" runat="server" Text="TDS Functionality"></asp:Label>
                                                        <asp:RadioButton ID="rbtnTDSAuto" Style="padding-left: 10px; padding-right: 10px;" Checked="true" GroupName="Probation" Text="Auto" runat="server" />
                                                        <asp:RadioButton ID="rbtnTDSManual" Style="padding-left: 10px; padding-right: 10px;" GroupName="Probation" Text="Manual" runat="server" />
                                                        <br />

                                                        <asp:CheckBox ID="chkSalaryPlanEnable" runat="server" Text="Is Salary Plan Enable" />

                                                        <br />

                                                        <asp:CheckBox ID="chkLogPostMendatory" runat="server" Text="Log Post Mendatory for Generate Payroll" />

                                                        <br />

                                                        <asp:Label ID="Label174" runat="server" Text="Senior Citizen (Age Limit)"></asp:Label>
                                                        <asp:TextBox ID="txtSeniorCitizenagelimit" runat="server" class="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" TargetControlID="txtSeniorCitizenagelimit"
                                                            FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3"></div>
                                                </div>
                                                <div class="col-md-6 form-control" style="text-align: center; height: 50px;">
                                                    <asp:Button ID="btnProbation" runat="server" CssClass="btn btn-success" OnClick="btnProbation_Click"
                                                        Text="<%$ Resources:Attendance,Save %>" />
                                                    &nbsp; &nbsp;
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                        OnClick="btnCancelPartial_Click" Text="<%$ Resources:Attendance,Close %>" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Help_New" runat="server">
                        <asp:UpdatePanel ID="Update_Help_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <div style="text-align: center;" class="col-md-12">
                                                        <asp:Button ID="Btn_Help" CssClass="btn btn-primary" Text="Help" OnClick="btnHelp_Click" runat="server" />

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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Tab_Time_Table">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Tab_Work_Calculation">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Tab_SMS_Email">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Tab_OT_PL">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Tab_Keys">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Tab_Color_Code">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Tab_Penalty">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Tab_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_HR_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Help_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="PnlTime" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlPartialLeave1" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlWork" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlKeyPreference" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlSMSEmail" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlColor" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlPaneltyCalc" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="panelHR" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlReportView" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="dvWorkCal" runat="server" Visible="false"></asp:Panel>
    <%--Div Not use--%>
    <div style="display: none;" id="Not_use">
        <table>
            <tr bgcolor="#90BDE9">
                <td align="left" colspan="2">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlTimetableList" Visible="false" runat="server" CssClass="a">
                                    <asp:Button ID="btnTime" runat="server" Text="<%$ Resources:Attendance,Time Table %>"
                                        Width="90px" BorderStyle="none" BackColor="Transparent" OnClick="btnTime_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlWorkCal" Visible="false" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="btnWork" runat="server" Text="<%$ Resources:Attendance,Work Calculation %>"
                                        Width="125px" BorderStyle="none" BackColor="Transparent" OnClick="btnWorkCalc_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlSMSEmailB" Visible="false" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="btnSMSEmail" runat="server" Text="<%$ Resources:Attendance,SMS/Email %>"
                                        Width="90px" BorderStyle="none" BackColor="Transparent" OnClick="btnSMSEmail_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlPartialLeave" Visible="false" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="Button7" runat="server" Text="<%$ Resources:Attendance,OT/PL %>"
                                        Width="70px" BorderStyle="none" BackColor="Transparent" OnClick="btnPartialLeave_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlKeyPref" Visible="false" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="btnKeyPref" runat="server" Text="<%$ Resources:Attendance,Keys %>"
                                        Width="60px" BorderStyle="none" BackColor="Transparent" OnClick="btnKeyPref_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlColorCode" Visible="false" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="Button12" runat="server" Text="<%$ Resources:Attendance,Color Code  %>"
                                        Width="100px" BorderStyle="none" BackColor="Transparent" OnClick="btnColorCode_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlPanelty" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="Button15" runat="server" Text="<%$ Resources:Attendance,Penalty  %>"
                                        Width="70px" BorderStyle="none" BackColor="Transparent" OnClick="btnPanelty_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlReport" Visible="true" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="btnReport" runat="server" Text="<%$ Resources:Attendance,Report  %>"
                                        Width="70px" BorderStyle="none" BackColor="Transparent" OnClick="btnReport_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="PnlAttendence" runat="server" CssClass="a">
                                    <asp:Button ID="btnAttendence" runat="server" Text="<%$ Resources:Attendance,TimeMan %>"
                                        Width="120px" BorderStyle="none" BackColor="Transparent" OnClick="btnAttendence_Click"
                                        Style="padding-left: 45px; background-image: url(  '../Images/device.PNG' ); padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="PnlHR" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="BtnHr" runat="server" Text="<%$ Resources:Attendance,HR %>" Width="90px"
                                        BorderStyle="none" BackColor="Transparent" OnClick="BtnHr_Click" Style="padding-left: 30px; padding-top: 3px; background-repeat: no-repeat; background-image: url(  '../Images/hr1.PNG' ); height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="PnlHelp" runat="server" CssClass="a" Width="100%">
                                    <asp:Button ID="btnHelp" runat="server" Text="<%$ Resources:Attendance,Help %>" Width="90px"
                                        BorderStyle="none" BackColor="Transparent" OnClick="btnHelp_Click" Style="padding-left: 40px; padding-top: 3px; background-image: url(  '../Images/help.PNG' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="PnlInventory" runat="server" CssClass="a" Width="100%" Visible="false">
                                    <asp:Button ID="btnInventory" runat="server" Text="<%$ Resources:Attendance,Inventory %>"
                                        Width="90px" BorderStyle="none" BackColor="Transparent" OnClick="btnInventory_Click"
                                        Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlList" runat="server" Visible="false">
            <asp:Panel ID="panNewEdit" runat="server" DefaultButton="btnCSave">
                <table width="100%" style="padding-left: 43px; padding-top: 10px; padding-bottom: 20px">
                    <tr>
                        <td width="120px">
                            <asp:Label ID="lblParameterName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Parameter Name %>" />
                        </td>
                        <td>:
                        </td>
                        <td width="270px">
                            <asp:TextBox ID="txtParameterName" ReadOnly="true" runat="server" Font-Names="Verdana"
                                CssClass="textComman"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblParameterValue" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Parameter Value %>" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtParameterValue" runat="server" CssClass="textComman" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <table>
                                <tr>
                                    <td width="90px">
                                        <asp:Button ID="btnCSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                            CssClass="buttonCommman" OnClick="btnCSave_Click" Visible="false" />
                                    </td>
                                    <td width="90px">
                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                            CssClass="buttonCommman" CausesValidation="False" OnClick="BtnReset_Click" />
                                        <asp:HiddenField ID="editid" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlSearchRecords" runat="server" DefaultButton="btnbind">
                <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                    <table width="100%" style="padding-left: 20px; height: 38px">
                        <tr>
                            <td width="90px">
                                <asp:Label ID="LblSelectField" runat="server" Text="<%$ Resources:Attendance,Select Field %>"
                                    CssClass="labelComman"></asp:Label>
                            </td>
                            <td width="180px">
                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="DropdownSearch" Height="25px"
                                    Width="170px">
                                    <asp:ListItem Text="<%$ Resources:Attendance,Parameter Name %>" Value="ParameterName" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Parameter Id %>" Value="Parameter_Id"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="135px">
                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="DropdownSearch" Height="25px"
                                    Width="120px">
                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="270px">
                                <asp:TextBox ID="txtValue" runat="server" CssClass="textCommanSearch" Height="14px"
                                    Width="250px"></asp:TextBox>
                            </td>
                            <td width="50px" align="center">
                                <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Height="25px"
                                    ImageUrl="~/Images/search.png" OnClick="btnbindrpt_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                            </td>
                            <td>
                                <asp:Panel ID="PnlRefresh" runat="server" DefaultButton="btnRefresh">
                                    <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Height="25px"
                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefreshReport_Click" Width="25px"
                                        ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                </asp:Panel>
                            </td>
                            <td colspan="9">
                                <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                    CssClass="labelComman"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvParameter" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvParameter_PageIndexChanging"
                AllowSorting="True" OnSorting="GvParameter_Sorting">

                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Parameter_Id") %>'
                                ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" Visible="false" CausesValidation="False"
                                ToolTip="<%$ Resources:Attendance,Edit %>" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemStyle />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parameter Id %>" SortExpression="Parameter_Id">
                        <ItemTemplate>
                            <asp:Label ID="lblParameterId" runat="server" Text='<%# Eval("Parameter_Id") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parameter Name %>" SortExpression="ParameterName">
                        <ItemTemplate>
                            <asp:Label ID="lblParameterName" runat="server" Text='<%# Eval("ParameterName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parameter Value %>" SortExpression="ParameterValue">
                        <ItemTemplate>
                            <asp:Label ID="lblParameterValue" runat="server" Text='<%# Eval("ParameterValue") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle />
                    </asp:TemplateField>
                </Columns>



            </asp:GridView>
        </asp:Panel>
        <table>
            <tr id="trAllBrand" visible="false" runat="server">
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label160" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Display Time Table In All Brand %>"></asp:Label>
                            </td>
                            <td width="1px">:
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtnBrandYes" GroupName="Brand" Text="<%$ Resources:Attendance,Yes %>"
                                    runat="server" CssClass="labelComman" />
                                &nbsp;&nbsp;
                                                                <asp:RadioButton ID="rbtnBrandNo" GroupName="Brand" Text="<%$ Resources:Attendance,No %>"
                                                                    runat="server" CssClass="labelComman" /><a style="color: Red">*</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr1" runat="server" visible="false">
                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                    <asp:Label ID="Label161" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Carry Forward Minutes %>"></asp:Label>
                </td>
                <td width="1px">:
                </td>
                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                    <asp:RadioButton ID="rbtnCarryYes" GroupName="SMS" Text="<%$ Resources:Attendance,Yes %>"
                        runat="server" CssClass="labelComman" />
                    &nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rbtnCarryNo" GroupName="SMS" Text="<%$ Resources:Attendance,No %>"
                                                                            runat="server" CssClass="labelComman" />
                </td>
            </tr>
            <tr>
                <div id="Div1" runat="server" visible="false">
                    <fieldset style="display: none;" visible="false">
                        <legend>
                            <asp:Label ID="Label156" Font-Names="Times New roman" Font-Size="18px" Font-Bold="true"
                                runat="server" Text="<%$ Resources:Attendance,Database Parameter%>" CssClass="labelComman"></asp:Label>
                        </legend>
                        <table height="20px">
                            <tr>
                                <td width="200px">
                                    <asp:Label ID="Label157" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Default Database Backup Location %>" />
                                </td>
                                <td width="1px">:
                                </td>
                                <td width="195px">
                                    <asp:CheckBox ID="ChkDbLocation" runat="server" AutoPostBack="true" OnCheckedChanged="ChkDbLocation_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td width="200px">
                                    <asp:Label ID="Label158" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Database Backup Location %>" />
                                </td>
                                <td width="1px">:
                                </td>
                                <td width="195px">
                                    <asp:TextBox ID="txtBackupLoc" Width="150px" runat="server" CssClass="textComman" /><a
                                        style="color: Red">*</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label159" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Example For Location %>"></asp:Label>
                                </td>
                                <td width="1px">:
                                </td>
                                <td height="128px" style="padding-top: 5px">
                                    <asp:Label ID="lblBakLoc" runat="server" Text="D:\Database Backup" CssClass="textComman"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </tr>

        </table>
        <asp:Panel ID="PnlOT" runat="server">
            <table width="100%" style="padding-left: 43px">
                <tr>
                    <td colspan="2"></td>
                    <td style="padding-left: 12PX;">
                        <asp:Button ID="Button1" runat="server" CssClass="buttonCommman" OnClick="btnSaveOverTime_Click"
                            Text="<%$ Resources:Attendance,Save %>" Width="75px" />
                        &nbsp; &nbsp;
                                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="buttonCommman"
                                                        OnClick="btnCancelOverTime_Click" Text="<%$ Resources:Attendance,Close %>" Width="75px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="dvSalInc" runat="server" Visible="false"></asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
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





        function Li_Tab_TimeMan() {
            document.getElementById('<%= Btn_Time.ClientID %>').click();
        }
        function Li_Tab_HR() {
            document.getElementById('<%= Btn_Hr.ClientID %>').click();
        }
        function Li_Tab_Help() {
            document.getElementById('<%= Btn_Help_New.ClientID %>').click();
        }

    </script>
</asp:Content>
