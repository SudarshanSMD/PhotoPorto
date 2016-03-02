var CanvasXSize;
var CanvasYSize;
(function () {
    //Defining Canvas dimentions
    CanvasXSize = getPageWith();
    CanvasYSize = getPageHeight();
})();


/**
 Function to draw canvas gallery. It prepares canvas, then call preloadImagesForGalleryCanvas(), which then calls drawOverGalleryCanvas().
*/
function prepareHomeCanvas(homeCanvasId, imageSrcs, splitColumnCount, splitRowCount) {
    var canvasElement = document.getElementById(homeCanvasId);
    canvasElement.height = CanvasYSize;
    canvasElement.width = CanvasXSize;

    //Load images and draw over galery canas
    preloadImagesForHomeCanvas(imageSrcs, homeCanvasId, splitColumnCount, splitRowCount);
}


/**
PreloadImages functions loads images using image path from array. Images are added to 'imgs' array. On comopletion of loading of all images from 'srcs' array, callback function is called.
*/
function preloadImagesForHomeCanvas(srcs, homeCanvasId, splitColumnCount, splitRowCount) {
    var images = [];
    var img;
    var remaining = srcs.length;
    //To sort the images
    srcs = srcs.sort();
    for (var i = 0; i < srcs.length; i++) {
        img = new Image();
        img.onload = function () {
            --remaining;
            if (remaining <= 0) {                
                //callback(galleryCanvasId, imgs);
                //callback.apply(galleryCanvasId, imgs);
                drawOverHomeCanvas(homeCanvasId, images, splitColumnCount, splitRowCount);
            }
        };
        img.src = srcs[i];
        images.push(img);
    }
}

/**
 * Draws over canvas having id homeCanvasId.
 *
 * @param {string} homeCanvasId
 * @param {[]} images 
 * @param {number} splitColumnCount
 * @param {number} splitRowCount
 */
function drawOverHomeCanvas(homeCanvasId, images, splitColumnCount, splitRowCount) {

    // pointer for tracking image number in array.  
    var imagesPointer = 0;    
    
    //image Width and height with all split images combined; Assuming image will always be split in equal parts.
    var imageWidth = images[imagesPointer].width * splitColumnCount;
    var imageHeight = images[imagesPointer].height * splitRowCount;

    //calculate scale factor;  scale factor is ratio of canvas width to image width
    var xScaleFactor = CanvasXSize / imageWidth;
    if (xScaleFactor == NaN) {
        xScaleFactor = 1;
    }
    
    //Get canvas context    
    var canvasCtx = document.getElementById(homeCanvasId).getContext('2d');
    canvasCtx.clearRect(0, 0, canvasCtx.width, canvasCtx.height);

    //scale canvas
    canvasCtx.scale(xScaleFactor, xScaleFactor);
    
    for (var xPivot = 0; xPivot < imageWidth; xPivot += images[imagesPointer].width) {
        for (var yPivot = 0; yPivot < imageHeight; yPivot += images[imagesPointer].height, imagesPointer++) {

            // reset the image pointer, if pointer moves beyond array length
            if (imagesPointer >= images.length) {
                imagesPointer = 0;
            }

            var canvasPattern = canvasCtx.createPattern(images[imagesPointer], "repeat");
            canvasCtx.fillStyle = canvasPattern;
            canvasCtx.beginPath();
            canvasCtx.moveTo(xPivot, yPivot);
            canvasCtx.lineTo(xPivot + images[imagesPointer].width, yPivot);
            canvasCtx.lineTo(xPivot + images[imagesPointer].width, yPivot + images[imagesPointer].height);
            canvasCtx.lineTo(xPivot, yPivot + images[imagesPointer].height);
            canvasCtx.closePath();
            canvasCtx.fill();
        }
    }
}