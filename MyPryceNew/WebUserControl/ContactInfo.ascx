<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactInfo.ascx.cs" Inherits="WebUserControl_ContactInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="tc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="tc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ContactNo.ascx" TagPrefix="uc1" TagName="ContactNo1" %>

<asp:HiddenField ID="hdnTableName" runat="server" />
<asp:HiddenField ID="hdnLocationId" runat="server" />

<div class="row">

    <asp:UpdatePanel ID="Update_Header" runat="server">
        <ContentTemplate>

            <div class="col-lg-6" style="max-width: 100%;">
                <h3>
                    <img src="../Images/follow-up.png" alt="Followup" /><asp:Label ID="lblHeaderL" runat="server"></asp:Label>
                </h3>
            </div>
            <div class="col-lg-6" style="text-align: right; margin-top: 12px; max-width: 100%;">
                <h4>
                    <asp:Label ID="lblHeaderR" runat="server"></asp:Label>
                </h4>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Header">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Li">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="col-lg-12">
        <div class="nav-tabs-custom">
            <ul class="nav nav-tabs pull-right bg-blue-gradient">

                <li id="Li_New_Contact"><a href="#NewContact" data-toggle="tab">
                    <asp:UpdatePanel ID="Update_Li" runat="server">
                        <ContentTemplate>
                            <i class="fa fa-file"></i>&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </a></li>

                <li id="Li_ContactList" class="active"><a href="#ContactList" data-toggle="tab">
                    <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List%>"></asp:Label></a></li>

            </ul>

        </div>
    </div>
</div>

