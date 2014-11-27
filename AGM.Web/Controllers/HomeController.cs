using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AGM.Web.Infrastructure;

namespace AGM.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Area Riservata";
            if (!string.IsNullOrEmpty(Request.GetCookieValue("SSID")))
                return View();
            
            var newSessionId = Guid.NewGuid().ToString();
            Response.AppendCookie(new System.Web.HttpCookie("SSID", newSessionId) {Path = "/"});
            return View();
        }
    }
}
