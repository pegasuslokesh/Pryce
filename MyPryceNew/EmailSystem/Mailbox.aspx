<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Mailbox.aspx.cs" Inherits="EmailSystem_Mailbox" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/EmailConfiguration.ascx" TagName="EmailConfig" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-envelope-open-text"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Mail Box%>"></asp:Label>
    </h1>

    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Email System%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Email System%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Mail Box%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Inbox" Style="display: none;" runat="server" OnClick="btnInbox_Click" Text="Inbox" />
            <asp:Button ID="Btn_Draft" Style="display: none;" runat="server" OnClick="btnDraft_Click" Text="Draft" />
            <asp:Button ID="Btn_Send" Style="display: none;" runat="server" OnClick="btnSend_Click" Text="Sent" />
            <asp:Button ID="Btn_Outbox" Style="display: none;" runat="server" OnClick="btnOutbox_Click" Text="Outbox" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Setting" Style="display: none;" runat="server" OnClick="btnSetting_Click" Text="Setting" />
            <asp:Button ID="Btn_Search" Style="display: none;" runat="server" OnClick="btnSearch_Click" Text="Search" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="btnupload_Click" Text="Upload" />
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
                    <li id="Li_Upload" runat="server" visible="false"><a href="#Upload" onclick="Li_Tab_Upload()" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a></li>
                    <li id="Li_Search"><a href="#Search" onclick="Li_Tab_Search()" data-toggle="tab">
                        <i class="fa fa-search"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Search %>"></asp:Label></a></li>
                    <li id="Li_Setting"><a href="#Setting" onclick="Li_Tab_Setting()" data-toggle="tab">
                        <i class="fas fa-wrench"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Setting %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#" onclick="Li_Tab_New()">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-list"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_Outbox"><a href="#Inbox" onclick="Li_Tab_Outbox()" data-toggle="tab">
                        <i class="fa fa-mail-reply"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Outbox %>"></asp:Label></a></li>
                    <li id="Li_Sent"><a href="#Inbox" onclick="Li_Tab_Sent()" data-toggle="tab">
                        <i class="fa fa-mail-forward"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Sent %>"></asp:Label></a></li>
                    <li style="display: none;" id="Li_Draft"><a href="#Inbox" onclick="Li_Tab_Draft()" data-toggle="tab">
                        <img src="../Images/draft.png" style="width: 25px;" alt="" /><asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Draft %>"></asp:Label></a></li>
                    <li id="Li_Inbox" class="active"><a href="#Inbox" onclick="Li_Tab_Inbox()" data-toggle="tab">
                        <i class="fas fa-inbox"></i>
                        <asp:DropDownList ID="ddlFolder" runat="server"
                            BorderStyle="none" BackColor="Transparent" Style="padding-top: 3px; background-size: 25px; background-repeat: no-repeat; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;"
                            AutoPostBack="true" OnSelectedIndexChanged="btnInbox_Click" Visible="false">
                            <asp:ListItem Text="Inbox"></asp:ListItem>
                            <asp:ListItem Text="Business"></asp:ListItem>
                            <asp:ListItem Text="Personal"></asp:ListItem>
                        </asp:DropDownList></a></li>
                    <li id="Li_Email_User">
                        <asp:UpdatePanel ID="Update_Email_User" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlEmailUser" runat="server" Style="margin-top: 5px;" CssClass="form-control" Width="200px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEmailUser_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="Inbox">
                        <asp:UpdatePanel ID="Update_Inbox" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblMoveTo" runat="server" Text="Move To" Visible="false"></asp:Label>
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="ddlMovetoFolder" runat="server" CssClass="form-control" Visible="false">
                                                                <asp:ListItem Text="Folder" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Inbox"></asp:ListItem>
                                                                <asp:ListItem Text="Business"></asp:ListItem>
                                                                <asp:ListItem Text="Personal"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="input-group-btn">
                                                                <asp:ImageButton ID="imgMove" Visible="false" runat="server" CausesValidation="False" Style="width: 22px; margin-left: 7px;"
                                                                    ImageUrl="~/Images/move.png" OnCommand="IbtnMove_Command" ToolTip="Move" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <br />
                                                            <asp:DropDownList ID="ddlDelete" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="Delete Mail" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Temporary" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Permanent" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="input-group-btn">
                                                                <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" ImageUrl="~/Images/Erase.png" Style="width: 22px; margin-top: 19px; margin-left: 7px;"
                                                                    OnCommand="IbtnDelete_Command" Visible="true" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2" style="margin-top: 5px;">
                                                        <br />
                                                        <asp:Label ID="lblTotalRecord" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Emails%>"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2" style="margin-top: 5px;">
                                                        <br />
                                                        <img src="../Images/newmailnew.png" style="vertical-align: middle;" />
                                                        <asp:Label ID="lblReadMail" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Read:0 %>"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2" style="margin-top: 5px;">
                                                        <br />
                                                        <img src="../Images/readmailnew.png" style="vertical-align: middle;" />
                                                        <asp:Label ID="lblUnReadMail" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,UnRead:0 %>"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Label23" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <br />
                                                    <asp:DropDownList ID="ddlGridSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGridSize_SelectedIndexChanged">
                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                        <asp:ListItem Text="150" Value="150"></asp:ListItem>
                                                        <asp:ListItem Text="200" Value="200"></asp:ListItem>
                                                        <asp:ListItem Text="250" Value="250"></asp:ListItem>
                                                        <asp:ListItem Text="300" Value="300"></asp:ListItem>
                                                        <asp:ListItem Text="350" Value="350"></asp:ListItem>
                                                        <asp:ListItem Text="400" Value="400"></asp:ListItem>
                                                        <asp:ListItem Text="450" Value="450"></asp:ListItem>
                                                        <asp:ListItem Text="500" Value="500"></asp:ListItem>
                                                        <asp:ListItem Text="Default" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" MaxLength="11" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_FromDate" runat="server" TargetControlID="txtFromDate" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date%>" />
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" MaxLength="11" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_ToDate" runat="server" TargetControlID="txtToDate" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <br />
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="settxtValue" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,TO %>" Value="To"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,CC %>" Value="Cc"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Subject %>" Value="Subject"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Read Mail %>" Value="Read Mail"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnRead Mail %>" Value="UnRead Mail"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Trash %>" Value="Trash"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <br />
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" />
                                                    </asp:Panel>

                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <br />
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>

                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="box box-warning box-solid" <%= GvEmail.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmail" runat="server" Width="100%" AllowPaging="true"
                                                        AllowSorting="true" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound"
                                                        OnSelectedIndexChanged="OnSelectedIndexChanged" OnPageIndexChanging="GvEmail_PageIndexChanging"
                                                        OnSorting="GvEmail_OnSorting">
                                                        <AlternatingRowStyle />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Field6" HeaderText="Read">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="imgAttachment1" runat="server" Text='<%# Eval("Field3").ToString() %>'
                                                                        Visible="false" />
                                                                    <asp:ImageButton ID="ImageReadUnRead" runat="server" ImageUrl='<%# getReadMailStatus(Eval("Field6").ToString(),Eval("Field3").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="IsAttach">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgAttachment" runat="server" ImageUrl='<%# getAttachmentstatus(Convert.ToInt32(Eval("IsAttach"))) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                    <asp:Label ID="lblTransId" Visible="false" runat="server"
                                                                        Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date%>" SortExpression="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# GetdateFormate(Eval("Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,FROM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToOrFr" runat="server" Text='<%#  GetlblEmail(Eval("Trans_Id").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Subject %>" SortExpression="Subject">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Size %>" SortExpression="MailSizeForSort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMailSize" runat="server" Text='<%# Eval("MailSize") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
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
                    <%--<div style="display:none;" class="tab-pane" id="Draft">
                        <asp:UpdatePanel ID="Update_Draft" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                                <h5></h5>
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
                                                    Grid
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Sent">
                        <asp:UpdatePanel ID="Update_Sent" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                                <h5></h5>
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
                                                    Grid
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Outbox">
                        <asp:UpdatePanel ID="Update_Outbox" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                                <h5></h5>
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
                                                    Grid
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                                <h5></h5>
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
                                                    Grid
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>--%>
                    <div class="tab-pane" id="Setting">
                        <asp:UpdatePanel ID="Update_Setting" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <uc:EmailConfig ID="Email_Config" runat="server"></uc:EmailConfig>

                                                    <%--     <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmailPassuser" PageSize="5" runat="server" AutoGenerateColumns="False"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IbtnEditEmail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            ImageUrl="~/Images/Edit.png" OnCommand="IbtnEditEmail_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IbtnDeleteEmail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteEmail_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailID%>" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblEmailId" Text='<%# Eval("Email") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="chkdefault" runat="server" Text='<%#Convert.ToBoolean(Eval("IsDefault")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                                                        <asp:TextBox ID="txtPasswordEmail" TextMode="Password" runat="server"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,SMTP %>"></asp:Label>
                                                        <asp:TextBox ID="txtSMTP" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Port %>"></asp:Label>
                                                        <asp:TextBox ID="txtPort" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblpop3" runat="server" Text="<%$ Resources:Attendance,POP3  %>"></asp:Label>
                                                        <asp:TextBox ID="txtPop3" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label98" runat="server" Text="<%$ Resources:Attendance,  Port %>"></asp:Label>
                                                        <asp:TextBox ID="txtpopport" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,EnableSSL %>"></asp:Label>
                                                        <asp:CheckBox ID="chkEnableSSL" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Is Default %>"></asp:Label>
                                                        <asp:CheckBox ID="chkDefault" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FileUploadImage"
                                                                OnClientUploadStarted="FuLogo_UploadStarted"
                                                                OnClientUploadError="FuLogo_UploadError"
                                                                OnClientUploadComplete="FuLogo_UploadComplete"
                                                                OnUploadedComplete="FuLogo_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FULogo_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:25px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:25px"></i></asp:LinkButton>
                                                                <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                            <div class="input-group-btn" style="width: 35px;">
                                                                <asp:LinkButton ID="ImgFileUploadAdd" runat="server" CausesValidation="False" OnClick="ImgLogoAdd_Click" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6" style="text-align: center">
                                                        <asp:Button ID="btnSaveEmail" runat="server" CssClass="btn btn-success" OnClick="btnSaveSMSEmail_Click"
                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                        <asp:Button ID="btnreset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="btnCancelSMSEmail_Click" Text="<%$ Resources:Attendance,Reset %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <cc2:Editor ID="txtEmailSignature" Height="300px" runat="server" />
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Search">
                        <asp:UpdatePanel ID="Update_Search" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label1" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsSearch" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Emails %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlPageType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Request%>" Value="PR"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Inquiry%>" Value="PI"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Quotation%>" Value="PQ"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Order%>" Value="PO"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Invoice%>" Value="PINV"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Goods Receive%>" Value="GR"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Inquiry%>" Value="SINQ"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Quotation%>" Value="SQ"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Order%>" Value="SO"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Invoice%>" Value="SINV"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlContactoremailOption" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlContactoremailOption_OnSelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="By Email Address"></asp:ListItem>
                                                        <asp:ListItem Text="By Contact Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:TextBox ID="txtEmailAdressSearch" placeholder="Search from Content" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnTextChanged="txtEmailAdressSearch_OnTextChanged"
                                                        BackColor="#eeeeee"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="txtMailTo_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                                        Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                        ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetEmailList" ServicePath=""
                                                        TargetControlID="txtEmailAdressSearch" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:TextBox ID="txtContactNameSearch" placeholder="Search from Content" runat="server" CssClass="form-control"
                                                        Visible="false" AutoPostBack="true" OnTextChanged="txtContactNameSearch_OnTextChanged"
                                                        BackColor="#eeeeee"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=";"
                                                        Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                        ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetCompletionListContact"
                                                        ServicePath="" TargetControlID="txtContactNameSearch" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnbindRecord_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False" OnClick="btnRefreshRecord_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvEmailSearch.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmailSearch" runat="server" Width="100%" AllowPaging="true"
                                                        AllowSorting="true" AutoGenerateColumns="False" OnRowDataBound="GvEmailSearch_OnRowDataBound"
                                                        OnSelectedIndexChanged="GvEmailSearch_OnSelectedIndexChanged" OnPageIndexChanging="GvEmailSearch_PageIndexChanging"
                                                        OnSorting="GvEmailSearch_OnSorting">

                                                        <Columns>
                                                            <asp:TemplateField SortExpression="Field6" HeaderText="Read">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="imgAttachment1" runat="server" Text='<%# Eval("Field3").ToString() %>'
                                                                        Visible="false" />
                                                                    <asp:ImageButton ID="ImageReadUnRead" runat="server" ImageUrl='<%# getReadMailStatus(Eval("Field6").ToString(),Eval("Field3").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="IsAttach">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgAttachment" runat="server" ImageUrl='<%# getAttachmentstatus(Convert.ToInt32(Eval("IsAttach"))) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                    <asp:Label ID="lblTransId" Visible="false" runat="server"
                                                                        Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date%>" SortExpression="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# GetdateFormate(Eval("ModifiedDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToOrFr" runat="server" Text='<%#  GetlblEmail(Eval("Trans_Id").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Subject %>" SortExpression="Subject">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Size %>" SortExpression="MailSize">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMailSize" runat="server" Text='<%# Eval("MailSize") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnContactMailId" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Upload">
                        <asp:UpdatePanel ID="Update_Upload" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:RadioButton ID="rbtnCreatecontact" runat="server" AutoPostBack="true"
                                                            GroupName="c" Text="EmailId Link with Create Contact" CausesValidation="false"
                                                            OnCheckedChanged="rbtncontact_OnCheckedChanged" />

                                                        <asp:RadioButton ID="rbtnexistcontact" runat="server" Style="margin-left: 30px;" AutoPostBack="true"
                                                            GroupName="c" Text="EmailId Link with Exists Contact" CausesValidation="false"
                                                            OnCheckedChanged="rbtncontact_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:UpdatePanel ID="Upload_Option" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnuploadoption" CssClass="btn btn-primary" Visible="false" runat="server"
                                                                    Text="Download and Upload Excel" OnClick="btnuploadoption_OnClick" />

                                                                <asp:Button ID="btngridviewoption" Style="margin-left: 30px;" CssClass="btn btn-primary" Visible="false" runat="server"
                                                                    Text="Gridview Format" OnClick="btngridviewoption_OnClick" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="Update_Upload_Button" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                                <asp:PostBackTrigger ControlID="lnkexportCountryList" />
                                <asp:PostBackTrigger ControlID="lnkexportdesignationList" />
                                <asp:PostBackTrigger ControlID="lnkExportContactCompanyList" />
                                <asp:PostBackTrigger ControlID="lnkexportContactList" />
                                <asp:PostBackTrigger ControlID="lnkexportContactGroup" />
                            </Triggers>
                            <ContentTemplate>
                                <div id="pnluploadEmaillist" runat="server" visible="false" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnExport" runat="server" CausesValidation="False" Text="Export Unregistered Email List"
                                                            CssClass="btn btn-primary" OnClick="btnExport_Click" Visible="false" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:LinkButton ID="lnkexportCountryList" runat="server" Text="Export Country List"
                                                            Visible="false" OnClick="lnkexportCountryList_OnClick" ForeColor="Blue"></asp:LinkButton>

                                                        <asp:LinkButton ID="lnkexportdesignationList" Style="margin-left: 20px;" runat="server"
                                                            Text="Export Designation List" OnClick="lnkexportdesignationList_OnClick" Visible="false"
                                                            ForeColor="Blue"></asp:LinkButton>

                                                        <asp:LinkButton ID="lnkExportContactCompanyList" Style="margin-left: 20px;" runat="server"
                                                            Text="Export Contact Company List" OnClick="lnkExportContactCompanyList_OnClick"
                                                            Visible="false" ForeColor="Blue"></asp:LinkButton>

                                                        <asp:LinkButton ID="lnkexportContactList" Style="margin-left: 20px;" runat="server" OnClick="lnkexportContactList_OnClick"
                                                            Text="Export Contact List" ForeColor="Blue" Visible="false"></asp:LinkButton>

                                                        <asp:LinkButton ID="lnkexportContactGroup" runat="server" Style="margin-left: 20px;"
                                                            OnClick="lnkexportContactGroup_OnClick" Text="Export Contact Group List" ForeColor="Blue"
                                                            Visible="false"></asp:LinkButton>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                            OnClick="btnConnect_Click" Text="<%$ Resources:Attendance,Upload %>" Visible="false" />

                                                        <asp:Button ID="btnResetEmailList" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                            OnClick="btnResetEmailList_Click" Text="<%$ Resources:Attendance,Reset %>" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="Update_Grid" runat="server">
                            <ContentTemplate>
                                <div id="pnlgridunregisterwithexistscontact" runat="server" visible="false">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldNameunregis" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,EmailId %>" Value="Email_Id"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionunregister" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%12$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="ImageButton3">
                                                        <asp:TextBox ID="txtvalueunregister" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>

                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnbindunRegisEmail_Click"
                                                        ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="btnSave_UnregisterEmail" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/Add.png" ToolTip="<%$ Resources:Attendance,Add %>"
                                                        OnClick="btnSave_UnregisterEmail_Click" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblTotalRecordsUnRegisEmail" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                    <asp:Label ID="Label11" runat="server" Visible="false"></asp:Label>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUnregisteredEmailexistcontact" runat="server" AllowPaging="true"
                                                            AutoGenerateColumns="False" OnPageIndexChanging="gvUnregisteredEmailexistcontact_PageIndexChanging"
                                                            Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IbtnDeleteMail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                            OnCommand="IbtnDeleteMail_Command" />

                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkselctemail" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkHeaderSelect" runat="server" OnCheckedChanged="chkHeaderSelect_OnCheckedChanged" AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>

                                                                <%--  <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="IbtnsaveEmail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                        ImageUrl="~/Images/Approve.png" OnCommand="btnsave_Command" ToolTip="<%$ Resources:Attendance,Save %>" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                --%>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IbtnEditEmail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            ImageUrl="~/Images/edit.png" OnCommand="btnEditEmail_Command" ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmailId" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdntransid" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtcontactname" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                            Width="392px"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtendercountry" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListContact" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtcontactname"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="50%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Country Name%>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCountryName" runat="server" CssClass="form-control" AutoPostBack="false"
                                                                            BackColor="#eeeeee" Text='<%# Eval("CountryName") %>' />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtendercountry2" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListCountryName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCountryName"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Country Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcountryName" runat="server" Text='<%# Eval("CountryName") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnCountryid" runat="server" Value='<%# Eval("CountryId") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="20%" />
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
                                        <div class="col-md-12" style="text-align: center">
                                            <asp:Button ID="btnresetexistscontactgridrecord" CssClass="btn btn-primary" runat="server"
                                                CausesValidation="False" OnClick="btnresetexistscontactgridrecord_Click" Text="<%$ Resources:Attendance,Reset %>" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="Update_Record_Grid" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="pnlgridrecord" runat="server" visible="false" class="col-md-12" style="overflow: auto; max-height: 500px;">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvunregisteredEmaillist" runat="server" AutoGenerateColumns="true"
                                                Width="100%">
                                                <Columns>
                                                </Columns>


                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                            <asp:Button ID="btnsavefinalemail" CssClass="btn btn-success" runat="server" CausesValidation="False"
                                                OnClick="btnsaveemail_Click" Visible="false" Text="<%$ Resources:Attendance,Save %>" />
                                            <asp:Button ID="btnResetEmailListall" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                OnClick="btnResetEmailList_Click" Text="<%$ Resources:Attendance,Reset %>" Visible="false" />
                                            <br />
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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Inbox">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Draft">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Sent">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Setting">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <%-- <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Outbox">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Search">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Upload">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Upload_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Grid">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_Record_Grid">
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

        function Li_Tab_Inbox() {
            document.getElementById('<%= Btn_Inbox.ClientID %>').click();
        }

        function Li_Tab_Draft() {
            document.getElementById('<%= Btn_Draft.ClientID %>').click();
        }

        function Li_Tab_Sent() {
            document.getElementById('<%= Btn_Send.ClientID %>').click();
        }

        function Li_Tab_Outbox() {
            document.getElementById('<%= Btn_Outbox.ClientID %>').click();
        }

        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }

        function Li_Tab_Setting() {
            document.getElementById('<%= Btn_Setting.ClientID %>').click();
        }

        function Li_Tab_Search() {
            document.getElementById('<%= Btn_Search.ClientID %>').click();
        }

        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";
        }
        function uploadError(sender, args) {
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function uploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }
    </script>
    <%--  <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
                }
                return false;
            }
        }--%>
    </script>
</asp:Content>
