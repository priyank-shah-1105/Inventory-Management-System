function showLoading() {
    var container = $('.main-content');
    container.mask('<h1><i class="fa  fa-refresh fa-spin"></i> Loading...</h1>');
}

function hideLoading() {
    var container = $('.main-content');
    container.unmask();
}

function showMessage(url, message, alertClass) {
    bootbox.alert(message, function () {
        window.location.href = url;
    }, alertClass);
}

function showMessageOnly(message, alertClass) {
    bootbox.alert(message, '', alertClass);
}

function showConfirmBox(message, callback) {
    bootbox.confirm(message, callback, 'popup-confirmation');

}

function clearInputById(id) {
    $("#" + id).val("");
}

function setFocusById(id) {
    $("#" + id).focus();
}

function setInputValueById(id, value) {
    return $("#" + id).val(value);
}

function setInputValue(element, value) {
    return $(element).val(value);
}

function removeElement(element) {
    $(element).remove();
}

function setCssProperty(element, cssProperty, propertyValue) {
    $(element).css(cssProperty, propertyValue);
}

function setCssPropertyWithLocation(element, location, cssProperty, propertyValue) {
    $(element).find(location).css(cssProperty, propertyValue);
}

function setAttribute(element, attribute, value) {
    $(element).attr(attribute, value);
}

function getLength(element) {
    return $(element).length;
}

function removeAttributeById(id, attribute) {
    $("#" + id).removeAttr(attribute);
}

function getInputValueById(id) {
    return $("#" + id).val();
}

function getInputValue(element) {
    return $(element).val();
}

function getLocalJsonData(key) {
    return JSON.parse(localStorage.getItem(key));
}

//to store json data locally 
function setLocalJsonData(key, jsonData) {
    localStorage.setItem(key, JSON.stringify(jsonData));
}

function setKendoDatePickerValue(dateTimePickerId, dateTimeValue) {
    $("#" + dateTimePickerId).data("kendoDatePicker").value(dateTimeValue);
}

function DateTimePickerValue(datepickerId) {
    return $("#" + datepickerId).data('kendoDatePicker').value();
}

function getWindowPathName() {
    if (window.pathArray != undefined) {
        if (window.pathArray.indexOf('localhost') < 0) {
            return window.location.protocol + '//' + window.location.host + window.pathArray.substr(0, window.pathArray.length - 1) + "/";
        } else {
            return window.location.protocol + '//' + window.location.host + "/";
        }
    }
    return window.location.protocol + '//' + window.location.host + "/";
}

function clearAllData(formId) {
    $("#" + formId + " input[type='text']").each(function (i, e) {
        e.value = "";
    });
    $("#" + formId + " input[type='date']").val('');
    $("#" + formId + " textarea").val('');
    $("#" + formId + " input[type='hidden']").val('');

    $("#" + formId + " select").each(function (i, e) {
        e.selectedIndex = 0;
    });

    $("#" + formId + " input[type='checkbox']").prop('checked', false);
}

function OnAjaxFailure(xhr, status, error) {
    showMessageOnly('There was an error processing your request. Please try again, or contact support if this occurs repeatedly.');
}

function OnAjaxBegin() {
    showLoading();
}

function OnAjaxComplete() {
    hideLoading();
}

function DateCompare(fromDateId, toDateId) {
    var fromDate = $.datepicker.parseDate('dd/mm/yy', DateTimePickerValue(fromDateId));
    var toDate = $.datepicker.parseDate('dd/mm/yy', DateTimePickerValue(toDateId));
    if (fromDate > toDate) {
        $('#' + toDateId).focus();
        return false;
    }
    return true;
}

