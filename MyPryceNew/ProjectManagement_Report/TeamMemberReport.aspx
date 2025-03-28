<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="TeamMemberReport.aspx.cs" Inherits="ProjectManagement_Report_TeamMemberReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_icon.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Team Member Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Team Member Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div class="row" id="pnlProjectfilter" runat="server">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Project Name %>"></asp:Label>



                                    <asp:DropDownList ID="ddlprojectname" runat="server" CssClass="form-control"></asp:DropDownList>



                                    <%--  <dx:ASPxComboBox ID="ddlprojectname" runat="server"  CssClass="form-control" DropDownWidth="550" 
        DropDownStyle="DropDownList"  ValueField="Project_Id" 
        ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains" 
        CallbackPageSize="30">
        <Columns>
            <dx:ListBoxColumn FieldName="Project_Name" />
           
        </Columns>
    </dx:ASPxComboBox>--%>
                                    <br />
                                </div>
                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjecttask" runat="server" AutoGenerateColumns="False" Width="100%"
                                        AllowPaging="True" OnPageIndexChanging="GvrProjecttask_PageIndexChanging" AllowSorting="True"
                                         OnSorting="GvrProjecttask_Sorting">
                                        
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

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>" SortExpression="Emp_Name">
                                                <ItemTemplate>

                                                    <asp:Label ID="lblprojectName" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>

                                                    <asp:Label ID="lblProjectId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign To %>" SortExpression="Emp_Name">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="HiddeniD" runat="server" />
                                                    <asp:Label ID="lblprojectIdList22" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="Emp_Name">
                                                <ItemTemplate>

                                                    <asp:Label ID="lblprojectIdList12" runat="server" Text='<%# GetAssignBy(Eval("CreatedBy").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Subject %>" SortExpression="Subject">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList1" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="Assign_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList2" runat="server" Text='<%# Formatdate(Eval("Assign_Date")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Time %>" SortExpression="Assign_Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList3" runat="server" Text='<%# FormatTime(Eval("Assign_Time")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Close Date %>" SortExpression="Emp_Close_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList121" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Close Time %>" SortExpression="Emp_Close_Time">
                                                <ItemTemplate>
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

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mail Staus %>" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList42" runat="server" Text='<%# Eval("Field2") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>


                                        </Columns>
                                        
                                        <PagerStyle CssClass="pagination-ys" />
                                        
                                    </asp:GridView>
                                    <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" OnClick="btnsave_Click" Visible="false"
                                        Text="<%$ Resources:Attendance,Next%>" />&nbsp;&nbsp; &nbsp;&nbsp;
                                                    <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click"
                                                        Text="<%$ Resources:Attendance,Reset%>" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="pnlReport" runat="server">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click" Text="<%$ Resources:Attendance,Back %>"></asp:LinkButton>
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
    </script>
</asp:Content>
