function validateEmployee(ctrl) {
    if (ctrl.value == "") {
        return;
    }
    var empCode = ctrl.value.split('/')[1];
    var empName = ctrl.value.split('/')[0];

    if (isNaN(empCode)) {
        alert("Please Select Valid Employee")
        ctrl.value = "";
        ctrl.focus();
        return;
    }
    $.ajax({
        url: '../webservices/employee.asmx/employeeValidation',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'empName':'" + empName + "','empCode':'" + empCode + "'}",
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

function validateEmployeeNSetValue(ctrl,setEmployeeId) {
    if (ctrl.value == "") {
        return;
    }
    var empCode = ctrl.value.split('/')[1];
    var empName = ctrl.value.split('/')[0];

    if (isNaN(empCode)) {
        alert("Please Select Valid Employee")
        ctrl.value = "";
        ctrl.focus();
        return;
    }
    $.ajax({
        url: '../webservices/employee.asmx/ValidationEmployeeGetEmpID',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'empName':'" + empName + "','empCode':'" + empCode + "'}",
        success: function (data) {
            if (data.d=="") {
                alert("Please Select From Suggestions Only !!");
                ctrl.value = "";
                ctrl.focus();
                setEmployeeId.value = "";
                return;
            }
            else
            {
                setEmployeeId.value = data.d;
            }
        }
    });
}

function validateEmployeeWithId(ctrl) {
    if (ctrl.value == "") {
        return;
    }
    var empId = ctrl.value.split('/')[1];
    var empName = ctrl.value.split('/')[0];

    if (isNaN(empId)) {
        alert("Please Select Valid Employee")
        ctrl.value = "";
        ctrl.focus();
        return;
    }
    $.ajax({
        url: '../webservices/employee.asmx/employeeValidationWithEmpId',
        method: 'post',
        async: true,
        contentType: "application/json; charset=utf-8",
        data: "{'empName':'" + empName + "','empId':'" + empId + "'}",
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

