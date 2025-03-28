<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CloudRegistration.aspx.cs" Inherits="CloudRegistration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
<head>
    <meta charset="UTF-8">
    <title>Pryce Registration</title>
    <link rel="shortcut icon" href="Images/favicon.ico" type="image/x-icon">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no, minimum-scale=1.0, maximum-scale=1.0" />
    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/jquery/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <link href="http://104.238.86.46:85/phomello_cloud/assets/javascript/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <link href="http://104.238.86.46:85/phomello_cloud/assets/javascript/summernote/summernote.css" rel="stylesheet" type="text/css" />
    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/summernote/summernote.js" type="text/javascript"></script>
    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/summernote/lang/summernote-ar-AR.js" type="text/javascript"></script>

    <link href="http://104.238.86.46:85/phomello_cloud/assets/stylesheet/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="http://104.238.86.46:85/phomello_cloud/assets/stylesheet/stylesheet.css" rel="stylesheet" type="text/css" />
    <link rel='stylesheet' id='sydney-font-awesome-css' href='https://pryceerp.com/wp-content/themes/sydney/fonts/font-awesome.min.css?ver=4.8.6' type='text/css' media='all' />



    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/jquery/datetimepicker/moment.js" type="text/javascript"></script>
    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/jquery/datetimepicker/bootstrap-datetimepicker.min.js" type="text/javascript"></script>
    <link href="http://104.238.86.46:85/phomello_cloud/assets/javascript/jquery/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />

    <link rel='stylesheet' id='sydney-style-css' href='https://pryceerp.com/wp-content/themes/sydney/style.css?ver=20170504' type='text/css' media='all' />



    <link rel='stylesheet' id='sydney-style-css' href='https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css' type='text/css' media='all' />

    <link href="http://104.238.86.46:85/phomello_cloud/assets/javascript/lightbox/ekko-lightbox.min.css" rel="stylesheet" type="text/css" />
    <!--        <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/angular/angular.min.js" type="text/javascript"></script>-->

    <!--        <link href="http://104.238.86.46:85/phomello_cloud/assets/javascript/owl-carousel/owl.carousel.css" rel="stylesheet" type="text/css"/>
        <link href="http://104.238.86.46:85/phomello_cloud/assets/javascript/owl-carousel/owl.transitions.css" rel="stylesheet" type="text/css"/>-->

    <!--        <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/owl-carousel/owl.carousel.min.js" type="text/javascript"></script>-->

    <script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/common.js" type="text/javascript"></script>
