using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
//using ImageProcessor.Imaging.Formats;
//using ImageProcessor;
//using ImageProcessor.Imaging;
using ImageMagick;
using ImageProcessor.Imaging.Formats;
using ImageProcessor;
//using System.Drawing.Drawing2D;

namespace PhotoPorto.Utility
{
    /// <summary>
    /// Utility class for image related operations.
    /// </summary>
    public class ImageUtility
    {
        /*
        //Using ImageMagick. Is slower.
        public static void ResizeImage(string inputImagePathString, String outputImagePathName, int maxWidth)
        {            
            // Read from file
            using (MagickImageCollection collection = new MagickImageCollection(inputImagePathString))
            {
                // This will remove the optimization and change the image to how it looks at that point
                // during the animation. More info here: http://www.imagemagick.org/Usage/anim_basics/#coalesce
                collection.Coalesce();

                // Resize each image in the collection to a width of 200. When zero is specified for the height
                // the height will be calculated with the aspect ratio.
                foreach (MagickImage image in collection)
                {
                    image.Resize(maxWidth, 0);
                }

                // Save the result
                collection.Write(outputImagePathName);
            }                       
        }*/

        /// <summary>
        /// Resized images.
        /// </summary>
        /// <param name="inputImagePathString">Input image path</param>
        /// <param name="outputImagePathName">Output image path</param>
        /// <param name="maxWidth">Maximum width of resized image. Keep this 0 if image is vertical</param>
        /// <param name="maxHeight">Maximum height of resized image. Keep this 0 if image is horizontal.</param>
        /// <remarks>NOTE: If you provide both maxWidth and maxHeight and image is not is that aspect ratio, image will be automatically padded.
        /// However, if you specify either of maxHeight or maxWidth of resized image to be 0, aspect ratio will be maintaned without padding the image.</remarks>
        public static void ResizeImage(string inputImagePathString, String outputImagePathName, int maxWidth, int maxHeight)
        {
            byte[] photoBytes = File.ReadAllBytes(inputImagePathString);
            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            Size size = new Size(maxWidth, maxHeight);
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Save(outStream);

                        FileStream fileStream = new FileStream(outputImagePathName, FileMode.Create);
                        outStream.WriteTo(fileStream);
                        fileStream.Close();
                    }
                    // Do something with the stream.                    
                    outStream.Close();                
                }
            }
        }

        /// <summary>
        /// Method used to crop image.
        /// </summary>
        /// <param name="inputImagePathString">input image path</param>
        /// <param name="outputImagePathName">ouput image path</param>
        /// <param name="x1">x</param>
        /// <param name="y1">y</param>
        /// <param name="width">widht of cropped image</param>
        /// <param name="height">height of cropped image</param>
        public static void CropImage(string inputImagePathString, String outputImagePathName, int x1, int y1, int width, int height)
        {
            byte[] photoBytes = File.ReadAllBytes(inputImagePathString);
            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            Rectangle rectangle = new Rectangle(x1, y1, width, height);
                        
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Crop(rectangle)                                    
                                    .Format(format)
                                    .Save(outStream);

                        FileStream fileStream = new FileStream(outputImagePathName, FileMode.Create);
                        outStream.WriteTo(fileStream);
                        fileStream.Close();
                    }
                    // Do something with the stream.                    
                    outStream.Close();
                }
            }
        }       
    }
}
