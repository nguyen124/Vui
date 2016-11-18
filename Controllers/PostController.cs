using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vui.Models;
using Microsoft.AspNet.Identity;
using PagedList;
namespace Vui.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Post
        [Authorize]
        public ActionResult Index(string sortOrder,int? page)
        {           
            //var posts = from s in db.Posts select s;            
            string userId = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.ApplicationUserId == userId);
            if (posts != null && posts.Count() != 0)
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
            return View("Index", null);
        }
        

        // GET: Post/Details/5
        public ActionResult Details()
        {
            //var post = new Post { Id = 1, postedDate = DateTime.Now, title = "so fun", url = "/Content/images/pic1.jpg" };
            long postId = long.Parse(Request.Params.Get("postId"));
            Post post = db.Posts.Where(c => c.Id == postId).First();
            return View("Details",post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(UploadFileModel model, HttpPostedFileBase FileUpload)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid && FileUpload != null)
                {
                    Post post = new Post();
                    var fileName = Path.GetFileName(FileUpload.FileName);
                    var serverPath = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    FileUpload.SaveAs(serverPath);
                    post.url = "/Content/images/"+fileName;
                    post.Title = model.title;
                    int count = db.Posts.Count();
                    post.Id = count + 1;
                    post.PostedDate = DateTime.Now;
                    post.ApplicationUserId = User.Identity.GetUserId();
                    db.Posts.Add(post);
                    db.SaveChanges();                
                    return RedirectToAction("Details", "Post", new {postId = post.Id});
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult LikeOrComment(string returnUrl)
        {
            //long postId = long.Parse(Request.Form["postId"]);
            //string sortOrder = Request.Form["sortOrder"];
            //string pageNo = Request.Form["pageNo"];
            //Post post = db.Posts.Where(p => p.Id == postId).First();
            //string userId = User.Identity.GetUserId();
            //string like_com = Request.Form["like_com"];
            long postId = long.Parse(Request.Params["postId"]);
            string sortOrder = Request.Params["sortOrder"];
            string pageNo = Request.Params["pageNo"];
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
                    //act.Id = db.LikeActivities.Count() + 1;
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
                    act.User = db.Users.Where(us => us.Id == User.Identity.GetUserId()).First();
                    db.CommentActivities.Add(act);                                            
                    post.TotalComments++;                    
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Post", new { sortOrder= "date", page=1});
        }

        public ActionResult ViewComment()
        {

            return View();
        }
        // GET: Post/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Post/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
