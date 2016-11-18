using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vui.Models;

namespace Vui.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LikeComment()
        {
            string userId = Request.Params["userId"];
            string sortOrder = Request.Params["sortOrder"];
            long commentId = long.Parse(Request.Params["commentId"]);
            long postId = long.Parse(Request.Params["postId"]);
            CommentActivity comment = db.CommentActivities.Where(c => c.CommentId == commentId).First();
            //comment.User = db.Users.Where(us => us.Id == comment.ApplicationUserId).First();           
            if (comment.LikeActivities.Where(c => c.CommentId == commentId).Count() == 0)
            {
                LikeActivity act = new LikeActivity();
                act = new LikeActivity();
                //act.Id = db.LikeActivities.Count() + 1;
                act.ApplicationUserId = userId;
                act.PostId = postId;
                act.CommentId = commentId;
                act.Like = true;
                //db.LikeActivities.Add(act);
                comment.LikeActivities.Add(act);
                comment.TotalLikes++;
            }
            else
            {
                LikeActivity act = comment.LikeActivities.Where(c => c.CommentId ==
                    commentId).First();
                if (!act.Like)
                {
                    comment.TotalLikes++;
                }                
                act.CommentId = commentId;
                act.Like = true;
              //  ViewBag.LikeComment = "UnLike";
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return RedirectToAction("Index","Home");
        }

        public ActionResult UnLikeComment()
        {
            //string userId = Request.Params["userId"];
            string sortOrder = Request.Params["sortOrder"];
            long commentId = long.Parse(Request.Params["commentId"]);
            long postId = long.Parse(Request.Params["postId"]);
            CommentActivity comment = db.CommentActivities.Where(c => c.CommentId == commentId).First();
            LikeActivity likeAct = comment.LikeActivities.Where(c => c.CommentId == commentId).First();
            likeAct.Like = false;
            //ViewBag.LikeComment = "Like";
            comment.TotalLikes--;            
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

       

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comment/Edit/5
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

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
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
