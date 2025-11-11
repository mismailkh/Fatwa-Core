var dotNetHelperReference;
const WARNING_BEFORE_MINUTES = 5;

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

$(document).on('click', function (event) {
    const clickedElement = $(event.target);

    // Check if the clicked element is outside the .rz-header and .rz-sidebar
    if (!clickedElement.closest('.rz-header').length && !clickedElement.closest('.rz-sidebar').length) {
        // Collapse the header if it's currently expanded
        if ($('.rz-header').hasClass('rz-header-expanded')) {
            $('.rz-sidebar').removeClass('rz-sidebar-expanded');
            $('.rz-sidebar').addClass('rz-sidebar-collapsed');
            $('.rz-header').removeClass('rz-header-expanded');
            $('.rz-header').addClass('rz-header-collapsed');

        }
    }
});
$(document).on('click', function (event) {
    const clickedElement = $(event.target);
    if (!clickedElement.closest(".rz-navigation-item").length) {
        if ($('.rz-navigation-item').hasClass('open')) {
            $('.rz-navigation-item').removeClass('open');
            $('.rz-navigation-item').removeClass('active');
            // Add any additional classes you want to apply after closing the side bar tab
        }
        if ($('.rz-navigation-item').hasClass('Active')) {
            $('.rz-navigation-item').removeClass('Active');
        }
    }
});
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
        if (window.myWarningTimeout !== undefined) {
            clearTimeout(window.myWarningTimeout);
        }
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
        if (window.myWarningTimeout !== undefined) {
            clearTimeout(window.myWarningTimeout);
        }
        var sessionTimeOut = localStorage.getItem('SessionTimeout');
        if (sessionTimeOut != null) {
            var InMilliSecond = sessionTimeOut * 60000;
            window.myTimeout = setTimeout(logout, InMilliSecond);
            if (InMilliSecond > WARNING_BEFORE_MINUTES * 60000) {
                window.myWarningTimeout = setTimeout(() => {
                    dotNetHelperReference.invokeMethodAsync("ShowSessionExpireWarringMessage", WARNING_BEFORE_MINUTES);
                }, InMilliSecond - WARNING_BEFORE_MINUTES * 60000);
            }
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
function RedirectToPreviousPage() {
    history.back();
}

function RedirectToSecondLastPage() {
    history.go(-2);
}



/*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Append Custom Action into the Pdf Viewer Toolbar through Html</History>*/
window.addRotateButtonToPdfToolbar = function () {
    var toolbar = document.querySelector('.e-pdfviewer .e-toolbar .e-toolbar-left');
    var customButton = document.createElement('button');
    customButton.setAttribute('type', 'button');
    customButton.setAttribute('class', 'btn-rotate-document');
    customButton.setAttribute('title', 'Rotate');
    customButton.innerHTML = '<i class="rz-button-icon-left rzi"><!--!-->rotate_right</i>';

    toolbar.appendChild(customButton);
}

let currentRotation = 0;

/*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Rotate the specific html tags/divs of pdf viewer using js on the click of appended button in the above function</History>*/
$(document).on('click', ".btn-rotate-document", function (e) {
    currentRotation += 90;
    $('.e-pv-page-div').css('transform', `rotate(${currentRotation}deg)`);

    // Reset rotation to 0 after 360 degrees (4 clicks)
    if (currentRotation === 360) {
        currentRotation = 0;
    }
});

window.openNewWindow = function (url) {
    var documentWindow = window.open(url, "_blank", "width=600,height=800,toolbar=no,menubar=no,location=0,directories=0");
    window.addEventListener("beforeunload", function () {
        if (documentWindow && !documentWindow.closed) {
            documentWindow.close();
        }
    });
};
function UploadFile() {
    document.querySelector(".k-upload-selected").click();
}

function adjustDropdownPosition() {
    let sidebarCollapsed = document.querySelector(".rz-sidebar-collapsed");

    if (!sidebarCollapsed) {
        return;
    }

    document.querySelectorAll('.rz-navigation-menu').forEach(dropdown => {
        let rect = dropdown.getBoundingClientRect();
        let windowHeight = window.innerHeight;

        if (rect.bottom > windowHeight) {
            dropdown.style.bottom = `0`;
            dropdown.style.transform = `translateY(-50%)`;
        } else {
            //dropdown.style.top = '';
        }
    });
}

document.addEventListener("DOMContentLoaded", adjustDropdownPosition);
window.addEventListener("resize", adjustDropdownPosition);

document.body.addEventListener("click", function (event) {
    let target = event.target;
    if (event.target.matches(".rz-navigation-item-link") || event.target.matches(".rz-navigation-item-icon")) {
        let parent = target.closest(".rz-sidebar-collapsed");
        if (parent) {
            setTimeout(adjustDropdownPosition, 100);
        }
    }
    closeFloatingButtonIfShown(event);
});

function closeFloatingButtonIfShown(event) {
    let parent = event.target.closest(".buttoncontainer"); debugger
    if (parent == null) {
        if ($(".buttoncontainer .nav").is(':visible')) {
            $("#toggle").removeProp("checked");
            $(".buttoncontainer .nav").css('display', 'none');
        }
    }
    else {
        if ($("#toggle").prop('checked')) {
            $(".buttoncontainer .nav").css('display', 'block');
        }
    }
}