function callwebserviceForChart(ajaxurl, parameter, callbackFunction, isErrorHandle, isEvalRequired, isAsync) {

    if (isEvalRequired == undefined)
        isEvalRequired = true;
    if (isAsync == undefined)
        isAsync = true;

    var url;
    url = ajaxurl;

    if (typeof (parameter) === 'undefined')
        parameter = '';

    try {
        var request = $.ajax({
            url: url,
            cache: false,
            dataType: 'json',
            data: parameter,
            async: isAsync
        });
        // callback handler that will be called on success
        request.done(function (response) {
            // log a message to the console
            try {
                var result;
                if (isEvalRequired)
                    result = eval(response)[0];
                else
                    result = response;
            } catch (error) {
                if (isErrorHandle == true)
                    callbackFunction("error");
            }

            if (typeof (result) === 'undefined')
                return;
            callbackFunction(result);
        });

        // callback handler that will be called on failure
        request.fail(function (jqXhr, textStatus, errorThrown) {
            // log the error to the console
            if (isErrorHandle == true)
                callbackFunction("error");
            else {
                if (errorThrown != "")
                    showMessageOnly("The following error occurred: " + errorThrown + " for " + url);
                else
                    showMessageOnly("There is an error while connecting to server. Please try again!");
            }
        });

        // callback handler that will be called regardless
        // if the request failed or succeeded
        request.always(function () {
            // reenable the inputs

        });
    }
    catch (e) {
        showMessageOnly("Error occurred " + e);
    }
}

function createGrid(gridDivId, gridData) {
    $("#" + gridDivId).kendoGrid(gridData);
}

//to create chart
function createChart(chartDivId, chartData) {
    $("#" + chartDivId).kendoChart(chartData);
}


function createRadialChart(chartDivId, chartData) {
    $("#" + chartDivId).kendoRadialGauge(chartData);
}

function reDrawChart(chartDivId) {
    $("#" + chartDivId).data("kendoChart").redraw();
}

function reDrawRadialGuage(chartDivId) {
    $("#" + chartDivId).data("kendoRadialGauge").redraw();
}


function changeStatus(controllerName, action, reloadAction, parameter, message, id, gridId) {
    var grid = $("#" + gridId).data("kendoGrid");
    bootbox.confirm(message, function (result) {
        if (result == true) {
            callwebservice(getWindowPathName() + controllerName + '/' + action, '', '', parameter, function (data) {
                if (data == "True") {
                    if (grid != undefined) {
                        grid.dataSource.read();
                        return;
                    }
                }
            }, true, true, datatypeEnum.text);
        }
    }, 'popup-confirmation');
}


function cleartextbox(primaryId, textboxId) {
    var pId = $("#" + primaryId).val();
    if (pId == 0 || pId == undefined) {
        $("#" + textboxId).val("");
    }
}

function readKendoGrid(id) {
    var grid = $("#" + id).data("kendoGrid");
    grid.dataSource.read();
}

function readkendoDropDownList(id) {
    var grid = $("#" + id).data("kendoDropDownList");
    grid.dataSource.read();
}

function bindKendDropdownlist(id, textField, valueField, data, placeHolder) {
    $("#" + id).kendoDropDownList({
        dataTextField: textField,
        dataValueField: valueField,
        dataSource: data,
        optionLabel: placeHolder
    }).data("kendoDropDownList");
}

function bindKendDropdownlistWithChangeEvent(id, textField, valueField, data, placeHolder, onChange) {
    $("#" + id).kendoDropDownList({
        dataTextField: textField,
        dataValueField: valueField,
        dataSource: data,
        optionLabel: placeHolder,
        change: onChange
    }).data("kendoDropDownList");
}

function bindKendCombobox(id, textField, valueField, data, placeHolder) {
    $("#" + id).kendoComboBox({
        dataTextField: textField,
        dataValueField: valueField,
        dataSource: data,
        placeholder: placeHolder
    }).data("kendoComboBox");
}

function bindKendComboboxWithChangeEvent(id, textField, valueField, data, placeHolder, onSelect, onChange) {
    $("#" + id).kendoComboBox({
        dataTextField: textField,
        dataValueField: valueField,
        dataSource: data,
        placeholder: placeHolder,
        select: onSelect,
        change: onChange
    }).data("kendoComboBox");
}

