using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vui.Models
{
    public class Post
    {
        [Key]
        public long Id {get;set;}
        [Display(Name="Posted Date")]
        public DateTime PostedDate { get; set; }
        [Display(Name = "Title")]
        [Required]
        [StringLength(50,ErrorMessage="Title has to be less than 50 characters")]    
        [Column(TypeName="varchar")]
        public string Title { get; set; }
        [Required]
        [StringLength(200)]        
        [Column(TypeName="varchar")]
        [Display(Name="Selected file")]
        public string url { get; set; }
        [Display(Name = "Likes")]
        public int TotalLikes {get;set;}        
        [Display(Name = "Comments")]
        public int TotalComments {get;set;}


        public virtual ICollection<LikeActivity> LikeActivities { get; set; }
        public virtual ICollection<CommentActivity> CommentActivities { get; set; }        
        public virtual ApplicationUser User { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
    }
}