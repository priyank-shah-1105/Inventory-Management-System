var datatypeEnum, typeEnum;
$(function () {
    if (typeof datatypeEnum == "undefined") {
        datatypeEnum = {
            "json": "json",
            "text": "text",
        };
    }

    if (typeof typeEnum == "undefined") {
        typeEnum = {
            "get": "get",
            "post": "post",
        };
    }
});

function callwebservice(ajaxurl, controller, method, parameter, callbackFunction, isloader, isErrorHandle, dataType) {
    var url = '';
    if (ajaxurl !== '')
        url = url + ajaxurl;
    if (controller !== '')
        url = url + "/" + controller;
    if (method !== '')
        url = url + "/" + method;

    if (typeof (parameter) === 'undefined')
        parameter = '';

    if (isloader != false) {
        showLoading();
    }

    try {
        $.support.cors = true;
        var request = $.ajax({
            url: url,
            cache: false,
            dataType: dataType,
            data: parameter,
            timeout: 40000,
            success: function (data) {
                hideLoading();
                callbackFunction(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // log the error to the console
                if (isErrorHandle == true)
                    callbackFunction("error");
                else {
                    hideLoading();
                    //if (errorThrown != "")
                    //    showMessageOnly("The following error occured: " + errorThrown, 'popup-error');
                    //else
                    //    showMessageOnly("There is an error while connecting to server. Please try again!", 'popup-error');
                }
            },
            always: function () {
                if (isloader != false) {
                    hideLoading();
                }
            }
        });
    }
    catch (e) {
        showMessageOnly("Errour occurred " + e, 'popup-error');
        if (isloader != false) {
            hideLoading();
        }
    }
}