<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddressControl.ascx.cs"  Inherits="WebUserControl_AddressControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactNo.ascx" TagPrefix="uc1" TagName="ContactNo" %>

<%--<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>--%>

<%--<link rel="stylesheet" href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css">
<link rel="stylesheet" href="../Bootstrap_Files/dist/css/AdminLTE.min.css">--%>

<asp:UpdatePanel ID="Update_Button" runat="server">
    <ContentTemplate>
        <asp:Button ID="Btn_AddList" Style="display: none;" runat="server" Text="Address List" />
        <asp:Button ID="Btn_AddNew" Style="display: none;" runat="server" Text="Address New" />
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
    <asp:UpdatePanel ID="Update_Header" runat="server">
        <ContentTemplate>

            <div class="col-lg-6" style="max-width: 100%;">
                <h3>
                    <img src="../Images/follow-up.png" alt="Address Master" /><asp:Label ID="lblHeaderL" runat="server"></asp:Label>
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

                <li id="Li_AddNew" class="active"><a onclick="Li_Tab_AddNew()" href="#AddNew" data-toggle="tab">
                    <asp:UpdatePanel ID="Update_Li" runat="server">
                        <ContentTemplate>
                            <i class="fa fa-file"></i>&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </a></li>

                <li id="Li_AddList" style="display: none;"><a href="#AddList" onclick="Li_Tab_AddList()" data-toggle="tab">
                    <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List%>"></asp:Label></a></li>

            </ul>

        </div>
    </div>
</div>



