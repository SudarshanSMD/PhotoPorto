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
function prepareHomeCanvas(homeCanvasId, imageSrcs) {

    //Set galleryCanvas dimentions
    var galleryCanvasElement = document.getElementById(homeCanvasId);
    galleryCanvasElement.height = CanvasYSize;
    galleryCanvasElement.width = CanvasXSize;  

    //Load images and draw over galery canas
    preloadImagesForHomeCanvas(imageSrcs, homeCanvasId);
}


/**
PreloadImages functions loads images using image path from array. Images are added to 'imgs' array. On comopletion of loading of all images from 'srcs' array, callback function is called.
*/
function preloadImagesForHomeCanvas(srcs, homeCanvasId) {
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
                drawOverGalleryCanvas(homeCanvasId, images);
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
    images = images.sort();

    var galleryCanvasCtx = document.getElementById(galleryCanvasId).getContext('2d');
      //CanvasYSize = galleryCanvasElement.height;
     // CanvasXSize = galleryCanvasElement.width;  

    //Clear Canvas
    galleryCanvasCtx.clearRect(0, 0, galleryCanvasCtx.width, galleryCanvasCtx.height);
  
    // Pointer for tracking image number in array.  
    var imagesPointer = 0;
    var xPivot = 0;
    var yPivot = 0;

    var xMax = CanvasXSize / images[imagesPointer].width;

    for (var yTemp = 0; yTemp < 2; yTemp += 1) {
        xPivot = 0;
        for (var xTemp = 0; xTemp < xMax; xTemp += 1) {          

            var galleryCanvasPattern = galleryCanvasCtx.createPattern(images[imagesPointer], "repeat");
            galleryCanvasCtx.fillStyle = galleryCanvasPattern;
            galleryCanvasCtx.beginPath();
            galleryCanvasCtx.moveTo(xPivot, yPivot);
            galleryCanvasCtx.lineTo(xPivot + images[imagesPointer].width, yPivot);
            galleryCanvasCtx.lineTo(xPivot + images[imagesPointer].width, yPivot + images[imagesPointer].height);
            galleryCanvasCtx.lineTo(xPivot, yPivot + images[imagesPointer].height);
            galleryCanvasCtx.closePath();
            galleryCanvasCtx.fill();

            xPivot = xPivot + images[imagesPointer].width;
            imagesPointer++;
            if (imagesPointer >= images.length) {
                imagesPointer = 0;
            }
        }
        yPivot = yPivot + images[imagesPointer].height;        
    }
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