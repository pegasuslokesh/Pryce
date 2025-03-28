<%@ Control Language="C#"  AutoEventWireup="true" CodeFile="AddTask.ascx.cs"  Inherits="WebUserControl_AddTask" %>

<%--<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />

<div class="row">
    <div class="col-md-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <%-- <Triggers>
                <asp:PostBackTrigger ControlID="GvExistingData" />
            </Triggers>--%>
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div id="Div_Box_Add" runat="server" class="box box-info collapsed-box">
                                <div class="box-header with-border">
                                    <h3><asp:Literal ID="literal2" runat="server" Text="<%$ Resources:Attendance,All Associated Task %>" ></asp:Literal>:</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i id="Btn_Add_Div" runat="server" class="fa fa-plus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="form-group">

                                        <div class="flow">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvExistingData" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvExistingData_PageIndexChanging"
                                                AllowSorting="True" OnSorting="GvExistingData_Sorting" >
                                                
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">

                                                        <ItemTemplate>
                                                            <asp:Panel ID="editBtn" runat="server">
                                                                <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                    Visible="false" ImageUrl="~/Images/edit.png"  OnCommand="btnEdit_Command1" />
                                                               
                                                            </asp:Panel>

                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="3%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                        <ItemTemplate>
                                                            <asp:Panel ID="deleteBtn" runat="server">
                                                                <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Ref_Table_PK") %>'  OnCommand="IbtnDelete_Command"
                                                                    ImageUrl="~/Images/Erase.png" Width="16px" Visible="false" />
                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                    TargetControlID="IbtnDelete">
                                                                </cc1:ConfirmButtonExtender>
                                                            </asp:Panel>

                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="4%" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Assign To" SortExpression="Assign_To">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssign_To" runat="server" Text='<%#Eval("AssignedToName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstatus" runat="server" Text='<%#Eval("status") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Due Date" SortExpression="Due_Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDue_Date" runat="server" Text='<%#GetDate(Eval("Due_Date").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date" SortExpression="Start_Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStart_Date" runat="server" Text='<%#GetDate(Eval("Start_Date").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                                    </asp:TemplateField>

                                                  

                                                    <asp:TemplateField HeaderText="Title" SortExpression="Title">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Created By" SortExpression="Created_By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreated_By" runat="server" Text='<%#Eval("CreatedByName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left"  Width="10%" />
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


        
    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

        <asp:UpdatePanel ID="Update_New" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">

                        <div class="box box-primary">
                            <h3><asp:Literal ID="literal1" runat="server" Text="<%$ Resources:Attendance,Assign New Task %>" ></asp:Literal>:</h3>
                            <div class="box-body">
                                <div class="form-group">
                                    <asp:HiddenField ID="Edit_ID" runat="server" />
                                    <asp:HiddenField ID="tblName" runat="server" />
                                    <asp:HiddenField ID="PrimaryId" runat="server" />
                                    <asp:HiddenField ID="Hdn_Contact_Id" runat="server" />
                                   
                                    <div class="col-md-4">
                                        <asp:Label ID="Label7" Font-Bold="true" Text="<%$ Resources:Attendance,Employee%>" runat="server"></asp:Label>
                                        <a style="color: Red">*</a>
                                  <%--      <asp:DropDownList ID="Ddl_Employee_New" runat="server" CssClass="form-control">
                                        </asp:DropDownList>--%>
                                          <asp:TextBox ID="txtEmployeeList" runat="server" CssClass="form-control" BackColor="#eeeeee" OnTextChanged="txtEmployeeList_TextChanged" AutoPostBack="true" />
                                        <cc1:AutoCompleteExtender ID="txtTablename_AutoCompleteExtender" runat="server" 
                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                            ServicePath="" TargetControlID="txtEmployeeList" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <asp:HiddenField ID="hdnEmpId" runat="server" />
                                        <br />
                                    </div>
                                          <div class="col-md-4">
                                        <asp:Label ID="Label19" Font-Bold="true" Text="<%$ Resources:Attendance,Start Date%>" runat="server"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:TextBox ID="Txt_Start_Date" runat="server" CssClass="form-control" />
                                        <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="Txt_Start_Date">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label20" Font-Bold="true" Text="<%$ Resources:Attendance,Due Date%>" runat="server"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:TextBox ID="Txt_Due_Date" runat="server" CssClass="form-control" />
                                        <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="Txt_Due_Date">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>

                                 
                                  

                                    <div class="col-md-12">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="<%$ Resources:Attendance,Title%>" runat="server"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:TextBox ID="Txt_Title" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Label ID="Label15" Font-Bold="true" Text="<%$ Resources:Attendance,Description%>" runat="server"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="SaveBtn"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Editor_Description" ErrorMessage="<%$ Resources:Attendance,Enter Description%>"></asp:RequiredFieldValidator>--%>
                                        <cc2:Editor ID="Editor_Description" runat="server" CssClass="form-control" />
                                        <%--<textarea id="editor1" name="editor1" rows="10" cols="80"></textarea>--%>
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center;">
                                    
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                           
                                                      <asp:Button ID="Btn_SaveT" runat="server" Text="<%$ Resources:Attendance,Save %>"  UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait..';" OnClick="Btn_SaveT_Click" class="btn btn-success" />
                                                    <asp:Button ID="Btn_CancelT" runat="server" Text="<%$ Resources:Attendance,Cancel %>" OnClick="Btn_CancelT_Click" class="btn btn-danger" CausesValidation="False" />
                                                    <br />
                                                    <asp:Label ID="EditCheck" runat="server" Visible="false" Text="0"></asp:Label>
                                                      <asp:Label ID="Trans_ID" runat="server" Visible="false"></asp:Label>                                    
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


                
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

                       
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel11">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    </div>
</div>

<%--<asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="UpdatePanel11">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

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
    function Div_AddProduct_Close() {
        $("#Btn_Add_Div").removeClass("fa fa-plus");
        $("#Div_Box_Add").removeClass("box box-primary collapsed-box");

        $("#Btn_Add_Div").addClass("fa fa-minus");
        $("#Div_Box_Add").addClass("box box-primary");

      
    }
    function Div_AddProduct_Open() {
       
        $("#Btn_Add_Div").removeClass("fa fa-minus");
        $("#Div_Box_Add").removeClass("box box-primary collapsed-box");

        $("#Btn_Add_Div").addClass("fa fa-plus");
        $("#Div_Box_Add").addClass("box box-primary");


    }
    function BtnDisable() {
        document.getElementById('<%=Btn_SaveT.ClientID%>').disabled = false;
    }

     function BtnEnable() {
        document.getElementById('<%=Btn_SaveT.ClientID%>').disabled = false;
    }
    
    function calendarShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 100004;
    }




</script>


<script>
    

function myTimer() {
    document.getElementById('<%=Btn_SaveT.ClientID%>').disabled = true;
    var myVar = setInterval(function () { myStopFunction() }, 5000);
}

function myStopFunction() {
     document.getElementById('<%=Btn_SaveT.ClientID%>').disabled = false;
    clearInterval(myVar);
}
</script>
