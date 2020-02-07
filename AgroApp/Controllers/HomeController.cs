using AgroApp.Common;
using AgroApp.Data;
using Common;
using Entities;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AgroApp.Controllers
{
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {
        return View();
        }

        public async Task<ActionResult> OpenChangePasswordWindow()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            model.RealPassword = SessionHelper.Password;
            model.CurrentUserId = SessionHelper.UserId;
            return PartialView("ChangePassword", model);
        }
        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordModel model)
        //{
        //      int result = 0;
        //        string message;

        //    if (model.OldPassword == model.RealPassword)
        //    {
        //        if (model.NewPassword == model.ConfirmPassword)
        //        {

        //            try
        //            {
        //                var UserIdParam = new SqlParameter
        //                {
        //                    ParameterName = "UserId",
        //                    DbType = DbType.Int32,
        //                    Value = model.CurrentUserId
        //                };
        //                var PasswordParam = new SqlParameter
        //                {
        //                    ParameterName = "Password",
        //                    DbType = DbType.String,
        //                    Value = model.NewPassword
        //                };

        //                AgroAppEntities context = new AgroAppEntities();

        //                result = context.Database.SqlQuery<int>("SP_User_ChangePassword @UserId,@Password", UserIdParam, PasswordParam).FirstOrDefault();

        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Message = CommonHelper.GetErrorMessage(ex), IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            result = 3;
        //        }
        //    }
        //    else
        //    {
        //        result = 2;
        //    }
        //    return View("Index");
        //}
        [HttpPost]
        public async Task<ActionResult> ChangePassword([DataSourceRequest] DataSourceRequest request, ChangePasswordModel model)
        {
            int result = 0;
            string message;
            try
            {
                var UserIdParam = new SqlParameter
                {
                    ParameterName = "UserId",
                    DbType = DbType.Int32,
                    Value = model.CurrentUserId
                };
                var PasswordParam = new SqlParameter
                {
                    ParameterName = "Password",
                    DbType = DbType.String,
                    Value = model.NewPassword
                };

                AgroAppEntities context = new AgroAppEntities();

                result = context.Database.SqlQuery<int>("SP_User_ChangePassword @UserId,@Password", UserIdParam, PasswordParam).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return Json(new { Message = CommonHelper.GetErrorMessage(ex), IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { id = result }, JsonRequestBehavior.AllowGet);
        }
    }
   
}