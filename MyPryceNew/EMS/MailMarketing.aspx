<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="true" AutoEventWireup="true" CodeFile="MailMarketing.aspx.cs" Inherits="EMS_MailMarketing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-mail-bulk" style="color: #00c0ef; padding: 3px;"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Email Marketing%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,EMS%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,EMS%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Email Marketing%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li style="display: none;" id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <%-- <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>--%>
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                        <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">

                            <%--<Triggers>
                                <asp:PostBackTrigger ControlID="gvTemplateMaster" />
                            </Triggers>--%>
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hdnCanView" />
                                <asp:HiddenField runat="server" ID="hdnCanDelete" />
                                <asp:HiddenField runat="server" ID="hdnCansave" />
                                <asp:HiddenField runat="server" ID="hdnCanDownload" />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Pending %>" Value="Pending"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Approved %>" Value="Approved"></asp:ListItem>
                                                        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>


                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Template Name%>" Value="Template_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,User Name%>" Value="Created_User"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                    OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvTemplateMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTemplateMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvTemplateMaster_PageIndexChanging" OnSorting="gvTemplateMaster_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblserialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    CausesValidation="False" OnCommand="btnEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,View %>"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>


                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total%>">
                                                                <ItemTemplate>

                                                                    <asp:UpdatePanel ID="up_btnTotalMail" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="btnTotalMail" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>

                                                                    <asp:LinkButton ID="btnTotalMail" runat="server" ForeColor="Blue" Enabled='<%#hdnCanDownload.Value=="true"?true:false %>'
                                                                        Text='<%#Eval("TotalMail") %>' OnCommand="btnTotalMail_OnCommand" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Download Total Mail Contact%>"></asp:LinkButton>
                                                                    <%-- <asp:ImageButton ID="btnTotalMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/download.png" CausesValidation="False" OnCommand="btnSuccessMail_Command"
                                                             ToolTip="Total Mail" />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sent%>">
                                                                <ItemTemplate>

                                                                    <asp:UpdatePanel ID="up_btnsentMail" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="btnsentMail" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>

                                                                    <asp:LinkButton ID="btnsentMail" runat="server" ForeColor="Blue" Enabled='<%#hdnCanDownload.Value=="true"?true:false %>'
                                                                        Text='<%#Eval("SentMail") %>' OnCommand="btnsentMail_OnCommand" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Download Sent Mail Contact%>"></asp:LinkButton>
                                                                    <%--  <asp:ImageButton ID="btnSuccessMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/download.png" CausesValidation="False" OnCommand="btnSuccessMail_Command"
                                                            Visible="false" ToolTip="Sent Mail" />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Failure%>">
                                                                <ItemTemplate>

                                                                    <asp:UpdatePanel ID="up_btnFailureMail" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="btnFailureMail" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>

                                                                    <asp:LinkButton ID="btnFailureMail" runat="server" ForeColor="Blue" Enabled='<%#hdnCanDownload.Value=="true"?true:false %>'
                                                                        Text='<%#Eval("FailureMail") %>' OnCommand="btnFailureMail_OnCommand" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Download Failure Mail Contact%>"></asp:LinkButton>
                                                                    <%--<asp:ImageButton ID="btnFailureMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/download.png" CausesValidation="False" OnCommand="btnFailureMail_Command"
                                                            Visible="false" ToolTip="Failure Mail" />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Pending%>">
                                                                <ItemTemplate>

                                                                    <asp:UpdatePanel ID="up_btnPendingMail" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="btnPendingMail" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>

                                                                    <asp:LinkButton ID="btnPendingMail" runat="server" ForeColor="Blue" Enabled='<%#hdnCanDownload.Value=="true"?true:false %>'
                                                                        Text='<%#Eval("PendingMail") %>' OnCommand="btnPendingMail_OnCommand" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Download Pending Mail Contact%>"></asp:LinkButton>
                                                                    <%--<asp:ImageButton ID="btnPendingMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/download.png" CausesValidation="False" OnCommand="btnFailureMail_Command"
                                                             ToolTip="Pending Mail" />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnResendAllMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' Visible='<%# hdnCansave.Value=="true"?true:false%>'
                                                                        ImageUrl="~/Images/resend.png" CausesValidation="False" OnCommand="btnResendAllMail_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Resend All Mail%>" />
                                                                    <cc1:ConfirmButtonExtender ID="Confirmsendall" runat="server" TargetControlID="btnResendAllMail"
                                                                        ConfirmText="<%$ Resources:Attendance,Are u sure to Resend All Mail ?%>">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnResendFailureMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' Visible='<%# hdnCansave.Value=="true"?true:false%>'
                                                                        ImageUrl="~/Images/resendfailure.png" CausesValidation="False" OnCommand="btnResendFailureMail_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Resend Failure Mail%>" />
                                                                    <cc1:ConfirmButtonExtender ID="Confirmfailureall" runat="server" TargetControlID="btnResendFailureMail"
                                                                        ConfirmText="<%$ Resources:Attendance,Are u sure to Resend All Failuer Mail ?%>">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnResendPendingMail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' Visible='<%# hdnCansave.Value=="true"?true:false%>'
                                                                        ImageUrl="~/Images/resendpending.png" CausesValidation="False" OnCommand="btnResendPendingMail_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Resend Pending Mail%>" />
                                                                    <cc1:ConfirmButtonExtender ID="Confirmpendingeall" runat="server" TargetControlID="btnResendPendingMail"
                                                                        ConfirmText="<%$ Resources:Attendance,Are u sure to Resend All Pending Mail ?%>">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Trans Id%>" SortExpression="Trans_Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVacancyName" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Template Name%>" SortExpression="Template_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLTempName" runat="server" Text='<%# Eval("Template_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date%>" SortExpression="CreatedDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# GetDate(Eval("CreatedDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name%>" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUser" runat="server" Text='<%# Eval("Created_User").ToString() %>'></asp:Label>
                                                                    <%--<asp:Label ID="lblUser" runat="server" Text='<%# GetEmployeeName(Eval("CreatedBy").ToString()) %>'></asp:Label>--%>
                                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Field1") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Field5">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Field5") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%--<asp:Image ID="img1" runat="server" ImageUrl='<%#"~/CompanyResource/Template/" +Eval("TemplateImage")%>'
                                                                        Height="100px" Width="100px" />--%>
                                                                    <asp:Image ID="img1" runat="server" ImageUrl='<%#GetImage(Eval("TemplateImage"))%>'
                                                                        Height="100px" Width="100px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" />
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
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lblstep1selectedRecord" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="Div_Step" runat="server" class="col-md-12">
                                                        <asp:Image ID="imgstep" runat="server" ImageUrl="~/Images/step1.png" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div id="Div_Filter" runat="server" class="col-md-12">
                                                        <div class="col-md-2">
                                                            <asp:Label ID="lblFilter" runat="server" Text="Favorite Filter"></asp:Label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                <asp:ListItem Text="My Favorite" Value="Personal"></asp:ListItem>
                                                                <%--<asp:ListItem Text="Other" Value="Other"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:CheckBox ID="chkInvoiceRecordOnly" runat="server" Text="Sales Invoice" AutoPostBack="true" OnCheckedChanged="chkInvoiceRecordOnly_CheckedChanged" />
                                                            <asp:Label ID="lblEmployee" runat="server" Text="Employee Name"></asp:Label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <%--<asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged"></asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtEmployeeName" BackColor="#eeeeee" runat="server" CssClass="form-control" OnTextChanged="txtEmployeeName_TextChanged" AutoPostBack="true" />

                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmployeeName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:MultiView ID="multiview" runat="server">
                                                            <asp:View ID="viewcontactList" runat="server">
                                                                <div class="col-md-3">
                                                                    <%--   <asp:Label ID="lblSelectGroup" runat="server" Text="Select Contact Group" Font-Bold="true"></asp:Label>--%>
                                                                    <asp:CheckBox ID="Chk_SelectAll_Contact" runat="server" Text="Select All" Style="margin-left: 15px; margin-right: 15px;" Font-Bold="true" OnCheckedChanged="Chk_SelectAll_Contact_CheckedChanged" AutoPostBack="true" />
                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                        <asp:TreeView ID="navTree" runat="server" OnSelectedNodeChanged="navTree_SelectedNodeChanged1"
                                                                            OnTreeNodeCheckChanged="navTree_TreeNodeCheckChanged" ShowCheckBoxes="All" Font-Names="Trebuchet MS"
                                                                            Font-Size="Small" ForeColor="Gray">
                                                                        </asp:TreeView>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkSelectAllCountry" runat="server" Text="Select Country" Font-Bold="true" OnCheckedChanged="chkSelectAllCountry_OnCheckedChanged" AutoPostBack="true" />
                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                        <asp:CheckBoxList ID="chkCountry" runat="server" Style="margin-left: 10px;" Font-Size="Small" ForeColor="Gray" Font-Names="Trebuchet MS"></asp:CheckBoxList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkSelectAllDesignation" runat="server" Text="Select Designation" Font-Bold="true" OnCheckedChanged="chkSelectAllDesignation_OnCheckedChanged" AutoPostBack="true" />
                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                        <asp:CheckBoxList ID="chkDesignation" Style="margin-left: 10px;" runat="server" Font-Size="Small" ForeColor="Gray" Font-Names="Trebuchet MS"></asp:CheckBoxList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkSelectAllcategory" runat="server" Text="Select Product category" Font-Bold="true" OnCheckedChanged="chkSelectAllcategory_OnCheckedChanged" AutoPostBack="true" />
                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                        <asp:CheckBoxList ID="ChkProductCategory" Style="margin-left: 10px;" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                            Font-Size="Small" ForeColor="Gray" AutoPostBack="true" OnSelectedIndexChanged="ddlcategorysearch_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </div>
                                                                </div>



                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>

                                                                <%-- <div class="col-md-4">
                                                                </div>--%>


                                                                <div class="col-md-4">
                                                                    <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>

                                                                    <asp:TextBox ID="txtModelNo" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                        AutoPostBack="true" OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelNo"
                                                                        ServicePath="" TargetControlID="txtModelNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>

                                                                    <asp:HiddenField ID="hdnModelId" runat="server" Value="0" />


                                                                </div>




                                                                <div class="col-md-4">

                                                                    <asp:Label ID="lblCategorysearch" Visible="false" runat="server" Text="<%$ Resources:Attendance,Product Category %>" />
                                                                    <asp:DropDownList ID="ddlcategorysearch" Visible="false" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlcategorysearch_SelectedIndexChanged">
                                                                    </asp:DropDownList>


                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Product Id %>" />
                                                                    <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                        OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                        ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                </div>


                                                                <div class="col-md-4">

                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                        OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>

                                                                    <asp:HiddenField ID="hdnproductId" runat="server" Value="0" />
                                                                </div>


                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:UpdatePanel ID="UpdDownload" runat="server">

                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="lblstep1selectedRecord" Font-Bold="true" runat="server" Text="Total Record" Font-Underline="true" OnClick="lblstep1selectedRecord_Click"></asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>






                                                                </div>

                                                                <div class="col-md-12" style="text-align: center">

                                                                    <asp:Button ID="BtnResetContactList" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                        OnClick="BtnResetContactList_Click" CssClass="btn btn-primary" ValidationGroup="a" />
                                                                    <asp:Button ID="BtnCancelContactList" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                        OnClick="BtnCancelContactList_Click" CssClass="btn btn-primary" ValidationGroup="a" />
                                                                    <asp:Button ID="BtnNextContactList" runat="server" Text="<%$ Resources:Attendance,Next %>"
                                                                        OnClick="BtnNextContactList_Click" CssClass="btn btn-primary" ValidationGroup="a" />
                                                                </div>
                                                            </asp:View>
                                                            <asp:View ID="ViewContactFinal" runat="server">
                                                                <div class="alert alert-info ">
                                                                    <div class="row">
                                                                        <div class="form-group">
                                                                            <div class="col-lg-3">
                                                                                <asp:DropDownList ID="ddlFieldNameContactFinal" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Name %>" Value="Name"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Field2"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,EmailId %>" Value="Field1"></asp:ListItem>
                                                                                    <asp:ListItem Text="Department Name" Value="DepName"></asp:ListItem>
                                                                                    <asp:ListItem Text="Country Name" Value="CountryName"></asp:ListItem>
                                                                                    <asp:ListItem Text="Designation" Value="DesgName"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-lg-2">
                                                                                <asp:DropDownList ID="ddlOptionContactFinal" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-lg-3">
                                                                                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindContactFinal">
                                                                                    <asp:TextBox ID="txtValueContactFinal" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                </asp:Panel>

                                                                            </div>
                                                                            <div class="col-lg-2" style="text-align: center">
                                                                                <asp:ImageButton ID="btnbindContactFinal" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                                                    ImageUrl="~/Images/search.png" OnClick="btnbindContactList_Click"
                                                                                    ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                                <asp:ImageButton ID="btnRefreshcontactfinal" runat="server" CausesValidation="False" Style="width: 33px;"
                                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshContactList_Click"
                                                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                            </div>
                                                                            <div class="col-lg-2">
                                                                                <h5>
                                                                                    <asp:Label ID="lblTotalSelectedRecordContactFinal" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvContactFinal" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                                        runat="server" AutoGenerateColumns="False">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Select">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkBxSelect" runat="server" onclick="selectCheckBox_Click(this)" />



                                                                                                    <%--  <asp:CheckBox ID="chkBxSelect" runat="server" OnCheckedChanged="chkBxSelect_OnCheckedChanged"
                                                                                                        AutoPostBack="true" />--%>
                                                                                                    <asp:Label ID="hdnFldId" runat="server" Text='<%# Eval("Contact_Id") %>' Visible="false" />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                                                <HeaderTemplate>

                                                                                                    <asp:CheckBox ID="chkBxHeader" runat="server" onclick="selectAllCheckbox_Click1(this)" />

                                                                                                    <%-- <asp:CheckBox ID="chkBxHeader" OnCheckedChanged="chkBxHeader_OnCheckedChanged" AutoPostBack="true"
                                                                                                        runat="server" />--%>
                                                                                                </HeaderTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name%>" SortExpression="Name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblContName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="30%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email%>" SortExpression="Field1">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblTName" runat="server" Text='<%# Eval("Field1") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="20%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mobile No%>" SortExpression="Field2">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblTempName" runat="server" Text='<%# Eval("Field2") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="10%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name%>" SortExpression="DepName">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbldept" runat="server" Text='<%# Eval("DepName") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="25%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Country Name%>" SortExpression="CountryName">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblCountryName" runat="server" Text='<%# Eval("CountryName") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="25%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" SortExpression="DesgName">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbldesgName" runat="server" Text='<%# Eval("DesgName") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="25%" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
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
                                                                                        <asp:Label ID="lbltotalselectedRecord" runat="server"></asp:Label>
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-12" style="text-align: center">

                                                                                        <asp:Button ID="BtnBackContactListFinal" runat="server" Text="<%$ Resources:Attendance,Back %>"
                                                                                            OnClick="BtnBackContactListFinal1_Click" CssClass="btn btn-primary" ValidationGroup="a" />
                                                                                        <asp:Button ID="BtnNextContactListFinal" runat="server" Text="<%$ Resources:Attendance,Next %>"
                                                                                            OnClick="BtnNextContactListFinal_Click" CssClass="btn btn-primary" ValidationGroup="a" />

                                                                                        <%-- <asp:Button ID="BtnResetContactListFinal" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                                OnClick="BtnResetContactListFinal_Click" CssClass="btn btn-primary" ValidationGroup="a" />&nbsp;&nbsp;&nbsp;
                                                                            <asp:Button ID="BtnCancelContactListFinal" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                OnClick="BtnCancelContactListFinal_Click" CssClass="btn btn-primary" ValidationGroup="a" />--%>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:View>
                                                            <asp:View ID="ViewFinalEmailList" runat="server">
                                                                <div class="col-md-6">
                                                                    <div class="col-md-4">
                                                                        <asp:DropDownList ID="ddlFieldnameEmailFilter" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Selected="True" Text="Email Id" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Country" Value="1"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEmailIdSearch" runat="server" CssClass="form-control" placeholder="Search"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1txtEmailIdSearch" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListCountryName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmailIdSearch"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <div class="input-group-btn">
                                                                                <asp:ImageButton ID="imgsearchEmailId" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                                                    ImageUrl="~/Images/search.png" ToolTip="<%$ Resources:Attendance,Search %>"
                                                                                    OnClick="imgsearchEmailId_OnClick"></asp:ImageButton>
                                                                            </div>
                                                                            <div class="input-group-btn">
                                                                                <asp:ImageButton ID="ImgRefreshEmailid" runat="server" CausesValidation="False" Style="width: 33px;"
                                                                                    ImageUrl="~/Images/refresh.png" ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                                                    OnClick="ImgRefreshEmailid_OnClick"></asp:ImageButton>
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6" style="text-align: center">
                                                                    <asp:Label ID="lblFinalEmailListrecord" runat="server" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="lblFinalEmailListrecord_Contact" runat="server" Font-Bold="true"></asp:Label>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <div class="col-md-5">
                                                                        <asp:ListBox ID="lstEmail" runat="server" Style="width: 100%;" Height="200px"
                                                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                            ForeColor="Gray"></asp:ListBox>
                                                                    </div>
                                                                    <div class="col-lg-2" style="text-align: center">
                                                                        <div style="margin-top: 55px; margin-bottom: 40px;" class="btn-group-vertical">

                                                                            <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />

                                                                            <%-- <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />--%>

                                                                            <asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server" CssClass="btn btn-info" />

                                                                            <%--  <asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server" CssClass="btn btn-info" />--%>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <asp:ListBox ID="lstEmailSelect" runat="server" Style="width: 100%;" Height="200px"
                                                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                            ForeColor="Gray"></asp:ListBox>
                                                                        <br />

                                                                    </div>
                                                                    <br />

                                                                </div>
                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12" style="text-align: center">

                                                                    <asp:Button ID="btnBackFinalEmailList" runat="server" Text="<%$ Resources:Attendance,Back %>"
                                                                        OnClick="btnBackFinalEmailList_Click" CssClass="btn btn-primary" ValidationGroup="a" />

                                                                    <asp:Button ID="btnRemoveemailList" runat="server" Text="<%$ Resources:Attendance,Remove %>"
                                                                        OnClick="btnPullDept_Click" CssClass="btn btn-primary" ValidationGroup="a" />


                                                                    <asp:Button ID="btnNextFinalEmailList" runat="server" Text="<%$ Resources:Attendance,Next %>"
                                                                        OnClick="btnNextFinalEmailList_Click" CssClass="btn btn-primary" ValidationGroup="a" />

                                                                </div>
                                                            </asp:View>
                                                            <asp:View ID="ViewProduct" runat="server">
                                                                <div class="col-md-4">
                                                                    <asp:Label ID="lblProductCategory" runat="server" Font-Bold="True" Font-Names="arial"
                                                                        Font-Size="13px" ForeColor="#666666" Text="<%$ Resources:Attendance,Product Category %>"></asp:Label>
                                                                    <hr />
                                                                    <div style="overflow: auto; max-height: 396px; height: 396px; margin-top: 7px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                        <%-- <asp:CheckBoxList ID="ChkProductCategory" Style="margin-left: 10px;" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                            AutoPostBack="true" Font-Size="Small" ForeColor="Gray" OnSelectedIndexChanged="ChkProductCategory_SelectedIndexChanged" />--%>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <asp:Label ID="lblProductName" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="13px" ForeColor="#666666" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                                    <hr />
                                                                    <asp:Label ID="lblProductSearch" runat="server" Text="Product Search" />
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtProductSearch" runat="server" CssClass="form-control" />
                                                                        <div class="input-group-btn">
                                                                            <asp:ImageButton ID="imgProductsearch" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                                                ImageUrl="~/Images/search.png" OnClick="imgProductsearch_Click"
                                                                                ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                                        </div>
                                                                        <div class="input-group-btn">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Style="width: 33px;"
                                                                                ImageUrl="~/Images/refresh.png" OnClick="imgProductRefresh_Click"
                                                                                ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                        </div>
                                                                    </div>
                                                                    <hr />
                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; margin-top: 7px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                        <asp:CheckBoxList ID="ChkProductChildCategory" runat="server" Style="margin-left: 10px;" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                            Font-Size="Small" ForeColor="Gray" AppendDataBoundItems="False" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12" style="text-align: center">

                                                                    <asp:Button ID="BtnBackProductList" runat="server" Text="<%$ Resources:Attendance,Back %>"
                                                                        OnClick="BtnBackProductList_Click" CssClass="btn btn-primary" ValidationGroup="a" />
                                                                    <asp:Button ID="BtnNextProductList" runat="server" Text="<%$ Resources:Attendance,Next %>"
                                                                        OnClick="BtnNextProductList_Click" CssClass="btn btn-primary" ValidationGroup="a" />

                                                                    <%--  <asp:Button ID="BtnResetProductList" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                            OnClick="BtnResetProductList_Click" CssClass="btn btn-primary" ValidationGroup="a" />&nbsp;&nbsp;&nbsp;
                                                                        <asp:Button ID="BtnCancelProductList" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                            OnClick="BtnCancelProductList_Click" CssClass="btn btn-primary" ValidationGroup="a" />--%>
                                                                </div>
                                                            </asp:View>
                                                            <asp:View ID="ViewTemplate" runat="server">

                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                                            <div class="box-header with-border">
                                                                                <h3 class="box-title">
                                                                                    <asp:Label ID="Label9" runat="server" Text="Advance Search"></asp:Label></h3>
                                                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecordTemplate" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                                                <asp:Label ID="Label4" runat="server" Visible="false"></asp:Label>

                                                                                <div class="box-tools pull-right">
                                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                                                    </button>
                                                                                </div>
                                                                            </div>
                                                                            <div class="box-body">
                                                                                <div class="col-lg-2">
                                                                                    <asp:DropDownList ID="ddlFieldNameTemplate" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Template Name%>" Value="Template_Name"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <div class="col-lg-2">
                                                                                    <asp:DropDownList ID="ddlOptionTemplate" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <div class="col-lg-4">
                                                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbindTemplate">
                                                                                        <asp:TextBox ID="txtValueTemplate" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    </asp:Panel>

                                                                                </div>
                                                                                <div class="col-lg-2" style="text-align: center">
                                                                                    <asp:LinkButton ID="btnbindTemplate" runat="server" CausesValidation="False"
                                                                                        OnClick="btnbindTemplate_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                                                <asp:LinkButton ID="btnRefreshTemplate" runat="server" CausesValidation="False"
                                                                                    OnClick="btnRefreshTemplate_Click"
                                                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                                </div>
                                                                                <div class="col-lg-2" style="text-align: center">
                                                                                    <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All" AutoPostBack="true" OnCheckedChanged="chkShowAll_CheckedChanged" />
                                                                                </div>




                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="box box-warning box-solid" <%= dtlistTemplate.Items.Count>0?"style='display:block'":"style='display:none'"%>>

                                                                    <div class="box-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <div class="flow">

                                                                                 
                                                                                           
                                                                                    <asp:DataList ID="dtlistTemplate" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                                                        <ItemTemplate>
                                                                                            <div class="col-md-12">
                                                                                                <div class="col-md-4">
                                                                                                    <asp:ImageButton ID="imgurlTemplate" runat="server" Width="80px" Height="80px" ImageUrl='<%#getImageUrl(Eval("Field1").ToString()) %>' OnClick="imgurlTemplate_OnClick" />
                                                                                                </div>
                                                                                                <div class="col-md-8">
                                                                                                    <asp:RadioButton ID="rbtnTemplate" runat="server" Text='<%#Eval("Template_Name") %>'   AutoPostBack="true" OnCheckedChanged="rbtnTemplate_OnCheckedChanged"/>
                                                                                                       
                                                                                                    <%-- AutoPostBack="false" OnCheckedChanged="rbtnTemplate_OnCheckedChanged" <asp:Label ID="lbltemplate" runat="server" Text='<%#Eval("Template_Name") %>' ></asp:Label>--%>
                                                                                                    <asp:HiddenField ID="hdnrbtnvalue" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                                                </div>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:DataList>
                      
                                                                                       
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
                                                        <div class="col-md-12" style="text-align: center">

                                                            <asp:Button ID="BtnBackTemplate" runat="server" Text="<%$ Resources:Attendance,Back %>"
                                                                OnClick="BtnBackTemplate_Click" CssClass="btn btn-primary" ValidationGroup="a" />
                                                            <asp:Button ID="BtnNextTemplate" runat="server" Text="<%$ Resources:Attendance,Next %>"
                                                                OnClick="BtnNextTemplate_Click" CssClass="btn btn-primary" />

                                                            <%--  <asp:Button ID="BtnResetTemplate" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                            OnClick="BtnResetTemplate_Click" CssClass="btn btn-primary" ValidationGroup="a" />&nbsp;&nbsp;&nbsp;
                                                                        <asp:Button ID="BtnCancelTemplate" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                            OnClick="BtnCancelTemplate_Click" CssClass="btn btn-primary" ValidationGroup="a" />--%>
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </asp:View>
                                                            <asp:View ID="ViewFinal" runat="server">
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="Label7" runat="server" Text="Display text" />
                                                                    <asp:TextBox ID="txtdisplayText" runat="server" CssClass="form-control" />
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblMailHeader" runat="server" Text="<%$ Resources:Attendance,Mail Header %>" />
                                                                    <asp:TextBox ID="txtMailHeader" runat="server" CssClass="form-control" />
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblsentmail" runat="server" Text="Total Email"></asp:Label>
                                                                    &nbsp : &nbsp
                                                                    <asp:Label ID="lblsentmailcount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEditContacts" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                        Width="100%" runat="server" Visible="false" OnPageIndexChanging="gvEditContacts_OnPageIndexChanging"
                                                                        AllowPaging="true" AutoGenerateColumns="False">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Id%>" SortExpression="Contact_Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblContId" runat="server" Text='<%# Eval("Contact_Id") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name%>" SortExpression="Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblContName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email%>" SortExpression="Field1">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTName" runat="server" Text='<%# Eval("EMailId") %>' Width="20%"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mobile No%>" SortExpression="Field2">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEMobNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date%>" SortExpression="ModifiedDate">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblModDate" runat="server" Text='<%# GetDate(Eval("ModifiedDate")) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                        </Columns>


                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <cc2:Editor ID="Editor1" runat="server" Height="300px" />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Email%>" Font-Bold="true" />
                                                                    <asp:DropDownList ID="ddlEmailUser" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    <asp:Label ID="lblemailid" runat="server" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblDate" runat="server" Visible="false" Font-Bold="true"></asp:Label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>" Font-Bold="true"></asp:Label>
                                                                    <asp:DropDownList ID="ddlbrandsearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12" style="text-align: center">
                                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Attendance,Back %>" OnClick="btnBack_Click"
                                                                        CssClass="btn btn-primary" ValidationGroup="a" />
                                                                    <asp:Button ID="btnSaveandsend" runat="server" Text="<%$ Resources:Attendance,Send %>"
                                                                        OnClick="btnSend_Click" Visible="false" CssClass="btn btn-success" ValidationGroup="a" />
                                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click"
                                                                        Visible="false" CssClass="btn btn-success" ValidationGroup="a" />
                                                                    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                        OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" />
                                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                        OnClick="btnCancel_Click" CssClass="btn btn-danger" CausesValidation="False" />
                                                                </div>
                                                            </asp:View>
                                    </asp:MultiView>
                                                        <br />
                                </div>
                                <div class="col-md-12">
                                    <asp:Panel ID="pnltrial" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td align='Left' colspan="4">
                                                    <div id="dvEditGrid" runat="server" visible="false" style="width: 97%; overflow: auto;">
                                                        <br />
                                                        <asp:ImageButton runat="server" ID="ImgBtngvEditContDelete" ImageUrl="~/Images/Erase.png"
                                                            OnCommand="ImgBtngvEditContDelete_Command" />
                                                        <asp:Label ID="lblEdit" runat="server" Text="Delete Selected Contacts"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblgvEditContactsSelected" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                    <asp:Panel ID="pnlToggle" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <div id="FilterBy" runat="server" visible="false">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblFilterBy" runat="server" Text="Filter By" />
                                                                                </td>
                                                                                <td>:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlFilterBy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFilterBy_SelectedIndexChanged">
                                                                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                                                        <asp:ListItem Text="By Department" Value="Dept"></asp:ListItem>
                                                                                        <asp:ListItem Text="By Country" Value="Country"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Panel ID="PnlDeptCountry" runat="server" ScrollBars="Auto" HorizontalAlign="Left" BackColor="White" Visible="false">
                                                                        <table>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDepartment" runat="server" AutoGenerateColumns="False">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Select">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkDeptBxSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkDeptBxSelect_CheckedChanged" />
                                                                                                    <asp:HiddenField ID="hdnFDeptId" runat="server" Value='<%# Eval("Dep_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkDeptBxHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkDeptBxHeader_CheckedChanged" />
                                                                                                </HeaderTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name%>" SortExpression="Name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblDeptName" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <asp:HiddenField ID="hdnDeptId" runat="server" />
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVCountry" runat="server" AutoGenerateColumns="False">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Select">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkBxCountrySelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkBxCountrySelect_CheckedChanged" />
                                                                                                    <asp:HiddenField ID="hdnFCountryId" runat="server" Value='<%# Eval("Country_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkCountryBxHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkCountryBxHeader_CheckedChanged" />
                                                                                                </HeaderTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Country Name%>" SortExpression="Name">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblCountryName" runat="server" Text='<%# Eval("Country_Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <asp:HiddenField ID="hdnCountryId" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td valign="top"></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Label ID="lblProductSelect" runat="server" Text="Select Products" Font-Bold="true"></asp:Label>
                                                    <%-- <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="PanelProduct"
                                                        ExpandControlID="lblProductSelect" CollapseControlID="lblProductSelect" ExpandDirection="Vertical"
                                                        Collapsed="True" AutoCollapse="False" AutoExpand="False">
                                                    </cc1:CollapsiblePanelExtender>--%>
                                                    <asp:Panel ID="PanelProduct" runat="server">
                                                        <table>
                                                            <tr>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <%--  </ContentTemplate>
                                                                              </cc1:TabPanel>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='Left' colspan="3">
                                                    <%-- <asp:RadioButtonList ID="rbn1" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rbn1_SelectedIndexChanged" RepeatColumns="4" CellSpacing="8">
                                                    </asp:RadioButtonList>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <div id="dvHTMLEditor" runat="server" visible="true">
                                                <tr>
                                                    <td align='Left'>
                                                        <asp:Label ID="lblHTMLEditor" runat="server" Font-Bold="true"
                                                            Text="<%$ Resources:Attendance,Template Content%>"></asp:Label>
                                                    </td>
                                                    <td>:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='Left' colspan="4"></td>
                                                </tr>
                                            </div>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="editid" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>

                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
