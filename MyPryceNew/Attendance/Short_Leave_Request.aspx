<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Short_Leave_Request.aspx.cs" Inherits="Attendance_Short_Leave_Request" %>

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
    <script type="text/javascript">
        function FreezeHeader() {
            try {


                var ScrollHeight = 250;
                var grid = document.getElementById("<%=gvOverTime.ClientID %>");

                //var grid = document.getElementById(GridId);
                var gridWidth = grid.offsetWidth;
                var gridHeight = grid.offsetHeight;
                var headerCellWidths = new Array();
                for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                    headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
                }
                grid.parentNode.appendChild(document.createElement("div"));
                var parentDiv = grid.parentNode;

                var table = document.createElement("table");
                for (i = 0; i < grid.attributes.length; i++) {
                    if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                        table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                    }
                }
                table.style.cssText = grid.style.cssText;
                table.style.width = gridWidth + "px";
                table.appendChild(document.createElement("tbody"));
                table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
                var cells = table.getElementsByTagName("TH");

                var gridRow = grid.getElementsByTagName("TR")[0];
                for (var i = 0; i < cells.length; i++) {
                    var width;
                    //if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                    //}
                    //else {
                    //    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                    //}
                    cells[i].style.width = parseInt(width) + "px";
                    gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width) + "px";
                }
                parentDiv.removeChild(grid);

                var dummyHeader = document.createElement("div");
                dummyHeader.appendChild(table);
                parentDiv.appendChild(dummyHeader);
                var scrollableDiv = document.createElement("div");
                if (parseInt(gridHeight) > ScrollHeight) {
                    gridWidth = parseInt(gridWidth) + 17;
                }
                scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
                scrollableDiv.appendChild(grid);
                parentDiv.appendChild(scrollableDiv);
            }
            catch (e) {

            }
        }




        




        function checkItem_All1_Header(objRef) {
            alert('test');
            debugger;
            $('#gvOverTime tbody tr td input[type="checkbox"]').each(function () {
                if (objRef.checked) {
                    $(this).parents('tr').find(':checkbox').prop('checked', true);
                }
                else {
                    // $(this).closest('tr').prop('checked', false);
                    $(this).parents('tr').find(':checkbox').prop('checked', false);
                }
            });
        }

        function checkItem_All1(objRef) {
            debugger;
            // $('#GvEmpListSelected tbody tr td input[type="checkbox"]').each(function () {
            if (objRef.checked) {
                $(objRef).parents('tr').find(':checkbox:enabled').prop('checked', true);
            }
            else {
                // $(this).closest('tr').prop('checked', false);
                $(objRef).parents('tr').find(':checkbox:enabled').prop('checked', false);
            }
            // });
        }



    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <asp:HiddenField ID="hdfCurrentRow" runat="server" />
    <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
    <h1>
        <i class="fas fa-desktop"></i>&nbsp;&nbsp;
            <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Short Leave Request and Approval%>"></asp:Label>
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
            <asp:Button ID="Btn_Modal_Popup" Style="display: none;" data-toggle="modal" data-target="#myModal" runat="server" Text="<%$ Resources:Attendance,Modal Popup%>" />
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
            <asp:PostBackTrigger ControlID="btnExport" />
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
                                    <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Leave Type%>"></asp:Label>
                                    <asp:DropDownList ID="ddlleaveType" runat="server" Class="form-control">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="Auto"></asp:ListItem>
                                        <asp:ListItem Text="Authorized" Value="Manual"></asp:ListItem>

                                    </asp:DropDownList>

                                </div>

                                <div class="col-md-6" style="text-align: center;">
                                    <br />
                                    <%--  <input type="button" onclick="FreezeHeader()" />
                                    <asp:Button ID="btntest" runat="server" Text="test" OnClientClick="FreezeHeader()" />--%>

                                    <asp:Button ID="btnGo" runat="server" CssClass="btn btn-primary" OnClick="btnGo_Click"
                                        Text="<%$ Resources:Attendance,Go%>" />
                                    <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click"
                                        Text="<%$ Resources:Attendance,Request%>" />
                                    <asp:Button ID="btnupdate" runat="server" CssClass="btn btn-primary" OnClick="btnupdate_Click"
                                        Text="<%$ Resources:Attendance,Approve%>" Visible="false" />
                                    <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval1" runat="server" TargetControlID="btnupdate"
                                        ConfirmText="Are you sure you want to Approve this request?">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="btnReject" runat="server" CssClass="btn btn-primary" OnClick="btnReject_Click"
                                        Text="Reject" Visible="false" />
                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnReject"
                                        ConfirmText="Are you sure you want to reject this request?">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Export%>"
                                        OnClick="ExportToExcel" />


                                </div>


                                <div class="col-md-12" runat="server" style="overflow: auto; max-height:500px;">
                                    <br />
                              

                               
                                <asp:GridView ID="gvOverTime"    Width="100%" runat="server" AutoGenerateColumns="False">

                                    <Columns>

                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" onclick="checkItem_All1(this)" runat="server" />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeaderSelect" runat="server"  AutoPostBack="true" OnCheckedChanged="chkHeaderSelect_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>' />
                                                <asp:Label ID="lblempcode" runat="server" Text='<%# Eval("Emp_Code") %>' />
                                                <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpIdName" runat="server" Text='<%# Eval("Emp_Name") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Local Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpIdlocal" runat="server" Text='<%# Eval("Emp_Name_L") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpartialDate" Width="100px" runat="server" Text='<%# GetDate(Eval("Partial_Leave_Date")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TimeTable">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTime" runat="server" Text='<%# Eval("TimeTable_Name")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="On Duty Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblondutyTime" runat="server" Text='<%# Convert.ToDateTime(Eval("onduty_Time")).ToString("HH:mm") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Off Duty Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloffdutyTime" runat="server" Text='<%# Convert.ToDateTime(Eval("offduty_Time")).ToString("HH:mm") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="In Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInTime" runat="server" Text='<%# Convert.ToDateTime(Eval("in_time")).ToString("HH:mm") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Out Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutTime" runat="server" Text='<%# Convert.ToDateTime(Eval("out_time")).ToString("HH:mm") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Partial In">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtfromTime" Enabled='<%# ((Session["EmpId"].ToString()== Eval("HR_Id").ToString() || Session["EmpId"].ToString()== Eval("ParentDepManager_Id").ToString()) && (Eval("RequestStatus").ToString().Trim().ToLower()!="approved")) ? true : false %>' Width="40px" runat="server" Text='<%# Eval("From_Time") %>' />

                                                <cc1:MaskedEditExtender ID="MaskedEditExtender_txtfromTime" runat="server" CultureAMPMPlaceholder=""
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                    Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtfromTime"
                                                    UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="EarliestTimeValidator_MaskedEditExtender_txtfromTime" runat="server" ControlExtender="MaskedEditExtender_txtfromTime"
                                                    ControlToValidate="txtfromTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                    SetFocusOnError="True" />

                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Partial Out">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txttoTime" Enabled='<%# ((Session["EmpId"].ToString()== Eval("HR_Id").ToString() || Session["EmpId"].ToString()== Eval("ParentDepManager_Id").ToString()) && (Eval("RequestStatus").ToString().Trim().ToLower()!="approved")) ? true : false %>' Width="40px" runat="server" Text='<%# Eval("To_Time") %>' />

                                                <cc1:MaskedEditExtender ID="MaskedEditExtender_txttoTime" runat="server" CultureAMPMPlaceholder=""
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                    Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txttoTime"
                                                    UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="EarliestTimeValidator_MaskedEditExtender_txttoTime" runat="server" ControlExtender="MaskedEditExtender_txttoTime"
                                                    ControlToValidate="txttoTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                    SetFocusOnError="True" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duration">
                                            <ItemTemplate>
                                                <asp:Label ID="blDuration" runat="server" Text='<%#GetTimeDuration( Eval("From_Time").ToString(),Eval("To_Time").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
                                              <asp:TemplateField HeaderText="<%$ Resources:Attendance,Partial Type%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPType" runat="server" Text='<%# Eval("PartialLeaveType") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Leave Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLeaveType" runat="server" Text='<%# Eval("Leave_Type") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <%-- <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblgenerated" runat="server" Text='<%# Eval("Entered") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>


                                        <asp:TemplateField HeaderText="Status">
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
                                                <asp:CheckBox ID="chkDepManager" runat="server" Enabled='<%# (Session["EmpId"].ToString()== Eval("DepManager_Id").ToString() && (Eval("ParentDepManager_Status").ToString()=="" || Eval("ParentDepManager_Status").ToString()=="False")) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("DepManager_Status").ToString()) ? false : Eval("DepManager_Status") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Department Manager">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkparentDepManager" runat="server" Enabled='<%# (Session["EmpId"].ToString()== Eval("ParentDepManager_Id").ToString() && (Eval("HR_Status").ToString()=="" || Eval("HR_Status").ToString()=="False")) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("ParentDepManager_Status").ToString()) ? false : Eval("ParentDepManager_Status") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="HR">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkHR" runat="server" Enabled='<%# ((((Session["EmpId"].ToString() == Eval("HR_Id").ToString()) ||(Convert.ToBoolean(Eval("HRState").ToString()))  ) && (Eval("RequestStatus").ToString().Trim().ToLower() != "approved")) ) ? true : false %>' Checked='<%# string.IsNullOrEmpty(Eval("HR_Status").ToString()) ? false : Eval("HR_Status") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <%--  <PagerStyle CssClass="pagination-ys" />--%>
                                </asp:GridView>
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


        function show_modal() {
            document.getElementById('<%=Btn_Modal_Popup.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Modal_Popup.ClientID %>').click();
        }




    </script>

</asp:Content>

