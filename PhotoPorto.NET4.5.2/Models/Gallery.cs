using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoPorto.Models
{
    public class Gallery
    {
	    public int ID { get; set; }

		[Required]        
        [StringLength(50)]
		public string Title { get; set; }		

		public string Description { get; set; }

        //[Required]
        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]    
        public DateTime CreationDate { get; set; }

        [DataType(DataType.DateTime)]        
        public DateTime LastModifiedDate { get; set; }

        public Photograph Photograph { get; set; }
        public virtual ICollection<Photograph> Photographs { get; set; }
    }
}