</asp:UpdatePanel>
                    </div>
                    <div style="display: none;" class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                    <%--  <asp:ListItem Text="<%$ Resources:Attendance,Vacancy Id %>" Value="Vacancy_Id"></asp:ListItem>--%>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Template Name %>" Value="Template_Name"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:Panel ID="Panel4" runat="server" DefaultButton="btnbinbind">
                                                    <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-3" style="text-align: center">
                                                <asp:ImageButton ID="btnbinbind" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnbinRefresh" runat="server" CausesValidation="False" Style="width: 33px;"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                <asp:ImageButton ID="imgBtnRestore" CausesValidation="False" Style="width: 33px;"
                                                    runat="server" ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click"
                                                    ToolTip="<%$ Resources:Attendance, Active %>" />

                                                <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" Style="width: 33px;"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"
                                                    ImageUrl="~/Images/selectAll.png" />
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTemplateMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvTemplateMasterBin_PageIndexChanging"
                                                        OnSorting="gvTemplateMasterBin_OnSorting" DataKeyNames="Trans_Id" AllowSorting="True">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    <asp:Label ID="lblFileId" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--   <asp:TemplateField HeaderText="<%$ Resources:Attendance,Trans Id%>" SortExpression="Trans_Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVacancyName" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Template Name%>" SortExpression="Template_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTempName" runat="server" Text='<%# Eval("Template_Name") %>'></asp:Label>
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>




    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <script type="text/javascript">
        $.noConflict();
        jQuery(document).ready(function () {
            $('[id*=lstFruits]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>--%>


    <%--<script type="text/javascript">
        $(function () {
            $('[id*=lstFruits]').multiselect({
                includeSelectAllOption: true
            });
            $("#Button1").click(function () {
                alert($(".multiselect-selected-text").html());
            });
        });
    </script>--%>




    <%--  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstFruits]').multiselect({
                includeSelectAllOption: true
            });

        });
    </script>--%>




    <script type="text/javascript">
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= navTree.ClientID%>", "");
            }
        }

        function selectAllCheckbox_Click() {

            var gridView = document.getElementById('<%=gvContactFinal.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[0];
                cell.getElementsByTagName("INPUT")[0].checked = true;
            }
            //SetEmpList();
            //SelectedEmpCount();
        }

        function selectAllCheckbox_Click1(id) {

            var gridView = document.getElementById('<%=gvContactFinal.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[0];
                cell.getElementsByTagName("INPUT")[0].checked = id.checked;
            }
            //SetEmpList();
            //SelectedEmpCount();
        }




        function selectCheckBox_Click(id) {

            var gridView = document.getElementById('<%=gvContactFinal.ClientID%>');
            var AtLeastOneCheck = false;
            var cell;
            for (var i = 1; i < gridView.rows.length; i++) {
                cell = gridView.rows[i].cells[0];
                if (cell.getElementsByTagName("INPUT")[0].checked == false) {
                    AtLeastOneCheck = true;
                    break;
                }
            }
            gridView.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = !AtLeastOneCheck;

            //SelectedEmpCount();
        }


    </script>




</asp:Content>
