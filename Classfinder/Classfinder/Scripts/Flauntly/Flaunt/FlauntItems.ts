/// <reference path="../jquery.d.ts" />
/// <reference path="../Core.ts" />
/// <reference path="../Wedge/Core.ts" />

//Provides basic Flaunt items that can be used to expand beyond the generic FlauntItem class

// Provides a Flaunt item for basic image display
class FlauntImage extends FlauntItem {
    canMakeLarge = false;
    constructor(Link: string, isLarge?: bool) {
        super();
        if (isLarge == undefined) {
            isLarge = false;
        }
        this.isLargeTile = isLarge;
        this.contents = <HTMLDivElement>document.createElement("div");
        var img = <HTMLImageElement>document.createElement("img");
        img.setAttribute("src", Link);
        img.style.height = "100%";
        img.style.width = "100%";
        this.contents.appendChild(img);
        this.container.appendChild(this.contents);
    };
}

// Provides a Flaunt Item designed to display a Flaunt post
// NOTE: This uses the conn variable, if you're not using
// our SignalR implementation in your application,
// you might want to remove this.
class FlauntPostWithPic extends FlauntItem {
    id: number;
    comments: any;
    likes: any;
    constructor(text: string, picLink: string, name: string, id: number, comments: any, likes: any, backgroundColor?: string = "#fff", style?: MSStyleCSSProperties, fontSize?: string) {
        super();
        this.afterAppend = function (height, width, tile: FlauntPostWithPic) {
            var cmtDiv = <HTMLDivElement>document.createElement("div"); //create a div to hold our comments
            cmtDiv.id = "cmts" + id;
            cmtDiv.style.margin = "0px";
            for (var cmt in this.comments) {
                var comment = this.comments[cmt];
                var cmtP = <HTMLParagraphElement>document.createElement("p");
                cmtP.textContent = comment.text;
                cmtDiv.appendChild(cmtP); //populate that comment div with our comments
            }

            var extraDiv = <HTMLDivElement>document.createElement("div");
            extraDiv.style.margin = "0px";

            var textBx = <HTMLTextAreaElement>document.createElement("textarea"); //create a textarea to make new comments
            textBx.style.width = "90%";
            textBx.rows = 5;
            textBx.id = "postID" + tile.id;

            var br = <HTMLBRElement>document.createElement("br");

            var likeTxt = <HTMLSpanElement>document.createElement("span"); //create a span to tell the user how many likes there are
            likeTxt.id = "likes" + tile.id;
            likeTxt.textContent = tile.likes.length + " like";
            if (tile.likes.length != 1) {
                likeTxt.textContent += "s";
            }
            var likeList = <HTMLUListElement>document.createElement("ul");
            likeList.id = "listForID" + id;
            for (var lk = 0; lk < tile.likes.length; lk++) {
                var li = <HTMLLIElement>document.createElement("li");
                li.textContent = tile.likes[lk].name;
                likeList.appendChild(li);
            }
            likeList.style.color = "#fff";
            $(likeList).hide();
            $("body").append(likeList);
            likeTxt.onmouseup = function () {
                initWedge(likeList.id, "", "div");
            }
            likeTxt.textContent += " ";

            var likeBtn = <HTMLButtonElement>document.createElement("button"); //create a button to like the post
            likeBtn.textContent = "Like";
            likeBtn.id = "lkbtnID" + tile.id + "R" + tile.row + "C" + tile.column;
            likeBtn.onclick = function () {
                var id = tile.id;
                if (likeBtn.textContent == "Like") {
                    likeBtn.textContent = "Unlike";
                    conn.server.likePost(uID, Challenge, id); //like the post
                } else if (likeBtn.textContent == "Unlike") {
                    likeBtn.textContent = "Like";
                    conn.server.unlikePost(uID, Challenge, id); //like the post
                }
            }

            var btn = <HTMLButtonElement>document.createElement("button"); //create a button to comment on the post
            btn.textContent = "Comment";
            btn.id = "cmtbtnID" + tile.id + "R" + tile.row + "C" + tile.column;
            btn.onclick = function () {
                var id = tile.id;
                var row = tile.row;
                var col = tile.column;
                var toPost = $(textBx).val();
                conn.server.newComment(uID, Challenge, toPost, id);
                $(textBx).val("");
            }

            extraDiv.appendChild(cmtDiv);
            extraDiv.appendChild(textBx);
            extraDiv.appendChild(br);
            extraDiv.appendChild(likeTxt);
            var hasLiked: bool = false;
            for (var lk in tile.likes) {
                if (tile.likes[lk].id == uID) {
                    hasLiked = true;
                    break;
                }
            }
            if (hasLiked) {
                likeBtn.textContent = "Unlike";
            }
            extraDiv.appendChild(likeBtn);

            extraDiv.appendChild(btn);
            var undisplay = Flauntly.getUndisplayHeight(<HTMLElement>tile.contents.children[1]);
            if (undisplay < height) {
                extraDiv.style.paddingTop = tile.container.style.height;
            }
            tile.contents.children[1].appendChild(extraDiv);
        }
        this.id = id;
        this.likes = likes;
        this.comments = comments;
        this.isLargeTile = true; //text tiles should default to a larget tile.
        this.contents = <HTMLDivElement>document.createElement("div");
        this.contents.style.margin = "0px";
        var PersonHolder = <HTMLDivElement>document.createElement("div");
        PersonHolder.style.cssFloat = "left";
        PersonHolder.style.margin = "0px";

        var Pic = <HTMLImageElement>document.createElement("img");
        Pic.src = picLink;
        //Pic.style.cssFloat = "left";
        Pic.style.margin = "0px";
        Pic.style.height = "100px";
        Pic.style.width = "100px";

        PersonHolder.appendChild(Pic);
        PersonHolder.innerHTML += "<br/>";
        var Name = <HTMLSpanElement>document.createElement("span");
        Name.textContent = name;
        PersonHolder.appendChild(Name);

        var Text = <HTMLParagraphElement>document.createElement("p");
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

        if (style != undefined) {
            this.contents.style = style;
        }
        
        if (fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        }
        else if (this.container.style.fontSize == undefined && this.container.style.fontSize == "auto" && this.container.style.fontSize == null) { //we don't want to override the style's fontSize
            this.container.style.fontSize = "3ex";
        }
    };
}