</head>
<body>

    <form runat="server">


        <div class="container" style="margin-top: 10px;">
            <div class="row">
                <div class="col-lg-10">
                    <h1><a href="http://www.Pryce.Us/">
                        <img src="https://pryceerp.com/wp-content/uploads/2017/10/Pryce-1.png" alt="Pryce Cloud" title="Pryce Cloud" width="200px" /></a></h1>
                    <h4 style="color: #333;"></h4>
                </div>

                <div class="col-lg-2">
                    </br>
                  <%--  <select onchange="javascript:window.location.href = 'http://104.238.86.46:85/phomello_cloud/index.php/language/language/index/' + this.value;">
                        <option value="english">English</option>
                        <option value="arabic">Arabic</option>
                    </select>--%>
                </div>
            </div>
            <hr>
        </div>


        <div class="container">

            <!--    <div class="jumbotron">
            <h1>Get started</h1>
            <p>Try Phomello free for 30 days. No contracts. No commitments. No credit card</p>
        </div>-->

            <div>
                <img src="http://www.pegasustech.net/image/cache/catalog/banners/timeMan-1500x500.jpeg" class="img-responsive" />
            </div>
            <br />
            <br />

            <div class="jumbotron">
                <h1>Start your free 30 day trial now.</h1>
                <p>No credit card. No commitment. Just a few quick questions to set up your trial.</p>
            </div>
            <br />
            <div class="row">

                <asp:UpdatePanel ID="UPD" runat="server">
                    <ContentTemplate>

                        <div class="col-md-6">


                            <div class="form-group">
                                <asp:Label ID="Label4" class="col-sm-2 control-label" runat="server" Text="Company Name" Font-Bold="true" Font-Size="16px"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Company Name"></asp:TextBox>


                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompanyName" ErrorMessage="Enter Company Name" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>



                            </div>

                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <asp:Label ID="Label1" class="col-sm-2 control-label" runat="server" Text="Contact Person" Font-Bold="true" Font-Size="16px"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" placeholder="Contact Person"></asp:TextBox>

                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtContactPerson" ErrorMessage="Enter Contact Person Name" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>


                            </div>
                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <asp:Label ID="Label2" class="col-sm-2 control-label" runat="server" Text="Email" Font-Bold="true" Font-Size="16px"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" placeholder="Email-Id"></asp:TextBox>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmailId" ErrorMessage="Enter Email-Id" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>


                            </div>
                            <br />
                            <br />
                            <br />

                            <div class="form-group">
                                <asp:Label ID="Label3" class="col-sm-2 control-label" runat="server" Text="Country" Font-Bold="true" Font-Size="16px"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>

                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" Height="47px" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                        Display="Dynamic" InitialValue="--Select--" SetFocusOnError="true" ControlToValidate="ddlCountry" ErrorMessage="Select Country" ForeColor="Red"></asp:RequiredFieldValidator>



                                </div>


                            </div>
                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <asp:Label ID="Label5" class="col-sm-2 control-label" runat="server" Text="Mobile Number" Font-Bold="true" Font-Size="16px"></asp:Label>
                                <div class="col-sm-3">

                                    <asp:TextBox ID="txtCountryCode" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>

                                </div>
                                <div class="col-sm-7">

                                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" placeholder="Mobile Number"></asp:TextBox>

                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: left;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMobileNo" ErrorMessage="Enter Mobile Number" ForeColor="Red"></asp:RequiredFieldValidator>

                                    <cc1:FilteredTextBoxExtender ID="txtMobileNo_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" TargetControlID="txtMobileNo" FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </div>


                            </div>
                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <label class="col-sm-2 control-label"></label>
                                <div class="col-sm-10">
                                    <asp:Button ID="btnRegister" runat="server" Text="Register" class="btn btn-block btn-primary" ValidationGroup="Save" OnClick="btnRegister_Click" />
                                    <%--<button type="submit" name="save" class="btn btn-block btn-primary">Register</button>--%>
                                </div>
                            </div>



                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>



                <div class="col-md-6">
                    <div class="jumbotron">

                        <h2>TimeMan Enterprise Solution</h2>
                        <p>TimeMan is a web based, Reliable, Fast & Easy to use attendance Solution. This Solution is our core retail,restaurant and government operations offering. It is flexible to use and can easily be set up to support a broad variety of attendance concepts and workflows. Our customers range is restaurants , schools, offices hospitals and so on where is need of employee attendance.. It is so easy to use that any common man can use it.</p>

                        <p>Apart of the attendance record it provides you, automated emails for reports,	automated emails for late employee, easy generates report, users control & privilege, permission & approval facility, real time reports etc. It also supports multiple languages, multiple brands & divisions. Its simple approach, ease of use and rock-solid reliability makes TimeMan a great Solution for your needs.</p>
                    </div>




                </div>

            </div>
        </div>


        <script type="text/javascript">
            $('select[name=\'project_id\']').val(5);
            $('#andorid').hide();
            $('#lite').hide();
            $('#enterprise').hide();

            $('select[name=\'project_id\']').on('change', function () {

                if (this.value == 5) {
                    $('#andorid').show();
                    $('#lite').hide();
                    $('#enterprise').hide();
                }
                if (this.value == 6) {
                    $('#andorid').hide();
                    $('#lite').show();
                    $('#enterprise').hide();
                }
                if (this.value == 18) {
                    $('#andorid').hide();
                    $('#lite').hide();
                    $('#enterprise').show();
                }
            });

            $('select[name=\'project_id\']').trigger('change');

            $('select[name=\'country_id\']').on('change', function () {
                $.ajax({
                    url: 'http://104.238.86.46:85/phomello_cloud/index.php/location/country/autocomplete?country_id=' + this.value,
                    dataType: 'json',
                    beforeSend: function () {
                        $('select[name=\'country_id\']').after(' <i class="fa fa-circle-o-notch fa-spin"></i>');
                    },
                    complete: function () {
                        $('.fa-spin').remove();
                    },
                    success: function (json) {
                        html = '<option value="">---Select---</option>';
                        if (json['zone'] && json['zone'] != '') {
                            $('input[name=\'mobile_no_prefix\']').val("(+" + json['isd_code'] + ")");
                            for (i = 0; i < json['zone'].length; i++) {
                                html += '<option value="' + json['zone'][i]['zone_id'] + '"';
                                if (json['zone'][i]['zone_id'] == '') {
                                    html += ' selected="selected"';
                                }
                                html += '>' + json['zone'][i]['name'] + '</option>';
                            }
                        } else {
                            html += '<option value="0" selected="selected">---None---</option>';
                        }

                        $('select[name=\'zone_id\']').html(html);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                    }
                });
            });

            $('select[name=\'country_id\']').trigger('change');
        </script>


        <div>
            <hr />
            <div id="sidebar-footer" class="footer-widgets widget-area" role="complementary">
                <div class="container">
                    <div class="sidebar-column col-md-4">
                        <aside id="sydney_video_widget-2" class="widget sydney_video_widget_widget">
                            <div class="sydney-video vid-normal">
                                <div class="video-overlay">
                                    <div class="sydney-video-inner">
                                        <span class="close-popup"><i class="fa fa-times"></i></span>
                                        <div class="fluid-width-video-wrapper" style="padding-top: 56.2393%;">
                                            <iframe src="https://www.youtube.com/embed/L4OFy-cQmzc?feature=oembed" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen="" id="fitvid51338"></iframe>
                                        </div>
                                    </div>
                                </div>
                                <div class="video-text"></div>
                                <a href="#" class="toggle-popup"><i class="fa fa-play"></i></a>
                            </div>
                        </aside>
                        <aside id="pages-5" class="widget widget_pages">
                            <h3 class="widget-title">Solutions</h3>
                            <ul>
                                <li class="page_item page-item-40"><a href="https://pryceerp.com/erp-solutions/distributors-industry/">Distributors Industry</a></li>
                                <li class="page_item page-item-685"><a href="https://pryceerp.com/erp-solutions/erp-education-industry/">Education Industry</a></li>
                                <li class="page_item page-item-36"><a href="https://pryceerp.com/erp-solutions/erp-e-governance/">ERP for e-Governance</a></li>
                                <li class="page_item page-item-683"><a href="https://pryceerp.com/erp-solutions/erp-for-construction-real-estate-industry/">ERP for Real Estate</a></li>
                                <li class="page_item page-item-677"><a href="https://pryceerp.com/erp-solutions/erp-for-warehouse-management/">ERP for Warehouse</a></li>
                                <li class="page_item page-item-32"><a href="https://pryceerp.com/erp-solutions/healthcare-industry/">Healthcare Industry</a></li>
                                <li class="page_item page-item-753"><a href="https://pryceerp.com/erp-solutions/erp-hospitality-industry/">Hospitality Industry</a></li>
                                <li class="page_item page-item-30"><a href="https://pryceerp.com/erp-solutions/erp-manufacturing-industry/">Manufacturing Industry</a></li>
                                <li class="page_item page-item-34"><a href="https://pryceerp.com/erp-solutions/oil-gas-industry/">Oil &amp; Gas Industry</a></li>
                                <li class="page_item page-item-679"><a href="https://pryceerp.com/erp-solutions/erp-for-professional-services/">Professional Services</a></li>
                                <li class="page_item page-item-28"><a href="https://pryceerp.com/erp-solutions/retail-industry/">Retail Industry</a></li>
                                <li class="page_item page-item-38"><a href="https://pryceerp.com/erp-solutions/erp-telecommunication-industry/">Telecommunication Industry</a></li>
                                <li class="page_item page-item-681"><a href="https://pryceerp.com/erp-solutions/erp-for-textiles-apparels-industry/">Textiles &amp; Apparels Industry</a></li>
                            </ul>
                        </aside>
                    </div>

                    <div class="sidebar-column col-md-4">
                        <aside id="text-2" class="widget widget_text">
                            <div class="textwidget">
                                <p>Pryce is an enterprise resource planning (ERP) solution developed by Pegasus. It is designed for midsize to large enterprises in a wide range of industries, including retail, construction, manufacturing, education, HR management and professional services among others. Pryce presents you comprehensive &amp; leading-edge business solutions for amplification and further growth of your business.</p>
                            </div>
                        </aside>
                        <aside id="pages-4" class="widget widget_pages">
                            <h3 class="widget-title">Products</h3>
                            <ul>
                                <li class="page_item page-item-590"><a href="https://pryceerp.com/erp-modules/business-intelligence-erp/">Business Intelligence</a></li>
                                <li class="page_item page-item-26"><a href="https://pryceerp.com/erp-modules/customer-relationship-management/">CRM</a></li>
                                <li class="page_item page-item-22"><a href="https://pryceerp.com/erp-modules/finance-accounting-management/">Finance Management</a></li>
                                <li class="page_item page-item-16"><a href="https://pryceerp.com/erp-modules/human-resource-management/">HR Management</a></li>
                                <li class="page_item page-item-20"><a href="https://pryceerp.com/erp-modules/production-management/">Production Management</a></li>
                                <li class="page_item page-item-24"><a href="https://pryceerp.com/erp-modules/project-management/">Project Management</a></li>
                                <li class="page_item page-item-18"><a href="https://pryceerp.com/erp-modules/supply-chain-management/">SCM</a></li>
                                <li class="page_item page-item-592"><a href="https://pryceerp.com/erp-modules/service-management-erp/">Service Management</a></li>
                            </ul>
                        </aside>
                    </div>

                    <div class="sidebar-column col-md-4">
                        <aside id="sydney_contact_info-2" class="widget sydney_contact_info_widget">
                            <h3 class="widget-title">Get in touch</h3>
                            <div class="contact-address"><span><i class="fa fa-home"></i></span>Pegasus Turnkey Solution Private Limited. Saheli Nagar, Udaipur, 313005 (Rajasthan) India</div>
                            <div class="contact-phone"><span><i class="fa fa-phone"></i></span>(+91) 9413138674</div>
                            <div class="contact-email"><span><i class="fa fa-envelope"></i></span><a href="mailto:Salesin1@PegasusTech.net">Salesin1@PegasusTech.net</a></div>
                        </aside>
                        <aside id="pages-6" class="widget widget_pages">
                            <h3 class="widget-title">Hardware</h3>
                            <ul>
                                <li class="page_item page-item-48"><a href="https://pryceerp.com/hardware/">Hardware</a></li>
                            </ul>
                        </aside>
                        <aside id="pages-7" class="widget widget_pages">
                            <h3 class="widget-title">Important Links</h3>
                            <ul>
                                <li class="page_item page-item-157"><a href="https://pryceerp.com/about-us/">About Us</a></li>
                                <li class="page_item page-item-44"><a href="https://pryceerp.com/contact-us/">Contact Us</a></li>
                                <li class="page_item page-item-741"><a href="https://pryceerp.com/privacy-policy/">Privacy Policy</a></li>
                                <li class="page_item page-item-254"><a href="https://pryceerp.com/pryce-erp-blog/">Pryce ERP Blog</a></li>
                                <li class="page_item page-item-743"><a href="https://pryceerp.com/terms-and-conditions/">Terms and Conditions</a></li>
                            </ul>
                        </aside>
                    </div>


                </div>
            </div>
            <footer id="colophon" class="site-footer" role="contentinfo">
                <div class="site-info container">
                    <a href="">All Rights Reserved PryceERP.com</a>
                    <span class="sep">| </span>
                    Copyright © <a href="" rel="designer"></a>PryceERP.com.	
                </div>
                <!-- .site-info -->
            </footer>
        </div>

    </form>

