using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vui.Models
{
    public class UploadFileModel
    {
        [Display(Name = "Title")]
        [Required]
        [StringLength(50, ErrorMessage = "Title has to be less than 50 characters")]
        [Column(TypeName = "varchar")]
        public string title { get; set; }
                
        //public File url { get; set; }
    }
    
}