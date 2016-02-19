var CanvasXSize;
var CanvasYSize;
(function () {
    //Defining Canvas dimentions
    // TODO: set width percent dynamically, depending upon device.
    var galleryCanavasWidthPercentage = 60;
    var pageWidth = getPageWith();
    if (pageWidth < 767 && pageWidth > 480) {
        galleryCanavasWidthPercentage = 80;
    }
    else if (pageWidth <= 480) {
        galleryCanavasWidthPercentage = 90;
    }
    CanvasXSize = ( pageWidth/ 100) * galleryCanavasWidthPercentage;
    CanvasYSize = 300;
 })();


/**
 Function to draw canvas gallery. It prepares canvas, then call preloadImagesForGalleryCanvas(), which then calls drawOverGalleryCanvas().
*/
function prepareGalleryCanvas(galleryId, galleryCanvasId, imageSrcs) {

    //Set galleryCanvas dimentions
    var galleryCanvasElement = document.getElementById(galleryCanvasId);
    galleryCanvasElement.height = CanvasYSize;
    galleryCanvasElement.width = CanvasXSize;

    //Adding onclick event on galleryCanvas.
    document.getElementById(galleryCanvasId).onclick = function (e) {
        window.location.href = '/Galleries/'+galleryId;
    };

    //Load images and draw over galery canas
    preloadImagesForGalleryCanvas(imageSrcs, galleryCanvasId);
}


/**
PreloadImages functions loads images using image path from array. Images are added to 'imgs' array. On comopletion of loading of all images from 'srcs' array, callback function is called.
*/
function preloadImagesForGalleryCanvas(srcs, galleryCanvasId) {
    var images = [];
    var img;
    var remaining = srcs.length;
    for (var i = 0; i < srcs.length; i++) {
        img = new Image();
        img.onload = function () {
            --remaining;
            if (remaining <= 0) {
                //callback(galleryCanvasId, imgs);
                //callback.apply(galleryCanvasId, imgs);
                drawOverGalleryCanvas(galleryCanvasId, images);
            }
        };
        img.src = srcs[i];
        images.push(img);
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