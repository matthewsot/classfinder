/// <reference path="../jquery.d.ts" />
/// <reference path="../Core.ts" />
/// <reference path="FlauntItems.ts" />

//converts an item from the Streamline library into a FlauntItem
//TODO: modify this to your own custom types
function convertItem(item): any {
    switch (item.type) {
        case "text":
            var txt = new FlauntText(item.text);
            return txt;
            break;
        case "post":
            var pst = new FlauntPostWithPic(item.text, "http://gravatar.com/avatar/" + hex_md5(item.email) + "?s=100", item.name, item.id, item.comments, item.likes);
            return pst;
            break;
        case "image":
            var img = new FlauntImage(item.link, item.isLarge);
            return img;
            break;
        case "person":
            var prsn = new FlauntPerson(item.name, "http://gravatar.com/avatar/" + hex_md5(item.email) + "?s=100", item.id, item.relationship);
            return prsn;
            break;
    }
}

// The generic Flaunt item class
class FlauntItem {
    public container: HTMLDivElement;
    public contents: HTMLElement;
    public type: string;
    public row: number;
    public column: number;
    public isLargeTile: bool = false;
    public isDone: bool = false;
    public isSelected: bool = false;
    public isOpen: bool = false;
    public canMakeLarge: bool = true;
    public afterAppend = function (height, width, tile) { };

    constructor() {
        this.container = <HTMLDivElement>document.createElement("div");
        this.container.style.position = "absolute";
    };
}

