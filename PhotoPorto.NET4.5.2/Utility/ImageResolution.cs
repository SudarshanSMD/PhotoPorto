using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoPorto.Utility
{
    /// <summary>
    /// Represents ImageResolution. To be used like enum in java.
    /// </summary>
    public class ImageResolution
    {
        private ImageResolution(int width, int height, string representation) {
            Width = width;
            Height = height;
            Representation = representation;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public string Representation { get; set; }

        public static ImageResolution UHD4K { get { return new ImageResolution(3840, 2160, Constants.RESOLUTION_4K); } }
        //public static ImageResolution QHD { get { return new ImageResolution(2560,"QHD"); } }
        public static ImageResolution FHD { get { return new ImageResolution(1920,1080, Constants.RESOLUTION_FHD); } }
        //public static ImageResolution HD { get { return new ImageResolution(1280, 720,"HD"); } }
        public static ImageResolution qHD { get { return new ImageResolution(960, 540, Constants.RESOLUTION_qHD); } }
        public static ImageResolution nHD { get { return new ImageResolution(640, 360, Constants.RESOLUTION_nHD); } }
        /// <summary>
        /// ImageResolution with representatin 'og' denoted it's original resolution.        
        /// </summary>
        /// <remarks>Use this only if orignal image resolution is smalled than nHD.
        /// Take special precaution while dealing with height and width of og image, as they are not predefined over here.
        /// </remarks>
        public static ImageResolution og { get { return new ImageResolution(-1, -1, Constants.RESOLUTION_og); } }
        //public static ImageResolution thumbnail { get { return new ImageResolution(400,300, ""); } }
    }
}