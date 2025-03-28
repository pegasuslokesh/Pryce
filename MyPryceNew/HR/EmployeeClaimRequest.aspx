<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="EmployeeClaimRequest.aspx.cs" Inherits="HR_EmployeeClaimRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
       <i class="fas fa-praying-hands"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Claim Request%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Claim Request%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Form" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div role="form">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                            <asp:TextBox ID="TxtEmployeeId" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="HidEmpId" runat="server" />
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="LblClaimName" runat="server" Text="<%$ Resources:Attendance,Claim Name %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                ControlToValidate="TxtClaimName" ErrorMessage="<%$ Resources:Attendance,Enter Claim Name %>"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="TxtClaimName" runat="server" MaxLength="150" BackColor="#eeeeee" CssClass="form-control"
                                                OnTextChanged="TxtClaimName_textChanged" AutoPostBack="true"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetClaimName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="TxtClaimName"
                                                UseContextKey="True"  
                                                 CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblClaimDiscription" runat="server" Text="<%$ Resources:Attendance,Claim Description %>"></asp:Label>
                                            <asp:TextBox ID="TxtClaimDiscription" runat="server" Height="100px" TextMode="MultiLine"
                                                CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="LabelValueType" runat="server" Text="<%$ Resources:Attendance,Value Type %>"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="DdlValueType" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Month %>" />
                                        <asp:DropDownList ID="DdlValueType" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>" Value="0" />
                                            <asp:ListItem Text="<%$ Resources:Attendance,Fixed %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, Percentage %>" Value="2" />
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="LblValue" runat="server" Text="<%$ Resources:Attendance,Value%>"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="txtCalValue" ErrorMessage="<%$ Resources:Attendance,Enter Value %>"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtCalValue" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<cc1:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                            TargetControlID="txtCalValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                        </cc1:FilteredTextBoxExtender>--%>
                                        <asp:RegularExpressionValidator ID="RangeValidator1" runat="server" ValidationGroup="Save"
                                                            ControlToValidate="txtCalValue" Display="Dynamic"
                                                            ErrorMessage="Invalid Value"
                                                        ValidationExpression="^\d+(?:\.\d{0,9})?$"                                                            
                                                            SetFocusOnError="True"></asp:RegularExpressionValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblMonth" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="ddlMonth" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Month %>" />
                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance, --Select-- %>" Value="0" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, January %>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, February %>" Value="2" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, March %>" Value="3" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, April %>" Value="4" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, May %>" Value="5" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, June %>" Value="6" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, July %>" Value="7" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, August %>" Value="8" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, September %>" Value="9" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, October %>" Value="10" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, November %>" Value="11" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, December %>" Value="12" />
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="LblYear" runat="server" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="TxtYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="TxtYear" runat="server" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true"
                                            TargetControlID="TxtYear" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </div>
                                </div>
                                <div style="text-align: center;" class="col-md-12">
                                    <asp:Button ID="btnSave" Visible="false" runat="server" ClientIDMode="Static" CssClass="btn btn-success"
                                        TabIndex="7" Text="<%$ Resources:Attendance,Save %>" OnClick="btnSaveClaim_Click"
                                        ValidationGroup="Save" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                        TabIndex="8" Text="<%$ Resources:Attendance,Reset %>" OnClick="btnReset_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                        TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Form">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White;
                        border-radius: 10px;" />
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
        </script>
</asp:Content>
