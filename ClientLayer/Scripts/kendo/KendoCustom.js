var conflictError = "This record is linked with other record and cannot be deleted.";
var processError = "An error occurred while processing your request.";
var norecords = "No records.";

var validationMessageTemplate = kendo.template(
                               "<div id='#=field#_validationMessage' " +
                                   "class='k-widget k-tooltip k-tooltip-validation k-invalid-msg field-validation-error'" +
                                   "style='margin: 0.5em;display:'' !important' data-for='#=field#' " +
                                   "data-val-msg-for='#=field#'>" +
                                   "#=message#" +
                                   "<div class='k-callout k-callout-n'></div>" +
                                   "</div>");



function onError(e, r) {
    try {
        if (e.errors) {
            var grid = $('#grid').data('kendoGrid');
            if (grid != undefined) {
                if (grid.editable != null || grid.editable != undefined) {
                    grid.one("dataBinding", function (element) {
                        element.preventDefault(); // cancel grid rebind
                        for (var error in e.errors) {
                            showMessageKendo(grid.editable.element, error, e.errors[error].errors);
                        }
                    });
                    return;
                }
            }
            e.sender.cancelChanges();
            var message = "";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n\n";
                    });
                }
            });
            if ((message.indexOf('The DELETE statement conflicted with the REFERENCE constraint') != -1) ||
                        (message.indexOf('The DELETE statement conflicted with the SAME TABLE REFERENCE') != -1)) {
                showMessageOnly(conflictError, 'popup-error');

            } else {
                showMessageOnly(message, 'popup-error');
            }
        } else {
            showMessageOnly(processError, 'popup-error');
        }
    }
    catch (err) {
        showMessageOnly(processError, 'popup-error');
    }
}

function showMessageKendo(container, name, errors) {
    container.find("[data-valmsg-for=" + name + "],[data-val-msg-for=" + name + "]")
        .replaceWith(validationMessageTemplate({ field: name, message: errors[0] }));

    setFocusById(name);
}




