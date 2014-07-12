function suffix(num) {
    var str = num.toString();

    switch(str.substr(str.length - 1)) {
        case "0":
            return str;
        case "1":
            return str + "st";
        case "2":
            return str + "nd";
        case "3":
            return str + "rd";
        default:
            return str + "th";
    }
}