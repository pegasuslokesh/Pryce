<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="TimeSheetReport.aspx.cs" Inherits="Attendance_Report_TimeSheetReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="exp1" Namespace="ControlFreak" Assembly="ExportPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Time Sheet Report %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Time Sheet Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

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
                                                <asp:Panel ID="pnlSearchdpl" runat="server">
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Images/refresh.png"
                                                        Style="width: 37px;" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>                                                    
                                                </asp:Panel>
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
                                                OnPageIndexChanging="gvEmployeeSal_PageIndexChanging"  Width="100%"
                                                PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                            <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                        SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  />
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
                                            OnPageIndexChanging="gvEmp_PageIndexChanging"  Width="100%" DataKeyNames="Emp_Id"
                                            PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged"/>
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
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                    SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                            </Columns>
                                            
                                            
                                            <PagerStyle CssClass="pagination-ys" />
                                            
                                        </asp:GridView>
                                        <asp:HiddenField ID="hdnFldSelectedValues" runat="server" />
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
                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                            <asp:TextBox  ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>                                   
                                             <cc1:CalendarExtender ID="txtFrom_CalendarExtender"    runat="server" Enabled="True" TargetControlID="txtFromDate"></cc1:CalendarExtender>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>                                   
                                            <cc1:CalendarExtender ID="CalendarExtender1"    runat="server" Enabled="True" TargetControlID="txtToDate"></cc1:CalendarExtender>
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                         

                                            <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                CssClass="btn btn-primary" Visible="true" />

                                               <asp:Button ID="btnLogProcess" runat="server" OnClick="btnGenerate_Click" Text="<%$ Resources:Attendance,Next %>"
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
    <script type="text/javascript">
        //Reference of the GridView. 
        var TargetBaseControl = null;
        //Total no of checkboxes in a particular column inside the GridView.
        var CheckBoxes;
        //Total no of checked checkboxes in a particular column inside the GridView.
        var CheckedCheckBoxes;
        //Array of selected item's Ids.
        var SelectedItems;
        //Hidden field that wil contain string of selected item's Ids separated by '|'.
        var SelectedValues;
        
        window.onload = function()
        {
            //Get reference of the GridView. 
            try
            {
                TargetBaseControl = document.getElementById('<%= this.gvEmployee.ClientID %>');
            }
            catch(err)
            {
                TargetBaseControl = null;
            }
            
            //Get total no of checkboxes in a particular column inside the GridView.
            try
            {
                CheckBoxes = parseInt('<%= this.gvEmployee.Rows.Count %>');
            }
            catch(err)
            {
                CheckBoxes = 0;
            }
            
            //Get total no of checked checkboxes in a particular column inside the GridView.
            CheckedCheckBoxes = 0;
            
            //Get hidden field that wil contain string of selected item's Ids separated by '|'.
            SelectedValues = document.getElementById('<%= this.hdnFldSelectedValues.ClientID %>');
            
            //Get an array of selected item's Ids.
            if(SelectedValues.value == '')
                SelectedItems = new Array();
            else
                SelectedItems = SelectedValues.value.split(',');
                
            //Restore selected CheckBoxes' states.
            if(TargetBaseControl != null)
                RestoreState();
        }
        
        function HeaderClick(CheckBox)
        {            
            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName('input');
            
            //Checked/Unchecked all the checkBoxes in side the GridView & modify selected items array.
            for(var n = 0; n < Inputs.length; ++n)
                if(Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf('chkBxSelect',0) >= 0)
                {
                    Inputs[n].checked = CheckBox.checked;
                    if(CheckBox.checked)
                        SelectedItems.push(document.getElementById(Inputs[n].id.replace('chkBxSelect','hdnFldId')).value);
                    else
                        DeleteItem(document.getElementById(Inputs[n].id.replace('chkBxSelect','hdnFldId')).value);
                }
                        
            //Update Selected Values. 
            SelectedValues.value = SelectedItems.join(',');
                        
            //Reset Counter
            CheckedCheckBoxes = CheckBox.checked ? CheckBoxes : 0;
        }
        
        function ChildClick(CheckBox, HCheckBox, Id)
        {            
            //Modifiy Counter;            
            if(CheckBox.checked && CheckedCheckBoxes < CheckBoxes)
                CheckedCheckBoxes++;
            else if(CheckedCheckBoxes > 0) 
                CheckedCheckBoxes--;
                
            //Change state of the header CheckBox.
            if(CheckedCheckBoxes < CheckBoxes)
                HCheckBox.checked = false;
            else if(CheckedCheckBoxes == CheckBoxes)
                HCheckBox.checked = true;
                
            //Modify selected items array.
            if(CheckBox.checked)
                SelectedItems.push(Id);
            else
                DeleteItem(Id);
                
            //Update Selected Values. 
            SelectedValues.value = SelectedItems.join(',');
        }   
        
        function RestoreState()
        {
            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName('input');
            
            //Header CheckBox
            var HCheckBox = null;
            
            //Restore previous state of the all checkBoxes in side the GridView.
            for(var n = 0; n < Inputs.length; ++n)
                if(Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf('chkBxSelect',0) >= 0)
                    if(IsItemExists(document.getElementById(Inputs[n].id.replace('chkBxSelect','hdnFldId')).value) > -1)
                    {
                        Inputs[n].checked = true;          
                        CheckedCheckBoxes++;      
                    }
                    else
                        Inputs[n].checked = false;   
                else if(Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf('chkBxHeader',0) >= 0)	
                    HCheckBox = Inputs[n];  
                    
            //Change state of the header CheckBox.
            if(CheckedCheckBoxes < CheckBoxes)
                HCheckBox.checked = false;
            else if(CheckedCheckBoxes == CheckBoxes)
                HCheckBox.checked = true;  
        }
        
        function DeleteItem(Text)
	    {
	        var n = IsItemExists(Text);
	        if( n > -1)
	            SelectedItems.splice(n,1);
	    }
        
        function IsItemExists(Text)
	    {
	        for(var n = 0; n < SelectedItems.length; ++n)
	            if(SelectedItems[n] == Text)
	                return n;
    	            
	        return -1;  
	    }     
    </script>
</asp:Content>
