using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.show.api;
using Newtonsoft.Json;

namespace airApi.Controllers
{
    public class SortController : Controller
    {
        // GET: Sort
        public ActionResult Index()
        {
            ViewBag.username = System.Web.HttpContext.Current.Session["username"];
            string res = new ShowApiRequest("http://route.showapi.com/104-41", "59148", "67c674e95dca4adbbed78307011f3cc2")
                .post();
            res = res.Replace("_", "");
            dynamic desc = JsonConvert.DeserializeObject<dynamic>(res);
            if (desc.showapirescode == 0)
            {
                ViewBag.List = desc.showapiresbody.list;
            }
            return View();
        }
    }
}