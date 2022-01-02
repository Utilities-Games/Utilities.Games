window.changeTheme = function (themeClass) {
    const root = document.getElementsByTagName("body")[0];
    const classes = root.classList;
    for (var i = 0; i < classes.length; i++) {
        const className = classes[i];
        if (className.startsWith('theme-')) {
            root.classList.remove(className);
        }
    }
    root.classList.add(themeClass);
}