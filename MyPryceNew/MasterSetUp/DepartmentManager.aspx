<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" CodeFile="DepartmentManager.aspx.cs" Inherits="MasterSetUp_DepartmentManager" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-tie"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Department Manager Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Department Manager Setup%>"></asp:Label></li>
    </ol>

    <asp:HiddenField runat="server" ID="hdnCanEdit" />
    <asp:HiddenField runat="server" ID="hdnCanDelete" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="GvDept" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="box box-danger">
                            <div class="box-header with-border">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                            <asp:DropDownList CssClass="form-control" runat="server" ID="ddlLocList"></asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblDeptCode" runat="server" Text="<%$ Resources:Attendance,Department Code %>"></asp:Label>

                                            <asp:TextBox ID="txtDepCode" Enabled="false" runat="server" CssClass="form-control" />
                                            <br />
                                            <asp:Label ID="lblManagerName" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                            <asp:TextBox ID="txtManagerName" runat="server" AutoPostBack="true" OnTextChanged="txtManager_TextChanged"
                                                CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtManagerName"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>

                                            <br />
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Phone No. %>" />
                                            <div style="width: 100%;" class="input-group">
                                                <asp:DropDownList ID="ddlCountryCode" Style="width: 30%;" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtPhoneNo" Style="width: 70%;" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                    TargetControlID="txtPhoneNo" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Department Name %>"></asp:Label>
                                            <asp:TextBox ID="txtDepName" Enabled="false" runat="server" CssClass="form-control" />
                                            <br />
                                            <asp:Label ID="lblEmailId" runat="server" Text="<%$ Resources:Attendance,Email Id %>" />
                                            <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control"></asp:TextBox>
                                            <br />
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Fax No. %>" />
                                            <div style="width: 100%;" class="input-group">
                                                <asp:DropDownList ID="ddlFaxCountryCode" Style="width: 30%;" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtFaxNo" runat="server" Style="width: 70%;" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtFaxNo" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                            <br />
                                        </div>

                                        <div class="col-md-6 ">

                                            <asp:Label ID="lblHR1" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                            <asp:TextBox ID="txtHR1" runat="server" AutoPostBack="true" OnTextChanged="txtHr1_TextChanged"
                                                CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHR1"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>



                                            <asp:Label ID="lblHR2" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                            <asp:TextBox ID="txtHR2" runat="server" AutoPostBack="true" OnTextChanged="txtHr2_TextChanged"
                                                CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHR2"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>

                                        </div>

                                        <div class="col-md-6 ">

                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                            <asp:TextBox ID="txtHR3" runat="server" AutoPostBack="true" OnTextChanged="txtHr3_TextChanged"
                                                CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHR3"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>



                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                            <asp:TextBox ID="txtHR4" runat="server" AutoPostBack="true" OnTextChanged="txtHr4_TextChanged"
                                                CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHR4"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>

                                        </div>

                                        <div class="col-md-6 ">

                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                            <asp:TextBox ID="txtHR5" runat="server" AutoPostBack="true" OnTextChanged="txtHr5_TextChanged"
                                                CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHR5"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>



                                        </div>


                                        <div style="text-align: center;">
                                            <asp:Button ID="btnCSave" ValidationGroup="Edit_Department" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                CssClass="btn btn-success" OnClick="btnCSave_Click" Visible="false" />
                                            <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                            <asp:HiddenField ID="hdnEmpId" runat="server" />
                                            <asp:HiddenField ID="hdnHR1" runat="server" />
                                            <asp:HiddenField ID="hdnHR2" runat="server" />
                                            <asp:HiddenField ID="hdnHR3" runat="server" />
                                            <asp:HiddenField ID="hdnHR4" runat="server" />
                                            <asp:HiddenField ID="hdnHR5" runat="server" />

                                            <asp:HiddenField ID="hdnDepId" runat="server" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-12">


                        <div class="row">
                            <div class="col-md-12">
                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-lg-12">
                                            <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance, Department Code %>" Value="Dep_Code" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, Department Name %>" Value="Dep_Name" />
                                                <asp:ListItem Text="<%$ Resources:Attendance, Manager %>" Value="Emp_Name" />
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
                                        <div class="col-lg-5">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                        </div>
                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                        <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                        <asp:HiddenField ID="editid" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:HiddenField ID="HiddenField3" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="box box-warning box-solid" <%= GvDept.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDept" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvDept_PageIndexChanging"
                                                AllowSorting="True" OnSorting="GvDept_Sorting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id")+"/"+Eval("Location_Id") %>' CommandName='<%# Eval("Dep_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%></asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Code %>" SortExpression="Dep_Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDepCode" runat="server" Text='<%# Eval("Dep_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Dep_Name" HeaderText="<%$ Resources:Attendance,Department Name %>"
                                                        SortExpression="Dep_Name" />
                                                    <asp:BoundField DataField="Dep_Name_L" HeaderText="<%$ Resources:Attendance,Department Name(Local) %>"
                                                        SortExpression="Dep_Name_L" />
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Manager %>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManager" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>" SortExpression="EmailId">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvEmailId" runat="server" Text='<%# Eval("EmailId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>" SortExpression="Phone_No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvPhone" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Fax No. %>" SortExpression="FaxNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvFaxNo" runat="server" Text='<%# Eval("FaxNo") %>'></asp:Label>
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_New">
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
    <script>
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
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
    </script>
</asp:Content>