class FlauntPeriod extends FlauntItem {
    constructor(name: string, others, backgroundColor?: string = "#fff", style?: MSStyleCSSProperties, fontSize?: string) {
        super();
        this.afterAppend = function (height, width, tile: FlauntPostWithPic) {
            var peopleDiv = <HTMLDivElement>document.createElement("div"); //create a div to hold our others
            peopleDiv.style.margin = "0px";
            for (var ot = 0; ot < others.length; ot++) {
                var newPerson = <HTMLDivElement>document.createElement("div");
                newPerson.textContent = others[ot].Name;
                peopleDiv.appendChild(newPerson);
            }
            var undisplay = Flauntly.getUndisplayHeight(<HTMLElement>tile.contents);
            if (undisplay < height) {
                peopleDiv.style.paddingTop = (undisplay - Flauntly.getUndisplayHeight(<HTMLElement>tile.contents.children[0])).toString() + "px";
            }
            tile.contents.children[0].appendChild(peopleDiv);
        }
        this.isLargeTile = true; //text tiles should default to a larget tile.
        this.contents = <HTMLDivElement>document.createElement("div");
        this.contents.style.margin = "0px";

        var Text = <HTMLHeadingElement>document.createElement("h1");
        Text.style.cssFloat = "top";
        Text.textContent = name;
        Text.style.height = "auto";
        Text.style.overflow = "hidden";
        Text.style.marginTop = "0px";

        this.contents.appendChild(Text);
        this.container.appendChild(this.contents);
        this.container.style.backgroundColor = backgroundColor;

        if (style != undefined) {
            this.contents.style = style;
        }
        
        if (fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        }
    };
}

