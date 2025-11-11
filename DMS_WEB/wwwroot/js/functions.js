var dotNetHelperReference;

function JSMethod() {
    var script_tag = document.createElement('script');
    script_tag.setAttribute("src", "spiceTheme/assets/plugins/morris/morris.min.js");
    (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag);

    var script_tag_1 = document.createElement('script');
    script_tag_1.setAttribute("src", "spiceTheme/assets/plugins/morris/raphael-min.js");
    (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag_1);

    var script_tag_2 = document.createElement('script');
    script_tag_2.setAttribute("src", "spiceTheme/assets/js/pages/chart/morris/morris_home_data.js");
    (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag_2);
}

function UpdateBodyStyle() {
    //$("body").attr("style", "display:flex !important");
}

function RevertBodyStyle() {
    $("body").attr("style", "display:block !important");
}

function FocusOnLoginForm() {
    $(".k-button").trigger("click");
}

async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

/*InActivity Logout Automatically*/

function initilizeInActivityTimer(dotnetHelper) {

    dotNetHelperReference = dotnetHelper;
    if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
        window.clearTimeout(window.myTimeout);
        var sessionTimeOut = localStorage.getItem('SessionTimeout');
        if (sessionTimeOut != null) {
            var InMilliSecond = sessionTimeOut * 60000;
            window.myTimeout = setTimeout(logout, InMilliSecond);
        }
        else {
            window.myTimeout = setTimeout(logout, 1800000);
        }
    }
    else {
        var user = localStorage.getItem('User');
        if (user != null) {
            var sessionTimeOut = localStorage.getItem('SessionTimeout');
            if (sessionTimeOut != null) {
                var InMilliSecond = sessionTimeOut * 60000;
                window.myTimeout = setTimeout(logout, InMilliSecond);
                resetTimer();
            }
            else {
                window.myTimeout = setTimeout(logout, 1800000);
                resetTimer();
            }
        }
    }
}

/*<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master"> Initialize timer on login to avoid the need of invokable C# method</History>*/
function initializeTimerOnLogin() {
    if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
        window.clearTimeout(window.myTimeout);
        var sessionTimeOut = localStorage.getItem('SessionTimeout');
        if (sessionTimeOut != null) {
            var InMilliSecond = sessionTimeOut * 60000;
            window.myTimeout = setTimeout(logout, InMilliSecond);
        }
        else {
            window.myTimeout = setTimeout(logout, 1800000);
        }
    }
    else {
        var user = localStorage.getItem('User');
        if (user != null) {
            var sessionTimeOut = localStorage.getItem('SessionTimeout');
            if (sessionTimeOut != null) {
                var InMilliSecond = sessionTimeOut * 60000;
                window.myTimeout = setTimeout(logout, InMilliSecond);
                resetTimer();
            }
            else {
                window.myTimeout = setTimeout(logout, 1800000);
                resetTimer();
            }
        }
    }
}


document.onmousemove = resetTimer;
document.onkeypress = resetTimer;

/*<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master"> Use local storage to reset timer on other tabs and restore timer on current tab</History>*/
function resetTimer() {
    localStorage.removeItem("timer");
    localStorage.setItem("timer", "timer");
    if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
        window.clearTimeout(window.myTimeout);
        var sessionTimeOut = localStorage.getItem('SessionTimeout');
        if (sessionTimeOut != null) {
            var InMilliSecond = sessionTimeOut * 60000;
            window.myTimeout = setTimeout(logout, InMilliSecond);
        }
        else {
            window.myTimeout = setTimeout(logout, 1800000);
        }
    }
}

function logout() {
    dotNetHelperReference.invokeMethodAsync("LogOut");
    if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
        window.clearTimeout(window.myTimeout);
    }
}

