using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using LR.Web;
using LR.DTO.UserModule;
using LR.Services.Interface;
using LR.Core.Core;
using System.Net;

namespace LR.Web.Controllers
{
    public class LoginController : Controller
    {
         #region Global declaration

        private readonly IUserServices _userServices;

        #endregion Global declaration

        public LoginController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public JsonResult RegisterAction(UserDTO userDTO)
        {
           var msg= _userServices.SaveUser(userDTO);
            return Json(msg, JsonRequestBehavior.DenyGet);
        }
        [System.Web.Http.HttpPost]
        public JsonResult LoginAction(UserDTO userDTO)
        {
            var msg  = _userServices.Login(userDTO);
            return Json(msg, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Logout()
        {
            FormsAuth.SingOut();
            return Redirect("~/Login");
        }
    }
}
