using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace airApi.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            string tsql = "select count(1) from users where user_name=@username and user_pwd=@password";
            SqlParameter[] parameters =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@password", password),
            };
            string count = SqlHelper.ExecuteScalar(SqlHelper.GetConnection(), CommandType.Text, tsql, parameters).ToString();
            if (count == "1")
            {
                System.Web.HttpContext.Current.Session["username"] = username;
                System.Web.HttpContext.Current.Session["password"] = password;
                System.Web.HttpContext.Current.Session.Timeout = 60;
                return Content("登陆成功");
            }
            return Content("登陆失败");
        }

        [HttpGet]
        public ActionResult Logon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logon(string username, string password)
        {
            string tsql = "select count(1) from users where user_name=@username";
            SqlParameter parameter=new SqlParameter("@username",username);
            string count = SqlHelper.ExecuteScalar(SqlHelper.GetConnection(), CommandType.Text, tsql, parameter)
                .ToString();
            if (count == "1")
            {
                return Content("用户名已存在");
            }
            string isql = "insert into users (user_name,user_pwd) values(@username,@password)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@password", password),
            };
            int v = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, isql, parameters);
            if (v > 0)
            {
                System.Web.HttpContext.Current.Session["username"] = username;
                System.Web.HttpContext.Current.Session["password"] = password;
                System.Web.HttpContext.Current.Session.Timeout = 60;
                return Content("注册成功");
            }
            return Content("注册失败");
        }
    }
}