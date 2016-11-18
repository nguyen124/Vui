using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vui.Models;

namespace Vui.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Activity/Like
        public ActionResult Like()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Like(LikeActivity act)
        //{
        //    if (ModelState.IsValid)
        //    {                
        //        //db.LikeActivities.Add(act);
        //        db.SaveChanges();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View();
        //}
    }
}