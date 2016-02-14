/**
PreloadImages functions loads images using image path from array. Images are added to 'imgs' array. On comopletion of loading of all images from 'srcs' array, callback function is called.
*/
function preloadImagesForGalleryCanvas(srcs, imgs, galleryCanvasId) {
    var img;
    var remaining = srcs.length;
    for (var i = 0; i < srcs.length; i++) {
        img = new Image();
        img.onload = function () {
            --remaining;
            if (remaining <= 0) {
                //callback(galleryCanvasId, imgs);
                //callback.apply(galleryCanvasId, imgs);
                drawOverGalleryCanvas(galleryCanvasId, imgs);
            }
        };
        img.src = srcs[i];
        imgs.push(img);
    }
}


/**
   Draws over canvas with id galleryCanvasId. images contains images that would be used for drawing on canvas.
*/
function drawOverGalleryCanvas(galleryCanvasId, images) {

    var galleryCanvasCtx = document.getElementById(galleryCanvasId).getContext('2d');

    //Clear Canvas
    galleryCanvasCtx.clearRect(0, 0, galleryCanvasCtx.width, galleryCanvasCtx.height);
    //galleryCanvasCtx.fillstyle = "grey";
    //galleryCanvasCtx.fill();

    // Pointer for tracking image number in array.  
    var imagesPointer = 0;
    var xPivot = 0;
    var yPivot = 0;
    for (var xTemp = 0; xTemp < 20; xTemp += 1) {
        xPivot = xTemp * 100;

        if (xPivot > galleryCanvasCtx.width) {
            break;
        }
        for (var yTemp = 0; yTemp < 3; yTemp += 1) {
            yPivot = yTemp * 100;

            var galleryCanvasPattern = galleryCanvasCtx.createPattern(images[imagesPointer], "repeat");
            galleryCanvasCtx.fillStyle = galleryCanvasPattern;
            galleryCanvasCtx.beginPath();
            galleryCanvasCtx.moveTo(xPivot + 5, yPivot);
            galleryCanvasCtx.lineTo(xPivot + 100, yPivot);
            galleryCanvasCtx.lineTo(xPivot + 100, yPivot + 50);
            //                            galleryCanvasCtx.lineTo(xPivot + 0, yPivot + 90);
            galleryCanvasCtx.closePath();
            galleryCanvasCtx.fill();

            //                            galleryCanvasCtx.fillStyle = galleryCanvasPattern;
            galleryCanvasCtx.beginPath();
            galleryCanvasCtx.moveTo(xPivot, yPivot);
            galleryCanvasCtx.lineTo(xPivot + 100, yPivot + 50);
            galleryCanvasCtx.lineTo(xPivot + 50, yPivot + 100);
            galleryCanvasCtx.lineTo(xPivot + 0, yPivot + 90);
            galleryCanvasCtx.closePath();
            galleryCanvasCtx.fill();

            //                            galleryCanvasCtx.fillStyle = galleryCanvasPattern;
            galleryCanvasCtx.beginPath();
            galleryCanvasCtx.moveTo(xPivot + 100, yPivot + 55);
            galleryCanvasCtx.lineTo(xPivot + 55, yPivot + 100);
            galleryCanvasCtx.lineTo(xPivot + 95, yPivot + 105);
            //                            galleryCanvasCtx.lineTo(xPivot + 0, yPivot + 90);
            galleryCanvasCtx.closePath();
            galleryCanvasCtx.fill();

            imagesPointer++;
            if (imagesPointer >= images.length) {
                imagesPointer = 0;
            }
        }
    }
    //                     galleryCanvasCtx.fillStyle = "black";
    //                     galleryCanvasCtx.font = "63px Arial";
    //                     galleryCanvasCtx.fillText("@Model.Title", 100, 100);
    //
    //                     galleryCanvasCtx.fillStyle = "black";
    //                     galleryCanvasCtx.font = "21px Arial";
    //                     galleryCanvasCtx.fillText("@Model.Description", 100, 150);
}


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