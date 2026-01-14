mergeInto(LibraryManager.library, {
    IsMobilePlatform: function () {
        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

        var isMobile =
            /\b(BlackBerry|webOS|iPhone|IEMobile)\b/i.test(userAgent) ||
            /\b(Android|Windows Phone|iPad|iPod)\b/i.test(userAgent) ||
            // iPad на iOS 13+
            (userAgent.includes("Mac") && "ontouchend" in document);

        return isMobile;
    }
});
