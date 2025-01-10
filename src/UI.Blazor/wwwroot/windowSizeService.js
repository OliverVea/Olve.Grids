window.getWindowSize = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.getElementSize = function (elementId) {
    const element = document.getElementById(elementId);
    if (!element) {
        return null;
    }

    return {
        width: element.clientWidth,
        height: element.clientHeight
    };
}