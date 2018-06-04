using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using airApi.Entity;

namespace airApi.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserSet()
        {
            string tsql = "select * from users where isdel = 1";
            DataTable dataTable = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, tsql).Tables[0];
            List<UserEntity> users =new List<UserEntity>();
            foreach (DataRow row in dataTable.Rows)
            {
                UserEntity userEntity = new UserEntity();
                userEntity.Id = row.Field<int>("user_id");
                userEntity.UserName = row.Field<string>("user_name");
                userEntity.IsDel = row.Field<bool>("isdel");
                users.Add(userEntity);
            }
            return View(users);
        }

        [HttpPost]
        public ActionResult GetInfo(int id)
        {
            string tsql = "select user_name from users where user_id = @user_id";
            SqlParameter parameter=new SqlParameter("@user_id",id);
            return Content(SqlHelper.ExecuteScalar(SqlHelper.GetConnection(), CommandType.Text, tsql, parameter).ToString());
        } 

        [HttpPost]
        public ActionResult DelUser(int id)
        {
            string tsql = "update users set isdel =0 where user_id = @Id";
            SqlParameter parameter = new SqlParameter("@id", id);
            if (SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, tsql, parameter) > 0)
            {
                return Content("删除成功");
            }

            return Content("删除失败");
        }

        public ActionResult YouKe()
        {
            return View();
        }

        public ActionResult YongHu()
        {
            return View();
        }
    }
}