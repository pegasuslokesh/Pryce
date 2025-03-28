<%@ Page Title="" Trace="false" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AttendanceRegister.aspx.cs" Inherits="Attendance_Report_AttendanceRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="exp1" Namespace="ControlFreak" Assembly="ExportPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Attendance Register %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Attendance Register%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Report" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
        <ContentTemplate>
            <div class="content">
                <section class="content">

                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6" style="text-align: left;">

                                    <br />
                                </div>
                                <div class="col-md-6" style="text-align: right;">

                                    <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Download%>" CssClass="btn btn-primary" OnClick="btnDownload_Click" />


                                    <br style="text-align: center; vertical-align: middle;" />
                                </div>
                                <br />
                                <div class="col-md-12" runat="server" id="divexport" style="width: 100%;">
                                    <div class="col-md-6" runat="server" visible="false">
                                        <asp:Label ID="lbltitle" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Text="<%$ Resources:Attendance,Attendance Register %>"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <asp:Label ID="lblmonthname" runat="server" Font-Bold="True"
                                            Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="width: 100%; overflow: auto;" runat="server" id="mDiv">
                                        <%--  <div class="col-md-12">--%>



                                        <%--<asp:Table ID="Table1" runat="server" BorderStyle="Solid" CellPadding="1" CellSpacing="1"
                                                    GridLines="Both" Height="22px" Width="100%">
                                                </asp:Table>--%>
                                        <br />
                                        <%--</div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </section>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>




    <asp:UpdatePanel ID="Update_List" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlEmpAtt" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:RadioButton ID="rbtnGroupSal" Style="margin-left: 20px; margin-right: 20px;" OnCheckedChanged="EmpGroupSal_CheckedChanged"
                                            runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroupSal"
                                            AutoPostBack="true" />

                                        <asp:RadioButton ID="rbtnEmpSal" Style="margin-left: 20px; margin-right: 20px;" runat="server" AutoPostBack="true"
                                            Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroupSal" Font-Bold="true"
                                            OnCheckedChanged="EmpGroupSal_CheckedChanged" />
                                        <br />
                                    </div>
                                    <div id="Div_location" runat="server" class="col-md-6">
                                        <asp:Label ID="lblEmp" runat="server"></asp:Label>
                                        <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div id="Div_Department" runat="server" class="col-md-6">
                                        <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Select Department %>"></asp:Label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged"></asp:DropDownList>
                                            <div class="input-group-btn">
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Images/refresh.png"
                                                    Style="width: 37px;" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlGroupSal" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:ListBox ID="lbxGroupSal" runat="server" Height="300px" Style="width: 100%" SelectionMode="Multiple"
                                                AutoPostBack="true" OnSelectedIndexChanged="lbxGroupSal_SelectedIndexChanged"></asp:ListBox>
                                            <br />
                                        </div>
                                        <div class="col-md-6" style="overflow: auto; max-height: 300px;">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeSal" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                OnPageIndexChanging="gvEmployeeSal_PageIndexChanging" Width="100%"
                                                PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                            <asp:Label ID="Label52" runat="server" Visible="false"></asp:Label>
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlEmp" runat="server" DefaultButton="ImageButton8">
                    <div class="alert alert-info ">

                        <div class="row">
                            <div class="form-group">
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
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
                                <div class="col-lg-2">
                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="ImageButton8">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" />
                                    </asp:Panel>

                                </div>
                                <div class="col-lg-3">
                                    <asp:ImageButton ID="ImageButton8" runat="server" CausesValidation="False" Style="margin-top: -5px; width: 43px;"
                                        ImageUrl="~/Images/search.png" OnClick="btnarybind_Click1" ToolTip="<%$ Resources:Attendance,Search %>" />

                                    <asp:ImageButton ID="ImageButton9" runat="server" CausesValidation="False" Style="width: 37px;"
                                        ImageUrl="~/Images/refresh.png" OnClick="btnaryRefresh_Click1" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                    <asp:ImageButton ID="ImageButton10" runat="server" OnClick="ImgbtnSelectAll_Clickary" Style="width: 37px;"
                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                </div>
                                <div class="col-lg-2">
                                    <h5>
                                        <asp:Label ID="lblTotalRecord" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                    <div style="overflow: auto; max-height: 500px;">
                                        <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            OnPageIndexChanging="gvEmp_PageIndexChanging" Width="100%" DataKeyNames="Emp_Id"
                                            PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                            AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>'
                                                                            ImageUrl="~/Images/edit.png" Visible="true" OnCommand="btnEditary_Command"
                                                                            Width="16px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>
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
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
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
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogProcess">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="--Select--" Value="0" />
                                                <asp:ListItem Text="January" Value="1" />
                                                <asp:ListItem Text="February" Value="2" />
                                                <asp:ListItem Text="March" Value="3" />
                                                <asp:ListItem Text="April" Value="4" />
                                                <asp:ListItem Text="May" Value="5" />
                                                <asp:ListItem Text="June" Value="6" />
                                                <asp:ListItem Text="July" Value="7" />
                                                <asp:ListItem Text="August" Value="8" />
                                                <asp:ListItem Text="September" Value="9" />
                                                <asp:ListItem Text="October" Value="10" />
                                                <asp:ListItem Text="November" Value="11" />
                                                <asp:ListItem Text="December" Value="12" />
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtLastTime_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="txtYear" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                            </cc1:FilteredTextBoxExtender>
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                            <asp:Button ID="btnLogProcess" runat="server" OnClick="btnGenerate_Click" Text="<%$ Resources:Attendance,Next %>"
                                                CssClass="btn btn-primary" Visible="true" />

                                            <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                CssClass="btn btn-primary" Visible="true" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlReport" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6" style="text-align: left;">
                                        <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click"
                                            Text="<%$ Resources:Attendance,Back %>"></asp:LinkButton>
                                        <br />
                                    </div>
                                    <div class="col-md-6" style="text-align: right;">
                                        <%--   <asp:ImageButton ID="btnExportPdf" runat="server" CommandArgument="1" CommandName="OP"
                                            Height="23px" ImageUrl="~/Images/pdfIcon.jpg" OnCommand="btnExportPdf_Command" />--%>
                                        <asp:ImageButton ID="btnExportToExcel" runat="server" CommandArgument="2" CommandName="OP"
                                            Height="21px" ImageUrl="~/Images/excel-icon.gif" OnCommand="btnExportPdf_Command" />
                                        <br />
                                    </div>

                                    <br />



                                </div>
                            </div>
                        </div>
                    </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Report">
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
    </script>
</asp:Content>
