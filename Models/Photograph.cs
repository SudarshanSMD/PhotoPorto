using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoPorto.Models
{
    /// <summary>
    /// Model ofr Photograph.
    /// </summary>
    public class Photograph
    {
		    public int ID { get; set; }

			[Required]        
			[StringLength(50)]
			public string Title { get; set; }		

			public string Description { get; set; }
			public string Type { get; set; }
            public string Tags { get; set; }


            /**
            RSA encrypted ID with Salt to be be used in naming of orignal and full images.
            Naming be like:
            ID_key.jpg              -for original image
            ID_key_resolution.jpg   -for full image in particular resolution viz. FHD, 4K.

            NOTE: the key is NOT stored here. If you want to access original image, you have to enctypt the RSA ID with Salt and then show it.
            Yes, it is heavy operation. However, we will need to calculate only in case if user wants to download orignal images.
            To prevent exposure of key to user, when giving image to user, write it on responseStream with changed name.
            */
            //[Required]
            //[Editable(false)]
            //public string Salt { get; set; }

            /// <summary>OriginalPhotographKey is key used while naming of original image files:
            /// Naming be like:
            ///                 ID_key_resolution.jpg
            ///          where,
            ///                  key = generated key stored here
            ///                  resolution = resolution to which split image is created
            /// </summary>
            [Editable(false)]
            public string OriginalPhotographKey { get; set; }

            /**
            key used in naming of split images.

            Naming be like:
            ID_key_column_row_resolution.jpg
            where,
                  column = column number of split image
                  row = row number of split image
                  resolution = resolution to which split image is created
                  key = unique key. 

            Reason for having SplitPhotographKey -> Knowing the pattern of image naming, someone can write script to get all split images. Split image would later be stitched.
            For now, we are using md5. Yes, someone could figure the pattern. If you don't want someone to guess it, use key based on salt.            
            */
            //[Required]
            [Editable(false)]
            public string SplitPhotographKey { get; set; }

            //[Required]
            [Editable(false)]
            public int PhotographWidth { get; set; }

            //[Required]
            [Editable(false)]
            public int PhotographHeight { get; set; }

            //[Required]
            [Editable(false)]
            public int SplitColumnCount { get; set; }

            //[Required]
            [Editable(false)]
            public int SplitRowCount { get; set; }

            /*
            Although we are defining resolution which are in 16:9 aspect ration, photograph are rarely of 16:9 aspect ration.
            Most common aspect ration in Photographs woud be 3:4.
            We are using these terms as they are (de facto) standards.
            For photogrphs, it represent what max side of image would be like.

            More sizes can be inlcuded, but these are good enough.
            */
            //4K UHD	3840	2160
            public bool UHD4K  { get; set; }
            //FHD	1920	1080
            public bool FHD { get; set; }
            //HD	1280	720
            public bool HD { get; set; }
            //nHD	960
            public bool qHD { get; set; }
            //nHD	640	360	
            public bool nHD { get; set; }
            // og for image in original format. Used in case, if image is smaller than nHD
            public bool og { get; set; }

            public int LikeCount { get; set; }
			public int FavouriteCount { get; set;}

            [Required]
            [DataType(DataType.DateTime)]
			// [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]        
			public DateTime CreationDate { get; set; }

			public virtual ICollection<Gallery> Galleries { get; set; }
    }
}
