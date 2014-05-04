var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var FlauntImage = (function (_super) {
    __extends(FlauntImage, _super);
    function FlauntImage(Link, isLarge) {
        _super.call(this);
        this.canMakeLarge = false;
        if(isLarge == undefined) {
            isLarge = false;
        }
        this.isLargeTile = isLarge;
        this.contents = document.createElement("div");
        var img = document.createElement("img");
        img.setAttribute("src", Link);
        img.style.height = "100%";
        img.style.width = "100%";
        this.contents.appendChild(img);
        this.container.appendChild(this.contents);
    }
    return FlauntImage;
})(FlauntItem);
var FlauntPostWithPic = (function (_super) {
    __extends(FlauntPostWithPic, _super);
    function FlauntPostWithPic(text, picLink, name, id, comments, likes, backgroundColor, style, fontSize) {
        if (typeof backgroundColor === "undefined") { backgroundColor = "#fff"; }
        _super.call(this);
        this.afterAppend = function (height, width, tile) {
            var cmtDiv = document.createElement("div");
            cmtDiv.id = "cmts" + id;
            cmtDiv.style.margin = "0px";
            for(var cmt in this.comments) {
                var comment = this.comments[cmt];
                var cmtP = document.createElement("p");
                cmtP.textContent = comment.text;
                cmtDiv.appendChild(cmtP);
            }
            var extraDiv = document.createElement("div");
            extraDiv.style.margin = "0px";
            var textBx = document.createElement("textarea");
            textBx.style.width = "90%";
            textBx.rows = 5;
            textBx.id = "postID" + tile.id;
            var br = document.createElement("br");
            var likeTxt = document.createElement("span");
            likeTxt.id = "likes" + tile.id;
            likeTxt.textContent = tile.likes.length + " like";
            if(tile.likes.length != 1) {
                likeTxt.textContent += "s";
            }
            var likeList = document.createElement("ul");
            likeList.id = "listForID" + id;
            for(var lk = 0; lk < tile.likes.length; lk++) {
                var li = document.createElement("li");
                li.textContent = tile.likes[lk].name;
                likeList.appendChild(li);
            }
            likeList.style.color = "#fff";
            $(likeList).hide();
            $("body").append(likeList);
            likeTxt.onmouseup = function () {
                initWedge(likeList.id, "", "div");
            };
            likeTxt.textContent += " ";
            var likeBtn = document.createElement("button");
            likeBtn.textContent = "Like";
            likeBtn.id = "lkbtnID" + tile.id + "R" + tile.row + "C" + tile.column;
            likeBtn.onclick = function () {
                var id = tile.id;
                if(likeBtn.textContent == "Like") {
                    likeBtn.textContent = "Unlike";
                    conn.server.likePost(uID, Challenge, id);
                } else {
                    if(likeBtn.textContent == "Unlike") {
                        likeBtn.textContent = "Like";
                        conn.server.unlikePost(uID, Challenge, id);
                    }
                }
            };
            var btn = document.createElement("button");
            btn.textContent = "Comment";
            btn.id = "cmtbtnID" + tile.id + "R" + tile.row + "C" + tile.column;
            btn.onclick = function () {
                var id = tile.id;
                var row = tile.row;
                var col = tile.column;
                var toPost = $(textBx).val();
                conn.server.newComment(uID, Challenge, toPost, id);
                $(textBx).val("");
            };
            extraDiv.appendChild(cmtDiv);
            extraDiv.appendChild(textBx);
            extraDiv.appendChild(br);
            extraDiv.appendChild(likeTxt);
            var hasLiked = false;
            for(var lk in tile.likes) {
                if(tile.likes[lk].id == uID) {
                    hasLiked = true;
                    break;
                }
            }
            if(hasLiked) {
                likeBtn.textContent = "Unlike";
            }
            extraDiv.appendChild(likeBtn);
            extraDiv.appendChild(btn);
            var undisplay = Flauntly.getUndisplayHeight(tile.contents.children[1]);
            if(undisplay < height) {
                extraDiv.style.paddingTop = tile.container.style.height;
            }
            tile.contents.children[1].appendChild(extraDiv);
        };
        this.id = id;
        this.likes = likes;
        this.comments = comments;
        this.isLargeTile = true;
        this.contents = document.createElement("div");
        this.contents.style.margin = "0px";
        var PersonHolder = document.createElement("div");
        PersonHolder.style.cssFloat = "left";
        PersonHolder.style.margin = "0px";
        var Pic = document.createElement("img");
        Pic.src = picLink;
        Pic.style.margin = "0px";
        Pic.style.height = "100px";
        Pic.style.width = "100px";
        PersonHolder.appendChild(Pic);
        PersonHolder.innerHTML += "<br/>";
        var Name = document.createElement("span");
        Name.textContent = name;
        PersonHolder.appendChild(Name);
        var Text = document.createElement("p");
        Text.id = "textForID" + id;
        Text.style.marginLeft = "110px";
        Text.style.cssFloat = "top";
        Text.textContent = text;
        Text.style.height = "auto";
        Text.style.overflow = "hidden";
        Text.style.marginTop = "0px";
        this.contents.appendChild(PersonHolder);
        this.contents.appendChild(Text);
        this.container.appendChild(this.contents);
        this.container.style.backgroundColor = backgroundColor;
        if(style != undefined) {
            this.contents.style = style;
        }
        if(fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        } else {
            if(this.container.style.fontSize == undefined && this.container.style.fontSize == "auto" && this.container.style.fontSize == null) {
                this.container.style.fontSize = "3ex";
            }
        }
    }
    return FlauntPostWithPic;
})(FlauntItem);
var FlauntPeriod = (function (_super) {
    __extends(FlauntPeriod, _super);
    function FlauntPeriod(name, others, backgroundColor, style, fontSize) {
        if (typeof backgroundColor === "undefined") { backgroundColor = "#fff"; }
        _super.call(this);
        this.afterAppend = function (height, width, tile) {
            var peopleDiv = document.createElement("div");
            peopleDiv.style.margin = "0px";
            for(var ot = 0; ot < others.length; ot++) {
                var newPerson = document.createElement("div");
                newPerson.textContent = others[ot].Name;
                peopleDiv.appendChild(newPerson);
            }
            var undisplay = Flauntly.getUndisplayHeight(tile.contents);
            if(undisplay < height) {
                peopleDiv.style.paddingTop = (undisplay - Flauntly.getUndisplayHeight(tile.contents.children[0])).toString() + "px";
            }
            tile.contents.children[0].appendChild(peopleDiv);
        };
        this.isLargeTile = true;
        this.contents = document.createElement("div");
        this.contents.style.margin = "0px";
        var Text = document.createElement("h1");
        Text.style.cssFloat = "top";
        Text.textContent = name;
        Text.style.height = "auto";
        Text.style.overflow = "hidden";
        Text.style.marginTop = "0px";
        this.contents.appendChild(Text);
        this.container.appendChild(this.contents);
        this.container.style.backgroundColor = backgroundColor;
        if(style != undefined) {
            this.contents.style = style;
        }
        if(fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        }
    }
    return FlauntPeriod;
})(FlauntItem);
var FlauntPerson = (function (_super) {
    __extends(FlauntPerson, _super);
    function FlauntPerson(name, picLink, id, relationship, backgroundColor, style, fontSize) {
        if (typeof backgroundColor === "undefined") { backgroundColor = "#fff"; }
        _super.call(this);
        this.afterAppend = function (height, width, tile) {
            var optionsDiv = document.createElement("div");
            optionsDiv.id = "opts" + id;
            optionsDiv.style.margin = "0px";
            var ProfileBtn = document.createElement("button");
            ProfileBtn.textContent = "Profile";
            ProfileBtn.onclick = function () {
                window.location.href = "/User/?id=" + id;
            };
            switch(relationship) {
                case "friend": {
                    var UnfriendMe = document.createElement("button");
                    UnfriendMe.textContent = "Unfriend";
                    UnfriendMe.onclick = function () {
                        conn.server.unFriend(uID, Challenge, id);
                        updateWithLayer(document.getElementById("friendsLink"), "friendlyPeople");
                    };
                    optionsDiv.appendChild(UnfriendMe);
                    break;

                }
                case "person": {
                    var FriendMe = document.createElement("button");
                    FriendMe.textContent = "Friend";
                    FriendMe.onclick = function () {
                        conn.server.sendFriendRequest(uID, Challenge, id);
                        optionsDiv.removeChild(FriendMe);
                        var RequestedText = document.createElement("span");
                        RequestedText.style.fontSize = "1ex";
                        RequestedText.textContent = "request sent ";
                        optionsDiv.innerHTML = "";
                        optionsDiv.appendChild(RequestedText);
                        var ProfBtn = document.createElement("button");
                        ProfBtn.textContent = "Profile";
                        ProfBtn.onclick = function () {
                            window.location.href = "/User/?id=" + id;
                        };
                        optionsDiv.appendChild(ProfBtn);
                    };
                    optionsDiv.appendChild(FriendMe);
                    break;

                }
                case "request": {
                    var FriendMe = document.createElement("button");
                    FriendMe.textContent = "Accept";
                    FriendMe.onclick = function () {
                        conn.server.acceptFriendRequest(uID, Challenge, id);
                    };
                    var DenyMe = document.createElement("button");
                    DenyMe.textContent = "Deny";
                    DenyMe.onclick = function () {
                        conn.server.denyFriendRequest(uID, Challenge, id);
                    };
                    optionsDiv.appendChild(FriendMe);
                    optionsDiv.appendChild(DenyMe);
                    break;

                }
                case "requested": {
                    var RequestedText = document.createElement("span");
                    RequestedText.style.fontSize = "1ex";
                    RequestedText.textContent = "request sent ";
                    optionsDiv.appendChild(RequestedText);
                    break;

                }
            }
            optionsDiv.appendChild(ProfileBtn);
            var undisplay = Flauntly.getUndisplayHeight(tile.contents);
            if(undisplay < height) {
                optionsDiv.style.paddingTop = (undisplay - Flauntly.getUndisplayHeight(tile.contents.children[1])).toString() + "px";
            }
            tile.contents.children[1].appendChild(optionsDiv);
        };
        this.id = id;
        this.isLargeTile = true;
        this.contents = document.createElement("div");
        this.contents.style.margin = "0px";
        var Pic = document.createElement("img");
        Pic.src = picLink;
        Pic.style.cssFloat = "left";
        Pic.style.margin = "0px";
        Pic.style.height = "100px";
        Pic.style.width = "100px";
        var Text = document.createElement("h1");
        Text.id = "textForUID" + id;
        Text.style.marginLeft = "110px";
        Text.style.cssFloat = "top";
        Text.textContent = name;
        if(relationship == "request") {
            Text.textContent += " wants to be your friend";
        }
        Text.style.height = "auto";
        Text.style.overflow = "hidden";
        Text.style.marginTop = "0px";
        this.contents.appendChild(Pic);
        this.contents.appendChild(Text);
        this.container.appendChild(this.contents);
        this.container.style.backgroundColor = backgroundColor;
        if(style != undefined) {
            this.contents.style = style;
        }
        if(fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        }
    }
    return FlauntPerson;
})(FlauntItem);
var FlauntText = (function (_super) {
    __extends(FlauntText, _super);
    function FlauntText(text, backgroundColor, style, fontSize) {
        if (typeof backgroundColor === "undefined") { backgroundColor = "#fff"; }
        _super.call(this);
        this.isLargeTile = true;
        this.contents = document.createElement("p");
        this.contents.textContent = text;
        this.contents.style.margin = "0px";
        this.container.appendChild(this.contents);
        this.container.style.backgroundColor = backgroundColor;
        if(style != undefined) {
            this.contents.style = style;
        }
        if(fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        } else {
            if(this.container.style.fontSize == undefined && this.container.style.fontSize == "auto" && this.container.style.fontSize == null) {
                this.container.style.fontSize = "3ex";
            }
        }
        this.fontSize = this.contents.style.fontSize;
    }
    return FlauntText;
})(FlauntItem);
var FlauntHeadingWithSubtitle = (function (_super) {
    __extends(FlauntHeadingWithSubtitle, _super);
    function FlauntHeadingWithSubtitle(heading, subtitle, allowHTML) {
        if (typeof allowHTML === "undefined") { allowHTML = false; }
        _super.call(this);
        this.isLargeTile = true;
        this.contents = document.createElement("div");
        this.heading = document.createElement("h1");
        this.heading.style.marginBottom = "2px";
        this.heading.style.marginTop = "0";
        this.heading.style.marginLeft = "0";
        if(allowHTML) {
            this.heading.innerHTML = heading;
        } else {
            this.heading.textContent = heading;
        }
        this.subtitle = document.createElement("h2");
        this.subtitle.style.marginTop = "0";
        this.subtitle.style.marginLeft = "2px";
        if(allowHTML) {
            this.subtitle.innerHTML = subtitle;
        } else {
            this.subtitle.textContent = subtitle;
        }
        this.contents.appendChild(this.heading);
        this.contents.appendChild(this.subtitle);
        this.container.appendChild(this.contents);
    }
    return FlauntHeadingWithSubtitle;
})(FlauntItem);