function readKendoGrid(id) {
    var grid = $("#" + id).data("kendoGrid");
    grid.dataSource.read();
}
function clearKendoGridDatasource(id) {
    $("#" + id).data('kendoGrid').dataSource.data([]);
}

function clearbindKendDropdownlist(id) {
    $("#" + id).data('kendoDropDownList').dataSource.data([]);
}

function setKendoDropdownlistValue(id, value) {
    $("#" + id).data("kendoDropDownList").value(value);
}

function getKendoDropdownlistValue(id) {
    return $("#" + id).data('kendoDropDownList').value();
}

function getKendComboboxValue(id) {
    return $("#" + id).data('kendoComboBox').value();
}

function getKendComboboxText(id) {
    return $("#" + id).data('kendoComboBox').text();
}

function SetTriStateCheckBox(id) {
    $("#" + id).prop("indeterminate", true);
}

function setKendComboboxValue(id, value) {
    $("#" + id).data('kendoComboBox').value(value);
}

function clearKendoDropdownlist(id) {
    var ddl = $("#" + id).data("kendoDropDownList");
    ddl.dataSource.data({}); // clears dataSource
    ddl.text(""); // clears visible text
    ddl.value("");
}


function clearkendoComboBox(id) {
    var ddl = $("#" + id).data("kendoComboBox");
    ddl.dataSource.data({}); // clears dataSource
    ddl.text(""); // clears visible text
    ddl.value("");
}

function openKendoWindow(id) {
    var dialog = $('#' + id);
    dialog.data("kendoWindow").open();
    dialog.data('kendoWindow').center();
}

function openKendoWindowMaximized(id) {
    var dialog = $('#' + id).data("kendoWindow");
    dialog.open();
    dialog.center();
    dialog.maximize();
}



function closeKendoWindow(id) {
    var dialog = $('#' + id);
    dialog.data("kendoWindow").close();
}

function blurBodyBackGround() {
    $("body").css("overflow", "hidden");
    $("body").append('<div id="divHover" class="modalOverlay">');
}

function removeBodyBlur() {
    $("body").css("overflow", "auto");
    $("#divHover").remove();
}

function getWindowPathName() {
    return window.location.protocol + '//' + window.location.host + "/";
}

function convertDateToString(value) {
    if (value != null && value != '')
        return kendo.toString(value, dateFormat);
    else return "";
}

//function for appending html to div
function appendHTML(id, html) {
    $("#" + id).append(html);
}


function getGUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
    });
    return uuid;
};

function resizeGrid(id) {
    var gridElement = $("#" + id),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 0;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(gridHeight - otherElementsHeight);
}

function setMonthStartDate(datePickerId) {
    var date = new Date();
    $("#" + datePickerId).kendoDatePicker({ value: new Date(date.getFullYear(), date.getMonth(), 1) });
}

//to set current date to the datepicker
function setCurrentDate(datePickerId) {
    $("#" + datePickerId).kendoDatePicker({ value: new Date(), format: dateFormat });
}

function TriplestateCheckBoxHandler(chkBox, hiddenFieldId) {

    var hiddenField = $("#" + hiddenFieldId);
    var checked = 'True';
    var unChecked = 'False';
    var undefined = '';

    switch (hiddenField.val()) {
        case undefined:
            hiddenField.val(checked);
            break;
        case checked:
            hiddenField.val(unChecked);
            break;
        case unChecked:
            hiddenField.val(undefined);
            break;
        default:
            hiddenField.val(checked);
            break;
    }

    var className;
    switch (hiddenField.val()) {
        case undefined:
            className = 'CheckBoxUnDefined';
            break;
        case checked:
            className = 'CheckBoxChecked';
            break;
        case unChecked:
            className = 'CheckBoxUnChecked';
            break;
        default:
            className = 'CheckBoxUnDefined';
            break;
    }
    chkBox.className = className;
}