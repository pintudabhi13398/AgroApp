using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Common
{
    public static class WebHelper
    {
        #region Contants

        public const string DateFormate = "dd/MM/yyyy";

        public const int PageSize = 25;

        public static int[] PageSizes = { 25, 50, 100, 200, 500 };

        public const string PleaseSelect = "--Select--";

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanEdit = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "80px" } };

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanStatus = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "120px" } };

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWith4Buttons = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "200px" } };

        public static readonly Dictionary<string, object> ActionCenterColumnStyleCanStatus = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "180px" } };

        public static readonly Dictionary<string, object> StatusColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "60px" } };

        public static readonly Dictionary<string, object> SmallColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "30px" } };
        #endregion

        #region Methods

        public static string SiteRootPathUrl
        {
            get
            {
                string msRootUrl;
                if (HttpContext.Current.Request.ApplicationPath != null && string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath.Split('/')[1]))
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +
                                ":" +
                                HttpContext.Current.Request.Url.Port;
                }
                else
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
                }

                return msRootUrl;
            }
        }

        

        #endregion
    }
}