</body>
</html>

<script src="http://104.238.86.46:85/phomello_cloud/assets/javascript/lightbox/ekko-lightbox.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function ($) {

        // delegate calls to data-toggle="lightbox"
        $(document).delegate('*[data-toggle="lightbox"]:not([data-gallery="navigateTo"])', 'click', function (event) {
            event.preventDefault();
            return $(this).ekkoLightbox({
                onShown: function () {
                    if (window.console) {
                        return console.log('onShown event fired');
                    }
                },
                onContentLoaded: function () {
                    if (window.console) {
                        return console.log('onContentLoaded event fired');
                    }
                },
                onNavigate: function (direction, itemIndex) {
                    if (window.console) {
                        return console.log('Navigating ' + direction + '. Current item: ' + itemIndex);
                    }
                }
            });
        });

        //Programatically call
        $('#open-image').click(function (e) {
            e.preventDefault();
            $(this).ekkoLightbox();
        });
        $('#open-youtube').click(function (e) {
            e.preventDefault();
            $(this).ekkoLightbox();
        });

        $(document).delegate('*[data-gallery="navigateTo"]', 'click', function (event) {
            event.preventDefault();
            return $(this).ekkoLightbox({
                onShown: function () {
                    var lb = this;
                    $(lb.modal_content).on('click', '.modal-footer a#jumpit', function (e) {
                        e.preventDefault();
                        lb.navigateTo(2);
                    });
                    $(lb.modal_content).on('click', '.modal-footer a#closeit', function (e) {
                        e.preventDefault();
                        lb.close();
                    });
                }
            });
        });

    });
</script>



