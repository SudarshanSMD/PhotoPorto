using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoPorto.Models;

namespace PhotoPorto.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private DAL.PhotoPortoDbContext db = new DAL.PhotoPortoDbContext();

        public ActionResult Index()
        {
            IEnumerable<Photograph> PhotographList = db.Photograph.OrderByDescending( p => p.FavouriteCount).Take(4).ToList();            
            return View(PhotographList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is PhotoPorto..";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me @";

            return View();
        }
    }
}