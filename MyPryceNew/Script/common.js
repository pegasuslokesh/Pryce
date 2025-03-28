function getRoundOffAmount(amount,decimalCount,denomination) {
    if (isNaN(denomination)) {
        denomination = 0;
    }
    try {

        var amount = parseFloat(amount);
        var decValue = amount.toString().split('.')[1];
        if (decValue <= 0) {
            return GetAmountWithDecimal(amount, decimalCount);
        }

        var roundOfValue = 0;
        if (denomimination != 0 && denomination != '' && denomination != NaN) {
            var modulerValue = amount % denomination;
            var actualValue = amount - modulerValue;
            var devideValue = denomination / 2;
            if (modulerValue >= devideValue)
                roundOfValue = actualValue + denomination;
            else
                roundOfValue = actualValue + 0;
        }
        else {
            roundOfValue = Amount;
        }
        roundOfValue = GetAmountWithDecimal(roundOfValue, decimalCount);
        return roundOfValue;
    }
    catch (e) {
        return GetAmountWithDecimal(amount, decimalCount);
    }
}

function GetAmountWithDecimal(value, n) {
    const v = value.toString().split('.');
    var data = "0";
    if (n <= 0) return v[0];
    let f = v[1] || '';
    if (f.length > n) {
        data = `${v[0]}.${f.substr(0, n)}`;
        if (data.split('.')[0] == "NaN") {
            return "0." + data.split('.')[1];
        }
        else {
            return `${v[0]}.${f.substr(0, n)}`;
        }
    }
    while (f.length < n) f += '0';
    data = `${v[0]}.${f}`;
    if (data.split('.')[0] == "NaN") {
        return "0." + data.split('.')[1];
    }
    else {
        return `${v[0]}.${f}`;
    }
}

function validateTime(ctrl) {
    var hr = ctrl.value.split(':')[0];
    var min = ctrl.value.split(':')[1];
    if (parseInt(hr) > 23) {
        alert("Not a valid Time");
        ctrl.value = "00:00:00";
    }
    else {
        if (parseInt(min) > 59) {
            alert("Not a valid Time");
            ctrl.value = "00:00:00";
        }
    }
}

function showAlert(data, bgcolor, txtcolor) {
    var x = document.getElementById("snackbar");
    x.innerHTML = data;
    x.style.backgroundColor = bgcolor;
    x.style.color = txtcolor;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

function validateAmount(ctrl)
{
    if(isNaN(ctrl.value))
    {
        alert("Not a valid Amount");
        ctrl.value = "";
        ctrl.focus();
    }
}
function getDate()
{
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var hr = today.getHours();
    var min = today.getMinutes();

    if (min == '1' || min == '2' || min == '3' || min == '4' || min == '5' || min == '6' || min == '7' || min == '8' || min == '9') {
        min = '0' + min;
    }

    if (dd < 10) {
        dd = '0' + dd;
    }

    var mmm;
    if (mm == '1') {
        mmm = 'Jan';
    } else if (mm == '2') {
        mmm = 'Feb';
    } else if (mm == '3') {
        mmm = 'Mar';
    } else if (mm == '4') {
        mmm = 'Apr';
    } else if (mm == '5') {
        mmm = 'May';
    } else if (mm == '6') {
        mmm = 'Jun';
    } else if (mm == '7') {
        mmm = 'Jul';
    } else if (mm == '8') {
        mmm = 'Aug';
    } else if (mm == '9') {
        mmm = 'Sep';
    } else if (mm == '10') {
        mmm = 'Oct';
    } else if (mm == '11') {
        mmm = 'Nov';
    } else if (mm == '12') {
        mmm = 'Dec';
    }
    var today = dd + '-' + mmm + '-' + yyyy;
    return today;
    //var time = hr + ':' + min;
}
function validatePercentage(ctrl) {
    if (isNaN(ctrl.value)) {
        alert("Not a valid Percentage");
        ctrl.value = "";
        ctrl.focus();
    }
    if(parseFloat(ctrl.value)>100)
    {
        alert("Percentage Should be less then or equal to 100");
        ctrl.value = "";
        ctrl.focus();
    }
}

function resetPosition(object, args) {
    var tb = object._element;
    $(object._completionListElement.children).each(function () {
        var data = $(this)[0];
        if (data != null) {
            data.style.paddingLeft = "10px";
            data.style.cursor = "pointer";
            data.style.borderBottom = "solid 1px #e7e7e7";
        }
    });
    object._completionListElement.className = "scrollbar scrollbar-primary force-overflow";
    var tbposition = findPositionWithScrolling(tb);
    var xposition = tbposition[0] + 2;
    var yposition = tbposition[1] + 25;
    var ex = object._completionListElement;
    if (ex)
        $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
}
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

function gridDropDown () {
    var datad = document.getElementById("ddl");
    datad.parentNode.className = "tddata";
}


//function getCount(ctrl) {
//    if (ctrl.nextElementSibling.childElementCount > 2) {
//        ctrl.parentElement.parentElement.parentElement.parentElement.parentElement.style.marginBottom = '80px';
//    }
//    if (ctrl.nextElementSibling.childElementCount > 5) {
//        ctrl.parentElement.parentElement.parentElement.parentElement.parentElement.style.marginBottom = '120px';
//    }
//}

String.prototype.replaceAll = function (str1, str2, ignore) {
    return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
    //"x".replaceAll("x", "xyz");
    // xyz

    //"x".replaceAll("", "xyz");
    // xyzxxyz

    //"aA".replaceAll("a", "b", true);
    // bb

    //"Hello???".replaceAll("?", "!");
    // Hello!!!
}