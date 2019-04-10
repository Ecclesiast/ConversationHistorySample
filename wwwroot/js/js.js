var _initialized = false;

$.fn.initialize = async function (o) {
    this.html(getHtml());
    show();
};

function getHtml() {
    return '<div class="s27bot"><div class="chatCont" id="chatCont">' +

        '<div class="resultDiv">' +
        '<div class="bot_profile">' +
        '<div class="cb"></div>' +
        '</div>' +
        '<div id="webchat"></div>' +
        '</div>' +
        '</div>' +
        '</div>';
}

function show() {
    if (!_initialized) {
        (async function () {
            const res = await fetch('/api/token/generate', { method: 'POST' });
            const { token, conversationId } = await res.json();

            var from = {
                id: "default-user",
                name: 'User',
            };

            var dl;
            if (readCookie("conversationId") !== null) {
                const response = await fetch('/api/token/refresh?conversationId=' + readCookie("conversationId"), { method: 'GET' });
                const refreshData = await response.json();
                dl = new DirectLine.DirectLine({
                    token: refreshData.token,
                    streamUrl: refreshData.streamUrl,
                    webSocket: false,
                    conversationId: readCookie("conversationId")
                })
            }
            else {
                dl = new DirectLine.DirectLine({ 
                    token: token
                });
                createCookie("conversationId", conversationId, 365);
            }

            window.WebChat.renderWebChat({
                directLine: dl,
                userID: "default-user"
            }, document.getElementById('webchat'));

            document.querySelector('#webchat > *').focus();
            _initialized = true;
        })().catch(err => console.error(err));
    }
}

function createCookie(name, value, days) {
    var expires;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
        expires = "; expires=" + date.toGMTString();
    }
    else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEq = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEq) === 0) return c.substring(nameEq.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}