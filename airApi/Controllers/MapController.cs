using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.show.api;
using Newtonsoft.Json;

namespace airApi.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            string city = Request.Params["city"];
            if (string.IsNullOrEmpty(city))
            {
                return View("maplocation");
            }
           
            string res = new ShowApiRequest("http://route.showapi.com/104-29", "59148", "67c674e95dca4adbbed78307011f3cc2")
                .addTextPara("city", city)
                .post();
            res = res.Replace("_", "");
            dynamic desc = JsonConvert.DeserializeObject<dynamic>(res);
            if (desc.showapirescode == 0)
            {
                ViewBag.Aqi = desc.showapiresbody.pm;
                ViewBag.Stations = desc.showapiresbody.siteList;
            }
            string sortRes =
                new ShowApiRequest("http://route.showapi.com/104-41", "59148", "67c674e95dca4adbbed78307011f3cc2")
                    .post();
            dynamic descSort = JsonConvert.DeserializeObject<dynamic>(sortRes);
            List<string> cityNameList = new List<string>();
            if (descSort.showapi_res_code == 0)
            {
                foreach (dynamic cityNode in descSort.showapi_res_body.list)
                {
                    cityNameList.Add(cityNode.area.ToString());
                }
            }
            string cityNameToSort = city;
            if (city.EndsWith("市"))
            {
                cityNameToSort = city.Replace("市", "");
            }
            int total = cityNameList.Count;
            int cityIndex = cityNameList.FindIndex(item => item.Equals(cityNameToSort)) + 1;
            string res1 = new ShowApiRequest("http://route.showapi.com/104-41", "59148", "67c674e95dca4adbbed78307011f3cc2")
                .post();
            res1 = res1.Replace("_", "");
            dynamic desc1 = JsonConvert.DeserializeObject<dynamic>(res1);
            if (desc.showapirescode == 0)
            {
                ViewBag.List = desc1.showapiresbody.list;
            }
            float f = (float)(total - cityIndex - 1) / (float)total;
            string sorts = f + "0000";
            string Pre = sorts.Substring(2, 2);
            ViewBag.Pre = Pre + "%";
            ViewBag.City = city;
            ViewBag.Sort = cityIndex;
            return View();
        }
    }
}