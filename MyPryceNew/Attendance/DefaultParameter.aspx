<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultParameter.aspx.cs" EnableEventValidation="true" Inherits="Attendance_DefaultParameter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>TimeMan Bio Attendance</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" type="text/javascript"></script>
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
    <link href="../BootStrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../BootStrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" type="text/css" />
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <script src="../Script/common.js"></script>
    <link href="../CSS/controlsCss.css" rel="stylesheet" />
</head>
<body>
    <div class="main">
        <div id="back">
            <nav class="navbar navbar-inverse" style="border-radius: 0;">
                <div class="container-fluid">
                    <div class="navbar-header" id="title">Default Setup</div>
                </div>
            </nav>
            <div class="container-fluid">

                <form runat="server">
                    <asp:ScriptManager ID="Home_Script" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="Home_Udpate" runat="server">
                        <ContentTemplate>
                            <%--  <h3>Default Configuration</h3>--%>

                            <asp:MultiView ID="MVDeafult" runat="server">
                                <asp:View ID="btnView1" runat="server">
                                    <div class="row">
                                        <div class="form-group col-md-4">
                                            <label for="inputEmail4">Company Name</label>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Submit" Font-Size="12px"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompanyName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>


                                        <div class="form-group col-md-4">
                                            <label for="inputPassword4">Brand Name</label>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_txtBrandName" ValidationGroup="Submit" Font-Size="12px"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtBrandName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtBrandName" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>


                                        <div class="form-group col-md-4">
                                            <label for="inputAddress">Location Name</label>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_txtLocationname" ValidationGroup="Submit" Font-Size="12px"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLocationname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtLocationname" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>

                                    </div>
                                    <!--- First-Row----->
                                    <div class="row">
                                        <div class="form-group col-md-6">
                                            <label for="inputState">Country</label>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_ddlCountry" ValidationGroup="Submit" Font-Size="12px"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlCountry" InitialValue="--Select--" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </div>


                                        <div class="form-group col-md-6">
                                            <label for="inputState">Currency</label>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_ddlCurrency" ValidationGroup="Submit" Font-Size="12px"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlCurrency" runat="server" Enabled="false" CssClass="form-control">
                                            </asp:DropDownList>

                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="form-group col-md-6">
                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Default Shift For Employee %>"></asp:Label>


                                            <asp:DropDownList ID="ddlDefaultShift" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Work Calculation Method %>"></asp:Label>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_ddlWorkCal" ValidationGroup="Submit" Font-Size="12px"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlWorkCal" InitialValue="--Select--" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlWorkCal" runat="server" class="form-control">
                                                <asp:ListItem Text="InOut" Value="InOut"></asp:ListItem>
                                                <asp:ListItem Text="PairWise" Value="PairWise"></asp:ListItem>

                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <!--- Second-Row----->
                                    <div class="container-fluid" style="overflow-x: auto;">
                                        <table class="table table-striped">

                                            <tr>
                                                <label>Week-Off</label>
                                                <td bgcolor="#e1f3f4">
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkSunday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Sunday</span>
                                                    </label>
                                                </td>


                                                <td>
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkMonday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Monday</span>
                                                    </label>
                                                </td>

                                                <td bgcolor="#e1f3f4">
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkTuesday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Tuesday</span>
                                                    </label>
                                                </td>

                                                <td>
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkWednesday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Wednesday</span>
                                                    </label>
                                                </td>

                                                <td bgcolor="#e1f3f4">
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkThursday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Thursday</span>
                                                    </label>
                                                </td>

                                                <td>
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkFriday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Friday</span>
                                                    </label>
                                                </td>

                                                <td bgcolor="#e1f3f4">
                                                    <label class="form-checkbox-label">
                                                        <input name="metal" class="form-checkbox-field" type="checkbox" runat="server" id="chkSaturday" />
                                                        <i class="form-checkbox-button"></i>
                                                        <span>Saturday</span>
                                                    </label>
                                                </td>
                                            </tr>


                                        </table>

                                    </div>
                                    <!----close container-fluid ---->


                                    <div class="row" style="text-align: center;">
                                        <br />
                                        <br />
                                        <asp:Button ID="btnback" runat="server" Text="<< Back to login" CssClass="btn btn-primary" OnClick="btnback_Click" />
                                        <asp:Button ID="btnNext1" runat="server" ValidationGroup="Submit" Text="Next >>" CssClass="btn btn-primary" OnClick="btnNext1_Click" />
                                    </div>
                                </asp:View>

                                <asp:View ID="btnViewOTPL" runat="server">

                                    <div class="row">
                                        <div class="form-group col-md-6">
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
                                        <div class="form-group col-md-6">
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
                                            <asp:Label ID="Label86" runat="server" Text="Week Off OT"></asp:Label>&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rbnWeekOffOTEnable" Enabled="false" GroupName="OS" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" />
                                            &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbnWeekOffOTDisable" Enabled="false" GroupName="OS" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" />
                                            <br />
                                            <asp:Label ID="Label87" runat="server" Text="Holiday OT"></asp:Label>&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;
                                                                        <asp:RadioButton ID="rbnHoliayOTEnable" Enabled="false" GroupName="OL" Text="<%$ Resources:Attendance,Enable %>"
                                                                            runat="server" />
                                            &nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbnHoliayOTDisable" Enabled="false" GroupName="OL" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" />
                                            <br />

                                            <br />
                                        </div>


                                        <div class="row" style="text-align: center;">
                                            <br />
                                            <br />
                                            <asp:Button ID="btnBackOTPL" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnBackOTPL_Click" />
                                            <asp:Button ID="btnNextOTPL" runat="server" Text="Next >>" CssClass="btn btn-primary" OnClick="btnNextOTPL_Click" />
                                        </div>

                                    </div>
                                </asp:View>


                                <asp:View ID="btnViewFuncKey" runat="server">
                                    <div class="row">
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


                                        </div>
                                        <div class="col-md-12" style="text-align: center;">
                                            <br />
                                            <asp:Button ID="btnbackFuncKy" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnbackFuncKy_Click" />
                                            <asp:Button ID="btnNextFunckey" runat="server" Text="Next >>" CssClass="btn btn-primary" OnClick="btnNextFunckey_Click" />

                                        </div>

                                    </div>

                                </asp:View>


                                <!-- Now all View Created by Rahul Sharma On Date 16-05-2024 -->
                                <asp:View ID="btnViewGratuiaty" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="nav-tabs-custom">
                                                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                                                    <li id="Li_New" class="active" runat="server"><a onclick="Li_New_Click()" href="#New" data-toggle="tab">
                                                        <asp:UpdatePanel ID="Update_Li" runat="server">
                                                            <ContentTemplate>
                                                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </a></li>
                                                </ul>
                                                <div class="tab-content">

                                                    <div class="tab-pane active " id="New">
                                                        <asp:UpdatePanel ID="Update_New" runat="server">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="GvGartuityDetail" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box box-primary">
                                                                            <div class="box-body">
                                                                                <div class="form-group">


                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label5" runat="server" Text="Plan Name"></asp:Label>


                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator3" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtPlanName" ErrorMessage="Enter Plan Name"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtPlanName" runat="server"
                                                                                            CssClass="form-control" />

                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label14" runat="server" Text="Eligible Month"></asp:Label>


                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator1" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtEligibleMonth" ErrorMessage="Enter Eligible Month"></asp:RequiredFieldValidator>


                                                                                        <asp:TextBox ID="txtEligibleMonth" runat="server" CssClass="form-control" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtEligibleMonth" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label15" runat="server" Text="Benefit Amount Limit"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator4" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtBenefitAmountLimit" ErrorMessage="Enter Benefit Amount Limit"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtBenefitAmountLimit" runat="server" AutoPostBack="true" OnTextChanged="txtBenefitAmountLimit_OnTextChanged"
                                                                                            CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtBenefitAmountLimit" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label16" runat="server" Text="Benefit Wage Month"></asp:Label>

                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator5" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtbenefitwagemonth" ErrorMessage="Enter Benefit Wage Month"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtbenefitwagemonth" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtBenefitAmountLimit_OnTextChanged" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                            TargetControlID="txtbenefitwagemonth" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>



                                                                                    <div class="col-md-6">
                                                                                        <br />
                                                                                        <asp:Label ID="Label17" runat="server" Text="Service Calculation Period"></asp:Label>

                                                                                        <asp:RadioButton ID="rbtnserviceCalc_Nearestinteger" runat="server" Text="Rounded of nearest Year" GroupName="srccalc" />
                                                                                        <asp:RadioButton ID="rbtnserviceCalc_proratebasis" runat="server" Text="Pro rate basis" GroupName="srccalc" />


                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label18" runat="server" Text="Applicable Allowances"></asp:Label>


                                                                                        <a style="color: Red">*</a>



                                                                                        <asp:TextBox ID="txtApplicableAllowance" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtLsApplicableAllowances_OnTextChanged" />


                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=","
                                                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="Get_Applicable_Allowance"
                                                                                            ServicePath="" TargetControlID="txtApplicableAllowance" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>

                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">

                                                                                        <br />
                                                                                        <asp:CheckBox ID="chkIsTaxFree" runat="server" Text="Is Tax Free" />

                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">

                                                                                        <br />
                                                                                        <asp:CheckBox ID="chkIsforefeitprovision" runat="server" Text="Is Forefeit provision" />

                                                                                        <br />
                                                                                        <br />
                                                                                    </div>



                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label19" runat="server" Text="Month Days Count(For Calculate per day salary)"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator6" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtMonthDaysCount" ErrorMessage="Enter Month Day Count"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtMonthDaysCount" runat="server"
                                                                                            CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                            TargetControlID="txtMonthDaysCount" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label20" runat="server" Text="Benefit(%) on termination"></asp:Label>

                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator7" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtbenefitperonTermination" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtbenefitperonTermination" runat="server" CssClass="form-control" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                            TargetControlID="txtbenefitperonTermination" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label21" runat="server" Text="Benefit(%) on resign"></asp:Label>

                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator8" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtbenefitperonresign" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtbenefitperonresign" runat="server" CssClass="form-control" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                                            TargetControlID="txtbenefitperonresign" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>


                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label22" runat="server" Text="Benefit(%) on retirement"></asp:Label>

                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator9" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtbenefitperonretirement" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtbenefitperonretirement" runat="server" CssClass="form-control" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                                            TargetControlID="txtbenefitperonretirement" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>



                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label23" runat="server" Text="Benefit(%) on death"></asp:Label>

                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator10" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtbenefitperondeath" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtbenefitperondeath" runat="server" CssClass="form-control" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                                            TargetControlID="txtbenefitperondeath" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>



                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label24" runat="server" Text="Benefit(%) on other"></asp:Label>

                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                            ID="RequiredFieldValidator11" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                            ControlToValidate="txtbenefitperonother" ErrorMessage="Enter Benefit %"></asp:RequiredFieldValidator>
                                                                                        <asp:TextBox ID="txtbenefitperonother" runat="server" CssClass="form-control" />

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                                                            TargetControlID="txtbenefitperonother" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <br />

                                                                                        <asp:CheckBox ID="chkisabsent" runat="server" Text="Count absent days for indemnity" CssClass="form-control" />

                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <br />

                                                                                        <asp:CheckBox ID="chkisweekoff" runat="server" Text="Count week off days for indemnity" CssClass="form-control" />

                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <br />

                                                                                        <asp:CheckBox ID="chkispaidleave" runat="server" Text="Count paid leave days for indemnity" CssClass="form-control" />

                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <br />

                                                                                        <asp:CheckBox ID="chkisunpaidLeave" runat="server" Text="Count unpaid leave days for indemnity" CssClass="form-control" />

                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <br />

                                                                                        <asp:CheckBox ID="chkisholiday" runat="server" Text="Count holidays for indemnity" CssClass="form-control" />

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
                                                                                        <asp:Label ID="Label4" runat="server" Text="Gratuity Days Detail" Font-Bold="true"></asp:Label>
                                                                                        <br />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label9" runat="server" Text="From year(In Days)"></asp:Label>
                                                                                        <asp:TextBox ID="txtFromYear" runat="server" CssClass="form-control" Enabled="false" Text="0" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDays" runat="server" Enabled="True"
                                                                                            TargetControlID="txtFromYear" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label11" runat="server" Text="To Year(In Days)"></asp:Label>
                                                                                        <asp:TextBox ID="txttoYear" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDaysto" runat="server" Enabled="True"
                                                                                            TargetControlID="txttoYear" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label12" runat="server" Text="Remuneration Days/Years"></asp:Label>
                                                                                        <asp:TextBox ID="txtremunerationDays" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtdeduction" runat="server" Enabled="True"
                                                                                            TargetControlID="txtremunerationDays" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <br />
                                                                                        <asp:Button ID="btnAddGratuity" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                            CssClass="btn btn-primary" OnClick="btnAddGartuaty_Click" />

                                                                                        <asp:Button ID="btnGratuityCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                            CssClass="btn btn-primary" OnClick="btnCancelGratuity_Click" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-12" class="flow">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvGartuityDetail" runat="server" AutoGenerateColumns="False"
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
                                                                                                <asp:TemplateField HeaderText="From Year(In Days)">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDaysFrom" runat="server" Text='<%#Eval("From_Year") %>' />
                                                                                                        <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="To Year(In Days)">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDaysTo" runat="server" Text='<%#Eval("To_Year") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Remuneration Days/Years">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbldeductionpercentage" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Remuneration_days").ToString(),Session["DBConnection"].ToString(),GetCurruncyValue()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="hdndeductionTransId" runat="server" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <hr />
                                                                                    <div class="col-md-12" style="text-align: center">
                                                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                                            CssClass="btn btn-success" ValidationGroup="a" OnClick="btnSaveGratuityPlan_Click" />

                                                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnResetGratuity_Click" />

                                                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                                                        <br />
                                                                                        <asp:HiddenField ID="editid" runat="server" />
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="box box-warning box-solid" <%= gvLeaveMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                                                    <div class="box-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <div class="flow">
                                                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMaster" PageSize="10"
                                                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                                                        OnPageIndexChanging="gvLeaveMaster_PageIndexChanging" OnSorting="gvLeaveMaster_OnSorting">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Action">
                                                                                                <ItemTemplate>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Plan Name" SortExpression="Plan_Name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Plan_Name") %>'></asp:Label>
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
                                                    <div class="row" style="text-align: center;">
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnbackEmpGratuity" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnbackFuncKy_Click" />

                                                        <asp:Button ID="btnNextGratutuity" runat="server" ValidationGroup="Submit" Text="Next >>" CssClass="btn btn-primary" OnClick="btnNextGratutuity_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="btnViewLabourLaw" runat="server">
                                    <h3>Labour Law</h3>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="nav-tabs-custom">
                                                <div class="tab-content">
                                                    <div class="tab-pane active" id="NewLabour">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box box-primary">
                                                                            <div class="box-body">
                                                                                <div class="form-group">
                                                                                    <asp:Label ID="lblLabourLawname" runat="server" Text="<%$ Resources:Attendance,Labour Law Name%>"></asp:Label>

                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                        ID="RequiredFieldValidator12" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                        ControlToValidate="txtLabourLawname" ErrorMessage="Enter Law Name"></asp:RequiredFieldValidator>
                                                                                    <asp:TextBox ID="txtLabourLawname" runat="server"
                                                                                        CssClass="form-control" />
                                                                                    <br />
                                                                                </div>


                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblfinancestartmonth" runat="server" Text="<%$ Resources:Attendance,Financial Start Month%>"></asp:Label>


                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                        ID="RequiredFieldValidator13" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                        ControlToValidate="ddlFinancestartMonth" InitialValue="--Select--" ErrorMessage="Enter Financial Start Month"></asp:RequiredFieldValidator>
                                                                                    <asp:DropDownList ID="ddlFinancestartMonth" runat="server" CssClass="form-control">

                                                                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                                                        <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                                                                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                                                                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                                                                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                                                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                                                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                                                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                                                                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                                                                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                                                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                                                                        <asp:ListItem Text="December" Value="12"></asp:ListItem>

                                                                                    </asp:DropDownList>


                                                                                    <br />
                                                                                </div>


                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:Attendance,Description%>"></asp:Label>


                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                        ID="RequiredFieldValidator18" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                        ControlToValidate="txtDescription" ErrorMessage="Enter Description"></asp:RequiredFieldValidator>
                                                                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                                                                        CssClass="form-control" />
                                                                                    <br />
                                                                                </div>


                                                                                <div class="col-md-6">

                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Work day Minute%>"></asp:Label>


                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                        ID="RequiredFieldValidator19" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                        ControlToValidate="txtWorkdayMinute" ErrorMessage="Enter Work day minute"></asp:RequiredFieldValidator>
                                                                                    <asp:TextBox ID="txtWorkdayMinute" runat="server"
                                                                                        CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                                                                        TargetControlID="txtWorkdayMinute" FilterType="Numbers">
                                                                                    </cc1:FilteredTextBoxExtender>

                                                                                    <br />
                                                                                </div>


                                                                                <div class="col-md-6" runat="server" id="Div_Indemnity">

                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Indemnity Plan%>"></asp:Label>

                                                                                    <asp:DropDownList ID="ddlPlanName" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                                    <br />


                                                                                </div>


                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Yearly Half Day%>"></asp:Label>


                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                        ID="RequiredFieldValidator20" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                                                        ControlToValidate="txtyearlyHalfDay" ErrorMessage="Enter Yearly Half day"></asp:RequiredFieldValidator>
                                                                                    <asp:TextBox ID="txtyearlyHalfDay" runat="server"
                                                                                        CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                                        TargetControlID="txtyearlyHalfDay" FilterType="Numbers">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance,Week Off Days%>"></asp:Label>
                                                                                    <asp:CheckBoxList ID="ChkWeekOffList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Sunday%>" Value="Sunday"></asp:ListItem>
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Monday%>" Value="Monday"></asp:ListItem>
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Tuesday%>" Value="Tuesday"></asp:ListItem>
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Wednesday%>" Value="Wednesday"></asp:ListItem>
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Thursday%>" Value="Thursday"></asp:ListItem>
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Friday%>" Value="Friday"></asp:ListItem>
                                                                                        <asp:ListItem class="form-control" Text="<%$ Resources:Attendance,Saturday%>" Value="Saturday"></asp:ListItem>
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
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblLeaveType" runat="server" Text="<%$ Resources:Attendance,Leave Type %>"></asp:Label><a style="color: Red">*</a>
                                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator21" ValidationGroup="L_Grid_Save"
                                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlLeaveType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Leave Type%>" />
                                                                                            <asp:DropDownList ID="ddlLeaveType" runat="server" class="form-control">
                                                                                            </asp:DropDownList>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblTotalLeave" runat="server" Text="<%$ Resources:Attendance,Total Leave Day %>"></asp:Label><a style="color: Red">*</a>
                                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator22" ValidationGroup="L_Grid_Save"
                                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalLeave" ErrorMessage="<%$ Resources:Attendance,Enter Total Leave Day %>" />
                                                                                            <asp:TextBox ID="txtTotalLeave" class="form-control" runat="server" MaxLength="3"
                                                                                                onkeypress="return isNumber(event)"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                                                                TargetControlID="txtTotalLeave" FilterType="Numbers">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblPaidLeave" runat="server" Text="<%$ Resources:Attendance,Paid Leave Day %>"></asp:Label>
                                                                                            <a style="color: Red">*</a>
                                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator23" ValidationGroup="L_Grid_Save"
                                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaidLeave" ErrorMessage="<%$ Resources:Attendance,Enter Paid Leave Day %>" />

                                                                                            <asp:TextBox ID="txtPaidLeave" runat="server" class="form-control" MaxLength="3" onkeypress="return isNumber(event)"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                                                TargetControlID="txtPaidLeave" FilterType="Numbers">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblSchType" runat="server" Text="<%$ Resources:Attendance,Schedule Type %>"></asp:Label><a style="color: Red">*</a>
                                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator24" ValidationGroup="L_Grid_Save"
                                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSchType" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Schedule Type %>" />

                                                                                            <asp:DropDownList ID="ddlSchType" class="form-control" runat="server">
                                                                                                <%--<asp:ListItem Value="0">--Select--</asp:ListItem>
                                               <asp:ListItem Value="Monthly">Monthly</asp:ListItem>--%>
                                                                                                <asp:ListItem Value="Yearly" Selected="True">Yearly</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                            <br />
                                                                                        </div>

                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Gender %>"></asp:Label><a style="color: Red">*</a>
                                                                                            <asp:DropDownList ID="ddlGender" runat="server" class="form-control">
                                                                                                <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                                                                                <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                                                                <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                                                            </asp:DropDownList>

                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <br />
                                                                                            <asp:CheckBox ID="chkYearCarry" runat="server" Text="<%$ Resources:Attendance,Is Year Carry%>" />
                                                                                            <asp:CheckBox ID="chkIsAuto" runat="server" Text="<%$ Resources:Attendance,Is Auto%>" />
                                                                                            <asp:CheckBox ID="ChkIsRule" runat="server" Text="<%$ Resources:Attendance,Is Rule%>" />
                                                                                            <asp:CheckBox ID="chkMonthCarry" Visible="false" runat="server"
                                                                                                Text="<%$ Resources:Attendance,IsMonthCarry %>" />
                                                                                            <br />
                                                                                        </div>


                                                                                        <div class="col-md-12" style="text-align: center;">

                                                                                            <br />
                                                                                            <asp:Button ID="btnAddLabour" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                                CssClass="btn btn-primary" ValidationGroup="L_Grid_Save" OnClick="btnAddLabour_Click" />

                                                                                            <asp:Button ID="btnCancelLabour" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                                CssClass="btn btn-primary" ValidationGroup="a" OnClick="btnCancelLabour_Click" />
                                                                                        </div>


                                                                                    </div>




                                                                                    <div class="col-md-12">
                                                                                        <br />
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvLeaveDetail" runat="server" AutoGenerateColumns="False"
                                                                                            Width="100%" DataKeyNames="Trans_Id" ShowFooter="false">

                                                                                            <Columns>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="imgBtnEmpoloyeeDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                            Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnEmpoloyeeDelete_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gender%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblGender" runat="server" Text='<%#Eval("Gender") %>' />

                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Name%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLeave_TypeName" runat="server" Text='<%#Common.GetleaveNameById(Eval("Leave_Type_Id").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString()) %>' />
                                                                                                        <asp:Label ID="lblLeave_Type_Id" Visible="false" runat="server" Text='<%#Eval("Leave_Type_Id") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Days%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblTotalleaveDay" runat="server" Text='<%#Eval("Total_Leave_days") %>' />

                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Paid Days%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblTotalPaidDay" runat="server" Text='<%#Eval("Paid_Leave_days") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,schedule_type%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblscheduleType" runat="server" Text='<%#Eval("schedule_type") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Year Carry%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblisYearcarry" runat="server" Text='<%#Eval("is_yearcarry") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Auto%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblisauto" runat="server" Text='<%#Eval("is_auto") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Rule%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblisRule" runat="server" Text='<%#Eval("is_rule") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                                </asp:TemplateField>

                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>

                                                                                    </div>



                                                                                    <hr />



                                                                                    <div class="col-md-12" style="text-align: center">


                                                                                        <br />


                                                                                        <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                                            CssClass="btn btn-success" ValidationGroup="a" OnClick="btnSavelabourLaw_Click" />

                                                                                        <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnResetLabourLaw_Click" />

                                                                                        <asp:Button ID="Button3" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                                                        <br />
                                                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="box box-warning box-solid" <%= gvLabourLaw.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="flow">
                                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLabourLaw" PageSize="10"
                                                                            runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                                            OnPageIndexChanging="gvLeaveMaster_PageIndexChanging" OnSorting="gvLeaveMaster_OnSorting">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Action">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkDefultLabourLaw" AutoPostBack="true" OnCheckedChanged="chkDefultLabourLaw_Changed" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Labour Law Name%>" SortExpression="Laborlaw_Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnTransId" Value='<%# Eval("Trans_Id") %>' runat="server" />
                                                                                        <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Laborlaw_Name") %>'></asp:Label>
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
                                                    <div class="row" style="text-align: center;">
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnLabourBack" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnLabourBack_Click" />

                                                        <asp:Button ID="btnLabourNext" runat="server" ValidationGroup="Submit" Text="Next >>" CssClass="btn btn-primary" OnClick="btnLabourNext_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:View>
                                <%-- <asp:View ID="btnViewEmpUpload"   runat="server">
                                   <asp:UpdatePanel ID="EmpUpload" Visible="false" runat="server">
    <ContentTemplate>
        <div class="row">
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
                                <asp:FileUpload ID="fileLoad"  runat="server" />
                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                    <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px"
                                        ImageUrl="../Images/Allow.png" Style="display: none" />
                                    <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px"
                                        ImageUrl="../Images/Delete1.png" Style="display: none" />
                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                </div>
                            </div>
                            <br />
                            <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                OnClick="btnGetSheet_Click" Visible="true"
                                Text="<%$ Resources:Attendance,Connect To DataBase %>" />
                        
                </div>
                <div class="col-md-6" style="text-align: center;">
                    <br />
                    <asp:Label runat="server" Text="Select Sheet" ID="Label170"></asp:Label>
                    <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                    <br />
                    <asp:Button ID="Button6" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                        OnClick="btnConnect_Click" Visible="true" Text="GetRecord" />
                    <br />
                </div>
            </div>
            <div class="row" id="uploadEmpdetail" runat="server" visible="false">
                <div class="col-md-6" style="text-align: left">
                    <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server"
                        GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupdall_OnCheckedChanged"
                        Text="All" />
                    <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server"
                        GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                    <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server"
                        GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                </div>
                <div class="col-md-12">
                    <div style="overflow: auto; max-height: 300px;">
                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected"
                            runat="server" Width="100%">
                            <PagerStyle CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                    <br />
                </div>
                <div class="col-md-6">
                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                    <asp:TextBox ID="txtuploadReferenceNo" runat="server" CssClass="form-control" ReadOnly="true" />
                    <br />
                </div>
                <div class="col-md-6" style="text-align: center">
                    <br />
                    <asp:Button ID="btnUploadEmpInfo" runat="server" CssClass="btn btn-primary"
                        OnClick="btnUploadEmpInfo_Click" Text="<%$ Resources:Attendance,Upload Data %>" />
                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server"
                        TargetControlID="btnUploadEmpInfo" ConfirmText="Are you sure to Save Records in Database.">
                    </cc1:ConfirmButtonExtender>
                    <asp:Button ID="btnResetEmpInfo" runat="server" CssClass="btn btn-primary" OnClick="btnResetEmpInfo_Click"
                        Text="<%$ Resources:Attendance,Reset %>" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

                                </asp:View>--%>

                                <asp:View ID="btnAllownceMaster" runat="server">
                                    <h3>Allownce Master</h3>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblApplicationName" runat="server" Text="<%$ Resources:Attendance,Allowance Name %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator25" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtAllowanceName" ErrorMessage="<%$ Resources:Attendance,Enter Allowance Name %>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtAllowanceName" runat="server" class="form-control" BackColor="#eeeeee"></asp:TextBox>


                                                            <cc1:AutoCompleteExtender ID="txtAllowanceName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath=""
                                                                CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAllowanceName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblApplicationNameL" runat="server" Text="<%$ Resources:Attendance,Allowance Name(Local) %>" />
                                                            <asp:TextBox ID="txtAllowanceNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No%>" />
                                                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAccountNo"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>



                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Calculation Type%>" />

                                                            <asp:DropDownList ID="ddlCalculationType" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="ddlCalculationType_TextChanged">

                                                                <asp:ListItem Text="Basis of attendance salary" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Basis of attendance day" Value="1"></asp:ListItem>

                                                            </asp:DropDownList>

                                                            <br />
                                                        </div>

                                                        <div class="col-md-12" runat="server" id="Div_IncludeDay" visible="false">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Include Day%>" />
                                                            :
                 
                 <asp:CheckBox ID="chkPresent" runat="server" Text="Present" />&nbsp;&nbsp;
                 <asp:CheckBox ID="chkweekoff" runat="server" Text="Week Off" />&nbsp;&nbsp;
                 <asp:CheckBox ID="chkHoliday" runat="server" Text="Holiday" />&nbsp;&nbsp;
                 <asp:CheckBox ID="chkabsent" runat="server" Text="Absent" />&nbsp;&nbsp;
                 <asp:CheckBox ID="chkPaidLeave" runat="server" Text="Paid Leave" />&nbsp;&nbsp;
                 <asp:CheckBox ID="chkUnpaidLeave" runat="server" Text="UnPaid Leave" />&nbsp;&nbsp;
                 <asp:CheckBox ID="chkHalfday" runat="server" Text="Half Day" />&nbsp;&nbsp;            
                                                        </div>

                                                        <div class="col-md-12" style="text-align: center;">
                                                            <asp:UpdatePanel ID="Update_Save" runat="server">
                                                                <ContentTemplate>

                                                                    <asp:Button ID="btnCSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                                        CssClass="btn btn-success" OnClick="btnCSave_Click" />
                                                                    <asp:Button ID="Button4" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                        CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="box box-warning box-solid" <%= GvAllowance.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                                <div class="box-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="flow">
                                                                <asp:HiddenField ID="HiddenField4" runat="server" />
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAllowance" runat="server" AutoGenerateColumns="False"
                                                                    Width="100%" AllowPaging="True"
                                                                    AllowSorting="True" PageSize="10">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField DataField="Allowance_Id" HeaderText="<%$ Resources:Attendance,Allowance Id %>"
                                                                            SortExpression="Allowance_Id">
                                                                            <ItemStyle />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Allowance" HeaderText="<%$ Resources:Attendance,Allowance Name %>"
                                                                            SortExpression="Allowance">
                                                                            <ItemStyle />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Allowance_L" HeaderText="<%$ Resources:Attendance,Allowance Name(Local) %>"
                                                                            SortExpression="Allowance_L">
                                                                            <ItemStyle />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblaccountname" runat="server" Text='<%#GetAccountNamebyTransId(Eval("Field1").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
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
                                    <div class="row" style="text-align: center;">
                                        <br />
                                        <br />
                                        <asp:Button ID="Button5" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnAllownceBack_Click" />

                                        <asp:Button ID="Button6" runat="server" ValidationGroup="Submit" Text="Next >>" CssClass="btn btn-primary" OnClick="btnAllownceNext_Click" />
                                    </div>

                                </asp:View>

                                <asp:View ID="btnViewDeductionMaster" runat="server">
                                    <h3>Deduction Master</h3>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <asp:HiddenField ID="hdnDeductionBransID" runat="server" />
                                                            <asp:HiddenField ID="hdnDeductionLocationId" runat="server" />
                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Deduction Name %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator26" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtDeductionName" ErrorMessage="<%$ Resources:Attendance,Enter Deduction Name %>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtDeductionName" runat="server" class="form-control"></asp:TextBox>

                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Deduction Name(Local) %>" />
                                                            <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtDeductionNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Type %>" />
                                                            <asp:DropDownList ID="ddlDeductionType" runat="server" class="form-control">
                                                                <asp:ListItem Text="General" Value="General"></asp:ListItem>
                                                                <asp:ListItem Text="PF" Value="PF"></asp:ListItem>
                                                                <asp:ListItem Text="ESIC" Value="ESIC"></asp:ListItem>
                                                                <asp:ListItem Text="TDS" Value="TDS"></asp:ListItem>
                                                                <asp:ListItem Text="Professional Tax" Value="PT"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Account No%>" />
                                                            <asp:TextBox ID="txtDeductionAccountName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDeductionAccountName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Calculation Type%>" />
                                                            <asp:DropDownList ID="ddlDeductionOptionType" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlCalculationType_TextChanged">
                                                                <asp:ListItem Text="Basis of attendance salary" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Basis of attendance day" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <br />
                                                            <asp:CheckBox runat="server" ID="chkAddToAllLocation" Text="Add To All Locations" CssClass="form-control" />
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6" runat="server" id="Div1" visible="false">
                                                            <br />
                                                            <asp:CheckBox ID="chkDeductionPresent" runat="server" Text="Present" />&nbsp;
                            <asp:CheckBox ID="chkDeductionweekoff" runat="server" Text="Week Off" />&nbsp;
                            <asp:CheckBox ID="chkDeductionHoliday" runat="server" Text="Holiday" />&nbsp;
                            <asp:CheckBox ID="chkDeductionabsent" runat="server" Text="Absent" />&nbsp;
                            <asp:CheckBox ID="chkDeductionPaidLeave" runat="server" Text="Paid Leave" />&nbsp;
                            <asp:CheckBox ID="chkDeductionUnpaidLeave" runat="server" Text="UnPaid Leave" />&nbsp;
                            <asp:CheckBox ID="chkDeductionHalfday" runat="server" Text="Half Day" />
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div id="Div_Box_Add" class="box box-info" runat="server">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">Add deduction Slab</h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i id="Btn_Add_Div" class="fa fa-minus" runat="server"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label28" runat="server" Text="From Amount"></asp:Label>
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                    ID="RequiredFieldValidator27" ValidationGroup="Add" Display="Dynamic" SetFocusOnError="true"
                                                                                    ControlToValidate="txtDeductionFromAmount" ErrorMessage="Enter From Amount"></asp:RequiredFieldValidator>
                                                                                <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtDeductionFromAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDeductionFromAmount" FilterType="Numbers">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label29" runat="server" Text="To Amount"></asp:Label>
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                    ID="RequiredFieldValidator28" ValidationGroup="Add" Display="Dynamic" SetFocusOnError="true"
                                                                                    ControlToValidate="txtDeductionToAmount" ErrorMessage="Enter To Amount"></asp:RequiredFieldValidator>
                                                                                <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtDeductionToAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDeductionToAmount" FilterType="Numbers">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label30" runat="server" Text="Calculation Type"></asp:Label>

                                                                                <asp:DropDownList ID="ddlDeductionclcType" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="Percentage" Value="Percentage"></asp:ListItem>
                                                                                    <asp:ListItem Text="Fixed" Value="Fixed"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label31" runat="server" Text="Value"></asp:Label>
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                                    ID="RequiredFieldValidator29" ValidationGroup="Add" Display="Dynamic" SetFocusOnError="true"
                                                                                    ControlToValidate="txtdeductionValue" ErrorMessage="Enter Value"></asp:RequiredFieldValidator>
                                                                                <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtdeductionValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                TargetControlID="txtdeductionValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
            </cc1:FilteredTextBoxExtender>--%>
                                                                                <asp:RegularExpressionValidator ID="RangeValidator1" runat="server" ValidationGroup="Add"
                                                                                    ControlToValidate="txtdeductionValue" Display="Dynamic"
                                                                                    ErrorMessage="Invalid Value"
                                                                                    ValidationExpression="^\d+(?:\.\d{0,9})?$"
                                                                                    SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:CheckBox ID="chkseniorcitizen" runat="server" Text="Is Senior Citizion" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:UpdatePanel ID="updadd" runat="server">

                                                                                    <ContentTemplate>

                                                                                        <asp:Button ID="btnAddDeduction" runat="server" Text="Add" CssClass="btn btn-warning" ValidationGroup="Add" OnClick="btnAddDeduction_Click" />
                                                                                        <asp:Button ID="btnCancelDeduction" runat="server" OnClick="btnCanceldeductionCancel_Click" Text="cancel" CssClass="btn btn-warning" />

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>


                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <br />

                                                                                <div class="flow">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVDeduction" runat="server" AutoGenerateColumns="False"
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
                                                                                            <asp:TemplateField HeaderText="From Amount">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblDaysFrom" runat="server" Text='<%#Eval("From_Amount") %>' />
                                                                                                    <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="To Amount">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblDaysTo" runat="server" Text='<%#Eval("To_Amount") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Calculation Type">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblcalculationType" runat="server" Text='<%#Eval("Calculation_Type") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Value">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblValue" runat="server" Text='<%#Eval("Value") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                            </asp:TemplateField>

                                                                                            <asp:TemplateField HeaderText="Is Senior citizen">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblSeniorcitizen" runat="server" Text='<%#Eval("Is_Senior_Citizen") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                            </asp:TemplateField>

                                                                                        </Columns>

                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <asp:HiddenField ID="HiddenField5" runat="server" />
                                                                                </div>

                                                                            </div>



                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div style="text-align: center;">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnDeductionSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                                        CssClass="btn btn-success" OnClick="btnSaveDeduction_Click" />
                                                                    <asp:Button ID="btnDeductionReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                        CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                                    <asp:HiddenField ID="HiddenField6" runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="box box-warning box-solid" <%= GvDeductionList.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                                            <br />
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="flow">
                                                                            <asp:HiddenField ID="HiddenField7" runat="server" />
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDeductionList" runat="server" AutoGenerateColumns="False"
                                                                                Width="100%">

                                                                                <Columns>

                                                                                    <asp:TemplateField HeaderText="Action">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="SN" Text="1" runat="server"></asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="Deduction_Id" HeaderText="<%$ Resources:Attendance,Deduction Id %>"
                                                                                        SortExpression="Deduction_Id">
                                                                                        <ItemStyle />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Deduction" HeaderText="<%$ Resources:Attendance,Deduction Name %>"
                                                                                        SortExpression="Deduction">
                                                                                        <ItemStyle />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Deduction_L" HeaderText="<%$ Resources:Attendance,Deduction Name(Local) %>"
                                                                                        SortExpression="Deduction_L">
                                                                                        <ItemStyle />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblaccountname" runat="server" Text='<%#GetAccountNamebyTransId(Eval("Field2").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
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


                                                        <div class="row" style="text-align: center;">
                                                            <br />
                                                            <br />
                                                            <asp:Button ID="Button7" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnbackDeduction_Click" />

                                                            <asp:Button ID="Button8" runat="server" ValidationGroup="Submit" Text="Next >>" CssClass="btn btn-primary" OnClick="btnNextDeduction_Click" />
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="btnViewApprovalMaster" runat="server">
                                    <h3>Approval Master</h3>
                                    <div class="row">
                                        <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
                                        <div class="box box-warning box-solid" <%= GvApproval.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvApproval" PageSize="10" runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                                AllowSorting="True">

                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                        <ItemTemplate>



                                                                            <div class="dropdown" style="position: absolute;">
                                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                                </button>
                                                                                <ul class="dropdown-menu">
                                                                                    <li>
                                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName='<%# Eval("Approval_Name") %>' CommandArgument='<%# Eval("Approval_Id") %>' OnCommand="btnApprovalEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approval Id %>" SortExpression="Approval_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDesigId1" runat="server" Text='<%# Eval("Approval_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Approval_Name" HeaderText="<%$ Resources:Attendance,Approval Name %>"
                                                                        SortExpression="Approval_Name" />
                                                                    <asp:BoundField DataField="ApprovalName_L" HeaderText="<%$ Resources:Attendance,Approval Name(Local) %>"
                                                                        SortExpression="ApprovalName_L" />
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="btnViewEmail" runat="server">
                                    <h3>Email Configuration</h3>
                                    <div class="row">
                                        <div class="form-group col-md-4">
                                            <label for="inputPassword45">Email</label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_txtEmail" ValidationGroup="Submit"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmail" ErrorMessage="Enter Email-Id" ForeColor="Red" Font-Size="12px"></asp:RequiredFieldValidator>
                                        </div>


                                        <div class="form-group col-md-4">
                                            <label for="inputPassword4">Password</label>
                                            <asp:TextBox ID="txtEmailPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator__txtEmailPassword" ValidationGroup="Submit"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmailPassword" ErrorMessage="Enter Password" ForeColor="Red" Font-Size="12px"></asp:RequiredFieldValidator>
                                        </div>


                                        <div class="form-group col-md-4">
                                            <label for="inputAddress">SMTP</label>
                                            <asp:TextBox ID="txtSMTP" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_txtSMTP" ValidationGroup="Submit"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSMTP" ErrorMessage="Enter SMTP" ForeColor="Red" Font-Size="12px"></asp:RequiredFieldValidator>
                                        </div>

                                    </div>
                                    <!--- First-Row----->
                                    <div class="row">
                                        <div class="form-group col-md-6">
                                            <label for="inputEmail4">Port</label>
                                            <asp:TextBox ID="txtPort" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator_txtPort" ValidationGroup="Submit"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPort" ErrorMessage="Enter Port No." ForeColor="Red" Font-Size="12px"></asp:RequiredFieldValidator>
                                        </div>


                                        <div class="form-group col-md-4">
                                            <label for="inputPassword4">Enable SSL</label>
                                            <asp:CheckBox ID="chkenablessl" runat="server" CssClass="form-control"></asp:CheckBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <br />
                                            <br />
                                            <asp:LinkButton ID="lnkTestemail" runat="server" Text="Test Email" ToolTip="Test Email Setting" Font-Underline="true" Font-Bold="true" OnClick="lnkTestemail_Click"></asp:LinkButton>
                                        </div>



                                    </div>

                                    <div class="row" style="text-align: center;">
                                        <br />
                                        <br />
                                        <asp:Button ID="btnbacktohome" runat="server" Text="<< Back" CssClass="btn btn-primary" OnClick="btnbacktohome_Click" />
                                        <asp:Button ID="btnsubmit" runat="server" ValidationGroup="Submit" Text="Submit" CssClass="btn btn-primary" OnClick="btnsubmit_Click" />
                                    </div>


                                </asp:View>
                            </asp:MultiView>



                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="lbledit" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></span></button>
                                    <h4 class="modal-title" id="myModalLabel">
                                        <asp:UpdatePanel ID="Update_Modal_Title" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Lbl_Modal_Title" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,View_Approval_Setup %>"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </h4>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                                        <Triggers>
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box box-primary">
                                                        <div class="box-body">
                                                            <div class="form-group">
                                                                <div style="text-align: center" class="col-md-12">
                                                                    <asp:Label ID="Label33" runat="server" Style="margin-right: 15px;" Text="<%$ Resources:Attendance,Approval Name %>"></asp:Label>
                                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblHeaderApprovalName" runat="server" Style="margin-left: 15px;"></asp:Label>
                                                                    <br />
                                                                </div>
                                                                <div style="text-align: center" class="col-md-12">
                                                                    <br />
                                                                    <asp:Label ID="lblBankCode" runat="server" Text="<%$ Resources:Attendance,Process Type%>"></asp:Label>

                                                                    <asp:RadioButton ID="rdopriority" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Priority%>"
                                                                        GroupName="Approval" OnCheckedChanged="rdoHierarchy_OnCheckedChanged" AutoPostBack="true" />
                                                                    <asp:RadioButton ID="rdoHierarchy" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Hierarchy%>"
                                                                        GroupName="Approval" OnCheckedChanged="rdoHierarchy_OnCheckedChanged" AutoPostBack="true" />
                                                                    <hr />
                                                                </div>
                                                                <div id="panelHerrachy" class="col-md-12" runat="server" visible="false">
                                                                    <div class="col-md-12">
                                                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Hierarchy Chain%>"></asp:Label>
                                                                        <asp:CheckBox ID="chkTeamLeader" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Team Leader%>" />
                                                                        <asp:CheckBox ID="chkDepartmentManager" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Department Manager%>" />
                                                                        <asp:CheckBox ID="chkParentDepartmentManager" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Parent Department Manager%>" />
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Hierarchy Rules%>"></asp:Label>
                                                                        <asp:RadioButton ID="rdoOpen" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Open%>" GroupName="Rules" />
                                                                        <asp:RadioButton ID="rdorestricted" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Restricted%>" GroupName="Rules" />
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Authorized department%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator30" ValidationGroup="Save"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtResponsibeDepartmentName" ErrorMessage="<%$ Resources:Attendance,Enter Authorized Department %>"></asp:RequiredFieldValidator>

                                                                        <asp:TextBox ID="txtResponsibeDepartmentName" BackColor="#eeeeee" runat="server"
                                                                            CssClass="form-control" OnTextChanged="txtDepName_OnTextChanged" AutoPostBack="true" />

                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListDepName" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsibeDepartmentName"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                                        </cc1:AutoCompleteExtender>
                                                                    </div>
                                                                </div>
                                                                <div id="panelPriority" class="col-md-12" runat="server" visible="false">
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label39" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator31" ValidationGroup="Emp_Add"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtResponsiblePerson" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>

                                                                        <asp:TextBox ID="txtResponsiblePerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                            AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />

                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsiblePerson" UseContextKey="True"
                                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                                        </cc1:AutoCompleteExtender>






                                                                        <%-- <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListEmployeeName" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtResponsiblePerson"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>--%>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <br />
                                                                        <asp:CheckBox ID="chkPriority" runat="server" Text="<%$ Resources:Attendance,Priority%>" />
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-12" style="text-align: center">
                                                                        <asp:Button ID="btnAddAppprovalEmployee" ValidationGroup="Emp_Add" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                            CssClass="btn btn-primary" OnClick="btnAddAppprovalEmployee_Click" />

                                                                    </div>
                                                                    <div style="overflow: auto" class="col-md-12">
                                                                        <br />
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvApprovalEmployeeDetail" runat="server" AutoGenerateColumns="False"
                                                                            Width="100%" DataKeyNames="Emp_Id" ShowFooter="false">

                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="imgBtnEmployeeEdit" runat="server" CommandArgument='<%# Eval("Emp_id") %>' CommandName='<%# Eval("Emp_name") %>' OnCommand="imgBtnApprovalEmployeeEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="imgBtnEmpoloyeeDelete" runat="server" CommandArgument='<%# Eval("Emp_id") %>' OnCommand="imgBtnApprovalEmpoloyeeDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvEmployeeCode" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%#Eval("Emp_Id") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvEmployeeName" runat="server" Text='<%#Eval("Emp_name") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority%>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvempPriority" runat="server" Text='<%#Eval("Priority") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
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
                                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="HiddenField8" runat="server" />
                                            <asp:Button ID="btnsaveConfig" runat="server" ValidationGroup="Save" CssClass="btn btn-success" OnClick="btnApprovalsaveConfig_Click" Text="<%$ Resources:Attendance,Save %>" Visible="false" />
                                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                                <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Close %>"></asp:Label></button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>



                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Home_Udpate">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div align="center">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px; position: relative;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <!---End First-Row----->
                </form>

                <div id="snackbar" align="center">
                    Saved Successfully-->
                             <div id="snackbar" align="center">
                                 Saved Successfully-->
                </form>

                <div id="snackbar" align="center">Saved Successfully</div>
                             </div>
                    <!----close container-fluid ---->
                    <br />
                    <br />

                </div>
                <br />
                <div align="center">© <%=DateTime.Now.Year.ToString() %> Pegasus All Rights Reserved | Design By <a href="#" target="_blank" style="color: red; font-weight: 2px;">Pegasus</a></div>
            </div>
            <script type="text/javascript">
                <%--function uploadComplete(sender, args) {
                    document.getElementById('<%= Img_Wrong .ClientID %>').style.display = "none";
                    document.getElementById('<%= Img_Right .ClientID %>').style.display = "";
                }
                function uploadError(sender, args) {
                    document.getElementById('<%= Img_Right .ClientID %>').style.display = "none";
                    document.getElementById('<%= Img_Wrong .ClientID %>').style.display = "";
                    alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
                }
                function uploadStarted(sender, args) {
                    var filename = args.get_fileName();
                    debugger;
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
                }--%>
                function View_Modal_Popup() {
                    document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
                }

                function Modal_Close() {
                    document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
                }
                function resetPosition1() {

                }
            </script>

            <style>
                .form-checkbox,
                .form-radio {
                    position: relative;
                    margin-top: 2.25rem;
                    margin-bottom: 2.25rem;
                    text-align: left;
                }

                .form-checkbox-inline .form-checkbox-label,
                .form-radio-inline .form-radio-label {
                    display: inline-block;
                    margin-right: 1rem;
                }

                .form-checkbox-legend,
                .form-radio-legend {
                    margin: 0 0 0.125rem 0;
                    font-weight: 500;
                    font-size: 1rem;
                    color: #333;
                }

                .form-checkbox-label,
                .form-radio-label {
                    position: relative;
                    cursor: pointer;
                    padding-left: 1.5rem;
                    text-align: left;
                    color: #333;
                    display: block;
                    margin-bottom: 0.5rem;
                }

                    .form-checkbox-label:hover i,
                    .form-radio-label:hover i {
                        color: #337ab7;
                    }

                    .form-checkbox-label span,
                    .form-radio-label span {
                        display: block;
                    }

                    .form-checkbox-label input,
                    .form-radio-label input {
                        width: auto;
                        opacity: 0.0001;
                        position: absolute;
                        left: 0.25rem;
                        top: 0.25rem;
                        margin: 0;
                        padding: 0;
                    }

                .form-checkbox-button {
                    position: absolute;
                    -webkit-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    user-select: none;
                    display: block;
                    color: #999;
                    left: 0;
                    top: 0.25rem;
                    width: 1rem;
                    height: 1rem;
                    z-index: 0;
                    border: 0.125rem solid currentColor;
                    border-radius: 0.0625rem;
                    transition: color 0.28s ease;
                    will-change: color;
                }

                    .form-checkbox-button::before,
                    .form-checkbox-button::after {
                        position: absolute;
                        height: 0;
                        width: 0.2rem;
                        background-color: #337ab7;
                        display: block;
                        transform-origin: left top;
                        border-radius: 0.25rem;
                        content: "";
                        transition: opacity 0.28s ease, height 0s linear 0.28s;
                        opacity: 0;
                        will-change: opacity, height;
                    }

                    .form-checkbox-button::before {
                        top: 0.65rem;
                        left: 0.38rem;
                        transform: rotate(-135deg);
                        box-shadow: 0 0 0 0.0625rem #fff;
                    }

                    .form-checkbox-button::after {
                        top: 0.3rem;
                        left: 0;
                        transform: rotate(-45deg);
                    }

                .form-checkbox-field:checked ~ .form-checkbox-button {
                    color: #337ab7;
                }

                    .form-checkbox-field:checked ~ .form-checkbox-button::after,
                    .form-checkbox-field:checked ~ .form-checkbox-button::before {
                        opacity: 1;
                        transition: height 0.28s ease;
                    }

                    .form-checkbox-field:checked ~ .form-checkbox-button::after {
                        height: 0.5rem;
                    }

                    .form-checkbox-field:checked ~ .form-checkbox-button::before {
                        height: 1.2rem;
                        transition-delay: 0.28s;
                    }


                #title {
                    float: left;
                    height: 50px;
                    padding: 15px 15px;
                    font-size: 20px;
                    line-height: 20px;
                    color: #FFFFFF;
                }

                #back {
                    background: #FFFFFF;
                    width: 800px;
                    margin: 0 auto;
                }

                .main {
                    padding: 60px;
                    0;
                }

                body {
                    background: #0099CC;
                }


                @media (max-width: 768px) {

                    #back {
                        width: 450px;
                    }
                }


                @media (max-width: 550px) {

                    #back {
                        width: 400px;
                    }
                }



                @media (max-width: 480px ) {

                    #back {
                        width: 350px;
                    }
                }
            </style>
</body>


</html>