class FlauntPerson extends FlauntItem {
    id: number;
    constructor(name: string, picLink: string, id: number, relationship: string, backgroundColor?: string = "#fff", style?: MSStyleCSSProperties, fontSize?: string) {
        super();
        this.afterAppend = function (height, width, tile: FlauntPostWithPic) {
            var optionsDiv = <HTMLDivElement>document.createElement("div"); //create a div to hold our options
            optionsDiv.id = "opts" + id;
            optionsDiv.style.margin = "0px";
            
            var ProfileBtn = <HTMLButtonElement>document.createElement("button");
            ProfileBtn.textContent = "Profile";

            ProfileBtn.onclick = function () {
                window.location.href = "/User/?id=" + id;
            }

            switch (relationship) {
                case "friend":
                    var UnfriendMe = <HTMLButtonElement>document.createElement("button");
                    UnfriendMe.textContent = "Unfriend";

                    UnfriendMe.onclick = function () {
                        conn.server.unFriend(uID, Challenge, id);
                        updateWithLayer(document.getElementById("friendsLink"), "friendlyPeople");
                    }
                    optionsDiv.appendChild(UnfriendMe);
                    break;
                case "person":
                    var FriendMe = <HTMLButtonElement>document.createElement("button");
                    FriendMe.textContent = "Friend";

                    FriendMe.onclick = function () {
                        conn.server.sendFriendRequest(uID, Challenge, id);
                        optionsDiv.removeChild(FriendMe);

                        var RequestedText = <HTMLSpanElement>document.createElement("span");
                        RequestedText.style.fontSize = "1ex";
                        RequestedText.textContent = "request sent ";
                        optionsDiv.innerHTML = "";
                        optionsDiv.appendChild(RequestedText);
                        var ProfBtn = <HTMLButtonElement>document.createElement("button");
                        ProfBtn.textContent = "Profile";

                        ProfBtn.onclick = function () {
                            window.location.href = "/User/?id=" + id;
                        }
                        optionsDiv.appendChild(ProfBtn);
                    }
                    optionsDiv.appendChild(FriendMe);
                    break;
                case "request":
                    var FriendMe = <HTMLButtonElement>document.createElement("button");
                    FriendMe.textContent = "Accept";

                    FriendMe.onclick = function () {
                        conn.server.acceptFriendRequest(uID, Challenge, id);
                    }

                    var DenyMe = <HTMLButtonElement>document.createElement("button");
                    DenyMe.textContent = "Deny";

                    DenyMe.onclick = function () {
                        conn.server.denyFriendRequest(uID, Challenge, id);
                    }
                    optionsDiv.appendChild(FriendMe);
                    optionsDiv.appendChild(DenyMe);
                    break;
                case "requested":
                    var RequestedText = <HTMLSpanElement>document.createElement("span");
                    RequestedText.style.fontSize = "1ex";
                    RequestedText.textContent = "request sent ";
                    optionsDiv.appendChild(RequestedText);
                    break;
            }

            optionsDiv.appendChild(ProfileBtn);

            var undisplay = Flauntly.getUndisplayHeight(<HTMLElement>tile.contents);
            if (undisplay < height) {
                optionsDiv.style.paddingTop = (undisplay - Flauntly.getUndisplayHeight(<HTMLElement>tile.contents.children[1])).toString() + "px";
            }
            tile.contents.children[1].appendChild(optionsDiv);
        }
        this.id = id;
        this.isLargeTile = true; //text tiles should default to a larget tile.
        this.contents = <HTMLDivElement>document.createElement("div");
        this.contents.style.margin = "0px";

        var Pic = <HTMLImageElement>document.createElement("img");
        Pic.src = picLink;
        Pic.style.cssFloat = "left";
        Pic.style.margin = "0px";
        Pic.style.height = "100px";
        Pic.style.width = "100px";

        var Text = <HTMLHeadingElement>document.createElement("h1");
        Text.id = "textForUID" + id;
        Text.style.marginLeft = "110px";
        Text.style.cssFloat = "top";
        Text.textContent = name;
        if (relationship == "request") {
            Text.textContent += " wants to be your friend";
        }
        Text.style.height = "auto";
        Text.style.overflow = "hidden";
        Text.style.marginTop = "0px";

        this.contents.appendChild(Pic);
        this.contents.appendChild(Text);
        this.container.appendChild(this.contents);
        this.container.style.backgroundColor = backgroundColor;

        if (style != undefined) {
            this.contents.style = style;
        }
        
        if (fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        }
    };
}

// Provides a Flaunt Item designed for basic text display
class FlauntText extends FlauntItem {
    public fontSize: string;

    constructor(text: string, backgroundColor?: string = "#fff", style?: MSStyleCSSProperties, fontSize?: string) {
        super();
        this.isLargeTile = true;
        this.contents = <HTMLParagraphElement>document.createElement("p");
        this.contents.textContent = text;
        this.contents.style.margin = "0px";
        this.container.appendChild(this.contents);

        this.container.style.backgroundColor = backgroundColor;
        if (style != undefined) {
            this.contents.style = style;
        }
        
        if (fontSize != undefined) {
            this.container.style.fontSize = fontSize;
        }
        else if (this.container.style.fontSize == undefined && this.container.style.fontSize == "auto" && this.container.style.fontSize == null) { //we don't want to override the style's fontSize
            this.container.style.fontSize = "3ex";
        }
        this.fontSize = this.contents.style.fontSize;
    };
}

// Provides a Flaunt Item designed for text display
// with one large and one small heading.
class FlauntHeadingWithSubtitle extends FlauntItem {
    heading: HTMLHeadingElement;
    subtitle: HTMLHeadingElement;

    constructor(heading: string, subtitle: string, allowHTML?: bool = false) {
        super();

        this.isLargeTile = true; //text tiles should default to a large tile.
        this.contents = <HTMLDivElement>document.createElement("div");
        this.heading = <HTMLHeadingElement>document.createElement("h1");
        this.heading.style.marginBottom = "2px";
        this.heading.style.marginTop = "0";
        this.heading.style.marginLeft = "0";

        if (allowHTML) {
            this.heading.innerHTML = heading;
        } else {
            this.heading.textContent = heading;
        }

        this.subtitle = <HTMLHeadingElement>document.createElement("h2");
        this.subtitle.style.marginTop = "0";
        this.subtitle.style.marginLeft = "2px";

        if (allowHTML) {
            this.subtitle.innerHTML = subtitle;
        } else {
            this.subtitle.textContent = subtitle;
        }

        this.contents.appendChild(this.heading);
        this.contents.appendChild(this.subtitle);
        this.container.appendChild(this.contents);
    }
}