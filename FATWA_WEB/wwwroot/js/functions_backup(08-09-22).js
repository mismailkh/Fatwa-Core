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

function UpdateBodyStyle(){
    $("body").attr("style", "display:flex !important");
}

function RevertBodyStyle(){
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
        window.myTimeout = setTimeout(logout, 6000000);
    }
    else {
        var user = localStorage.getItem('User');
        if (user != null) {
            window.myTimeout = setTimeout(logout, 6000000);
            resetTimer();
        }
    }
}

/*<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master"> Initialize timer on login to avoid the need of invokable C# method</History>*/
function initializeTimerOnLogin() {
    if (window.myTimeout != undefined && window.myTimeout != 'undefined') {
        window.clearTimeout(window.myTimeout);
        window.myTimeout = setTimeout(logout, 6000000);
    }
    else {
        var user = localStorage.getItem('User');
        if (user != null) {
            window.myTimeout = setTimeout(logout, 6000000);
            resetTimer();
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
        window.myTimeout = setTimeout(logout, 6000000);
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
            window.myTimeout = setTimeout(logout, 6000000);
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