using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PhotoPorto.Models
{
    //Model to be used whe gallery and all photographs are needed in view
    public class GalleryPhotographsModel
    {
        public Gallery Gallery { get; set; }
        public List<Photograph> Photographs { get; set; }
    }
}