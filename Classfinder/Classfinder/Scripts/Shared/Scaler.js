function FixHeight(el, targetHeight, targetWidth) {
    $(el).css("height", ($(el).height() * (window.innerHeight / targetHeight)));
}