class Flaunt {
    private expandTile(tile: FlauntItem, divToUse, smallTileSize: number, force: bool = false, addThisMuch: number = null) {
        if (tile.canMakeLarge != false && Flauntly.checkOverflow(tile.contents) || force) {
            tile.isOpen = true;
            
            $(tile.contents).css("height", "auto");
            $(tile.container).css("height", "auto");
            
            //slide all other tiles out of the way
            var thisColumn = tile.column;
            var nextColumn = tile.column + 1;
            var isLarge = tile.isLargeTile;
            var thisRow = tile.row;
            var nowRow = thisRow + 1;
            while (true) {
                if ($("#tileR" + nowRow + "C" + thisColumn).length > 0) {
                    var toChange;
                    if (addThisMuch == null) {
                        toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + tile.container.clientHeight - smallTileSize).toString() + "px";
                    } else {
                        toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + addThisMuch).toString() + "px";
                    }
                    $("#tileR" + nowRow + "C" + thisColumn).css("margin-top", toChange);
                }
                else {
                    break;
                }
                nowRow++;
            }
            if (isLarge) {
                thisColumn++;
                nowRow = 1;
                while (true) {
                    if (nowRow > thisRow) {
                        if ($("#tileR" + nowRow + "C" + thisColumn).length > 0) {
                            var toChange;
                            if (addThisMuch == null) {
                                toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + tile.container.clientHeight - $("#tileR" + nowRow + "C" + thisColumn).height()).toString() + "px";
                            } else {
                                toChange = (parseInt($("#tileR" + nowRow + "C" + thisColumn).css("margin-top").replace("px", "")) + addThisMuch).toString() + "px";
                            }
                            $("#tileR" + nowRow + "C" + thisColumn).css("margin-top", toChange);
                        }
                        else {
                            break;
                        }
                    }
                    nowRow++;
                }
            }

            var currHeight = parseInt(divToUse.style.height.replace("px", ""));
            if ($(divToUse).attr("CHA") == undefined || $(divToUse).attr("CHA") == null || $(divToUse).attr("CHA") == "") {
                $(divToUse).attr("CHA", "0");
            }
            var currHeightAdditions = parseInt($(divToUse).attr("CHA"));

            if (currHeightAdditions < $(tile.container).height()) {
                divToUse.style.height = (currHeight + $(tile.container).height()).toString() + "px";
                currHeightAdditions += $(tile.container).height();
            }
            $(divToUse).attr("CHA", currHeightAdditions);
        }
    }

    public tilePress(tile, makeBig: bool) {
        var change = 10;

        var currWidth = parseFloat(tile.container.style.width.replace("px", ""));
        var currWidthConts = parseFloat(tile.contents.style.width.replace("px", ""));

        var currHeight = parseFloat(tile.container.style.height.replace("px", ""));
        var currHeightConts = parseFloat(tile.contents.style.height.replace("px", ""));

        var currMarginLeft = parseFloat(tile.container.style.marginLeft.replace("px", ""));
        var currMarginLeftConts = parseFloat(tile.contents.style.marginLeft.replace("px", ""));

        var currMarginTop = parseFloat(tile.container.style.marginTop.replace("px", ""));
        var currMarginTopConts = parseFloat(tile.contents.style.marginTop.replace("px", ""));

        if (makeBig) {
            tile.container.style.width = (currWidth + change).toString() + "px";
            tile.contents.style.width = (currWidthConts + change).toString() + "px";

            tile.container.style.height = (currHeight + change).toString() + "px";
            tile.contents.style.height = (currHeightConts + change).toString() + "px";

            tile.container.style.marginLeft = (currMarginLeft - (change / 2)).toString() + "px";

            tile.container.style.marginTop = (currMarginTop - (change / 2)).toString() + "px";
            tile.contents.style.marginTop = (currMarginTopConts - (change / 2)).toString() + "px";

            tile.isSelected = false;
        }
        else {
            tile.container.style.width = (currWidth - change).toString() + "px";
            tile.contents.style.width = (currWidthConts - change).toString() + "px";

            tile.container.style.height = (currHeight - change).toString() + "px";
            tile.contents.style.height = (currHeightConts - change).toString() + "px";

            tile.container.style.marginLeft = (currMarginLeft + (change / 2)).toString() + "px";

            tile.container.style.marginTop = (currMarginTop + (change / 2)).toString() + "px";
            tile.contents.style.marginTop = (currMarginTopConts + (change / 2)).toString() + "px";

            tile.isSelected = true;
        }
    }

    //adds the hover, mouse down, and mouse up events
    public applyEvents(tile: FlauntItem) {
        var divToUse = this.divToUse;
        var tilePress = this.tilePress;
        var expandTile = this.expandTile;
        var smallTileSize = this.smallTileSize;
        tile.container.onmousedown = function () {
            if (!tile.isOpen) {
                tilePress(tile, false);
            }
        };

        tile.container.onmouseup = function () {
            if (tile.isSelected && !tile.isOpen) {
                tilePress(tile, true);
                expandTile(tile, divToUse, smallTileSize);
            }
        };

        tile.container.onmouseenter = function () {
        };

        tile.container.onmouseleave = function () {
            if (tile.isSelected) {
                var x;
                tile.container.onmouseup(x);
            }
        };
    }

    padding: number;
    smallTileSize: number;
    largeTileWidth: number;
    divToUse: HTMLDivElement;

    private makeRegularSize(tile: FlauntItem, currentColumn: number, currentRow: number, isLargeTile: bool) {
        if (isLargeTile) {
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
    }

    private addTile(tile: FlauntItem, currentColumn: number, currentRow: number, isLarge: bool) {
        tile.column = currentColumn;
        tile.row = currentRow;
        this.makeRegularSize(tile, currentColumn, currentRow, isLarge);

        tile.container.style.marginLeft = ((this.smallTileSize * (currentColumn - 1)) + (this.padding * currentColumn)).toString() + "px";
        tile.container.style.marginTop = ((this.smallTileSize * (currentRow - 1)) + (this.padding * currentRow)).toString() + "px";

        tile.container.id = "tileR" + currentRow + "C" + currentColumn;        
        
        tile.container.style.display = "none";

        tile.isDone = true;
        
        this.divToUse.appendChild(tile.container);

        if (isLarge) {
            tile.afterAppend(this.largeTileWidth, this.smallTileSize, tile);
        } else {
            tile.afterAppend(this.smallTileSize, this.smallTileSize, tile);
        }
    }
    
    tiles: FlauntItem[];
    
    /*
     * divToUse: an HTMLDivElement which contains the div we should fill with the items.
     * items: an array of items based off of FlauntItem
     * color: the color of each tile's background. if not specified, a white background is applied
     * padding: the amount of padding between tiles, defaults to 5 px
     * perRow: the number of small tiles per row. defaults to 6
     * autoSpace: whether to automatically space the tiles based on tile size (favoring position in array, of course). Defaults to true
     * handleScrollbar: when the content goes off the screen, a vertical scrollbar is placed, which then makes our horizontal content
     * go off the screen as well, making another scrollbar. To fix this, we'll automatically subtract the scrollbar's width while calculating
     * the sizes of our tiles. It's highly reccomended that you leave this to its default.
     * doAutoHeight: automatically sets the height of the div based on the number of tiles.
     */
    constructor(divToUse: HTMLDivElement, items: FlauntItem[], backgroundColor?: string = "#FFFFFF", padding?: number = 5, perRow?: number = 6, autoSpace?: bool = true, handleScrollbar?: bool = true, doAutoHeight?: bool = false) {
        this.tiles = items.slice(0, items.length); //copy the supplied item array into a local array
        this.divToUse = divToUse;
        this.padding = padding;

        var largePerRow = (perRow / 2); //number of large tiles per row.

        if (document.documentElement.clientHeight < document.body.offsetHeight) {
            handleScrollbar = false;
        }

        //set variables for the sizes of large and small tiles
        if (handleScrollbar) {
            this.largeTileWidth = ((divToUse.clientWidth - Flauntly.getScrollBarWidth()) - (padding * (largePerRow + 1))) / largePerRow;
            this.smallTileSize = (this.largeTileWidth - padding) / 2;
        } else {
            this.largeTileWidth = (divToUse.clientWidth - (padding * (largePerRow + 1))) / largePerRow;
            this.smallTileSize = (this.largeTileWidth - padding) / 2;
        }
        
        //these keep track of the current row/column
        var currentRow: number = 1;
        var currentColumn: number = 1;

        for (var curr in this.tiles) {
            if (this.tiles[curr].isDone) { //if we've already handled this tile (for whatever reason), skip it
                continue;
            }

            this.applyEvents(this.tiles[curr]);
            this.tiles[curr].container.style.backgroundColor = backgroundColor; //setting the background is important, because some tiles will show a transparent border which screws up the beautiful look

            if (!this.tiles[curr].isLargeTile) { //handles small tiles
                if (currentColumn > perRow) { //if we've gone onto the next row, reflect that
                    currentColumn = 1;
                    currentRow++;
                }
                
                this.addTile(this.tiles[curr], currentColumn, currentRow, true); //add the tile
                currentColumn++;
            }
            else { //handles large tiles (or undefined O.O)
                if (currentColumn > perRow) { //(see above)
                    currentColumn = 1;
                    currentRow++;
                }

                /*
                 * The problem with having two sizes of tiles is that if we have only one small tile space left, then
                 * we can't fit a large tile in it! To fix this, we'll look at all the small tiles we haven't done yet
                 * in the order they appear in the tiles array, then add the first small tile we see to fill that gap.
                 */

                if (currentColumn == perRow) {
                    var isFixed: bool = false;

                    if (autoSpace) { //we won't do this if the dev tells us not to
                        for (var i = 0; i < this.tiles.length; i++) {
                            if (!this.tiles[i].isDone && !this.tiles[i].isLargeTile) {
                                this.addTile(this.tiles[i], currentColumn, currentRow, false);
                                isFixed = true;
                                break;
                            }
                        }
                    }
                    //skip to the next line
                    currentColumn = 1;
                    currentRow++;
                    if (!isFixed) {
                        //oh no - the problem hasn't been fixed! either the dev set autoSpace = false, or there weren't any small tiles available.

                        //do whatever you want to here, like add a dummy tile or something
                    }
                }
                
                this.addTile(this.tiles[curr], currentColumn, currentRow, true);

                currentColumn++; //increment the column variable twice, since a large tile is like 2 small ones
                currentColumn++;
            }
            $(this.tiles[curr].contents).css("margin-left", parseInt($(this.tiles[curr].contents).css("margin-left").replace("px", "")) - 20);

            $(this.tiles[curr].contents).animate({
                marginLeft: '+=20'
            }, 500);
            this.tiles[curr].isDone = true;
        }
        for (var x = 1; x <= perRow; x++) {
            for (var y = 1; y <= currentRow; y++) {
                try {
                    $("#tileR" + y + "C" + x).delay(((x - 1) * 75)).fadeIn("slow");
                } catch (e) {
                    console.log("non-fatal error encountered, " + e.message + " with column " + x + " and row " + y);
                }
            }
        }
        
        if (doAutoHeight) {
            divToUse.style.height = ((this.smallTileSize * currentRow) + (padding * (currentRow + 1))).toString() + "px";
        }
    };
}