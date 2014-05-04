function convertItem(item) {
    switch(item.type) {
        case "text": {
            var txt = new FlauntText(item.text);
            return txt;
            break;

        }
        case "post": {
            var pst = new FlauntPostWithPic(item.text, "http://gravatar.com/avatar/" + hex_md5(item.email) + "?s=100", item.name, item.id, item.comments, item.likes);
            return pst;
            break;

        }
        case "image": {
            var img = new FlauntImage(item.link, item.isLarge);
            return img;
            break;

        }
        case "person": {
            var prsn = new FlauntPerson(item.name, "http://gravatar.com/avatar/" + hex_md5(item.email) + "?s=100", item.id, item.relationship);
            return prsn;
            break;

        }
    }
}
var FlauntItem = (function () {
    function FlauntItem() {
        this.isLargeTile = false;
        this.isDone = false;
        this.isSelected = false;
        this.isOpen = false;
        this.canMakeLarge = true;
        this.afterAppend = function (height, width, tile) {
        };
        this.container = document.createElement("div");
        this.container.style.position = "absolute";
    }
    return FlauntItem;
})();
var Flaunt = (function () {
    function Flaunt(divToUse, items, backgroundColor, padding, perRow, autoSpace, handleScrollbar, doAutoHeight) {
        if (typeof backgroundColor === "undefined") { backgroundColor = "#FFFFFF"; }
        if (typeof padding === "undefined") { padding = 5; }
        if (typeof perRow === "undefined") { perRow = 6; }
        if (typeof autoSpace === "undefined") { autoSpace = true; }
        if (typeof handleScrollbar === "undefined") { handleScrollbar = true; }
        if (typeof doAutoHeight === "undefined") { doAutoHeight = false; }
        this.tiles = items.slice(0, items.length);
        this.divToUse = divToUse;
        this.padding = padding;
        var largePerRow = (perRow / 2);
        if(document.documentElement.clientHeight < document.body.offsetHeight) {
            handleScrollbar = false;
        }
        if(handleScrollbar) {
            this.largeTileWidth = ((divToUse.clientWidth - Flauntly.getScrollBarWidth()) - (padding * (largePerRow + 1))) / largePerRow;
            this.smallTileSize = (this.largeTileWidth - padding) / 2;
        } else {
            this.largeTileWidth = (divToUse.clientWidth - (padding * (largePerRow + 1))) / largePerRow;
            this.smallTileSize = (this.largeTileWidth - padding) / 2;
        }
        var currentRow = 1;
        var currentColumn = 1;
        for(var curr in this.tiles) {
            if(this.tiles[curr].isDone) {
                continue;
            }
            this.applyEvents(this.tiles[curr]);
            this.tiles[curr].container.style.backgroundColor = backgroundColor;
            if(!this.tiles[curr].isLargeTile) {
                if(currentColumn > perRow) {
                    currentColumn = 1;
                    currentRow++;
                }
                this.addTile(this.tiles[curr], currentColumn, currentRow, true);
                currentColumn++;
            } else {
                if(currentColumn > perRow) {
                    currentColumn = 1;
                    currentRow++;
                }
                if(currentColumn == perRow) {
                    var isFixed = false;
                    if(autoSpace) {
                        for(var i = 0; i < this.tiles.length; i++) {
                            if(!this.tiles[i].isDone && !this.tiles[i].isLargeTile) {
                                this.addTile(this.tiles[i], currentColumn, currentRow, false);
                                isFixed = true;
                                break;
                            }
                        }
                    }
                    currentColumn = 1;
                    currentRow++;
                    if(!isFixed) {
                    }
                }
                this.addTile(this.tiles[curr], currentColumn, currentRow, true);
                currentColumn++;
                currentColumn++;
            }
            $(this.tiles[curr].contents).css("margin-left", parseInt($(this.tiles[curr].contents).css("margin-left").replace("px", "")) - 20);
            $(this.tiles[curr].contents).animate({
                marginLeft: '+=20'
            }, 500);
            this.tiles[curr].isDone = true;
        }
        for(var x = 1; x <= perRow; x++) {
            for(var y = 1; y <= currentRow; y++) {
                try  {
                    $("#tileR" + y + "C" + x).delay(((x - 1) * 75)).fadeIn("slow");
                } catch (e) {
                    console.log("non-fatal error encountered, " + e.message + " with column " + x + " and row " + y);
                }
            }
        }
        if(doAutoHeight) {
            divToUse.style.height = ((this.smallTileSize * currentRow) + (padding * (currentRow + 1))).toString() + "px";
        }
    }
    Flaunt.prototype.expandTile = function (tile, divToUse, smallTileSize, force, addThisMuch) {
        if (typeof force === "undefined") { force = false; }
        if (typeof addThisMuch === "undefined") { addThisMuch = null; }
        if(tile.canMakeLarge != false && Flauntly.checkOverflow(tile.contents) || force) {
            tile.isOpen = true;
            $(tile.contents).css("height", "auto");
            $(tile.container).css("height", "auto");
            var thisColumn = tile.column;
            var nextColumn = tile.column + 1;
            var isLarge = tile.isLargeTile;
            var thisRow = tile.row;
            var nowRow = thisRow + 1;
            while(true) {
                if($("#tileR" + nowRow + "C" + thisColumn).length > 0) {
                    var toChange;
                    if(addThisMuch == null) {
                        toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + tile.container.clientHeight - smallTileSize).toString() + "px";
                    } else {
                        toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + addThisMuch).toString() + "px";
                    }
                    $("#tileR" + nowRow + "C" + thisColumn).css("margin-top", toChange);
                } else {
                    break;
                }
                nowRow++;
            }
            if(isLarge) {
                thisColumn++;
                nowRow = 1;
                while(true) {
                    if(nowRow > thisRow) {
                        if($("#tileR" + nowRow + "C" + thisColumn).length > 0) {
                            var toChange;
                            if(addThisMuch == null) {
                                toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + tile.container.clientHeight - $("#tileR" + nowRow + "C" + thisColumn).height()).toString() + "px";
                            } else {
                                toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + addThisMuch).toString() + "px";
                            }
                            $("#tileR" + nowRow + "C" + thisColumn).css("margin-top", toChange);
                        } else {
                            break;
                        }
                    }
                    nowRow++;
                }
            }
            var currHeight = parseInt(divToUse.style.height.replace("px", ""));
            if($(divToUse).attr("CHA") == undefined || $(divToUse).attr("CHA") == null || $(divToUse).attr("CHA") == "") {
                $(divToUse).attr("CHA", "0");
            }
            var currHeightAdditions = parseInt($(divToUse).attr("CHA"));
            if(currHeightAdditions < $(tile.container).height()) {
                divToUse.style.height = (currHeight + $(tile.container).height()).toString() + "px";
                currHeightAdditions += $(tile.container).height();
            }
            $(divToUse).attr("CHA", currHeightAdditions);
        }
    };
    Flaunt.prototype.tilePress = function (tile, makeBig) {
        var change = 10;
        var currWidth = parseFloat(tile.container.style.width.replace("px", ""));
        var currWidthConts = parseFloat(tile.contents.style.width.replace("px", ""));
        var currHeight = parseFloat(tile.container.style.height.replace("px", ""));
        var currHeightConts = parseFloat(tile.contents.style.height.replace("px", ""));
        var currMarginLeft = parseFloat(tile.container.style.marginLeft.replace("px", ""));
        var currMarginLeftConts = parseFloat(tile.contents.style.marginLeft.replace("px", ""));
        var currMarginTop = parseFloat(tile.container.style.marginTop.replace("px", ""));
        var currMarginTopConts = parseFloat(tile.contents.style.marginTop.replace("px", ""));
        if(makeBig) {
            tile.container.style.width = (currWidth + change).toString() + "px";
            tile.contents.style.width = (currWidthConts + change).toString() + "px";
            tile.container.style.height = (currHeight + change).toString() + "px";
            tile.contents.style.height = (currHeightConts + change).toString() + "px";
            tile.container.style.marginLeft = (currMarginLeft - (change / 2)).toString() + "px";
            tile.container.style.marginTop = (currMarginTop - (change / 2)).toString() + "px";
            tile.contents.style.marginTop = (currMarginTopConts - (change / 2)).toString() + "px";
            tile.isSelected = false;
        } else {
            tile.container.style.width = (currWidth - change).toString() + "px";
            tile.contents.style.width = (currWidthConts - change).toString() + "px";
            tile.container.style.height = (currHeight - change).toString() + "px";
            tile.contents.style.height = (currHeightConts - change).toString() + "px";
            tile.container.style.marginLeft = (currMarginLeft + (change / 2)).toString() + "px";
            tile.container.style.marginTop = (currMarginTop + (change / 2)).toString() + "px";
            tile.contents.style.marginTop = (currMarginTopConts + (change / 2)).toString() + "px";
            tile.isSelected = true;
        }
    };
    Flaunt.prototype.applyEvents = function (tile) {
        var divToUse = this.divToUse;
        var tilePress = this.tilePress;
        var expandTile = this.expandTile;
        var smallTileSize = this.smallTileSize;
        tile.container.onmousedown = function () {
            if(!tile.isOpen) {
                tilePress(tile, false);
            }
        };
        tile.container.onmouseup = function () {
            if(tile.isSelected && !tile.isOpen) {
                tilePress(tile, true);
                expandTile(tile, divToUse, smallTileSize);
            }
        };
        tile.container.onmouseenter = function () {
        };
        tile.container.onmouseleave = function () {
            if(tile.isSelected) {
                var x;
                tile.container.onmouseup(x);
            }
        };
    };
    Flaunt.prototype.makeRegularSize = function (tile, currentColumn, currentRow, isLargeTile) {
        if(isLargeTile) {
            tile.container.style.width = this.largeTileWidth.toString() + "px";
            tile.contents.style.width = (this.largeTileWidth - (this.padding * 2)).toString() + "px";
        } else {
            tile.container.style.width = this.smallTileSize.toString() + "px";
            tile.contents.style.width = (this.smallTileSize - (this.padding * 2)).toString() + "px";
        }
        tile.container.style.height = this.smallTileSize.toString() + "px";
        tile.contents.style.height = (this.smallTileSize - (this.padding * 2)).toString() + "px";
        tile.contents.style.paddingTop = this.padding.toString() + "px";
        tile.contents.style.paddingLeft = this.padding.toString() + "px";
        tile.contents.style.overflow = "hidden";
    };
    Flaunt.prototype.addTile = function (tile, currentColumn, currentRow, isLarge) {
        tile.column = currentColumn;
        tile.row = currentRow;
        this.makeRegularSize(tile, currentColumn, currentRow, isLarge);
        tile.container.style.marginLeft = ((this.smallTileSize * (currentColumn - 1)) + (this.padding * currentColumn)).toString() + "px";
        tile.container.style.marginTop = ((this.smallTileSize * (currentRow - 1)) + (this.padding * currentRow)).toString() + "px";
        tile.container.id = "tileR" + currentRow + "C" + currentColumn;
        tile.container.style.display = "none";
        tile.isDone = true;
        this.divToUse.appendChild(tile.container);
        if(isLarge) {
            tile.afterAppend(this.largeTileWidth, this.smallTileSize, tile);
        } else {
            tile.afterAppend(this.smallTileSize, this.smallTileSize, tile);
        }
    };
    return Flaunt;
})();
