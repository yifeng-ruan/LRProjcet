using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using LR.Services.Implementation;
using LR.DTO;
using LR.DTO.DemoModule;
using LR.Services.Interface;

namespace LR.Web.Areas.Demo.Controllers
{
    public class DemoController : Controller
    {
        #region 全局变量

        private readonly IDemoServices _demoServices;

        #endregion 全局变量

        #region 构造函数

        public DemoController(IDemoServices demoServices)
        {
            _demoServices = demoServices;
        }

        #endregion 构造函数

        #region 公共方法
        public JsonResult GetAllDemos()
        {
            var demos = _demoServices.FindDemos(1, 20).AsQueryable();
            return this.Json(demos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDemoById(int id)
        {
            var demo = _demoServices.FindDemoById(id);
            return this.Json(demo, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult SaveDemo(DemoDTO demoDTO)
        {
            _demoServices.SaveDemo(demoDTO);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPut]
        public HttpStatusCodeResult UpdateDemo(int id, DemoDTO demoDTO)
        {
            _demoServices.UpdateDemo(id, demoDTO);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// Delete an existing profile
        /// </summary>
        /// <param name="id"></param>
        [System.Web.Http.HttpDelete]
        public void DeleteDemo(int id)
        {
            try
            {
                if (id != 0)
                {
                    _demoServices.DeleteDemo(id);
                }
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
        #endregion 公共方法

        #region Views

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEdit()
        {
            return View();
        }
        #endregion Views
    }
}
