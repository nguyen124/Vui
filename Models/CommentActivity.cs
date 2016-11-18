using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vui.Models
{
     //[Table("CommentActivities")]
    public class CommentActivity
    {
        [Key]
        public long CommentId { get; set; }
        //[Column(Order = 0)]
        //[Index("IX_PostIdUserId", 1, IsUnique = true)]
        public long PostId { get; set; }
        //public virtual ApplicationUser User { get; set; }
        //[Key]
        //[Column(Order = 1)]
        [StringLength(128)]
        //[Index("IX_PostIdUserId", 2, IsUnique = true)]
        public string ApplicationUserId { get; set; }
        
        [StringLength(1000, ErrorMessage = "Your comment is too long")]
        [Column(TypeName = "varchar")]
        public string Comment { get; set; }        
        public virtual ApplicationUser User { get; set; }
        [Display(Name = "Likes")]
        public int TotalLikes { get; set; }
        [Display(Name = "Replies")]
        public int TotalReplies{ get; set; }

        public virtual ICollection<LikeActivity> LikeActivities { get; set; }
        public virtual ICollection<CommentActivity> ReplyCommentActivities { get; set; }  
    }
}