<div class="tab-content">

    <div class="tab-pane active" id="ContactList">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"></h3>
                        <asp:Label ID="lblheaderTitle" runat="server"></asp:Label>
                        <asp:Label ID="lblTotalRecordsFollowup" Font-Bold="true" runat="server"></asp:Label></h5>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="flow">
                                    <asp:HiddenField ID="hdntransID" runat="server"></asp:HiddenField>
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvListData" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvListData_PageIndexChanging" OnSorting="GvListData_Sorting"
                                        AllowSorting="true" >
                                        <Columns>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact%>" SortExpression="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("Name") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" SortExpression="Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation")  %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department%>" SortExpression="Dep_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Dep_Name") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact No%>" SortExpression="ContactNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("ContactNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email%>" SortExpression="email">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblEmail" BorderStyle="None" Enabled="false" ToolTip='<%#Eval("email") %>' BackColor="White" runat="server" Text='<%#Eval("email") %>' MaxLength="50" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  Width="10%" />
                                            </asp:TemplateField>

                                        </Columns>
                                        
                                        
                                        <PagerStyle CssClass="pagination-ys" />
                                        
                                    </asp:GridView>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <div class="tab-pane" id="NewContact">
        <asp:UpdatePanel ID="Update_New" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">


                        <div class="form-group">
                            <asp:HiddenField ID="hdncust_Id" runat="server" />
                            <asp:HiddenField ID="hdnMob_no" runat="server" />
                            <asp:HiddenField ID="hdnEmail_Id" runat="server" />
                            <asp:HiddenField ID="hdnContact_Id" runat="server" />

                            <div class="col-md-12" id="div_AddNewContact" runat="server">
                                <div class="box box-primary">
                                    <div id="Div_Box_Add2" class="box box-primary">
                                        <div class="box-header with-border">
                                            <h4>
                                                <asp:Literal runat="server" Text="Add New Contact" />:</h4>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="Btn_Add_Div2" class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">

                                                <div class="col-md-2">
                                                    <br />
                                                    <asp:DropDownList ID="ddlSalutation" Class="form-control" runat="server">
                                                        <asp:ListItem Text="Mr." Value="Mr"></asp:ListItem>
                                                        <asp:ListItem Text="Mrs." Value="Mrs"></asp:ListItem>
                                                        <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>
                                                        <asp:ListItem Text="Dr." Value="Dr"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label><a style="color: red">*</a>
                                                    <asp:TextBox ID="txtName" Class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="lblDesignation" runat="server" Text="Designation"></asp:Label><a style="color: red">*</a>
                                                    <asp:TextBox ID="txtDesignation" Class="form-control" onchange="validateDesignation(this)" runat="server"></asp:TextBox>
                                                    <tc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="0" ServiceMethod="GetCompletionListDesignationMaster" ServicePath=""
                                                        TargetControlID="txtDesignation" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                    </tc1:AutoCompleteExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email."
                                                        ControlToValidate="txtEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label><a style="color: red">*</a>
                                                    <asp:TextBox ID="txtEmail" Class="form-control" runat="server"></asp:TextBox>
                                                    <tc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmailMaster" ServicePath=""
                                                        TargetControlID="txtEmail" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                    </tc1:AutoCompleteExtender>
                                                    <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                                                        ControlToValidate="txtEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="lblMobile" runat="server" Text="Mobile No."></asp:Label><a style="color: red">*</a>
                                                    <asp:TextBox ID="txtMob" Class="form-control" runat="server"></asp:TextBox>
                                                    <tc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtMob" FilterType="Numbers" FilterMode="ValidChars"></tc1:FilteredTextBoxExtender>
                                                    <tc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="0" ServiceMethod="GetCompletionListContactNumber" ServicePath=""
                                                        TargetControlID="txtMob" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                    </tc1:AutoCompleteExtender>

                                                </div>

                                                <div class="col-md-3">
                                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label><a style="color: red">*</a>
                                                    <asp:TextBox ID="txtDepartment" Class="form-control" runat="server" onchange="validateDepartment(this)"></asp:TextBox>
                                                    <tc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="0" ServiceMethod="GetCompletionListDepartmentMaster" ServicePath=""
                                                        TargetControlID="txtDepartment" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                    </tc1:AutoCompleteExtender>

                                                </div>

                                                <div class="col-md-3">
                                                    <asp:Label ID="lblGroup" runat="server" Text="Group"></asp:Label><a style="color: red">*</a>
                                                    <asp:DropDownList ID="ddlGroup" Class="form-control" runat="server"></asp:DropDownList>
                                                </div>


                                                <div class="col-md-3">
                                                    <br />
                                                    <asp:Button ID="BtnSaveContact" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="BtnSaveContact_Click" />
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-12" id="div_MobileNo" style="display: none" runat="server">
                                <div class="box box-primary">
                                    <div id="Div_Box_Add1" class="box box-primary">
                                        <div class="box-header with-border">
                                            <h4>
                                                <asp:Literal runat="server" Text="Mobile Numbers" />:</h4>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="Btn_Add_Div1" class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">

                                                <uc1:ContactNo1 runat="server" ID="ContactNo1" />

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
    </div>




    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>



</div>


<script type="text/javascript">
    function resetPosition1(object, args) {


        var tbposition = findPositionWithScrolling(100004);
        var xposition = tbposition[0] + 2;
        var yposition = tbposition[1] + 25;
        var ex = object._completionListElement;

        if (ex)
            $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
    }
    function findPositionWithScrolling1(oElement) {
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



    function alertMe() {
        $("#Li_List").removeClass("active");
        $("#List").removeClass("active");

        $("#Li_New").addClass("active");
        $("#New").addClass("active");
    }

    function calendarShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 100004;
    }



    function AddContact() {
        document.getElementById('div_AddNewContact').style.display = 'block';

        <%--document.getElementById('<%= txtContactList.ClientID %>').value = "";--%>
        document.getElementById('<%= txtName.ClientID %>').focus();
        //window.open("../EMS/ContactMaster.aspx?Page=SINQ");
    }

    function openBalance(customerId) {
        window.open("../CustomerReceivable/CustomerStatement.aspx?Id=" + customerId);
    }









</script>

<script>




    function DisplayMsg(str) {
        alert(str);
        return;
    }


    function Open_Div_Mobile_Number() {
        var NAME = document.getElementById("Btn_Add_Div1");
        var BODY = document.getElementById("Div_Box_Add1");
        var currentClass = NAME.className;
        if (currentClass == "fa fa-plus") { // Check the current class name
            NAME.className = "fa fa-minus";
            BODY.className = "box box-primary";
        } else {
            NAME.className = "fa fa-minus";  // Otherwise, use `second_name`
        }
    }
</script>

<script src="../Script/master.js"></script>
