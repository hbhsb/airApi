using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using airApi.Entity;
using airApi.utils;
using com.show.api;
using Newtonsoft.Json;

namespace airApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.username = System.Web.HttpContext.Current.Session["username"];
            List<CityEntity> citys = new List<CityEntity>();
            string tsq = "select * from city";
            DataTable tablec = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, tsq).Tables[0];
            foreach (DataRow row in tablec.Rows)
            {
                CityEntity cityEntity = new CityEntity();
                cityEntity.CnName = row.Field<string>("city");
                cityEntity.EnName = row.Field<string>("allpy");
                citys.Add(cityEntity);
            }
            string city = Request.Params["city"];
            if (string.IsNullOrEmpty(city))
            {
                return View("location");
            }
            List<CityEntity> cityEntities = new List<CityEntity>();
            string tsql = "select * from city";
            DataTable table = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, tsql).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                CityEntity cityEntity = new CityEntity();
                cityEntity.CnName = row.Field<string>("city");
                cityEntity.EnName = row.Field<string>("allpy");
                cityEntities.Add(cityEntity);
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

            float f = (float)(total-cityIndex - 1) / (float)total;
            string sorts = f + "0000";
            string Pre = sorts.Substring(2, 2);
            ViewBag.Pre = Pre + "%";
            ViewBag.City = city;
            ViewBag.Sort = cityIndex;
            ViewBag.cityJson = JsonConvert.SerializeObject(citys);
            return View(cityEntities);
        }

        public ActionResult Detail(string cityName)
        {
            //string postData = "{\"showapi_appid\":\"59148\",\"showapi_sign\":\"67c674e95dca4adbbed78307011f3cc2\",\"city\":\"" +
            //                  cityName + "\"}";
            //string data = JsonPostUtil.PostUrl("http://route.showapi.com/104-29", postData);
            ViewBag.username = System.Web.HttpContext.Current.Session["username"];
            string res = new ShowApiRequest("http://route.showapi.com/104-29", "59148", "67c674e95dca4adbbed78307011f3cc2")
                .addTextPara("city", cityName)
                .post();
            res = res.Replace("_", "");
            dynamic desc = JsonConvert.DeserializeObject<dynamic>(res);
            if (desc.showapirescode == 0)
            {
                ViewBag.Aqi = desc.showapiresbody.pm;
                ViewBag.Stations = desc.showapiresbody.siteList;
            }
            return View();
        }

        public JsonResult SelectCity(string search)
        {
            List<CityEntity> citys = new List<CityEntity>();
            string tsql = "select * from city where city like @cityName or allpy like @cityname";
            SqlParameter parameter = new SqlParameter("@cityName", "%" + search + "%");
            DataTable table = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, tsql, parameter).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                CityEntity cityEntity = new CityEntity();
                cityEntity.CnName = row.Field<string>("city");
                cityEntity.EnName = row.Field<string>("allpy");
                citys.Add(cityEntity);
            }
            return Json(citys);
        }
    }
}