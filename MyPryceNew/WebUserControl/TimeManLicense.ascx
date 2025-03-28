<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeManLicense.ascx.cs" Inherits="WebUserControl_TimeManLicense" %>
<asp:UpdatePanel runat="server" ID="asd">
    <ContentTemplate>


        <div class="modal-body">


            <div class="container">


                <br />
                <div class="row">
                    <div class="col-md-5">
                        <h4> <asp:Label ID="lblProductCaption" runat="server"></asp:Label>  </h4>
                        <hr />

                         <asp:Label ID="lblProductText" runat="server"></asp:Label>  

                    </div>


                    <div class="col-md-4">
                        <h4>License Information</h4>
                        <hr />
                        <table class="table table-striped">

                            <tr>
                                <td>Registration Code</td>
                                <td>
                                    <asp:Label ID="lblRegistrationcode" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>License Key</td>
                                <td>
                                    <asp:Label ID="lblLicenseKey" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>Expiry Date</td>
                                <td>
                                    <asp:Label ID="lblExpirydate" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>Email ID</td>
                                <td>
                                    <asp:Label ID="lblEmailId" runat="server"></asp:Label></td>
                            </tr>

                              <tr>
                                <td>App Mode</td>
                                <td>
                                    <asp:Label ID="lblAppMode" runat="server"></asp:Label></td>
                            </tr>
                            
                              <tr>
                                <td>Product Code</td>
                                <td>
                                    <asp:Label ID="lblProductCode" runat="server"></asp:Label></td>
                            </tr>


                            <tr>
                                <td>Biometric Device</td>
                                <td>
                                    <asp:Label ID="lblDeviceCount" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>Users</td>
                                <td>
                                    <asp:Label ID="lblUserCount" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>Employees</td>
                                <td>
                                    <asp:Label ID="lblEmployeeCount" runat="server"></asp:Label></td>
                            </tr>



                        </table>



                    </div>




                </div>
                <!---row end--->



            </div>
            <br />
            <br />
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal" onclick="Redirect()">Close</button>
            </div>
        </div>

        </div>
</div>



</div><!---container end--->



        <style>
            * {
                box-sizing: border-box;
            }

            body {
                margin: 0;
                font-family: Arial, Helvetica, sans-serif;
            }

            .header {
                overflow: hidden;
                background-color: #f1f1f1;
                padding: 10px 30px;
            }

                .header a {
                    float: left;
                    color: black;
                    text-align: center;
                    padding: 12px;
                    text-decoration: none;
                    font-size: 18px;
                    border-radius: 4px;
                }

                    .header a.logo {
                        font-size: 25px;
                        font-weight: bold;
                    }

                    .header a:hover {
                        color: black;
                    }

                    .header a.active {
                        background-color: dodgerblue;
                        color: white;
                    }

            .header-right {
                float: right;
            }

            h3 {
                padding-top: 10px;
            }

            p {
                text-align: justify;
            }

            @media screen and (max-width: 500px) {
                .header a {
                    float: none;
                    display: block;
                    text-align: left;
                    font-size: 12px;
                }

                .header-right {
                    float: none;
                }
            }
        </style>


        <script type="text/javascript">

            function Redirect() {
                window.location = "../Dashboard/AttendanceDashboard.aspx";
            }

        </script>





    </ContentTemplate>
</asp:UpdatePanel>
