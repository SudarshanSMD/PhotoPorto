using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PhotoPorto.Service
{
    /// <summary>
    /// PhotographService is service class from Photograph. <see cref="PhotoPorto.Models.Photograph"/>
    /// </summary>
    public class PhotographService
    {
        /// <summary>
        /// SplitImage splits images in parts. Input image is split in provided number of rows and columns.
        /// <para>Naming of split image:
        ///         photographID_splitImageKey_x_y_resolution.jpg
        ///         Where,
        ///               x= column number
        ///               y= row number
        /// </para>
        /// </summary>
        /// <param name="inputImagePathName">Input image path</param>
        /// <param name="imageFolderPath">Image folder path, wherein all images after splitting are to be placed</param>
        /// <param name="photographId"></param>
        /// <param name="splitImageKey">Photographs specific key used in naming of split images</param>
        /// <param name="columnCount">Number of colums the image is to be splitted into</param>
        /// <param name="rowCount">Number of rows the images is to splitted into</param>
        /// <param name="imageRepresentation">Resolution name to be included in naming of split image. </param>
        public static void SplitImage(string inputImagePathName, string imageFolderPath, string photographId, string splitImageKey, int columnCount, int rowCount, String imageRepresentation)
        {
            Bitmap img = new Bitmap(inputImagePathName);
            var imageHeight = img.Height;
            var imageWidth = img.Width;
            
            //TODO: floor
            var blockWidth = imageWidth / columnCount;
            var blockHeight = imageHeight / rowCount;
            
            // adjsut widht or height offset if iamgeis not perfectly divisible
            if((imageWidth% columnCount) != 0)
            {
                var offsetWidth = imageWidth % columnCount;
            }
            if ((imageHeight % rowCount) != 0)
            {
                var offsetHeight = imageHeight % rowCount;
            }

            for(int i=0; i< rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    //TODO: add offset
                    var outputImagePath = System.IO.Path.Combine(imageFolderPath, photographId + "_" + splitImageKey + "_" + j +"_" + i + "_" + imageRepresentation + ".jpg");
                    Utility.ImageUtility.CropImage(inputImagePathName, outputImagePath, (j * blockWidth), (i * blockHeight), blockWidth, blockHeight);
                }
            }
        }
    }
}