/*<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master"> Logout all tabs using storage change and restore timer on activity in any tab</History>*/
window.addEventListener('storage', (e) => {
    if (e.key == "timer" || e.oldValue == "timer" || e.newValue == "timer") {
        if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
            window.clearTimeout(window.myTimeout);
            var sessionTimeOut = localStorage.getItem('SessionTimeout');
            if (sessionTimeOut != null) {
                var InMilliSecond = sessionTimeOut * 60000;
                window.myTimeout = setTimeout(logout, InMilliSecond);
            }
            else {
                window.myTimeout = setTimeout(logout, 1800000);
            }
        }
    }
    else {
        if (!e.url.includes("/Login")) {
            var user = localStorage.getItem('User');
            if (user == null || user == undefined) {
                window.location.href = "/Login";
                if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
                    window.clearTimeout(window.myTimeout);
                }
            }
        }
    }
});

function subscribeToChange(inputRef) {
    inputRef.onkeydown = function (event) {
        event.preventDefault();
    };

    inputRef.onkeyup = function (event) {
        event.preventDefault();
    };
}

/*<History Author = 'Hassan Abbas' Date='2022-05-17' Version="1.0" Branch="master"> Generic File Download Service Trigger Events</History>*/
async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

/*<History Author = 'Hassan Abbas' Date='2022-05-17' Version="1.0" Branch="master"> Generic File Download Service Trigger Events</History>*/
function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}



$(document).on('click', ".demos-sidebar.rz-sidebar-collapsed ul li", function () {
    if ($(this).hasClass('active')) {
        $('.rz-navigation-item').removeClass('active');
    }
    else {
        $('.rz-navigation-item').removeClass('active');
        $(this).addClass('active');
    }
    if ($(this).hasClass('open')) {
        $('.rz-navigation-item').removeClass('open');
    }
    else {
        $('.rz-navigation-item').removeClass('open');
        $(this).addClass('open');
    }
});

$(document).on('click', ".demos-sidebar.rz-sidebar-collapsed ul.rz-navigation-menu li", function (e) {
    var abc = $(this).closest('ul.rz-panel-menu').find('.rz-navigation-item.active.open');
    if (abc[0] != undefined && abc[0].classList.contains('open')) {
        abc[0].classList.remove('open');
    }
    if (abc[0] != undefined && abc[0].classList.contains('active')) {
        abc[0].classList.remove('active');
    }
    e.stopPropagation();
});

$(window).resize(function () {
    if (window.matchMedia("(max-width: 767px)").matches) {
        // The viewport is less than 768 pixels wide
        $(".mostUsedData .profileTopMenu").removeClass("rz-menu-closed").addClass("rz-menu-open");
    } 
});

function UploadFile() {
    document.querySelector(".k-upload-selected").click();
}

function RedirectToPreviousPage() {
    history.back();
}

function RedirectToSecondLastPage() {
    history.go(-2);
}

function ToggleSideBar() {
    if ($('.rz-sidebar').hasClass('rz-sidebar-collapsed')) {
        $('.rz-sidebar').removeClass('rz-sidebar-collapsed');
        $('.rz-sidebar').addClass('rz-sidebar-expanded');
    }
    else {
        $('.rz-sidebar').removeClass('rz-sidebar-expanded');
        $('.rz-sidebar').addClass('rz-sidebar-collapsed');
    }
    if ($('.rz-header').hasClass('rz-header-collapsed')) {
        $('.rz-header').removeClass('rz-header-collapsed');
        $('.rz-header').addClass('rz-header-expanded');
    }
    else {
        $('.rz-header').removeClass('rz-header-expanded');
        $('.rz-header').addClass('rz-header-collapsed');
    }
}

function ScrollPortfolioGridToBottom() {
    $(".rz-body").scrollTop($('.rz-body')[0].scrollHeight);
}
function ScrollCaseRequestToTop() {
    $(".rz-body").scrollTop(100);
    $(".save-case-request-col").scrollTop(0);
}

window.getCookieResult = function () {
    if (document.cookie.match('.AspNetCore.Culture')) {
        return true;
    }
    else {
        return false;
    }
};

window.getEditorHtmlContent = (editorId) => {
debugger
    const editor = document.getElementById(editorId + "_viewerContainer");
    if (editor) {
        return editor.contentModule.getHtml();
    }
    return '';
};