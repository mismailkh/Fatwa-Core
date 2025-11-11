var dotNetHelperReference;
var signaturePanel;
var principleDotNetHelperReference;
var draftDotNetHelperReference;
var mentionUserDotNetReference;
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

function downloadFile(byteArray, fileName) {
    try
    {
        console.log(fileName);
        const blob = new Blob([new Uint8Array(byteArray)], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const link = document.createElement('a');

        link.href = window.URL.createObjectURL(blob);
        link.download = fileName;

        // Append the link to the document
        document.body.appendChild(link);

        // Trigger the click event
        link.click();

        // Remove the link from the document
        document.body.removeChild(link);
    } catch (error) {
        console.error('Error in downloadFile:', error);
    }
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
function ScrollPortfolioGridToBottom() {
    $(".rz-body").scrollTop($('.rz-body')[0].scrollHeight);
}
function ScrollCaseRequestToTop() {
    $(".rz-body").scrollTop(100);
    $(".save-case-request-col").scrollTop(0);

}
function ScrollWorkFlowToTop() {
    $(".rz-body").scrollTop(100);
}

window.getCookieResult = function () {
    if (document.cookie.match('.AspNetCore.Culture')) {
        return true;
    }
    else {
        return false;
    }
};

function OpenRelationPopup(id)
{
    console.log(id);
}


function initilizePrincipleDetailReference(dotnetHelper) {
    principleDotNetHelperReference = dotnetHelper;
}

$(document).on('click', ".relation-attachment-popup", function (e) {
    var id = this.id;
    var modelName = this.name;
    if (modelName == "Principle") {
        principleDotNetHelperReference.invokeMethodAsync("PrincipleRelationDetailByUsingReferenceId", id, modelName);
    }
    else if (modelName == "Legislation"){
        principleDotNetHelperReference.invokeMethodAsync("LegislationRelationDetailByUsingReferenceId", id, modelName);
    }
    
});

function InitializeDraftReference(dotnetHelper) {
    draftDotNetHelperReference = dotnetHelper;
}

function initializeEditor() {
    const editorElement = document.querySelector('.rz-html-editor-content');
    editorElement.addEventListener("keydown", function (e) {
        const selection = window.getSelection();
        const focusNode = selection.focusNode;
        if (e.key === "Backspace") {

            function findReadonlySpan(node) {
                if (!node) return null;

                if (node.nodeType === Node.ELEMENT_NODE) {
                    if (node.getAttribute('contenteditable') === 'false' && (node.tagName === 'SPAN' || node.tagName === 'DIV')) {
                        return node;
                    }
                }
                for (let i = 0; i < node.childNodes.length; i++) {
                    const result = findReadonlySpan(node.childNodes[i]);
                    if (result) {
                        return result;
                    }
                }
                return null;
            }

            const readonlySpan = findReadonlySpan(focusNode);

            if (readonlySpan) {
                e.preventDefault();
            }
            else {
                if (selection.focusOffset === 0) {
                    const readSpan = findReadonlySpan(focusNode.previousElementSibling);
                    if (readSpan) {
                        e.preventDefault();
                    }
                }
            }
        }
    });
    
}


function ScrollPageToTop() {
    $(".rz-body").scrollTop(100);
    $(".save-case-request-col").scrollTop(0);
}
function forceReload()
{
    location.reload(true);
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

window.addSingaturePanelBtnToPdfToolbar = function () {
    var toolbar = document.querySelector('.e-pdfviewer .e-toolbar .e-toolbar-left');
    var customButton = document.createElement('button');
    customButton.setAttribute('type', 'button');
    customButton.setAttribute('class', 'btn-signature-panel');
    customButton.setAttribute('title', 'Signature Panel');
    customButton.innerHTML = '<i class="k-icon k-i-signature k-button-icon"><!--!--></i>';
    toolbar.appendChild(customButton);
}
function InitilizeSignaturePanel(dotnetHelper) {

    signaturePanel = dotnetHelper;
}
$(document).on('click', ".btn-signature-panel", function (e) {
    signaturePanel.invokeMethodAsync("OpenSignaturePanel");
});
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
/*< History Author = 'ijaz Ahmad' Date = '2024-18-03' Version = "1.0" Branch = "master" > Print Grid List of Task History of Grid all Pages</History > */
window.printGridList = function (printableContent, headerContent) {
    var imageDiv = document.createElement('div');
    imageDiv.innerHTML = headerContent;
    document.body.appendChild(imageDiv);
    var printWindow = document.createElement('iframe');
    printWindow.style.visibility = 'hidden';
    document.body.appendChild(printWindow);
    var printDocument = printWindow.contentDocument || printWindow.contentWindow.document;
    printDocument.write(printableContent);
    printDocument.close();

    
    var images = imageDiv.querySelectorAll('img');
    var loadedImages = 0;
    var totalImages = images.length;

    var checkAllImagesLoaded = function () {
        loadedImages++;
        if (loadedImages === totalImages) {
           
            printWindow.contentWindow.print();
           
            printWindow.parentElement.removeChild(printWindow);
            document.body.removeChild(imageDiv); 
        }
    };

    images.forEach(function (img) {
        if (img.complete) {
            checkAllImagesLoaded();
        } else {
            img.onload = checkAllImagesLoaded;
        }
    });
};


window.openNewWindow = function (url) {
    var documentWindow = window.open(url, "_blank", "width=600,height=800,toolbar=no,menubar=no,location=0,directories=0");
    window.addEventListener("beforeunload", function () {
        if (documentWindow && !documentWindow.closed) {
            documentWindow.close();
        }
    });
};

 // Hijri date picker



function initHijrDatePicker(dotNetObjRef = "", ModelPropertyDate = "", MaxDate = "", MinDate = "") {
    try {
        var MaxDate = MaxDate;
        var MinDate = MinDate;
        var dotNetObjRef = dotNetObjRef;
        var ModelPropertyDate = ModelPropertyDate;
        var HijriDate = "";
        HijriDate = $(".hijri-date-input").hijriDatePicker({
            locale: "ar-sa",
            format: "DD/MM/YYYY",
            hijriFormat: "iDD/iMM/iYYYY",
            dayViewHeaderFormat: "MMMM YYYY",
            hijriDayViewHeaderFormat: "iMMMM iYYYY",
            showSwitcher: false,
            allowInputToggle: true,
            showTodayButton: true,
            useCurrent: false,
            isRTL: true,
            viewMode: 'days',
            keepOpen: false,
            hijri: true,
            debug: false,
            showClear: true,
            showClose: true,
            maxDate: MaxDate,
            minDate: MinDate,
        }).val();
        if (dotNetObjRef != "") {
            if (HijriDate == "" && ModelPropertyDate == "") {
                HijriDate = moment().format('iDD/iMM/iYYYY');
            }
            else if (ModelPropertyDate != "") {
                HijriDate = ModelPropertyDate.split('T')[0];
            }
            dotNetObjRef.invokeMethodAsync("GetHijriDateFromJS", HijriDate);
            OpenHijriCalendarClickingIcon();
        }
    } catch (e) { alert("Function Not Working" + e.message); }
}


function OpenHijriCalendarClickingIcon() {
    document.getElementById("HijriIcon").addEventListener('click', function () {
        document.getElementById("HijriInput").focus();
    });
}
//===================
function initHijrDatePickerSecond(dotNetObjRef = "", ModelPropertyDate = "", MaxDate = "", MinDate = "") {
    try {
        var MaxDate = MaxDate;
        var MinDate = MinDate;
        var dotNetObjRef = dotNetObjRef;
        var ModelPropertyDate = ModelPropertyDate;
        var HijriDateSecond = "";
        HijriDateSecond = $(".hijri-date-inputSecond").hijriDatePicker({
            locale: "ar-sa",
            format: "DD/MM/YYYY",
            hijriFormat: "iDD/iMM/iYYYY",
            dayViewHeaderFormat: "MMMM YYYY",
            hijriDayViewHeaderFormat: "iMMMM iYYYY",
            showSwitcher: false,
            allowInputToggle: true,
            showTodayButton: true,
            useCurrent: false,
            isRTL: true,
            viewMode: 'days',
            keepOpen: false,
            hijri: true,
            debug: false,
            showClear: true,
            showClose: true,
            maxDate: MaxDate,
            minDate: MinDate,
        }).val();
        if (dotNetObjRef != "") {
            if (HijriDateSecond == "" && ModelPropertyDate == "") {
                HijriDateSecond = moment().format('iDD/iMM/iYYYY');
            }
            else if (ModelPropertyDate != "") {
                HijriDateSecond = ModelPropertyDate.split('T')[0];
            }
            dotNetObjRef.invokeMethodAsync("GetHijriDateFromJSSecond", HijriDateSecond);
            OpenSecondHijriCalendarClickingIcon();
        }
    } catch (e) { alert("Function Not Working" + e.message); }
}
function OpenSecondHijriCalendarClickingIcon() {
    document.getElementById("HijriIconSecond").addEventListener('click', function () {
        document.getElementById("HijriInputSecond").focus();
    });
}



function refreshPage() {
    location.reload();
}

// Digital Signature
function LocalSigning(documentId, userId, profName, sessionToken, uiLang) {

    return new Promise((resolve, reject) => {

        // Calling the DSP agent
        SignDocumentLocalized(documentId, userId, profName, sessionToken, uiLang, function (code) {
            try {
                UnBlockUIBootstrap();
            } catch { }

            if (code == 0) {
                console.log(code);
                resolve(code);  // Resolve the promise with the success code
            }
            else if (code == 2003) {
                console.log("Local Signing was Canceled by user with Error Code [" + code + "]", "error");
                reject("Local Signing was Canceled by user with Error Code [" + code + "]");  // Reject the promise with error message
            }
            else {
                console.log("DSP Agent Error", "Local Signing Failed with Error Code [" + code + "]", "error");
                reject("Local Signing Failed with Error Code [" + code + "]");  // Reject the promise with error message
            }
        });
    });
}



function downloadHtmlAsWord(filename, byteArray) {
    var blob = new Blob([byteArray], { type: "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });

    var link = document.createElement('a');

    var url = window.URL.createObjectURL(blob);
    link.href = url;

    link.download = filename;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);

    window.URL.revokeObjectURL(url);
}
// Refresh Page
function refreshPage() {
    location.reload();
}
//document.addEventListener("DOMContentLoaded", function () {
//    document.body.addEventListener("mouseover", function (event) {
//        let target = event.target.closest(".e-mention-chip");
//        if (target) {
//            let userName = target.innerText.trim();
//            let popup = document.getElementById("mentionPopup");
//            if (popup) {
//                popup.style.display = "block";
//                popup.style.left = event.pageX + "px";
//                popup.style.top = event.pageY + "px";
//                popup.innerHTML = "Loading...";
//            }
//        }
//    });

//    document.body.addEventListener("mouseout", function (event) {
//        let target = event.target.closest(".e-mention-chip");
//        if (target) {
//            let popup = document.getElementById("mentionPopup");
//            if (popup) {
//                popup.style.display = "none";
//            }
//        }
//    });
//});
//document.body.addEventListener("mouseover", function (event) {
//        let target = event.target.closest(".e-mention-chip");
//        if (target) {
//            let userName = target.innerText.trim();
//            let popup = document.getElementById("mentionPopup");
//            if (popup) {
//                popup.style.display = "block";
//                popup.style.left = event.pageX + "px";
//                popup.style.top = event.pageY + "px";
//                popup.innerHTML = "Loading...";
//            }
//        }
//});
function InitilizeMentionUserComponent(dotnetHelper) {
    mentionUserDotNetReference = dotnetHelper;
}
$(document).on("mouseenter", ".e-mention-chip", function (event) {
    let target = event.target.closest(".e-mention-chip");
    if (target) {
        let link = target.querySelector("a"); // Find the first <a> tag inside
        if (link) {
            let id = link.id;
            mentionUserDotNetReference.invokeMethodAsync("GetMentionUserDetailById", id);
        }

    }
});
$(document).on("mouseout", ".e-mention-chip", function (event) {
            let target = event.target.closest(".e-mention-chip");
            if (target) {
                let popup = document.getElementById("mentionPopup");
                if (popup) {
                    popup.style.display = "none";
                }
            }
});
function randomClick() {
    const x = Math.floor(Math.random() * window.innerWidth);
    const y = Math.floor(Math.random() * window.innerHeight);

    const event = new MouseEvent("click", {
        bubbles: true,
        cancelable: true,
        view: window,
        clientX: x,
        clientY: y
    });

    document.dispatchEvent(event);
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
    let parent = event.target.closest(".buttoncontainer");
    if (parent == null) {
        if ($(".buttoncontainer .nav").is(':visible')) {
            $("#toggle").removeProp("checked");
            $(".buttoncontainer .nav").css('display', 'none');
        }
    }
    else
    {
        if ($("#toggle").prop('checked')) {
            $(".buttoncontainer .nav").css('display', 'block');
        }
    }
}


//event for beforeunload to save changes in CMS and COMS
var beforeUnloadListenerCMS = null;
var beforeUnloadListenerCOMS = null;
window.attachUnloadSaveListenerForCMS = (dotNetRef) => {
    const handleBeforeUnload = (e) => {
        e.preventDefault();
    };
    window.removeUnloadListenerCMS();
    beforeUnloadListenerCMS = handleBeforeUnload;
    window.addEventListener('beforeunload', beforeUnloadListenerCMS);
};
window.removeUnloadListenerCMS = () => {
    if (beforeUnloadListenerCMS != null) {
        window.removeEventListener('beforeunload', beforeUnloadListenerCMS);
        beforeUnloadListenerCMS = null;
    }
};
window.attachUnloadSaveListenerForCOMS = (dotNetRef) => {
    const handleBeforeUnload = (e) => {
        e.preventDefault();
    };
    window.removeUnloadListenerCOMS();
    beforeUnloadListenerCOMS = handleBeforeUnload;
    window.addEventListener('beforeunload', beforeUnloadListenerCOMS);
};
window.removeUnloadListenerCOMS = () => {
    if (beforeUnloadListenerCOMS != null) {
        window.removeEventListener('beforeunload', beforeUnloadListenerCOMS);
        beforeUnloadListenerCOMS = null;
    }
};