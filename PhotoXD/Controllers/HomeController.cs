using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoXD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About us.";

            return View();
        }


        public ActionResult Gallery()
        {
            ViewBag.Message = "Our gallery.";
            return View();
        }
    }
}