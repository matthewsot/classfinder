/// <reference path="jquery.d.ts" />

var FadeAnimation = (function () {
    function FadeAnimation() {
    }
    FadeAnimation.prototype.animateIn = function (overlayId, contentId, completed) {
        $('#' + overlayId).fadeIn('slow');
        $('#' + contentId).animate({ opacity: 1 }, 'slow', completed);
    };

    FadeAnimation.prototype.animateOut = function (overlayId, contentId, completed) {
        $('#' + overlayId).fadeOut('slow', completed);
        $('#' + contentId).fadeOut('slow');
    };
    return FadeAnimation;
})();

var SlideAnimation = (function () {
    function SlideAnimation() {
    }
    SlideAnimation.prototype.animateIn = function (overlayId, contentId, completed) {
        $('#' + overlayId).fadeIn('slow');
        var content = '#' + contentId;
        var regularMarginLeft = parseInt($(content).css('margin-left').replace('px', ''));
        $(content).css('margin-left', (regularMarginLeft - 50) + 'px');
        $(content).animate({
            "opacity": 1,
            marginLeft: regularMarginLeft + 'px'
        }, completed);
    };
    SlideAnimation.prototype.animateOut = function (overlayId, contentId, completed) {
        $('#' + overlayId).fadeOut('slow', completed);
        var regularMarginLeft = parseInt($('#' + contentId).css('margin-left').replace('px', ''));
        $('#' + contentId).animate({
            "opacity": 0,
            marginLeft: (regularMarginLeft - 50) + 'px'
        });
    };
    return SlideAnimation;
})();

/* initWedge()
* Displays the Wedge Lightbox
*
* link: A link to the youtube video or picture
* title: Text displayed below the content
* type: The type of link provided - youtube, img, or div
* animator: An IWedgeAnimator that controls how animations are handled
* exitOnEscape: Controls whether the lightbox can be exited by pressing the escape key
* doAutoPosition: Controls whether the lightbox is automatically centered
* opacity: The final opacity of the overlay
* allowExit: Controls whether the user is allowed to exit the lightbox
*/
function initWedge(link, type, title, animator, exitOnEscape, doAutoPosition, opacity, allowExit, overlayId, contentId) {
    if (typeof animator === "undefined") { animator = new FadeAnimation; }
    if (typeof exitOnEscape === "undefined") { exitOnEscape = true; }
    if (typeof doAutoPosition === "undefined") { doAutoPosition = true; }
    if (typeof opacity === "undefined") { opacity = 0.9; }
    if (typeof allowExit === "undefined") { allowExit = true; }
    if (typeof overlayId === "undefined") { overlayId = 'wedge-overlay'; }
    if (typeof contentId === "undefined") { contentId = 'wedge-content'; }
    /*
    * Shows the overlay
    */
    var overlay = '#' + overlayId;
    var content = '#' + contentId;

    $('body').append('<div id="' + overlayId + '" style="overflow:hidden;top:0px;left:0px;position:fixed;z-index:2147483630;opacity:' + opacity.toString() + ';background-color:#000000;display:none;width:100%;height:100%;" />');

    if (allowExit) {
        var doExit = function () {
            animator.animateOut(overlayId, contentId, function () {
                if (type == 'div') {
                    $('#' + link).appendTo($('body'));
                    $('#' + link).hide();
                }
                $(overlay).unbind('click', doExit);
                $(document).unbind("keyup", keyUp);

                $(overlay).stop().remove();
                $(content).stop().remove();
            });
        };

        var keyUp = function (e) {
            if (e.keyCode == 27) {
                doExit();
            }
        };

        $(overlay).click(doExit);
        if (exitOnEscape) {
            $(document).keyup(keyUp);
        }
    }

    /*
    * Shows the content
    */
    $('body').append('<div id="' + contentId + '" style="text-align:center;z-index:2147483641;opacity:0;position:fixed;">');
    switch (type) {
        case 'youtube':
            $(content).append('<iframe id="youtube-img" width="853" height="480" src="' + link.replace('/watch?v=', '/embed/').replace(/&.*/, '') + '" frameborder="0" allowfullscreen></iframe><h3 style="color:#A0A0A0;">' + title + '</h3>');
            break;
        case 'img':
            $(content).append('<img id="wedge-img" src="' + link + '" style="max-height:' + (window.innerHeight - 100) + 'px;max-width:' + (window.innerWidth - 100) + 'px;"/><h3 style="color:#A0A0A0;">' + title + '</h3>');
            break;
        case 'div':
            $('#' + link).show();
            $(content).append($("#" + link));
    }
    if (doAutoPosition) {
        //this is more of a sure-fire way to do it, even if it's a bit sketcy
        var doPositioning = function () {
            $(content).css({ top: '50%', left: '50%', marginTop: '-' + ($(content).height() / 2) + 'px', marginLeft: '-' + ($(content).width() / 2) + 'px' });
        };
        doPositioning();
        if (type == 'img') {
            //Some images can take a while to load, so make sure to position them once they load
            var isShownYet = false;
            $("#wedge-img").load(function () {
                if (isShownYet) {
                    //we don't want to mess with anything until after it's shown, since some animations rely on the margins
                    doPositioning();
                }
            });
            animator.animateIn(overlayId, contentId, function () {
                isShownYet = true;
                doPositioning(); //in case the image has already loaded
            });
        } else {
            animator.animateIn(overlayId, contentId, function () {
            });
        }
    } else {
        animator.animateIn(overlayId, contentId, function () {
        });
    }
}
//# sourceMappingURL=Wedge.js.map
