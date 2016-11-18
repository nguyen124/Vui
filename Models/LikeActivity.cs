using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vui.Models
{    
    public class LikeActivity
    {
        [Key]
        public long LikeId { get; set; }
        //[Key]
        //[Column(Order = 0)]
        //[Index("IX_PostIdUserId", 1, IsUnique = true)]
        public long PostId { get; set; }
        //public virtual ApplicationUser User { get; set; }
        //[Key]
        //[Column(Order = 1)]
        [StringLength(128)]
        //[Index("IX_PostIdUserId", 2, IsUnique = true)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public long? CommentId { get; set; }
        public bool Like { get; set; }       
    }
}