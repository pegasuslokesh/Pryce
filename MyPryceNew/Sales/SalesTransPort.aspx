<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesTransPort.aspx.cs" Inherits="Sales_SalesTransPort" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/Reminder.ascx" TagPrefix="uc1" TagName="Reminder1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Transport</title>
  <link rel="SHORTCUT ICON" href="Images/favicon.ico" />
<link href="../Bootstrap_Files/Additional/Ajax_Tab.css" rel="stylesheet" type="text/css" />
<link href="../CSS/controlsCss.css" rel="stylesheet" />
<link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
<link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" type="text/css" />
<link href="../CSS/InvStyle.css" rel="stylesheet" />
<!-- Tell the browser to be responsive to screen width -->
<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
<!-- DataTable Jquery  -->
<script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>

<script src="//cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"> </script>
<script src="https://cdn.datatables.net/1.10.19/js/dataTables.bootstrap.min.js"> </script>
<script src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js" type="text/javascript"></script>
<script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js" type="text/javascript"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js" type="text/javascript"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js" type="text/javascript"></script>
<script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js" type="text/javascript"></script>
<script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>
    

<link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />


<!-- Font Awesome -->
<link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css"
    rel="stylesheet" type="text/css" />




    <style type="text/css">
        .pager {
            LI_Edit_Active background-color: #3AC0F2;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .active_pager {
            background-color: #2E99C1;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .grid td, .grid th {
            text-align: center;
        }

        tooltip-inner {
            max-width: none;
            white-space: nowrap;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        function showAlert(message, bgColor, textColor) {
            // Create the alert div
            var alertDiv = document.createElement("div");
            alertDiv.style.position = "fixed";
            alertDiv.style.top = "10px";
            alertDiv.style.right = "10px";
            alertDiv.style.backgroundColor = bgColor;
            alertDiv.style.color = textColor;
            alertDiv.style.padding = "15px";
            alertDiv.style.borderRadius = "5px";
            alertDiv.style.boxShadow = "0 0 10px rgba(0, 0, 0, 0.1)";
            alertDiv.style.zIndex = "1000";
            alertDiv.style.fontSize = "16px";
            alertDiv.style.fontFamily = "Arial, sans-serif";
            alertDiv.innerText = message;

            // Add the alert div to the body
            document.body.appendChild(alertDiv);

            // Remove the alert after a few seconds
            setTimeout(function () {
                document.body.removeChild(alertDiv);
            }, 3000);
        }
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function Li_Import_Active() {

        }
        function resetPosition() {

        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
        function closeWindow(Message) {
            alert(Message);
            window.close(); // Close the current window
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
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/customer.js"></script>
 
</head>
<body>
   <div class="wraper">
       <form id="Form_Transport" runat="server" enctype="multipart/form-data">
                  <asp:ScriptManager ID="SM1" runat="server" EnablePartialRendering="true" EnablePageMethods="true" AsyncPostBackTimeout="36000">
        </asp:ScriptManager>

           <div class="row">
               <<div class="col-md-12">
                  <asp:UpdatePanel ID="UpdateNewTransporrt" runat="server">
                      <ContentTemplate>
                          <div class="container">
                              <div class="row">
                                  <div class="col-md-12">
                                      <label id="lblModalInvoiceNo" runat="server">Invoice No.</label>
                                      <label id="txtModalInvoiceNo" runat="server"></label>
                                  </div>
                              </div>
                              <div class="row">
                                  <div class="col-md-12 text-center">
                                      <asp:RadioButton ID="chkModalCustomer" runat="server" Checked="true" OnCheckedChanged="ChkModalTrans_Changed" AutoPostBack="true" GroupName="Trans" Text="Courier" />
                                      &nbsp;
                               <asp:RadioButton ID="chkModalEmployee" runat="server" OnCheckedChanged="ChkModalTrans_Changed" AutoPostBack="true" GroupName="Trans" Text="Employee" />
                                  </div>
                              </div>
                              <div class="row" id="pnlModalCustomer" runat="server">
                                  <div class="col-md-12">
                                  <div class="col-md-12">
                                      <asp:Label ID="lblModalCourierService" runat="server" Text="Courier Services"></asp:Label>
                                      <a style="color: Red">*</a>
                                      <asp:TextBox ID="txtModalcustomername" runat="server" BackColor="#eeeeee" AutoPostBack="true" CssClass="form-control"></asp:TextBox>

                                      <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                          Enabled="True" ServiceMethod="GetCompletionContactList" ServicePath="" CompletionInterval="100"
                                          MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtModalcustomername"
                                          UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                      </cc1:AutoCompleteExtender>


                                      <br />
                                  </div>
                                  <div class="col-md-6">
                                      <asp:Label ID="lblModalPermanentMobileNo" runat="server" Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                      <asp:TextBox ID="txtModalPermanentMobileNo" runat="server" CssClass="form-control" />
                                      <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                          TargetControlID="txtModalPermanentMobileNo" ValidChars="+,0,1,2,3,4,5,6,7,8,9">
                                      </cc1:FilteredTextBoxExtender>
                                      <br />
                                  </div>
                                  <div class="col-md-6">
                                      <asp:Label ID="lblModalAreaName" runat="server" Text="<%$ Resources:Attendance,Area Name  %>"></asp:Label>
                                      <asp:TextBox ID="txtModalAreaName" TabIndex="104" BackColor="#eeeeee" runat="server" CssClass="form-control" />
                                      <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                          Enabled="True" ServiceMethod="GetCompletionListAreaName" ServicePath="" CompletionInterval="100"
                                          MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtModalAreaName"
                                          UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                      </cc1:AutoCompleteExtender>
                                      <asp:HiddenField ID="hdnSalesId" runat="server" Value="0" />
                                      <br />
                                  </div>
                                  <div class="col-md-6">
                                      <asp:Label ID="lblModalPersonName" runat="server" Text="Person Name"></asp:Label>
                                      <asp:TextBox ID="txtModalPersonName" runat="server" CssClass="form-control"></asp:TextBox>
                                      <br />
                                  </div>
                                  <div class="col-md-6">
                                      <asp:Label ID="lblModalPersonMobileNo" runat="server" Text="Person Mobile No."></asp:Label>
                                      <asp:TextBox ID="txtModalPersonMobileNo" runat="server" CssClass="form-control"></asp:TextBox>
                                      <br />
                                  </div>
                                      </div>
                              </div>
                              <div class="row" id="pnlModalEmployee" visible="false" runat="server">
                                  <div class="col-md-12">
                                      <div class="col-md-6">
                                          <asp:Label ID="lblModalVisitDate" runat="server" Text="<%$ Resources:Attendance,Visit Date %>"></asp:Label>
                                          <a style="color: Red">*</a>
                                          <asp:TextBox ID="txtModalVisitDate" runat="server" CssClass="form-control" />
                                          <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtModalVisitDate">
                                          </cc1:CalendarExtender>
                                          <br />
                                      </div>
                                      <div class="col-md-6">
                                          <asp:Label ID="lblModalVisitTime" runat="server" Text="<%$ Resources:Attendance,Visit Time %>"></asp:Label>
                                          <a style="color: Red">*</a>
                                         <asp:TextBox ID="txtModalVisitTime" runat="server" CssClass="form-control" />
                                          <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                              CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                              CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                              Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtModalVisitTime"
                                              UserTimeFormat="TwentyFourHour">
                                          </cc1:MaskedEditExtender>
                                          <br />
                                      </div>
                                      <div class="col-md-6">
                                          <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance, Employee Name %>"></asp:Label>
                                          <a style="color: Red">*</a>
                                          <asp:TextBox ID="txtModalDriverName" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                              OnTextChanged="TxtModaldrivername_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                              TabIndex="5"></asp:TextBox>
                                          <cc1:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                              Enabled="True" ServiceMethod="GetCompletionListDriverName" ServicePath="" CompletionInterval="100"
                                              MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtModalDriverName"
                                              UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                          </cc1:AutoCompleteExtender>
                                          <br />
                                      </div>
                                      <div class="col-md-6">
                                          <asp:Label ID="lblModalVichleName" runat="server" Text="<%$ Resources:Attendance,Vehicle Name %>" />
                                          <a style="color: Red">*</a>
                                          <asp:TextBox ID="txtModalvehiclename" runat="server" AutoPostBack="true"
                                              OnTextChanged="TxtModalvehiclename_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                          <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                              Enabled="True" ServiceMethod="GetCompletionListVehicleName" ServicePath="" CompletionInterval="100"
                                              MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtModalvehiclename"
                                              UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                          </cc1:AutoCompleteExtender>
                                          <br />
                                      </div>
                                  </div>
                              </div>
                              <div class="row">
                                  <div class="col-md-12">
                                      <div class="col-md-6">
                                          <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Chargeable Amount %>"></asp:Label>
                                          <asp:TextBox ID="txtModalChargableAmount" runat="server" CssClass="form-control" />
                                          <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                              TargetControlID="txtModalChargableAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                          </cc1:FilteredTextBoxExtender>
                                          <br />
                                      </div>
                                      <div class="col-md-6">
                                          <asp:Label ID="lblModalTrackId" runat="server" Text="Tracking ID"></asp:Label>
                                          <asp:TextBox ID="txtModalTrakingId" runat="server" CssClass="form-control"></asp:TextBox>
                                      </div>
                                      <div class="col-md-12">
                                          <asp:Label ID="Label42" runat="server" Text="Description"></asp:Label>
                                          <asp:TextBox ID="txtModaldescription" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                          <br />
                                      </div>
                                  </div>
                                  <div class="row">
                                      <div class="col-md-12 text-center">
                                          <asp:Button ID="btnModalTransportSave" ValidationGroup="Save" CssClass="btn btn-success" runat="server" Text="Save" OnClick="btnModalTransportSave_Click" />
                                          <asp:Button ID="btnModalTransportReset" CssClass="btn btn-danger" runat="server" Text="Reset" OnClick="btnModalTransPortReset_Click" />
                                      </div>
                                  </div>

                              </div>
                          </div>
                         

                      </ContentTemplate>
                      </asp:UpdatePanel>
                </div>
           </div>
       </form>
   </div>
</body>
</html>
