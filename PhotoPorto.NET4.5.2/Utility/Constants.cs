using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoPorto.Utility
{
    /// <summary>
    /// Constant values used in application go here.
    /// </summary>
    public class Constants
    {
        public const int SPLIT_IMAGE_COLUMN_COUNT = 10;
        public const int SPLIT_IMAGE_ROW_COUNT = 2;
               
        /*
        Resolution
        */
        public const string RESOLUTION_4K = "4K";
        public const string RESOLUTION_FHD = "FHD";
        //public const string RESOLUTION_HD = "HD";
        public const string RESOLUTION_qHD = "qHD";
        public const string RESOLUTION_nHD = "nHD";
        //og represents original resolution of image
        public const string RESOLUTION_og = "og";
    }
}