<div class="tab-content">

    <div class="tab-pane" id="AddList">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                <asp:HiddenField ID="hdnIsDefault" runat="server" />

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

                                    <asp:UpdatePanel ID="upAddress" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressList" runat="server"  AutoGenerateColumns="False" Width="100%">
                                                
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="GvIBtnAddEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                ImageUrl="~/Images/edit.png" Width="16px" OnCommand="GvIBtnAddEdit_Command" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnDelete_Command" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAddressName" Width="100px" runat="server" Text='<%#Eval("Address_Name") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAddress" runat="server" Text='<%# Eval("Address") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailId %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvEmailId" runat="server" CssClass="labelComman" Text='<%#Eval("EmailId1") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,FaxNo. %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvFaxNo" Width="80px" runat="server" CssClass="labelComman" Text='<%#Eval("FaxNos") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,PhoneNo.%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvPhoneNo" runat="server" CssClass="labelComman" Text='<%#Eval("PhoneNos") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,MobileNo.%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvMobileNo" runat="server" CssClass="labelComman" Text='<%#Eval("MobileNos") %>' /><br />
                                                            <asp:LinkButton ID="BtnMoreNumber" runat="server" ToolTip='<%#Eval("AllNos")%>' Text='<%#Eval("More")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                                        <ItemTemplate>

                                                            <asp:CheckBox ID="chkdefault" runat="server" Checked='<%#Eval("Is_Default") %>' AutoPostBack="true" OnCheckedChanged="chkdefault_CheckedChanged" />

                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                <PagerStyle CssClass="pagination-ys" />
                                                
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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


    <div class="tab-pane active" id="AddNew">
        <asp:UpdatePanel ID="Update_New" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4">
                        <asp:HiddenField ID="Hdn_Address_ID" runat="server" />
                        <asp:Label ID="lblAddressCategory" runat="server" Text="<%$ Resources:Attendance,Address Category %>"></asp:Label>
                        <a style="color: Red">*</a>
                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Add_Save"
                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlAddressCategory" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Address Category %>" />
                        <asp:DropDownList ID="ddlAddressCategory" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <br />
                    </div>

                    <div class="col-md-8">
                        <asp:UpdatePanel ID="Update" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update">
                                    <ProgressTemplate>
                                        <div class="modal_Progress">
                                            <div class="center_Progress">
                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label><a style="color: Red">*</a>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Add_Save"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddressNameNew" ErrorMessage="<%$ Resources:Attendance,Enter Address Name %>" />
                                <asp:HiddenField ID="Hdn_Address_Name" runat="server" />
                                <asp:TextBox ID="txtAddressNameNew" runat="server" CssClass="form-control" onchange="txtAddressNameNew_TextChanged()" BackColor="#eeeeee" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAddressNameNew"
                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                </cc1:AutoCompleteExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </div>

                    <div class="col-md-12">
                        <asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:Attendance,Address %>"></asp:Label><a style="color: Red">*</a>
                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Add_Save"
                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddress" ErrorMessage="<%$ Resources:Attendance,Enter Address Name %>" />
                        <asp:TextBox ID="txtAddress" Style="width: 100%; resize: vertical; max-height: 200px; min-height: 50px; height: 50px;" TextMode="MultiLine" runat="server" CssClass="form-control">
                        </asp:TextBox>
                        <br />
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Country %>"></asp:Label>
                        <asp:DropDownList ID="ddlCountry" onchange="DDLCountry_Change()" runat="server" CssClass="form-control"></asp:DropDownList>
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblState" runat="server" Text="<%$ Resources:Attendance,State %>"></asp:Label>
                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" onchange="txtState_TextChanged()"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetCompletionListStateName" ServicePath="" CompletionInterval="100"
                            MinimumPrefixLength="0" CompletionSetCount="1" TargetControlID="txtState"
                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                        </cc1:AutoCompleteExtender>

                        <br />
                    </div>
                    <div class="col-md-4">

                        <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:Attendance,City %>"></asp:Label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" onchange="txtCity_TextChanged()"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetCompletionListCityName" ServicePath="" CompletionInterval="100"
                            MinimumPrefixLength="0" CompletionSetCount="1" TargetControlID="txtCity"
                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                        </cc1:AutoCompleteExtender>
                        <br />
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lblPinCode" runat="server" Text="<%$ Resources:Attendance,PinCode %>"></asp:Label>
                        <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtPinCode" FilterType="Numbers" FilterMode="ValidChars"></cc1:FilteredTextBoxExtender>

                        <br />
                    </div>


                    <div class="col-md-4">
                        <asp:Label ID="lblEmailId1" runat="server" Text="Email"></asp:Label>
                        <asp:TextBox ID="txtEmailId1" runat="server" CssClass="form-control"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmailMaster" ServicePath=""
                            TargetControlID="txtEmailId1" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                        </cc1:AutoCompleteExtender>
                        <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                            ControlToValidate="txtEmailId1" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                        <br />
                    </div>

                    <div class="row">
                        <br />
                        <div class="col-md-12">


                            <div id="Div_Additional_Info" runat="server" class="box box-primary collapsed-box">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Additional Information</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i id="Btn_Div_Additional_Info" runat="server" class="fa fa-plus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="form-group">



                                        <div class="col-md-12">
                                            <br />
                                        </div>
                                        <uc1:ContactNo runat="server" ID="ContactNo" />

                                        <div class="col-md-4">
                                            <asp:Label ID="lblStreet" runat="server" Text="<%$ Resources:Attendance,Street No.%>"></asp:Label>
                                            <asp:TextBox ID="txtStreet" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblBlock" runat="server" Text="<%$ Resources:Attendance,Block %>"></asp:Label>
                                            <asp:TextBox ID="txtBlock" runat="server" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblAvenue" runat="server" Text="<%$ Resources:Attendance,Avenue %>"></asp:Label>
                                            <asp:TextBox ID="txtAvenue" runat="server" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>

                                        <div class="col-md-6">
                                            <asp:Label ID="lblEmailId2" runat="server" Text="Alternate Email"></asp:Label>
                                            <asp:TextBox ID="txtEmailId2" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server"
                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmailMaster" ServicePath=""
                                                TargetControlID="txtEmailId2" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                            </cc1:AutoCompleteExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email."
                                                ControlToValidate="txtEmailId2" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblWebsite" runat="server" Text="<%$ Resources:Attendance,Website %>"></asp:Label>
                                            <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-12"></div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLongitude" runat="server" Text="<%$ Resources:Attendance,Longitude %>"></asp:Label>
                                            <asp:TextBox ID="txtLongitude" runat="server" onkeypress="return NumberFloatAndOneDOTSign(this)" Enabled="false" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLatitude" runat="server" Text="<%$ Resources:Attendance,Latitude %>"></asp:Label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" onkeypress="return NumberFloatAndOneDOTSign(this)" Enabled="false"></asp:TextBox>
                                                <div class="input-group-btn">
                                                    <asp:Button ID="BtnUpdateLatLong" OnClick="BtnUpdateLatLong_Click" Style="margin-left: 10px;" CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Set Value %>" />
                                                </div>
                                                <div class="input-group-btn">
                                                    <asp:Button ID="btnGetLatLong" OnClick="btnGetLatLong_Click" Style="margin-left: 10px;" CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Map %>" />
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>



                        </div>
                    </div>

                    <asp:HiddenField ID="hdnstateId" runat="server" />
                    <asp:HiddenField ID="hdncityId" runat="server" />

                    <div class="col-md-12" style="text-align: center;">
                        <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnAddressSave" CssClass="btn btn-success" ValidationGroup="Add_Save" OnClick="btnAddressSave_Click" runat="server" Text="<%$ Resources:Attendance,Save %>" />
                                <asp:Button ID="btnAddressReset" Style="margin-left: 15px;" CausesValidation="False" OnClick="btnAddressReset_Click" CssClass="btn btn-primary" runat="server"
                                    Text="<%$ Resources:Attendance,Reset %>" />
                                <asp:Button ID="btnAddressCancel" Style="margin-left: 15px;" Visible="false" CausesValidation="False" OnClick="btnAddressCancel_Click" CssClass="btn btn-danger" runat="server"
                                    Text="<%$ Resources:Attendance,Cancel %>" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
                            <ProgressTemplate>
                                <div class="modal_Progress">
                                    <div class="center_Progress">
                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <asp:HiddenField ID="editid" runat="server" />
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
    function Div_Additional_Info_Open() {
        $("#Btn_Div_Additional_Info").removeClass("fa fa-plus");
        $("#Div_Additional_Info").removeClass("box box-primary collapsed-box");

        $("#Btn_Div_Additional_Info").addClass("fa fa-minus");
        $("#Div_Additional_Info").addClass("box box-primary");
    }

    function Li_Tab_AddList() {
        $("#Li_AddList").removeClass("active");
        $("#AddList").removeClass("active");

        $("#Li_AddNew").addClass("active");
        $("#AddNew").addClass("active");
    }
    function Li_Tab_AddNew() {
        $("#Li_AddNew").removeClass("active");
        $("#AddNew").removeClass("active");

        $("#Li_AddList").addClass("active");
        $("#AddList").addClass("active");
    }

    function DispMsg(data) {
        alert(data);
    }

    function displayList() {
        var data = document.getElementById("Li_AddList");
        data.style.display = data.style.display = '';
    }

    function DDLCountry_Change() {
        var ddlcountry = document.getElementById('<%= ddlCountry.ClientID %>');
        if (ddlcountry.value != "--Select--") {
            PageMethods.ddlCountry_IndexChanged(ddlcountry.value, onSuccessChange, ErrorOnChange);
        }
    }
    function onSuccessChange(data) {
        document.getElementById('<%= txtState.ClientID %>').value = "";
        document.getElementById('<%= txtState.ClientID %>').focus();
        document.getElementById('<%= txtCity.ClientID %>').value = "";
        var contactno = document.getElementById('<%= ContactNo.FindControl("txtCountryCode").ClientID %>').value = data;
    }
    function ErrorOnChange(error) {
        alert(error.getMessage());
    }

    function txtCity_TextChanged() {
        debugger;
        var stateId = document.getElementById('<%= hdnstateId.ClientID %>').value;
        var CityName = document.getElementById('<%= txtCity.ClientID %>').value;
        if (CityName == "")
        {
            return;
        }
        if (document.getElementById('<%= txtState.ClientID %>').value == "") {
            DispMsg("Please Select State");
            document.getElementById('<%= txtState.ClientID %>').focus();
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            return;
        }
        if (stateId == "0" || stateId == "") {
            DispMsg("Please Select State");
            document.getElementById('<%= txtState.ClientID %>').focus();
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            return;
        }
        PageMethods.txtCity_TextChanged(stateId, CityName, onSuccess_CityChange, ErrorOnChange);
    }
    function onSuccess_CityChange(CityId) {
        if (CityId == "" || CityId == "0") {
            DispMsg('Select From Suggestions Only');
            document.getElementById('<%= txtCity.ClientID %>').focus();
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            return;
        }
        document.getElementById('<%= hdncityId.ClientID %>').value = CityId;
    }

    function txtState_TextChanged() {
        debugger;
        var stateName = document.getElementById('<%= txtState.ClientID %>').value;
        if (stateName == "")
        {
            PageMethods.resetAddress();
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            return;
        }
        var CountryId = document.getElementById('<%= ddlCountry.ClientID %>').value;
        if (CountryId != "--Select--") {
            PageMethods.txtState_TextChanged(CountryId, stateName, onSuccess_StateChange, ErrorOnChange);
        }
        else {
            DispMsg('Please Select Country');
            document.getElementById('<%= txtState.ClientID %>').focus();
            document.getElementById('<%= txtState.ClientID %>').value = "";
            return;
        }

    }

    function onSuccess_StateChange(StateId) {
        if (StateId == "" || StateId == "0") {
            DispMsg('Select From Suggestions Only');
            document.getElementById('<%= txtState.ClientID %>').focus();
            document.getElementById('<%= txtState.ClientID %>').value = "";
        }
        document.getElementById('<%= txtCity.ClientID %>').value = "";
        document.getElementById('<%= hdnstateId.ClientID %>').value = StateId;
    }
    function txtAddressNameNew_TextChanged() {
        var addressName = document.getElementById('<%= txtAddressNameNew.ClientID %>');
        var addressid = document.getElementById('<%= Hdn_Address_ID.ClientID %>');

        if (addressid == "@NOTFOUND@") {
            addressid = "";
        }

        if (addressName.value != "") {
            PageMethods.txtAddressNameNew_TextChanged(addressName.value, addressid.value, OnSuccess_txtAddressNameNew, ErrorOnChange);
        }
    }

    function OnSuccess_txtAddressNameNew(data) {
        if (data == 1) {
            DispMsg("Address Name Already Exists");
            document.getElementById('<%= txtAddressNameNew.ClientID %>').focus();
            document.getElementById('<%= txtAddressNameNew.ClientID %>').value = "";
        }
    }
</script>
