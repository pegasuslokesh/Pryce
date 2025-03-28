<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_Laborlaw.aspx.cs" Inherits="HR_HR_Laborlaw" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-gavel"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,labour law%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="labour law"></asp:Label></a></li>


    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List" runat="server" Style="display: none;" Text="List" OnClick="btnList_Click" />
            <asp:Button ID="Btn_New" runat="server" Style="display: none;" Text="New" OnClick="btnNew_Click" />
            <asp:Button ID="Btn_Bin" runat="server" Style="display: none;" Text="Bin" OnClick="btnBin_Click" />
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
                    <li id="Li_Bin"><a onclick="Li_Bin_Click()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					    <asp:Label ID="lblTotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Labour Law Name%>"
                                                            Value="Laborlaw_Name"></asp:ListItem>

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
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMaster" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvLeaveMaster_PageIndexChanging" OnSorting="gvLeaveMaster_OnSorting">
                                                        <Columns>


                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TRans_Id") %>'
                                                                                    ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="btnEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                    TargetControlID="IbtnDelete">
                                                                                </cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Labour Law Name%>" SortExpression="Laborlaw_Name">
                                                                <ItemTemplate>
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





                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLabourLawname" runat="server" Text="<%$ Resources:Attendance,Labour Law Name%>"></asp:Label>


                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator11" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtLabourLawname" ErrorMessage="Enter Law Name"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtLabourLawname" runat="server"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblfinancestartmonth" runat="server" Text="<%$ Resources:Attendance,Financial Start Month%>"></asp:Label>


                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator12" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
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
                                                            ID="RequiredFieldValidator13" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtDescription" ErrorMessage="Enter Description"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">

                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Work day Minute%>"></asp:Label>


                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator15" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtWorkdayMinute" ErrorMessage="Enter Work day minute"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtWorkdayMinute" runat="server"
                                                            CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                            TargetControlID="txtWorkdayMinute" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>

                                                        <br />
                                                    </div>


                                                    <div class="col-md-6" runat="server" id="Div_Indemnity" visible="false">

                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Indemnity Plan%>"></asp:Label>

                                                        <asp:DropDownList ID="ddlPlanName" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />


                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Yearly Half Day%>"></asp:Label>


                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                            ID="RequiredFieldValidator16" ValidationGroup="a" Display="Dynamic" SetFocusOnError="true"
                                                            ControlToValidate="txtyearlyHalfDay" ErrorMessage="Enter Yearly Half day"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtyearlyHalfDay" runat="server"
                                                            CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
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
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlLeaveType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Leave Type%>" />
                                                            <asp:DropDownList ID="ddlLeaveType" runat="server" class="form-control">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTotalLeave" runat="server" Text="<%$ Resources:Attendance,Total Leave Day %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator17" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalLeave" ErrorMessage="<%$ Resources:Attendance,Enter Total Leave Day %>" />
                                                            <asp:TextBox ID="txtTotalLeave" class="form-control" runat="server" MaxLength="3"
                                                                onkeypress="return isNumber(event)"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                TargetControlID="txtTotalLeave" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPaidLeave" runat="server" Text="<%$ Resources:Attendance,Paid Leave Day %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator18" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaidLeave" ErrorMessage="<%$ Resources:Attendance,Enter Paid Leave Day %>" />

                                                            <asp:TextBox ID="txtPaidLeave" runat="server" class="form-control" MaxLength="3" onkeypress="return isNumber(event)"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtPaidLeave" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSchType" runat="server" Text="<%$ Resources:Attendance,Schedule Type %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator19" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSchType" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Schedule Type %>" />

                                                            <asp:DropDownList ID="ddlSchType" class="form-control" runat="server">
                                                                <%--<asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>--%>
                                                                <asp:ListItem Value="Yearly" Selected="True">Yearly</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Gender %>"></asp:Label><a style="color: Red">*</a>
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


                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="a" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />

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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                               

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Labour Law Name%>"
                                                            Value="Laborlaw_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinOption" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server"  CausesValidation="False" 
                                                        OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" 
                                                        OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore"  CausesValidation="False" runat="server"
                                                          OnClick="imgBtnRestore_Click"
                                                        ToolTip="<%$ Resources:Attendance, Active %>" > <span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 35px;" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="box box-warning box-solid"  <%= gvLeaveMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                   
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Trans_Id" Width="100%" AllowPaging="True" OnPageIndexChanging="gvLeaveMasterBin_PageIndexChanging"
                                                        OnSorting="gvLeaveMasterBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Labour Law Name%>" SortExpression="Laborlaw_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Laborlaw_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Visible="false"></asp:Panel>
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

        function Li_List_Click() {
            document.getElementById("<%=Btn_List.ClientID %>").click();
        }

        function Li_New_Click() {
            document.getElementById("<%=Btn_New.ClientID %>").click();
        }

        function Li_Bin_Click() {
            document.getElementById("<%=Btn_Bin.ClientID %>").click();
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

