var _url, _menuString;

function BindMenu(url) {
    callwebservice(url, '', '', '', BindMenuComplete, false, true, datatypeEnum.json);
}

function BindMenuComplete(data) {
    if (data != null && data != "") {
        _menuString = "<ul class='nav nav-list'>";
        var childMenuString = "";
        var haveChildern;
        var menu = eval(data);

        for (var i = 0; i < menu.length; i++) {
            if (menu[i][7] == "") {
                haveChildern = false;
                if (menu[i][1] == "" || menu[i][0] == "")
                    _menuString += "<li class='submenu'><a class='dropdown'  onclick='return false;' href='#'>" +
                        "<i class='" + menu[i][2] + "'></i><span class='hidden-minibar' >" + menu[i][3] + " <i class='fa fa-chevron-right  pull-right'></i></span></a>";
                else
                    _menuString += "<li class=' '><a id=" + menu[i][1] + " href=" + menu[i][5] + "/" + menu[i][1] + "/" + menu[i][0] + ">" +
                        "  <i class='" + menu[i][2] + "'></i><span class='hidden-minibar'>" + menu[i][3] + "</span></a>";


                for (var k = 0; k < menu.length; k++) {
                    if (menu[k][7] == menu[i][6]) {
                        haveChildern = true;
                        childMenuString += "<li class=' '><a id=" + menu[k][1] + " href=" + menu[k][5] + "/" + menu[k][1] + "/" + menu[k][0] + " >" +
                            "  <i class='" + menu[k][2] + "'></i><span class='hidden-minibar'>" + menu[k][3] + "</span></a></li>";
                    }
                }

                if (haveChildern) {
                    _menuString += "<ul class='animated fadeInDown'>";
                    _menuString += childMenuString;
                    _menuString += "</ul>";
                    childMenuString = "";
                }

                _menuString += "</li>";
            }
        }

        _menuString += "</ul>";

        $("#leftMenu").html(_menuString);

        setActiveMenulink();

    } else {
        $("#leftMenu").html("");
    }

}


function setActiveMenulink() {

    var url = window.location.pathname.split("/");

    var questions = url[1];

    $('ul.nav-list li ').removeClass('active');

    $('ul.nav-list li:has(a[id="' + questions + '"])').addClass('active').closest('.submenu').addClass('current').find('ul').css('display', 'block');

    if ($('.left-sidebar').hasClass('left-sidebar-open')) {
        $('.left-sidebar').toggleClass('left-sidebar-open');
    }
    if ($('.site-holder').hasClass('mini-sidebar')) {
        $('ul.nav-list li ul ').css('display', 'none');
    }

    $('ul.nav-list').accordion();
}