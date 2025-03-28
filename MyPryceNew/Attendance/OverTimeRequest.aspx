<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="OverTimeRequest.aspx.cs" Inherits="Attendance_OverTimeRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-plus"></i>&nbsp;&nbsp;
            <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Overtime Request%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Overtime Request%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="box box-warning box-solid" <%= gvOvertimeStatus.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOvertimeStatus" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        OnPageIndexChanging="gvOvertimeStatus_PageIndexChanging" AllowPaging="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-edit"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Overtime Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,OverTime Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOverTimeDate" runat="server" Text='<%# GetDate(Eval("Overtime_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromTime" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToTime" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Duration (In Hours) %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTimeDuration" runat="server" Text='<%# Eval("Time_Duration") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# GetOverTimeStatus(Eval("Trans_Id"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnTransid" runat="server" />
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
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                            <asp:HiddenField ID="hdnEdit" runat="server" />
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,OverTime Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtOverTimeDate" runat="server" CssClass="form-control" />
                                                            <asp:HiddenField ID="hdnEditDate" runat="server" Value="0" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtOverTimeDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,From Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtFromTime" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtTimeDuration_OnTextChanged" />
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtFromTime">
                                                            </cc1:MaskedEditExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,To Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtToTime" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtTimeDuration_OnTextChanged" />
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtToTime">
                                                            </cc1:MaskedEditExtender>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblTimeDuration" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Time Duration (In Hours) %>"></asp:Label>
                                                            <asp:TextBox ID="txtTimeDuration" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtTimeDuration_OnTextChanged" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtTimeDuration" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server"
                                                                CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                        <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Attendance,Apply %>"
                                                            Visible="false" CssClass="btn btn-success" OnClick="btnApply_Click" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            Visible="true" CssClass="btn btn-primary" OnClick="btnReset_Click" />

                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="overflow: auto">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOverTimeSumary_Pending" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            OnPageIndexChanging="gvOverTimeSumary_Pending_PageIndexChanging" AllowPaging="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,OverTime Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,OverTime Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOverTimeDate" runat="server" Text='<%# GetDate(Eval("Overtime_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromTime" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblToTime" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Duration (In Hours) %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTimeDuration" runat="server" Text='<%# Eval("Time_Duration") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# GetOverTimeStatus(Eval("Trans_Id"))%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>

                                                    <div class="col-md-12" style="overflow: auto">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOverTimeSumary_Approved" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            OnPageIndexChanging="gvOverTimeSumary_Approved_PageIndexChanging" AllowPaging="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,OverTime Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,OverTime Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOverTimeDate" runat="server" Text='<%# GetDate(Eval("Overtime_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromTime" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblToTime" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Duration (In Hours) %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTimeDuration" runat="server" Text='<%# Eval("Time_Duration") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# GetOverTimeStatus(Eval("Trans_Id"))%>'></asp:Label>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">

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

    </script>
</asp:Content>



