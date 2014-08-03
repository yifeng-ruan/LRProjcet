using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LR.Core;
using LR.Web;


namespace LR.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return Demo();
        }

        public ActionResult Demo()
        {
            ViewBag.CnName = "测试";
            return View("Index");
        }


        //public JsonResult DoAction(JObject request)
        //{
        //    var message = new sys_userService().Login(request);
        //    return Json(message, JsonRequestBehavior.DenyGet);
        //}

        public ActionResult Logout()
        {
            FormsAuth.SingOut();
            return Redirect("~/Login");
        }
    }
}
