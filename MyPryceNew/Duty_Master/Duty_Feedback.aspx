<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Duty_Feedback.aspx.cs" Inherits="Duty_Master_Duty_Feedback" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .ratingEmpty {
            background-image: url(../Images/ratingStarEmpty.gif);
            width: 18px;
            height: 18px;
        }

        .ratingFilled {
            background-image: url(../Images/ratingStarFilled.gif);
            width: 18px;
            height: 18px;
        }

        .ratingSaved {
            background-image: url(../Images/ratingStarSaved.gif);
            width: 18px;
            height: 18px;
        }

        .Selected {
            color: orange;
        }
    </style>

    <style>
        .GridStyle {
            border: 1px solid rgb(217, 231, 255);
            background-color: White;
            font-family: arial;
            font-size: 12px;
            border-collapse: collapse;
            margin-bottom: 0px;
        }

            .GridStyle tr {
                border: 1px solid rgb(217, 231, 255);
                color: Black;
                height: 25px;
            }
            /* Your grid header column style */
            .GridStyle th {
                background-color: rgb(217, 231, 255);
                border: none;
                text-align: left;
                font-weight: bold;
                font-size: 15px;
                padding: 4px;
                color: Black;
                text-align: center;
            }
            /* Your grid header link style */
            .GridStyle tr th a, .GridStyle tr th a:visited {
                color: Black;
            }

            .GridStyle tr th, .GridStyle tr td table tr td {
                border: none;
            }

            .GridStyle td {
                border-bottom: 1px solid rgb(217, 231, 255);
                padding: 2px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
       <i class="far fa-comments"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Duty Feedback %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,My Duties%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,My Duties%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="Hdn_Contact_Id" runat="server" />
            <asp:HiddenField ID="Emp_List_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Duty_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Select_Emp_ID" runat="server" />
            <asp:HiddenField ID="Edit_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Conversation_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Feedback_Trans" runat="server" />
            <asp:HiddenField ID="Hdn_Status" runat="server" />
            <asp:HiddenField ID="Hdn_Emp_Status" runat="server" />
            <asp:HiddenField ID="Hdn_TL_Status" runat="server" />
            <asp:HiddenField ID="Hdn_Trans_Date" runat="server" />

            <asp:HiddenField ID="Hdn_Effect_Date_From" runat="server" />
            <asp:HiddenField ID="Hdn_Effect_Date_To" runat="server" />

            <asp:Button ID="Btn_Delete_Feedback" Style="display: none;" runat="server" OnClick="Btn_Delete_Feedback_Click" Text="Delete Feedback" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Conversation" Text="View Modal" />
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

    <asp:UpdatePanel ID="Update_List" runat="server">
        <ContentTemplate>
          

            <div class="row">
		<div class="col-md-12">
			<div id="Div1" runat="server" class="box box-info collapsed-box">
				<div class="box-header with-border">
					<h3 class="box-title">
						<asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
					&nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Lbl_TotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

					<div class="box-tools pull-right">
						<button type="button" class="btn btn-box-tool" data-widget="collapse">
							<i id="I1" runat="server" class="fa fa-plus"></i>
						</button>
					</div>
				</div>
				<div class="box-body">
                    <div class="col-lg-3">
                                <asp:Label ID="Label2" Font-Bold="true" Text="<%$ Resources:Attendance,Start Date%>" runat="server"></asp:Label>
                                <asp:TextBox ID="Txt_Start_Date_List" runat="server" CssClass="form-control" />
                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="Txt_Start_Date_List">
                                </cc1:CalendarExtender>
                            </div>
                            <div class="col-lg-3">
                                <asp:Label ID="Label3" Font-Bold="true" Text="<%$ Resources:Attendance,Due Date%>" runat="server"></asp:Label>
                                <asp:TextBox ID="Txt_Due_Date_List" runat="server" CssClass="form-control" />
                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="Txt_Due_Date_List">
                                </cc1:CalendarExtender>
                            </div>
                            <div class="col-lg-6">
                                <asp:Label ID="Label4" Font-Bold="true" Text="<%$ Resources:Attendance,Employee%>" runat="server"></asp:Label>
                                <a style="color: Red">*</a>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Search" Display="Dynamic"
                                    SetFocusOnError="true" ControlToValidate="Ddl_Employee_List" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Employee %>" />
                                <div class="input-group" style="width: 100%;">
                                    <asp:DropDownList ID="Ddl_Employee_List" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="Img_Search" runat="server"  ValidationGroup="Search" OnClick="Img_Search_Click"  ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px; margin-left:10px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    </div>
                                </div>
                            </div>

                          <div class="col-lg-12">
                              <br />
                              </div>
                     <div class="col-lg-2">
                            <asp:DropDownList ID="ddlFieldName" runat="server" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true" class="form-control">
                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Duty%>" Value="Title" />
                                <%--<asp:ListItem Text="<%$ Resources:Attendance, Default Count%>" Value="Default_Count" />--%>
                                <asp:ListItem Text="<%$ Resources:Attendance, Duty Cycle%>" Value="Duty_Cycle" />
                                <asp:ListItem Text="<%$ Resources:Attendance, Report To%>" Value="Report_To_Name" />
                                <asp:ListItem Text="<%$ Resources:Attendance, Emp Status%>" Value="Emp_Status" />
                                <asp:ListItem Text="<%$ Resources:Attendance, TL Status%>" Value="TL_Status" />
                                <asp:ListItem Text="<%$ Resources:Attendance, TL Status By%>" Value="TL_Status_Modified_By" />
                                
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2">
                            <asp:DropDownList ID="ddlOption" runat="server" class="form-control">
                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-5">
                            <asp:TextBox ID="txtValue" placeholder="Search from Content" onkeypress="return Accept_Enter_Key_List(this);" runat="server" class="form-control"></asp:TextBox>
                            <asp:DropDownList ID="Ddl_Duty_Cycle" runat="server" Visible="false" class="form-control">
                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,--Select-- %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Daily%>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Weekly%>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Biweekly%>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Monthly%>" Value="4"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Quarterly%>" Value="5"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Half Yearly%>" Value="6"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Attendance,Yearly%>" Value="7"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3" >
                            <asp:LinkButton ID="btnbind"  OnClick="btnbind_Click" runat="server" CausesValidation="False" 
                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="Btn_Bind_Duty_Cycle"  Visible="false" OnClick="Btn_Bind_Duty_Cycle_Click" runat="server" CausesValidation="False" 
                                ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="btnRefresh" runat="server"  CausesValidation="False" OnClick="btnRefresh_Click"
                                 ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                        </div>

				</div>
			</div>
		</div>
	</div>


            
            <div class="box box-primary box-solid">
                <div class="box-header with-border">
                    <h3 class="box-title"></h3>
                    <div class="col-md-2">
                        <h5>
                            <asp:Label ID="Lbl_Duty_Status" Visible="false" Font-Bold="true" Text="<%$ Resources:Attendance,Duty Status %>" runat="server"></asp:Label>
                        </h5>
                    </div>
                    <div class="col-md-2" style="margin-left: -87px;">
                        <asp:DropDownList ID="Ddl_Duty_Status" Visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Duty_Status_SelectedIndexChanged" class="form-control">
                            <asp:ListItem Text="<%$ Resources:Attendance,Completed%>" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Pending%>" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            <div class="box-body">
                <div class="row">
                    <div class="col-md-12">
                        <div style="overflow: auto; max-height: 500px;">
                            <asp:GridView ID="Gv_Feedback_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Trans_ID" 
                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" CssClass="GridStyle" OnPageIndexChanging="Gv_Feedback_List_PageIndexChanging" OnSorting="Gv_Feedback_List_Sorting">
                                <AlternatingRowStyle CssClass="GridStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Feedback %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Conversation" Style="width: 30px;" OnCommand="Btn_Conversation_Command" CommandArgument='<%# Eval("Trans_ID") %>' runat="server" ImageUrl="~/Images/Feedback1.png" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Conversation%>" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty%>" SortExpression="Title">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_ID_List" runat="server" Visible="false" Text='<%# Eval("Trans_ID")%>'></asp:Label>
                                            <asp:Label ID="Lbl_Duty_ID_List" runat="server" Visible="false" Text='<%# Eval("Duty_ID")%>'></asp:Label>
                                            <asp:Label ID="Lbl_Task_List" runat="server" ToolTip='<%# Eval("Description")%>' Text='<%# Eval("Title")%>'></asp:Label>
                                            <asp:Label ID="Lbl_Task_Description_List" runat="server" Visible="false" Text='<%# Eval("Description")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Cycle%>" SortExpression="Duty_Cycle">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Duty_Cycle_List" runat="server" Text='<%#GetCycle(Eval("Duty_Cycle"))%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,WEF Date%>" SortExpression="WEF_Date">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_WEF_Date_List" runat="server" Text='<%# Convert.ToDateTime(Eval("WEF_Date")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Effect Date From%>" SortExpression="Effect_Date_From">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Effect_Date_From_List" runat="server" Text='<%# Convert.ToDateTime(Eval("Effect_Date_From")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Effect Date To%>" SortExpression="Effect_Date_To">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Effect_Date_To_List" runat="server" Text='<%# Convert.ToDateTime(Eval("Effect_Date_To")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Report To%>" SortExpression="Report_To">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Report_To_List" Visible="false" runat="server" Text='<%# Eval("Report_To")%>'></asp:Label>
                                            <asp:Label ID="Lbl_Report_To_Name_List" runat="server" Text='<%# Eval("Report_To_Name")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Default Count%>" SortExpression="Default_Count">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Default_Count_List" runat="server" Text='<%# Eval("Default_Count")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Emp Status%>" SortExpression="Emp_Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Emp_Status_List" runat="server" Text='<%# Eval("Emp_Status")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,TL Status%>" SortExpression="TL_Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_TL_Status_List" runat="server" Text='<%# Eval("TL_Status")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,TL Status By%>" SortExpression="TL_Status_Modified_By">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_TL_Status_By_List" runat="server" Text='<%# Eval("TL_Status_Modified_By")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By%>" SortExpression="createdByName">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_createdByName" runat="server" Text='<%# Eval("createdByName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Trans Date%>" SortExpression="Trans_Date">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Trans_Date_List" runat="server" Text='<%# Eval("Trans_Date")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="grid" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="Invgridheader" />
                                <PagerStyle CssClass="pagination-ys" />
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>
            <!-- /.box-body -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="Modal_Conversation" tabindex="-1" role="dialog" aria-labelledby="Modal_ConversationLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_ConversationLabel">Conversation</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <!-- Chat box -->
                            <div class="box box-success">
                                <div class="box-header">
                                    <i class="fa fa-comments-o"></i>
                                    <h3 class="box-title">
                                        <asp:Label ID="Lbl_C_Task" Font-Bold="true" runat="server"></asp:Label></h3>
                                    <div class="box-tools pull-right" data-toggle="tooltip">
                                        <div class="btn-group" data-toggle="btn-toggle">
                                            <small class="text-muted pull-right"><i class="fa fa-clock-o"></i>&nbsp;
                                                    <asp:Label ID="Lbl_C_Date" runat="server"></asp:Label></small>&nbsp;
                                        </div>
                                    </div>
                                    <p>
                                        <asp:Literal ID="Ltr_C_Description" runat="server"></asp:Literal>
                                    </p>
                                    <hr />
                                </div>
                                <div class="box-body chat" id="chat-box">
                                    <div id="Div_Conversation" class='attachment' style="overflow: auto; max-height: 200px;">
                                        <asp:Literal ID="Ltr_Conversion" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <cc1:Rating ID="ratingControl" OnChanged="ratingControl_Changed" Visible="true" AutoPostBack="true" runat="server" StarCssClass="ratingEmpty" WaitingStarCssClass="ratingSaved" EmptyStarCssClass="ratingEmpty" FilledStarCssClass="ratingFilled"></cc1:Rating>
                                    <br />
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Send" Display="Dynamic"
                                        SetFocusOnError="true" ControlToValidate="DDL_Status_Conversation" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Status %>" />
                                    <asp:DropDownList ID="DDL_Status_Conversation" Visible="true" runat="server" class="form-control">
                                        <asp:ListItem Selected="True" Text="Status" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Not Started" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Deferred" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="In-Progress" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Waiting For Input" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Send"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_FeedBack" ErrorMessage="<%$ Resources:Attendance,Enter Feedback%>"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="Txt_FeedBack" runat="server" TextMode="MultiLine" MaxLength="1000" onKeyUp="textareaLengthCheck()" onkeypress="return Shift_Enter(this);" Style="resize: none; width: 100%; height: 70px;" Placeholder="Enter Feedback" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="Feedback_Count" runat="server" class='text-muted pull-right' ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="Btn_Send_Feedback" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Btn_Send_Feedback_Click" ValidationGroup="Send" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">

    <script type="text/javascript">
        function resetPosition(object, args) {
            var tb = object._element;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
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
        function View_Conversation() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Edit_Feedback(Trans_ID, User_ID, Emp_ID, RefTblPk, Feedback) {
            var regex = /<br\s*[\/]?>/gi;
            var feed = Feedback.replace(regex, "\n");
            document.getElementById('<%= Hdn_Feedback_Trans.ClientID %>').value = Trans_ID;
            document.getElementById('<%= Hdn_Status.ClientID %>').value = 'Edit';
            document.getElementById('<%= Txt_FeedBack.ClientID %>').value = feed;
            document.getElementById('<%= Txt_FeedBack.ClientID %>').focus();
        }

        function Delete_Feedback(Trans_ID, User_ID, Emp_ID, RefTblPk) {
            if (confirm("Are you sure you want to delete the record?") == true) {
                document.getElementById('<%= Hdn_Feedback_Trans.ClientID %>').value = Trans_ID;
                document.getElementById('<%= Hdn_Status.ClientID %>').value = 'Delete';
                document.getElementById('<%= Btn_Delete_Feedback.ClientID %>').click();
            }
            else
                return false;
        }

        function Accept_Enter_Key_List(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered == 13)) {
                document.getElementById('<%= btnbind.ClientID %>').click();
                return false;
            }
        }

        function textareaLengthCheck() {
            document.getElementById('<%=Txt_FeedBack.ClientID%>').maxLength = 1000;
            var length = document.getElementById('<%=Txt_FeedBack.ClientID%>').value.length;
            var charactersLeft = 1000 - length;
            var count = document.getElementById('<%=Feedback_Count.ClientID%>');
            count.innerHTML = "Characters left: " + charactersLeft;
        }

        $('#Modal_Conversation').on('shown.bs.modal', function () {
            $('#Div_Conversation').animate({
                scrollTop: $('#Div_Conversation')[0].scrollHeight
            }, 5);
            $("[data-modalfocus]", this).focus();
        });

        function On_Load_Modal() {
            $('#Div_Conversation').animate({
                scrollTop: $('#Div_Conversation')[0].scrollHeight
            }, 5);
        }

        function Shift_Enter(elementRef) {
            if (event.keyCode == 13 && event.shiftKey == true) {
                this.value = content.substring(0, caret - 1) + "\n" + content.substring(caret, content.length);
                event.stopPropagation();
                return false;
            }
            else if (event.keyCode == 13 && event.shiftKey == false) {
                document.getElementById('<%= Btn_Send_Feedback.ClientID %>').click();
                    return false;
                }
        }
    </script>
</asp:Content>

