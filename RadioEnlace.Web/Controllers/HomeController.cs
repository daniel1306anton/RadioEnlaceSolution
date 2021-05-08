using RadioEnlace.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioEnlace.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeModel() { 
                Frequency = "5",
                At = "0.25",
                Bt = "0.5"
            
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Hola.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}