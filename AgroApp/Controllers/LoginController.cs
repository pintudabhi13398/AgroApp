using AgroApp.Common;
using AgroApp.Controllers;
using AgroApp.Data;
using Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


namespace TransportMS.Controllers
{
    public class LoginController : Controller
    {

        private IFormsAuthenticationService FormsService { get; set; }
        #region Methods
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Index", model);
            //}
            var UserNameParam = new SqlParameter
            {
                ParameterName = "UserName",
                DbType = DbType.String,
                Value = model.UserName
            };
            var PasswordParam = new SqlParameter
            {
                ParameterName = "Password",
                DbType = DbType.String,
                Value = model.Password
            };

            UserModel logedInUser = new UserModel();
            using (AgroAppEntities obj = new AgroAppEntities())
            {
                try
                {
                    var List = obj.Database.SqlQuery<UserModel>("Sp_UserLogin @UserName,@Password", UserNameParam, PasswordParam).FirstOrDefault();
                    logedInUser = List;
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
                

            if (logedInUser == null)
            {
                ModelState.AddModelError("UserName", "Invalid Login Credentials.");
                return View(model);
            }

            if (!logedInUser.IsActive)
            {
                ModelState.AddModelError("UserName", string.Format("User '{0}' is no longer active.", logedInUser.UserName));
                return View(model);
            }

            SessionHelper.UserId = Convert.ToInt32(logedInUser.UserId);
            SessionHelper.WelcomeUser = logedInUser.Name;
            SessionHelper.RoleId = logedInUser.RoleId;
            SessionHelper.Email = logedInUser.EmailAddress;
            SessionHelper.Password = logedInUser.Password;
            SessionHelper.IsSuperAdmin = logedInUser.IsSuperAdmin;
            SessionHelper.ProfilePic = logedInUser.Image;
            SessionHelper.UserAccessPermissions = CustomRepository.UserAccessPermissions(SessionHelper.RoleId, logedInUser.IsSuperAdmin);
            FormsService.SignIn(logedInUser.UserName, model.RememberMe);
           
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsService.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {

            try
            {
                UserModel model = new UserModel();
                using (AgroAppEntities obj = new AgroAppEntities())
                {
                    var EmailParam = new SqlParameter
                    {
                        ParameterName = "EmailAddress",
                        DbType = DbType.String,
                        Value = Email
                    };
                    model = obj.Database.SqlQuery<UserModel>("SP_ForgotPassGetDetail @EmailAddress", EmailParam).FirstOrDefault();
                }
                string token = Guid.NewGuid().ToString();
                if (model.UserId > 0)

                {
                    string link = Url.Action("ResetPassword", "Login", new RouteValueDictionary(new { Token = token }), System.Web.HttpContext.Current.Request.Url.Scheme);

                    string resetLink = "<a href='" + link + "'>" + link + "</a>";
                    string toEmail = model.EmailAddress;


                    string bodyTemplate = System.IO.File.ReadAllText(Server.MapPath("~/Template/ForgotPassword.html"));


                    bodyTemplate = bodyTemplate.Replace("[@FirstName]", model.Name);
                    bodyTemplate = bodyTemplate.Replace("[@link]", resetLink);

                    Task task = new Task(() => EmailHelper.SendAsyncEmail(toEmail, "ForgotPassword", bodyTemplate, true));
                    task.Start();


                    model.Token = token;
                    model.TokenExpiryDate = DateTime.Now.AddDays(1);

                    using (AgroAppEntities obj = new AgroAppEntities())
                    {
                        var UserIdParam = new SqlParameter
                        {
                            ParameterName = "UserId",
                            DbType = DbType.Int32,
                            Value = model.UserId
                        };
                        var TokenParam = new SqlParameter
                        {
                            ParameterName = "Token",
                            DbType = DbType.String,
                            Value = model.Token
                        };
                        var TokenExpiryDateParam = new SqlParameter
                        {
                            ParameterName = "TokenExpiryDate",
                            DbType = DbType.DateTime,
                            Value = model.TokenExpiryDate
                        };
                        var result = obj.Database.SqlQuery<UserModel>("SP_UserForgotPassTokenSave @UserId,@Token,@TokenExpiryDate", UserIdParam, TokenParam, TokenExpiryDateParam).FirstOrDefault();
                    }

                    ModelState.AddModelError("Email", "Email For Reset Passsword has been sent. Please check your email.");
                    return View();

                }
                else
                {
                    ModelState.AddModelError("Email", "Please enter valid Email Address.");
                    return View();

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", "Please enter valid Email Address.");
                return View("Index");
            }
        }

        public ActionResult ResetPassword(string Token)
        {
            UserModel userModel = new UserModel();
            using (AgroAppEntities obj = new AgroAppEntities())
            {

                var TokenParam = new SqlParameter
                {
                    ParameterName = "Token",
                    DbType = DbType.String,
                    Value = Token
                };
                userModel = obj.Database.SqlQuery<UserModel>("SP_CompareTokenForResetPassword", TokenParam).FirstOrDefault();
            }
                if (userModel != null)
                {
                    if (userModel.UserId > 0 && userModel.TokenExpiryDate >= DateTime.Now)
                    {
                        ResetPasswordModel model = new ResetPasswordModel { UserId = userModel.UserId };
                        return View(model);
                    }

                    if (userModel.TokenExpiryDate < DateTime.Now)
                    {
                        TempData["Message"] = "Token for reset password has been expired.please try agian to reset password.";
                        return RedirectToAction("Index", "Login");
                    }
                    TempData["Message"] = "Something Went wrong. Please try again later.";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["Message"] = "Link has been expired";
                    return RedirectToAction("Index", "Login");
                }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel userModel)
        {

            if (userModel.ConfirmPassword != userModel.Password)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password must be same.");
                return View("Index");
            }

            UserModel model = new UserModel();
            using (AgroAppEntities obj = new AgroAppEntities())
            {
                var UserIdParam = new SqlParameter
                {
                    ParameterName = "UserId",
                    DbType = DbType.String,
                    Value = userModel.UserId
                };
                model = obj.Database.SqlQuery<UserModel>("SP_GetUserDetail @UserId", UserIdParam).FirstOrDefault();
            }

            if (model != null)
            {
                model.Token = null;
                model.TokenExpiryDate = null;
                model.Password = userModel.Password;

                using (AgroAppEntities obj = new AgroAppEntities())
                {
                    var UserIdParam = new SqlParameter
                    {
                        ParameterName = "UserId",
                        DbType = DbType.Int32,
                        Value = model.UserId
                    };
                    var PasswordParam = new SqlParameter
                    {
                        ParameterName = "Password",
                        DbType = DbType.String,
                        Value = model.Password
                    };
                    var result= obj.Database.SqlQuery<UserModel>("SP_UpdateUserPassword @UserId,@Password", UserIdParam, PasswordParam).FirstOrDefault();
                }
            }
            if (model.UserId > 0)
            {
                ModelState.AddModelError("ConfirmPassword", "Password reset successful.");
                return View();
            }
            ModelState.AddModelError("ConfirmPassword", "Something went wrong. Please try again later.");
            return View();
        }

        
        #endregion
    }

    #region Form Authentication
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(string.Empty, nameof(userName));
            }

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);

        void SignOut();
    }
    #endregion
}