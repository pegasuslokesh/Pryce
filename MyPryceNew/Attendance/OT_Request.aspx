﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="OT_Request.aspx.cs" Inherits="Attendance_OT_Request" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
     <script type="text/javascript">
        var ddlText_From, ddlValue_From, ddlFrom, ddlText_to, ddlValue_to, ddlto;
        function CacheItems() {
            ddlText_From = new Array();
            ddlValue_From = new Array();
            ddlText_to = new Array();
            ddlValue_to = new Array();
            ddlFrom = document.getElementById("<%=ddlcodefrom.ClientID %>");
            ddlto = document.getElementById("<%=ddlcodeto.ClientID %>");

            for (var i = 0; i < ddlFrom.options.length; i++) {
                ddlText_From[ddlText_From.length] = ddlFrom.options[i].text;
                ddlValue_From[ddlValue_From.length] = ddlFrom.options[i].value;
                ddlText_to[ddlText_to.length] = ddlto.options[i].text;
                ddlValue_to[ddlValue_to.length] = ddlto.options[i].value;
            }
        }


        function FilterItems(value) {

            if (value == "") {
                ddlFrom.selectedIndex = 0;
            }
            else {
                for (var i = 0; i < ddlText_From.length; i++) {
                    if (ddlText_From[i] == value) {
                        ddlFrom.value = ddlValue_From[i];

                        //AddItem(ddlText_From[i], ddlValue_From[i]);
                    }

                }

            }

            //if ( ddlFrom.options.length == 0) {


            //}
        }


        function FilterItems1(value) {

            if (value == "") {
                ddlto.selectedIndex = ddlText_to.length - 1;
            }
            else {
                for (var i = 0; i < ddlText_to.length; i++) {
                    if (ddlText_to[i] == value) {
                        ddlto.value = ddlValue_to[i];

                        //AddItem(ddlText_From[i], ddlValue_From[i]);
                    }

                }

            }

            //if ( ddlFrom.options.length == 0) {


            //}
        }
        function GetMonthName(monthNumber) {
            var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
            return months[monthNumber - 1];
        }
        $(document).ready(function () {
            $('#aspnetForm').on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
            $("#ctl00_MainContent_txt1").keydown(function (e) {
                if (e.keyCode == 13) {
                    var len = $('#ctl00_MainContent_txt1').val().length;
                    if (len == 6 || len == 8) {
                        if (len == 6) {
                            var d = $('#ctl00_MainContent_txt1').val().substr(0, 2);
                            var m = $('#ctl00_MainContent_txt1').val().substr(2, 2);
                            var y = $('#ctl00_MainContent_txt1').val().substr(4, 2);



                            console.warn(m);
                            console.warn(y);
                            if ((m > 12) || (d > 31)) {
                                $('#ctl00_MainContent_txt1').select();
                                $('#ctl00_MainContent_txt1').focus();
                            }
                            else {
                                console.warn(d + '-' + GetMonthName(m) + '-' + y);
                                $('#ctl00_MainContent_txtFromdate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                                $('#ctl00_MainContent_txt2').focus();
                            }
                        }
                        if (len == 8) {
                            var d = $('#ctl00_MainContent_txt1').val().substr(0, 2);
                            var m = $('#ctl00_MainContent_txt1').val().substr(2, 2);
                            var y = $('#ctl00_MainContent_txt1').val().substr(6, 2);

                            console.warn(m);
                            console.warn(y);

                            if ((m > 12) || (d > 31)) {
                                $('#ctl00_MainContent_txt1').select();
                                $('#ctl00_MainContent_txt1').focus();
                            }
                            else {

                                console.warn(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);

                                $('#ctl00_MainContent_txtFromdate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                                $('#ctl00_MainContent_txt2').focus();
                            }

                        }


                    }
                    else {
                        $('#ctl00_MainContent_txtFromdate').val('');
                        $('#ctl00_MainContent_txt1').select();
                        $('#ctl00_MainContent_txt1').focus();
                    }
                }
            });



            $('#ctl00_MainContent_txt1').change(function () {
                var len = $('#ctl00_MainContent_txt1').val().length;
                if (len == 6 || len == 8) {
                    if (len == 6) {
                        var d = $('#ctl00_MainContent_txt1').val().substr(0, 2);
                        var m = $('#ctl00_MainContent_txt1').val().substr(2, 2);
                        var y = $('#ctl00_MainContent_txt1').val().substr(4, 2);



                        console.warn(m);
                        console.warn(y);
                        if ((m > 12) || (d > 31)) {
                            $('#ctl00_MainContent_txt1').select();
                            $('#ctl00_MainContent_txt1').focus();
                        }
                        else {
                            console.warn(d + '-' + GetMonthName(m) + '-' + y);
                            $('#ctl00_MainContent_txtFromdate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                            $('#ctl00_MainContent_txt2').focus();
                        }
                    }
                    if (len == 8) {
                        var d = $('#ctl00_MainContent_txt1').val().substr(0, 2);
                        var m = $('#ctl00_MainContent_txt1').val().substr(2, 2);
                        var y = $('#ctl00_MainContent_txt1').val().substr(6, 2);

                        console.warn(m);
                        console.warn(y);

                        if ((m > 12) || (d > 31)) {
                            $('#ctl00_MainContent_txt1').select();
                            $('#ctl00_MainContent_txt1').focus();
                        }
                        else {

                            console.warn(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);

                            $('#ctl00_MainContent_txtFromdate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                            $('#ctl00_MainContent_txt2').focus();
                        }

                    }


                }
                else {
                    $('#ctl00_MainContent_txtFromdate').val('');
                    $('#ctl00_MainContent_txt1').select();
                    $('#ctl00_MainContent_txt1').focus();
                }

            });
            $('#ctl00_MainContent_txt1').click(function () {
                $('#ctl00_MainContent_txt1').select();
            });

            $('#ctl00_MainContent_txt2').click(function () {
                $('#ctl00_MainContent_txt2').select();
            });
            $("#ctl00_MainContent_txt2").keydown(function (e) {
                if (e.keyCode == 13) {
                    var len = $('#ctl00_MainContent_txt2').val().length;
                    if (len == 6 || len == 8) {
                        if (len == 6) {
                            var d = $('#ctl00_MainContent_txt2').val().substr(0, 2);
                            var m = $('#ctl00_MainContent_txt2').val().substr(2, 2);
                            var y = $('#ctl00_MainContent_txt2').val().substr(4, 2);



                            console.warn(m);
                            console.warn(y);
                            if ((m > 12) || (d > 31)) {
                                $('#ctl00_MainContent_txt2').select();
                                $('#ctl00_MainContent_txt2').focus();
                            }
                            else {
                                console.warn(d + '-' + GetMonthName(m) + '-' + y);
                                $('#ctl00_MainContent_txtTodate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                                $('#ctl00_MainContent_txtSearch').focus();
                            }
                        }
                        if (len == 8) {
                            var d = $('#ctl00_MainContent_txt2').val().substr(0, 2);
                            var m = $('#ctl00_MainContent_txt2').val().substr(2, 2);
                            var y = $('#ctl00_MainContent_txt2').val().substr(6, 2);


                            console.warn(m);
                            console.warn(y);
                            if ((m > 12) || (d > 31)) {
                                $('#ctl00_MainContent_txt2').select();
                                $('#ctl00_MainContent_txt2').focus();
                            }
                            else {
                                console.warn(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                                $('#ctl00_MainContent_txtTodate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);

                                $('#ctl00_MainContent_txtSearch').focus();
                            }


                        }

                    }
                    else {
                        $('#ctl00_MainContent_txtTodate').val('');
                        $('#ctl00_MainContent_txt2').select();
                        $('#ctl00_MainContent_txt2').focus();
                    }
                }
            });
            $('#ctl00_MainContent_txt2').change(function () {
                var len = $('#ctl00_MainContent_txt2').val().length;
                if (len == 6 || len == 8) {
                    if (len == 6) {
                        var d = $('#ctl00_MainContent_txt2').val().substr(0, 2);
                        var m = $('#ctl00_MainContent_txt2').val().substr(2, 2);
                        var y = $('#ctl00_MainContent_txt2').val().substr(4, 2);



                        console.warn(m);
                        console.warn(y);
                        if ((m > 12) || (d > 31)) {
                            $('#ctl00_MainContent_txt2').select();
                            $('#ctl00_MainContent_txt2').focus();
                        }
                        else {
                            console.warn(d + '-' + GetMonthName(m) + '-' + y);
                            $('#ctl00_MainContent_txtTodate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                            $('#ctl00_MainContent_txtSearch').focus();
                        }
                    }
                    if (len == 8) {
                        var d = $('#ctl00_MainContent_txt2').val().substr(0, 2);
                        var m = $('#ctl00_MainContent_txt2').val().substr(2, 2);
                        var y = $('#ctl00_MainContent_txt2').val().substr(6, 2);


                        console.warn(m);
                        console.warn(y);
                        if ((m > 12) || (d > 31)) {
                            $('#ctl00_MainContent_txt2').select();
                            $('#ctl00_MainContent_txt2').focus();
                        }
                        else {
                            console.warn(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);
                            $('#ctl00_MainContent_txtTodate').val(d + '-' + GetMonthName(m).substr(0, 3) + '-' + y);

                            $('#ctl00_MainContent_txtSearch').focus();
                        }


                    }

                }
                else {
                    $('#ctl00_MainContent_txtTodate').val('');
                    $('#ctl00_MainContent_txt2').select();
                    $('#ctl00_MainContent_txt2').focus();
                }

            });
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <asp:HiddenField ID="hdfCurrentRow" runat="server" />
    <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
    <h1>
        <i class="fas fa-desktop"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,OT/Absent Request and Approval%>">"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Log Monitoring Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_Popup" Style="display: none;" data-toggle="modal" data-target="#myModal" runat="server" Text="Modal Popup" />
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



    <asp:UpdatePanel ID="Update_Page" runat="server">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnExport" />--%>
            <asp:PostBackTrigger ControlID="btnOtReport" />
            <asp:PostBackTrigger  ControlID="gvOverTime" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">


                                <div class="col-md-6">
                                    <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Date Range%>"></asp:Label>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txt1" runat="server" style="width: 70px;margin-left: -15px;height:34px;"  ></asp:TextBox>
                                            </div>
                                            <div class="col-md-9">

                                                <asp:TextBox ID="txtFromdate" placeholder="From Date" runat="server" Class="form-control"></asp:TextBox>
                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalenderFromdate" runat="server" Enabled="true" TargetControlID="txtFromdate" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txt2" runat="server" style="width: 70px;margin-left: -15px;height:34px;"></asp:TextBox>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtTodate" placeholder="To Date" runat="server" Class="form-control"></asp:TextBox>
                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalenderTodate" runat="server" Enabled="true" TargetControlID="txtTodate" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">

                                    <asp:Label ID="lblLocation" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <br />
                                        <div class="input-group-btn" style="margin-top: 5px;">
                                            <asp:Button ID="btnfilterdepartment" runat="server" CssClass="btn btn-info" OnClick="btnfilterdepartment_Click" Text="<%$ Resources:Attendance,Filter Department%>" />

                                        </div>
                                    </div>
                                </div>



                                <div class="col-md-6">

                                    <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Employee Code Range%>"></asp:Label>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" Class="form-control"
                                                    onkeyup="FilterItems(this.value)"></asp:TextBox>
                                                <div class="input-group-btn" style="width: 50%;">
                                                    <asp:DropDownList ID="ddlcodefrom" runat="server" Class="form-control"></asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtSearchto" runat="server" placeholder="Search" Class="form-control"
                                                    onkeyup="FilterItems1(this.value)"></asp:TextBox>
                                                <div class="input-group-btn" style="width: 50%;">
                                                    <asp:DropDownList ID="ddlcodeto" runat="server" Class="form-control"></asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>

                                    </div>



                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Grade Range%>"></asp:Label>
                                    <div class="input-group" style="width: 100%;">
                                        <asp:DropDownList ID="ddlGradeFrom" placeholder="From Date" runat="server" Class="form-control"></asp:DropDownList>
                                        <div class="input-group-btn" style="width: 50%;">
                                            <asp:DropDownList ID="ddlGradeTo" placeholder="To Date" runat="server" Class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Select Approval Type%>"></asp:Label>
                                    <asp:DropDownList ID="ddlRequestType" runat="server" Class="form-control">
                                        <asp:ListItem Text="OverTime" Value="OverTime"></asp:ListItem>
                                        <asp:ListItem Text="Absent" Value="Absent"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                 <div class="col-md-6">
                                    <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Record Type%>"></asp:Label>
                                    <asp:DropDownList ID="ddlOTFilter" runat="server" Class="form-control">
                                        <asp:ListItem Text="OverTime Days Only" Value="OverTime"></asp:ListItem>
                                        <asp:ListItem Text="All" Selected="True" Value="All"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-12" style="text-align: center;">
                                    <br />

                                    <asp:Button ID="btnLogProcess" runat="server" CssClass="btn btn-primary" OnClick="btnLogProcess_Click"
                                        Text="<%$ Resources:Attendance,Log Process%>" />


                                    <asp:Button ID="btnGo" runat="server" CssClass="btn btn-primary" OnClick="btnGo_Click"
                                        Text="<%$ Resources:Attendance,Search%>" />

                                    <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click"
                                        Text="<%$ Resources:Attendance,Request%>" />

                                    <asp:Button ID="btnupdate" runat="server" CssClass="btn btn-primary" OnClick="btnupdate_Click"
                                        Text="Approve" Visible="false" />

                                    <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval1" runat="server" TargetControlID="btnupdate"
                                        ConfirmText="Are you sure you want to Approve this request?">
                                    </cc1:ConfirmButtonExtender>

                                    <asp:Button ID="btnReject" runat="server" CssClass="btn btn-primary" OnClick="btnReject_Click"
                                        Text="<%$ Resources:Attendance,Reject%>" Visible="false" />

                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnReject"
                                        ConfirmText="Are you sure you want to reject this request?">
                                    </cc1:ConfirmButtonExtender>

                                   <%-- <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export"
                                        OnClick="ExportToExcel" />--%>

                                       <asp:Button ID="btnOtReport" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Over Time Report%>"
                                        OnClick="btnOtReport_Click" />

                                       

                                </div>

                            
                                    <div class="col-md-12" runat="server" style="overflow: auto; max-height: 1500px;" id="scrollArea" onscroll="SetDivPosition()">
                                        <br />
                                        <asp:GridView  ID="gvOverTime" runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="gvOverTime_RowDataBound">
                                            <Columns>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" onclick="SetSelectedRow(this)" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkHeaderSelect_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Code %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>' />
                                                        <asp:Label ID="lblempcode" runat="server" Text='<%# Eval("Emp_Code") %>' />
                                                        <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpIdName" runat="server" Text='<%# Eval("Emp_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %> ">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# GetDate(Eval("att_Date")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Day %> ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDay" runat="server" Text='<%# GetDay(Eval("att_Date")) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,TimeTable %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTime" runat="server" Text='<%# Eval("TimeTable_Name")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,On Duty Time %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblondutyTime" runat="server" Text='<%# Convert.ToDateTime(Eval("onduty_Time")).ToString("HH:mm") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Off Duty Time %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbloffdutyTime" runat="server" Text='<%# Convert.ToDateTime(Eval("offduty_Time")).ToString("HH:mm") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,In Time %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInTime" runat="server" Text='<%# Convert.ToDateTime(Eval("In_Time")).ToString("HH:mm") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Out Time %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbloutTime" runat="server" Text='<%# Convert.ToDateTime(Eval("out_Time")).ToString("HH:mm") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDayType" runat="server" Text='<%# Eval("DayType") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assigned OT %>">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblAssignedOT" runat="server" Text='<%#GetHours(Eval("AssignedOT").ToString()) %>' />--%>
                                                        <asp:Label ID="lblAssignedOT" runat="server" Text='<%#GetHours(Eval("PDOT").ToString()) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual OT %>">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="txtOtMinute" Width="50px" runat="server" Enabled='<%# ((Session["EmpId"].ToString()== Eval("HR_Id").ToString() || Session["EmpId"].ToString()== Eval("ParentDepManager_Id").ToString()) && (Eval("RequestStatus").ToString().Trim().ToLower()!="approved")) ? true : false %>' Text='<%# GetHours(Eval("ActualOTValue").ToString()) %>' onchange="SetSelectedRow(this)" />--%>
                                                        <asp:TextBox ID="txtOtMinute" Width="50px" runat="server" Enabled='<%# ((Session["EmpId"].ToString()== Eval("HR_Id").ToString() || Session["EmpId"].ToString()== Eval("ParentDepManager_Id").ToString()) && (Eval("RequestStatus").ToString().Trim().ToLower()!="approved")) ? true : false %>' Text='<%# GetHours(Eval("AOT").ToString()) %>' onchange="SetSelectedRow(this)" />

                                                       <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtOtMinute"
                                                                    UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                </cc1:MaskedEditExtender>
                                                                <cc1:MaskedEditValidator ID="EarliestTimeValidator" runat="server" ControlExtender="MaskedEditExtender1"
                                                                    ControlToValidate="txtOtMinute" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                    SetFocusOnError="True" />

                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                              
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("RequestStatus") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="TL Status" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chktl" runat="server" Enabled='<%# (Session["EmpId"].ToString()== Eval("TeamLeader_Id").ToString() && (Eval("DepManager_Status").ToString()=="" || Eval("DepManager_Status").ToString()=="False")) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("TeamLeader_Status").ToString()) ? false : Eval("TeamLeader_Status") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Section Manager">
                                                    <ItemTemplate>
                                                        <asp:CheckBox  ID="chkDepManager" runat="server" Enabled='<%# (Session["EmpId"].ToString()== Eval("DepManager_Id").ToString() && (Eval("ParentDepManager_Status").ToString()=="" || Eval("ParentDepManager_Status").ToString()=="False")) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("DepManager_Status").ToString()) ? false : Eval("DepManager_Status") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Department Manager">
                                                    <ItemTemplate>
                                                        <asp:CheckBox  ID="chkparentDepManager" runat="server" Enabled='<%# (Session["EmpId"].ToString()== Eval("ParentDepManager_Id").ToString() && (Eval("HR_Status").ToString()=="" || Eval("HR_Status").ToString()=="False")) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("ParentDepManager_Status").ToString()) ? false : Eval("ParentDepManager_Status") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="HR">
                                                    <ItemTemplate>
                                                        <asp:CheckBox  ID="chkHR" runat="server" Enabled='<%# (Session["EmpId"].ToString()== Eval("HR_Id").ToString() && (Eval("RequestStatus").ToString().Trim().ToLower()!="approved")) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("HR_Status").ToString()) ? false : Eval("HR_Status") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtremarks" Width="100px" Height="20px" runat="server" TextMode="MultiLine" Text='<%# Eval("Remarks") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                <br />
                                <br />
                                <hr />
                               <div class="col-md-12" style="text-align: left;">
                                     <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Delete Record%>"
                                        OnClick="btnDelete_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Filter Department%>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">

                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="labelComman" onClick="new_validation();" Text="<%$ Resources:Attendance,Select All %>" />


                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">

                                                <div class="col-md-12" style="overflow: auto; max-height: 300px">


                                                    <asp:TreeView ID="TreeViewDepartment" runat="server"></asp:TreeView>

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
                            <asp:Button ID="btnSave" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click" />

                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Page">
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
    <script language="javascript" type="text/javascript">
        function new_validation() {
            var val = document.getElementById("<%= chkSelectAll.ClientID  %>").checked;
            if (val) {
                $('[id*=TreeViewDepartment] input[type=checkbox]').prop('checked', true);

            }
            else {
                $('[id*=TreeViewDepartment] input[type=checkbox]').prop('checked', false);
            }

        }



        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels

                CheckUncheckParents(src, src.checked);

            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            if (!check) {

                return;
            }
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        checkUncheckSwitch = true;
                    //return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
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

        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= gvOverTime.ClientID %>');
                var rowIndex = parseInt(rowIndex);
                parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");

                document.getElementById("<%=scrollArea.ClientID%>").scrollTop = h.value;

            }
            catch (e) {

            }
        }

        function SetDivPosition() {
            var intY = document.getElementById("<%=scrollArea.ClientID%>").scrollTop;
            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
            h.value = intY;

        }


        function SetSelectedRow(lnk) {
            //Reference the GridView Row.
            var row = lnk.parentNode.parentNode;
            $('#<%= hdfCurrentRow.ClientID %>').val(row.rowIndex - 1);

            row.style.backgroundColor = "#A1DCF2";
        }
        
        function show_modal() {
            document.getElementById('<%=Btn_Modal_Popup.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Modal_Popup.ClientID %>').click();
        }




    </script>

</asp:Content>



