function setReportData(moduleId, objectId) {
    debugger;

    //var _location = document.location.toString();
    //var applicationNameIndex = _location.indexOf('/', _location.indexOf('://') + 3);
    //var applicationName = _location.substring(0, applicationNameIndex) + '/';
    //var webFolderIndex = _location.indexOf('/', _location.indexOf(applicationName) + applicationName.length);
    //var webFolderFullPath = _location.substring(0, webFolderIndex);


    //var host = window.location.host;
    //alert(host);
    //var base_url = '@Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)@Request.ApplicationPath'

    //// navigate around the site, base url should be the same
   // alert(base_url);

    //var orig = window.location.origin;
    //alert(orig);

    var url = '..' + window.location.pathname;
    //var url = '..' + window.location.pathname.substring(6, window.location.pathname.length);
    alert(url);
    //alert(url1);
    //debugger;
    //url = url.split('/').r
    $.ajax({
        url: '../webservices/master.asmx/getObjectIdNModuleIDByURL',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'url':'" + url + "'}",
        success: function (val) {

            $.ajax({
                url: '../webservices/reportSystem.asmx/getReportData',
                method: 'post',
                async: true,
                contentType: "application/json; charset=utf-8",
                data: "{'moduleId':'" + val.d[0] + "','objectId':'" + val.d[1] + "'}",
                success: function (reportData) {
                    if (reportData.d != "") {

                        var data = JSON.parse(reportData.d);

                        $('#tblReport tbody tr').remove();
                        $('#ReportSystem').modal('show');

                        $(data).each(function () {
                            var row = $(this)[0];
                            var htmlRow = "<tr>";
                            htmlRow = htmlRow + "<td>" + row.ReportId + "</td>";
                            htmlRow = htmlRow + "<td><span>" + row.DisplayName + "</span>  <input type='hidden' id='moduleId' value='" + row.ModuleId + "'> <input type='hidden' id='objectId' value='" + row.objectId + "' > </td>";
                            htmlRow = htmlRow + "<td><input type='checkbox' " + row.isdefault + " id='chkDefault' onchange='setDefaultReport(this," + row.ReportId + "," + row.ModuleId + "," + row.objectId + ")' /></td>";
                            htmlRow = htmlRow + "</tr>";
                            $('#tblReport > tbody:last-child').append(htmlRow);
                        });                       
                    }
                    else {
                        alert("no report assigned for this module");
                    }

                    $("#prgBar").css("display", "none");
                }
            });



        }
    });



}





function setDefaultReport(ctrl, reportID, moduleId, objectId) {
    var reportId = '';
    var removeDefault = '';
    $('#tblReport tbody tr').each(function () {
        var row = $(this)[0];
        if (row.cells[2].childNodes[0] != ctrl) {
            if (row.cells[2].childNodes[0].checked) {
                removeDefault = row.cells[0].innerHTML;
            }
        }
        row.cells[2].childNodes[0].checked = false;
    });
    ctrl.checked = true;

    $.ajax({
        url: '../webservices/reportSystem.asmx/setDefaultReport',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'check':'" + ctrl.checked + "','reportId':'" + reportID + "','moduleId':'" + moduleId + "','objectId':'" + objectId + "','removeDefault':'" + removeDefault + "'}",
        success: function (reportData) { }
    });
}