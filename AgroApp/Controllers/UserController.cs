using AgroApp.Common;
using AgroApp.Data;
using Common;
using Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Nexmo.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace AgroApp.Controllers
{
    public class UserController : BaseController
    {
        AgroAppEntities context = new AgroAppEntities();
        

        // GET: User
        public  ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> KendoRead([DataSourceRequest]DataSourceRequest request, string str)
        {
            try
            {
                var StringParam = new SqlParameter
                {
                    ParameterName = "String",
                    DbType = DbType.String,
                    Value = str
                };
                List<UserModel> model = new List<UserModel>();
                
                    model = context.Database.SqlQuery<UserModel>("SP_UserMaster_GetAllUser @String", StringParam).ToList();
                

                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Message = CommonHelper.GetErrorMessage(ex), IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public async Task<ActionResult> KendoDestroy([DataSourceRequest]DataSourceRequest request, UserModel model)
        {
            string message = string.Empty;
            try
            {
                var UserIdParam = new SqlParameter
                {
                    ParameterName = "UserId",
                    DbType = DbType.Int32,
                    Value = (object)model.UserId ?? DBNull.Value
                };

                int result = context.Database.SqlQuery<int>("SP_UserMaster_DeleteUser @UserId",UserIdParam).FirstOrDefault();
                if (result == 1)
                {
                    message = "User Deleted SucessFully !!! ";
                }
                else if (result == 0)
                {
                    message = "User Deleted Failed !  Try After Some Time!!! ";
                }
                return (result == 0)
                 ? Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet)
                : Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Success) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> UserAddEdit(int? userId)
        {
            UserModel model = new UserModel();
            //model.UserId =Convert.ToInt32( userId);
            try
            {
                if (userId > 0)
                {
                    var UserIdParam = new SqlParameter
                    {
                        ParameterName = "UserId",
                        DbType = DbType.Int32,
                        Value = (object)userId ?? DBNull.Value
                    };

                    model = context.Database.SqlQuery<UserModel>("SP_UserMaster_GetPerticalarUserDetail @UserId", UserIdParam).FirstOrDefault();
                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                return Json(new { Message = ex, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public async Task<ActionResult> UserSave(UserModel model, HttpPostedFileBase file)
        {
            string message = string.Empty;
            model.Token = Guid.NewGuid().ToString();
            model.TokenExpiryDate = DateTime.Now.AddDays(1);

            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);
                var fileName = Path.GetFileName(file.FileName) + Guid.NewGuid().ToString() + ext;
                var physicalPath = Path.Combine(Server.MapPath("~/Image/UserProfile"), fileName);
                file.SaveAs(physicalPath);
                model.Image= fileName;
            }
            try
            {
                var UserIdParam = new SqlParameter
                {
                    ParameterName = "UserId",
                    DbType = DbType.Int32,
                    Value = (object)model.UserId ?? DBNull.Value
                };
                var UserNameParam = new SqlParameter
                {
                    ParameterName = "UserName",
                    DbType = DbType.String,
                    Value = (object)model.UserName ?? DBNull.Value
                };
                var PasswordParam = new SqlParameter
                {
                    ParameterName = "Password",
                    DbType = DbType.String,
                    Value = (object)model.Password ?? DBNull.Value
                };
                var NameParam = new SqlParameter
                {
                    ParameterName = "Name",
                    DbType = DbType.String,
                    Value = (object)model.Name ?? DBNull.Value
                };
                var EmailAddressParam = new SqlParameter
                {
                    ParameterName = "EmailAddress",
                    DbType = DbType.String,
                    Value = (object)model.EmailAddress ?? DBNull.Value
                };
                var RoleIdParam = new SqlParameter
                {
                    ParameterName = "RoleId",
                    DbType = DbType.Int32,
                    Value = (object)model.RoleId ?? DBNull.Value
                };
                var PhoneParam = new SqlParameter
                {
                    ParameterName = "Phone",
                    DbType = DbType.String,
                    Value = (object)model.Phone ?? DBNull.Value
                };
                var AddressParam = new SqlParameter
                {
                    ParameterName = "Address",
                    DbType = DbType.String,
                    Value = (object)model.Address ?? DBNull.Value
                };
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = (object)model.StateId ?? DBNull.Value
                };
                var DistIdParam = new SqlParameter
                {
                    ParameterName = "DistId",
                    DbType = DbType.Int32,
                    Value = (object)model.DistId ?? DBNull.Value
                };
                 var ZipCodeParam = new SqlParameter
                {
                    ParameterName = "ZipCode",
                    DbType = DbType.String,
                    Value = (object)model.ZipCode ?? DBNull.Value
                };
                var ImageParam = new SqlParameter
                {
                    ParameterName = "Image",
                    DbType = DbType.String,
                    Value = (object)model.Image ?? DBNull.Value
                };
                var TokenParam = new SqlParameter
                {
                    ParameterName = "Token",
                    DbType = DbType.String,
                    Value = (object)model.Token ?? DBNull.Value
                };
                var TokenExpiryDateParam = new SqlParameter
                {
                    ParameterName = "TokenExpiryDate",
                    DbType = DbType.DateTime,
                    Value = (object)model.TokenExpiryDate ?? DBNull.Value
                };
                var IsSuperAdminParam = new SqlParameter
                {
                    ParameterName = "IsSuperAdmin",
                    DbType = DbType.Boolean,
                    Value = (object)model.IsSuperAdmin ?? DBNull.Value
                };
                var IsActiveParam = new SqlParameter
                {
                    ParameterName = "IsActive",
                    DbType = DbType.Boolean,
                    Value = (object)model.IsActive ?? DBNull.Value
                };

                int result = context.Database.SqlQuery<int>("SP_UserMaster_InsertAndUpdateUser " +
                    "@UserId,@UserName,@Password,@Name,@EmailAddress,@RoleId,@Phone,@Address,@StateId,@DistId,@ZipCode,@Image," +
                    "@Token,@TokenExpiryDate,@IsSuperAdmin,@IsActive"
                    , UserIdParam, UserNameParam, PasswordParam, NameParam, EmailAddressParam, RoleIdParam,PhoneParam,AddressParam,StateIdParam,DistIdParam,ZipCodeParam,ImageParam,
                    TokenParam, TokenExpiryDateParam, IsSuperAdminParam, IsActiveParam).FirstOrDefault();
                if (result == 1)
                { MessageModel sms= new MessageModel();
                   sms.To ="91"+model.Phone ;
                    sms.ContentMsg ="Hello, "+model.Name+" Your Account Is Created On AgroApp."+"Your User Name Is: "+model.UserName+" And Your Password Is: "+model.Password;
                    var contr = new SMSController();
                    contr.SendSMS(sms);
                    message = "User Inserted SucessFully !!! ";
                    SuccessNotification(message);
                    
                }
                else if (result == 2)
                {
                    message = "User Updated SucessFully !!! ";
                    SuccessNotification(message);
                    
                }
                else if (result == 3)
                {
                    message = "User Name Or Email Id Already Exist !!! ";
                    ErrorNotification(message);
                    
                }

                if(result == 3)
                { return RedirectToAction("UserAddEdit", new { userId = model.UserId }); }
                else { return RedirectToAction("Index", "User"); }
                
                
                
            }

            catch (Exception ex)
            {
                return Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }

        }
        
    }
}