using AgroApp.Common;
using AgroApp.Data;
using Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.Reporting.Interfaces;
using WebGrease;

namespace AgroApp.Controllers
{
   
        public class BaseController : Controller
        {
        AgroAppEntities context = new AgroAppEntities();

        #region Authentication
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);

                if (Request.IsAuthenticated)
                {
                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                    if (!controllerName.Contains("error") && !controllerName.Contains("nopagefound"))
                    {
                        #region Handle Session Time Out
                        if (SessionHelper.UserId == 0)
                        {
                            SetSessionValues(filterContext);
                        }
                        #endregion

                        if (!AuthorizationHelper.IsAuthorized(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName))
                        {
                            RedirectToLoginPage(filterContext);
                        }
                    }
                    else
                    {
                        RedirectToLoginPage(filterContext);
                    }
                }
                else
                {
                    RedirectToLoginPage(filterContext);
                }
            }

            protected override void OnAuthorization(AuthorizationContext filterContext)
            {
                base.OnAuthorization(filterContext);
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    FormsAuthenticationTicket newTicket = FormsAuthentication.RenewTicketIfOld(ticket);
                    if (newTicket != null && newTicket.Expiration != ticket.Expiration)
                    {
                        string encryptedTicket = FormsAuthentication.Encrypt(newTicket);

                        cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                        {
                            Path = FormsAuthentication.FormsCookiePath
                        };
                        Response.Cookies.Add(cookie);
                    }
                }
            }

            private void RedirectToLoginPage(ActionExecutingContext filterContext)
            {
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Action("Index", "Login");
                if (loginUrl != null) filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }

        #endregion

        #region Session Value
        /// <summary>
        /// Set Session Values
        /// </summary>
        /// <param name="filterContext"></param>
        private void SetSessionValues(ActionExecutingContext filterContext)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                {
                    RedirectToLoginPage(filterContext);
                }
                else
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket == null || authTicket.Expired)
                    {
                        RedirectToLoginPage(filterContext);
                    }
                    else
                    {
                        string userName = authTicket.Name;

                        UserModel logedInUser = new UserModel();

                        if (logedInUser == null)
                        {
                            RedirectToLoginPage(filterContext);
                        }
                        else if (logedInUser.IsActive == false)
                        {
                            RedirectToLoginPage(filterContext);
                        }
                        else
                        {
                            SessionHelper.UserId = Convert.ToInt32(logedInUser.UserId);
                            SessionHelper.WelcomeUser = logedInUser.Name;
                            SessionHelper.RoleId = logedInUser.RoleId;
                            SessionHelper.Email = logedInUser.EmailAddress;
                            SessionHelper.IsSuperAdmin = logedInUser.IsSuperAdmin;
                            SessionHelper.ProfilePic = logedInUser.Image;
                            //SessionHelper.AccountId = logedInUser.AccountId;
                            SessionHelper.UserAccessPermissions = CustomRepository.UserAccessPermissions(logedInUser.RoleId, logedInUser.IsSuperAdmin);
                        }
                    }
                }

            }

        #endregion

        #region Notification
        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">The Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="saveInSession">save message in session</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true, bool saveInSession = false)
        {
            AddNotification(Enums.NotifyType.Success, message, persistForTheNextRequest, saveInSession);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">The Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="saveInSession">if set to <c>true</c> [save in session].</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true, bool saveInSession = false)
        {
            AddNotification(Enums.NotifyType.Error, message, persistForTheNextRequest, saveInSession);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">The Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            AddNotification(Enums.NotifyType.Error, exception.Message, persistForTheNextRequest, false);
        }

        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">the Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="saveInSession">if set to <c>true</c> [save in session].</param>
        protected virtual void AddNotification(Enums.NotifyType type, string message, bool persistForTheNextRequest, bool saveInSession)
        {
            var dataKey = string.Format("tmp.notifications.{0}", type);

            if (!saveInSession)
            {
                if (persistForTheNextRequest)
                {
                    if (TempData[dataKey] == null)
                    {
                        TempData[dataKey] = new List<string>();
                    }

                    ((List<string>)TempData[dataKey]).Add(message);
                }
                else
                {
                    if (ViewData[dataKey] == null)
                    {
                        ViewData[dataKey] = new List<string>();
                    }

                    ((List<string>)ViewData[dataKey]).Add(message);
                }
            }
            else
            {
                if (Session[dataKey] == null)
                {
                    Session[dataKey] = new List<string>();
                }

                ((List<string>)Session[dataKey]).Add(message);
            }
        }
        #endregion

    }
}
