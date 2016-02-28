//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Math/random
// Returns a random integer between min (included) and max (included)
// Using Math.round() will give you a non-uniform distribution!
function getRandomIntInclusive(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}


/**
 * Returns width of scaled image for corresponding maxHeight.
 *
 * Use this if you want to calculate width of image for fixed height, keeping the aspect ratio.
 */
function getWidthForMaxHeightOfImage(width, height, maxHeight) {
    var ratio = maxHeight / height;
    return width * ratio;
}


/**
Return page width
*/
function getPageWith() {
    if (self.innerHeight) {
        return self.innerWidth;
    }

    if (document.documentElement && document.documentElement.clientWidth) {
        return document.documentElement.clientWidth;
    }

    if (document.body) {
        return document.body.clientWidth;
    }
}

/**
Return page Height
*/
function getPageHeight() {
    if (self.innerWidth) {
        return self.innerHeight;
    }

    if (document.documentElement && document.documentElement.clientHeight) {
        return document.documentElement.clientHeight;
    }

    if (document.body) {
        return document.body.clientHeight;
    }
}