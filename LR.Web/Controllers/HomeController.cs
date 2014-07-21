using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LR.Core;
using LR.Core.Base;

namespace LR.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            ViewBag.Title = "管理平台";
            ViewBag.UserName = loginer.UserName;
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}
