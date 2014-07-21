using LR.DTO.DemoModule;
using LR.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace LR.Web.Controllers
{
    public class DemoController : Controller
    {
        #region 全局变量

        private readonly IDemoServices _demoServices;

        #endregion 全局变量
         
        #region 构造器

        public DemoController(IDemoServices demoServices)
        {
            _demoServices = demoServices;
        }

        #endregion 构造器

        #region Public Methods

        /// <summary>
        /// Get all Demo information
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllDemos()
        {
            var demo = _demoServices.FindDemos(0, 20).AsQueryable();
            return this.Json(demo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Demo by demo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetDemoById(int id)
        {
            var demo = _demoServices.FindDemoById(id);
            return this.Json(demo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create a new Demo
        /// </summary>
        /// <param name="demoDTO"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult SaveDemoInformation(DemoDTO demoDTO)
        {
            _demoServices.SaveDemo(demoDTO);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// Update an existing demo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="demoDTO"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut]
        public HttpStatusCodeResult UpdateDemoInformation(int id, DemoDTO demoDTO)
        {
            _demoServices.UpdateDemo(id, demoDTO);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// Delete an existing demo
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

        /// <summary>
        /// Get all initialization data for Contact page
        /// </summary>
        /// <returns></returns>
        public JsonResult InitializePageData()
        {
            var contactForm = _demoServices.InitializePageData();
            return this.Json(contactForm, JsonRequestBehavior.AllowGet);
        }

        #endregion Public Methods

        #region 视图
        public ActionResult Index()
        {
            string a = "123";
            return View();
        }

        public ActionResult CreateEdit()
        {
            return View();
        }
        #endregion 视图

    }
}
