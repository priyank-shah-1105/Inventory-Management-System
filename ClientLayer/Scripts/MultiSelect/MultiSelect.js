/****************************************** MultiSelect Functions Start ***************************************************************/
// Function for bind User id in Hidden Field
var callEvent;
var isDivisionSelect = false;
var isDepartmentSelect = false;

function setMultiSelectValue(multiSelectArrayValue, multiSelectControlId) {
    var multiSelectValue = "";
    $(".ui-multiselect-checkboxes").eq(multiSelectArrayValue).find("li input:checked").each(function (i, el) {
        var inputValue = getInputValue(el);
        if (inputValue != "0" || inputValue != "") {
            multiSelectValue += inputValue + ",";
        }
    });
    setInputValueById(multiSelectControlId, multiSelectValue);
}

function createMultiSelectControl(id, clickFunction, nonSelectText) {
    $("#" + id).multiselect({
        click: function () {
            clickFunction();
        },
        noneSelectedText: nonSelectText
    });
}

function setMultiSelectCheckBoxValue(multiSelectArrayValue, multiSelectControlId, nonSelectText) {
    $(".ui-multiselect-checkboxes").eq(multiSelectArrayValue).find("li input").each(function (i, el) {
        var demo = getInputValueById(multiSelectControlId);
        if (demo != undefined) {
            var substr = demo.split(',');
            for (var j = 0; j < substr.length; j++) {
                if (substr[j] == $(el).val()) {
                    $(el).attr('checked', 'checked');
                }
            }
            if (substr.length > 1) {
                $('.ui-multiselect').eq(multiSelectArrayValue).children().eq(1).text((substr.length - 1) + ' selected');
            } else {
                $('.ui-multiselect').eq(multiSelectArrayValue).children().eq(1).text(nonSelectText);
            }
        }
    });
}

function bindMultiSelectClickEvent(multiSelectArrayValue, clickFunction) {
    $(".ui-multiselect-all").eq(multiSelectArrayValue).on('click', function () {
        callEvent = "'" + multiSelectArrayValue + "'";
        clickFunction();
    });

    $(".ui-multiselect-none").eq(multiSelectArrayValue).on('click', function () {
        callEvent = "'" + multiSelectArrayValue + "'";
        clickFunction();
    });
}
/****************************************** MultiSelect Functions End ***************************************************************/