using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Common
{
    public class SessionHelper
    {
        

        public static int UserId
        {
            get
            {
                return HttpContext.Current.Session["UserId"] == null ? 0 : (int)HttpContext.Current.Session["UserId"];
            }
            set { HttpContext.Current.Session["UserId"] = value; }
        }

        public static int RoleId
        {
            get
            {
                return HttpContext.Current.Session["RoleId"] == null ? 0 : (int)HttpContext.Current.Session["RoleId"];
            }
            set { HttpContext.Current.Session["RoleId"] = value; }
        }

        public static string WelcomeUser
        {
            get
            {
                return HttpContext.Current.Session["WelcomeUser"] == null
                    ? "Guest"
                    : (string)HttpContext.Current.Session["WelcomeUser"];
            }
            set { HttpContext.Current.Session["WelcomeUser"] = value; }
        }

        public static string Email
        {
            get { return Convert.ToString(HttpContext.Current.Session["Email"]); }

            set { HttpContext.Current.Session["Email"] = value; }
        }

        public static string ProfilePic
        {
            get { return Convert.ToString(HttpContext.Current.Session["Image"]); }

            set { HttpContext.Current.Session["Image"] = value; }
        }
        public static string Password
        {
            get { return Convert.ToString(HttpContext.Current.Session["Password"]); }

            set { HttpContext.Current.Session["Password"] = value; }
        }

        public static bool IsSuperAdmin
        {
            get
            {
                return HttpContext.Current.Session["IsSuperAdmin"] != null &&
                       (bool)HttpContext.Current.Session["IsSuperAdmin"];
            }
            set { HttpContext.Current.Session["IsSuperAdmin"] = value; }
        }

        public static bool ShowMenu
        {
            get
            {
                return HttpContext.Current.Session["ShowMenu"] == null ? true : (bool)HttpContext.Current.Session["ShowMenu"];
            }
            set
            {
                HttpContext.Current.Session["ShowMenu"] = value;
            }
        }

        public static void RememberLoginDetails(string Email, string Password)
        {
            HttpCookie objCookie = HttpContext.Current.Request.Cookies["EmployeeLoginDetails"] ?? new HttpCookie("EmployeeLoginDetails");
            objCookie.Values["LastVisit"] = DateTime.Now.ToString();
            objCookie.Values["Email"] = Email;
            objCookie.Values["Password"] = Password;
            objCookie.Expires = DateTime.Now.AddDays(30);
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        public static HttpCookie GetLoginDetails()
        {
            HttpCookie objCookie = HttpContext.Current.Request.Cookies["EmployeeLoginDetails"];
            if (objCookie != null)
            {
                return objCookie;
            }
            return null;
        }

        public static void ClearCookie(string Key)
        {
            HttpCookie objCookie = HttpContext.Current.Request.Cookies[Key] ?? new HttpCookie(Key);
            objCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }
        public static List<UserAccessPermissionModel> UserAccessPermissions
        {
            get
            {
                return HttpContext.Current.Session["UserAccessPermissions"] == null
                    ? new List<UserAccessPermissionModel>()
                    : HttpContext.Current.Session["UserAccessPermissions"] as
                        List<UserAccessPermissionModel>;
            }

            set { HttpContext.Current.Session["UserAccessPermissions"] = value; }
        }
    }
}
