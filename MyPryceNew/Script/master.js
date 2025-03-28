function validateDesignation(ctrl) {
    if (ctrl.value == "") {
        return;
    }
    var designationId = ctrl.value.split('/')[1];
    var designationName = ctrl.value.split('/')[0];

    if (isNaN(designationId)) {
        alert("Please Select Valid Designation")
        ctrl.value = "";
        ctrl.focus();
        return;
    }
    $.ajax({
        url: '../webservices/master.asmx/validateDesignation',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'designationName':'" + designationName + "','designationId':'" + designationId + "'}",
        success: function (data) {
            if (!data.d) {
                alert("Please Select From Suggestions Only !!");
                ctrl.value = "";
                ctrl.focus();
                return;
            }
        }
    });
}

function validateDepartment(ctrl) {
    if (ctrl.value == "") {
        return;
    }
    var departmentId = ctrl.value.split('/')[1];
    var departmentName = ctrl.value.split('/')[0];

    if (isNaN(departmentId)) {
        alert("Please Select Valid Department")
        ctrl.value = "";
        ctrl.focus();
        return;
    }
    $.ajax({
        url: '../webservices/master.asmx/validateDepartment',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'departmentName':'" + departmentName + "','departmentId':'" + departmentId + "'}",
        success: function (data) {
            if (!data.d) {
                alert("Please Select From Suggestions Only !!");
                ctrl.value = "";
                ctrl.focus();
                return;
            }
        }
    });
}

function validateCountry(ctrl) {
    if (ctrl.value == "") {
        return;
    }
    var Name = ctrl.value.split('/')[0];

    $.ajax({
        url: '../webservices/master.asmx/validateCountry',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'Name':'" + Name + "'}",
        success: function (data) {
            if (!data.d) {
                alert("Please Select From Suggestions Only !!");
                ctrl.value = "";
                ctrl.focus();
                return;
            }
        }
    });
}

function getDocumentNo(ctrl, txtCtrl) {
    var url = '..' + window.location.pathname;
    $.ajax({
        url: '../webservices/master.asmx/getDocumentNo',
        method: 'post',
        contentType: "application/json; charset=utf-8",
        data: "{'LocationId':'" + ctrl.value + "','PageUrl':'" + url + "'}",
        success: function (data) {
            txtCtrl.value = data.d;
        }
    });
}

function getObjectIDNModuleID() {
    var url = '..' + window.location.pathname;
    var data = [];
    data = $.ajax({
        url: '../webservices/master.asmx/getObjectIdNModuleIDByURL',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'url':'" + url + "'}",
        success: function (val) {
            data[0] = val.d[0];
            data[1] = val.d[1];
            return data;
        }
    });
    return data;
}

function getDocumentNoByModuleNObjectId(ctrl, txtCtrl, moduleId, objectId) {
    $.ajax({
        url: '../webservices/master.asmx/getDocumentNoByModuleNObjectID',
        method: 'post',
        contentType: "application/json; charset=utf-8",
        data: "{'LocationId':'" + ctrl.value + "','moduleId':'" + moduleId + "','objectId':'" + objectId + "'}",
        success: function (data) {
            txtCtrl.value = data.d;
        }
    });
}