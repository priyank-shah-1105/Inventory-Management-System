String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};

var tools = {
    getStringBuilder: function () {
        var data = [];
        var counter = 0;
        return {
            // adds string s to the stringbuilder
            append: function (s) {
                data[counter++] = s;
                return this;
            },
            // removes j elements starting at i, or 1 if j is omitted
            remove: function (i, j) {
                data.splice(i, j || 1);
                return this;
            },
            // inserts string s at i
            insert: function (i, s) {
                data.splice(i, 0, s);
                return this;
            },
            // builds the string
            toString: function (s) {
                return data.join(s || "");
            }
        };
    }

};

function appendHTML(id, html) {
    $("#" + id).append(html);
}

function appendChartHtml(columnWidth, chartId, chartHeaderId, chartName, isBackRequired, backbuttonId, onBackClickMethod, noOfHiddenField) {
    var htmlString = tools.getStringBuilder();
    htmlString.append("<div class='" + columnWidth + "'>");
    htmlString.append("<div class='panel panel-primary'>");
    htmlString.append("<div class='panel-heading'>");
    htmlString.append("<h3 class='panel-title' id='" + chartHeaderId + "'>" + chartName + " </h3>");
    if (isBackRequired) {
        htmlString.append("<input type='button' id='" + backbuttonId + "' value='Back' onclick='" + onBackClickMethod + "(this)' class='floatRight displayNone' />");
        if (noOfHiddenField != undefined)
            for (var i = 1; i <= noOfHiddenField; i++) {
                htmlString.append("<span id='" + chartId + " hiddenField" + i + "'></span>");
            }
    }
    htmlString.append("</div>");
    htmlString.append("<div class='panel-body chartWrapper' id='" + chartId + "'></div>");
    htmlString.append("</div>");
    htmlString.append("</div>");
    return htmlString.toString();
}

