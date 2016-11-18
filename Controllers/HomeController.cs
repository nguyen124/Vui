using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Vui.Models;
using PagedList;
namespace Vui.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index(string sortOrder, int? page)
        {
            var posts = from s in db.Posts select s;                        
            if (posts != null && posts.Count()!=0)
            {
                switch (sortOrder)
                {
                    case "title_desc":
                        posts = posts.OrderByDescending(s => s.Title);
                        ViewBag.SortByTitle = "";
                        ViewBag.SortByDate = "date";
                        break;
                    case "date":
                        posts = posts.OrderBy(s => s.PostedDate);
                        ViewBag.SortByDate = "date_desc";
                        break;
                    case "date_desc":
                        posts = posts.OrderByDescending(s => s.PostedDate);
                        ViewBag.SortByDate = "date";
                        break;
                    case "like":
                        posts = posts.OrderBy(s => s.TotalLikes);
                        ViewBag.SortByDate = "like";
                        break;
                    case "comment":
                        posts = posts.OrderBy(s => s.TotalComments);
                        ViewBag.SortByDate = "comment";
                        break;
                    default:
                        posts = posts.OrderBy(s => s.Title);
                        ViewBag.SortByTitle = "title_desc";
                        ViewBag.SortByDate = "date";
                        break;
                }
                ViewBag.CurrentSort = sortOrder;
                int pageSize = 6;
                int pageNumber = (page ?? 1);
                ViewBag.CurrentPage = pageNumber;
                return View(posts.ToPagedList(pageNumber, pageSize));
            }
            return View("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult LikeOrComment(string returnUrl)
        {
            long postId = long.Parse(Request.Params["postId"]);
            var post = DoLikeOrComment(postId);
            ViewData.Add(new KeyValuePair<string,object>("counter",Request.Params["counter"]));
            return PartialView("_LikeAndCommentPartial", post);            
        }

        [HttpPost]
        [Authorize]
        public ActionResult LikeOrCommentInModal(string returnUrl)
        {
            long postId = long.Parse(Request.Params["postId"]);
            ViewBag.target = Request.Params["target"];
            var post = DoLikeOrComment(postId);
            return PartialView("_PictureModal", post);            
        }
        private Post DoLikeOrComment(long postId)
        {         
            Post post = db.Posts.Where(p => p.Id == postId).First();
            string userId = User.Identity.GetUserId();
            string like_com = Request.Params["like_com"];
            if (like_com == "Like")
            {
                if (db.LikeActivities.Where(activity => activity.PostId == postId &&
                    activity.ApplicationUserId == userId).Count() == 0)
                {
                    LikeActivity act = new LikeActivity();
                    act = new LikeActivity();
                    act.ApplicationUserId = userId;
                    act.PostId = postId;
                    act.Like = true;
                    db.LikeActivities.Add(act);
                    post.TotalLikes++;
                }
                else
                {
                    LikeActivity likeAct = db.LikeActivities.Where(activity => activity.PostId ==
                        postId && activity.ApplicationUserId == userId).First();
                    if (!likeAct.Like)
                    {
                        post.TotalLikes++;
                    }
                    likeAct.Like = true;

                    ViewBag.Like = "UnLike";
                }
                db.SaveChanges();
            }
            else if (like_com == "UnLike")
            {
                LikeActivity likeAct = db.LikeActivities.Where(activity => activity.PostId ==
                      postId && activity.ApplicationUserId == userId).First();
                likeAct.Like = false;
                ViewBag.Like = "Like";
                post.TotalLikes--;
                db.SaveChanges();
            }
            else if (like_com == "Comment")
            {
                string comment = Request.Params["UserComment"];
                if (!string.IsNullOrWhiteSpace(comment))
                {
                    CommentActivity act = new CommentActivity();
                    act.ApplicationUserId = userId;
                    act.PostId = postId;
                    act.Comment = comment;
                    act.User = db.Users.Where(us => us.Id == userId).First();
                    db.CommentActivities.Add(act);
                    post.TotalComments++;
                    db.SaveChanges();
                }
            }
            return post;
        }
        public ActionResult ViewComment()
        {
            long postId = long.Parse(Request.Params["postId"]);
            Post post = db.Posts.Where(p => p.Id == postId).First();
            ViewBag.target = Request.Params["target"];
            return PartialView("_PictureModal", post);

        }
        [ActionName("about-vui")]
        [Route("home/create")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string message)
        {
            ViewBag.Message = "Got ur message";

            return View();
        }
    }
}