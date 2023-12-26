var Global = {
    currentProxyAouth: {
        username: '',
        password: ''
    }
}

chrome.webRequest.onAuthRequired.addListener(
    function (details, callbackFn) {
        console.log('onAuthRequired >>>: ', details, callbackFn);
        callbackFn({
            authCredentials: Global.currentProxyAouth
        });
    }, {
    urls: ["<all_urls>"]
}, ["asyncBlocking"]);

chrome.runtime.onMessage.addListener(
    function(request, sender, sendResponse) {
        console.log('Background received a message: ', request);

        POPUP_PARAMS = {};
        if (request.command && requestHandler[request.command])
            requestHandler[request.command](request);
    }
);
