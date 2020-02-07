using Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;



namespace Common
{
    public static class HtmlHelperExtensions
    {
       

        public static MvcHtmlString GenerateMenu(this HtmlHelper helper)
        {
            List<UserAccessPermissionModel> parentMenuList = SessionHelper.UserAccessPermissions.Where(x => x.ParentMenuId == null).OrderBy(item => item.DisplayOrder).ToList();

            List<UserAccessPermissionModel> childMenuList = SessionHelper.UserAccessPermissions.Where(x => x.ParentMenuId != null).OrderBy(item => item.DisplayOrder).ToList();

            if (parentMenuList.Any())
            {
                TagBuilder li = new TagBuilder("li");
                
                StringBuilder sb = new StringBuilder();

                foreach (UserAccessPermissionModel menu in parentMenuList)
                {
                    List<UserAccessPermissionModel> childList = childMenuList.Where(x => x.ParentMenuId == menu.MenuId).ToList();

                    if (childList.Any())
                    {
                        StringBuilder sbChild = new StringBuilder();
                        TagBuilder liWithChild = new TagBuilder("li");
                        
                        TagBuilder firstSpan = ITag(menu.ImagePath);
                        TagBuilder secondSpan = SpanTag("");

                        TagBuilder aLink = AnchorLink(null, null, true);
                        aLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(firstSpan),"<span>"+ Convert.ToString(menu.Name)+"</span>"/*, Convert.ToString(secondSpan)*/);

                    
                        TagBuilder ulChild = new TagBuilder("ul");
                        
                        foreach (UserAccessPermissionModel childMenu in childList)
                        {
                            var icon = childMenu.ImagePath;
                            TagBuilder firstSpanchild = ITag(icon);

                            TagBuilder liChildaLink = AnchorLink(childMenu.Action, childMenu.Controller, false);
                            liChildaLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(firstSpanchild),"<span>"+Convert.ToString(childMenu.Name)+"</span>");

                            TagBuilder liChild = new TagBuilder("li") { InnerHtml = Convert.ToString(liChildaLink) };
                           
                            sbChild.Append(Convert.ToString(liChild));
                        }

                        ulChild.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}", Convert.ToString(sbChild));
                        liWithChild.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(aLink), Convert.ToString(ulChild));
                        sb.Append(Convert.ToString(liWithChild));
                    }
                    else
                    {
                        TagBuilder imageTag = ITag(menu.ImagePath);

                        TagBuilder aLink = AnchorLink(menu.Action, menu.Controller, false);
                        aLink.InnerHtml = string.Format("{0}{1}", Convert.ToString(imageTag),"<span>"+ menu.Name+"</span>");
                  


                        TagBuilder liWithChild = new TagBuilder("li") { InnerHtml = Convert.ToString(aLink) };
                        sb.Append(Convert.ToString(liWithChild));

                    }
                }

                li.InnerHtml = sb.ToString();
                return MvcHtmlString.Create(li.ToString());
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        private static TagBuilder AnchorLink(string actionName, string controllerName, bool isParent)
        {

            TagBuilder aLink = new TagBuilder("a");

            if (isParent)
            {
               //aLink.MergeAttribute("class", "has-ul");
                //aLink.MergeAttribute("data-action", "click-trigger");
                //aLink.MergeAttribute("data-toggle", "dropdown");
            }

            if (string.IsNullOrEmpty(actionName) || string.IsNullOrEmpty(controllerName))
            {
                aLink.MergeAttribute("href", "#");
                aLink.MergeAttribute("class", "has-ul");
            }
            else
            {
                aLink.MergeAttribute("id", controllerName);
                
                aLink.MergeAttribute("href", HtmlHelperExtensions.SiteRootPathUrl + "/" + controllerName + "/" + actionName);
            }
            return aLink;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static TagBuilder ITag(string className)
        {
            TagBuilder span = new TagBuilder("i");
            span.MergeAttribute("class", className);
            return span;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static TagBuilder SpanTag(string className)
        {
            TagBuilder span = new TagBuilder("span");
            span.MergeAttribute("class", className);
            return span;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static TagBuilder ItalicTag(string className)
        {
            TagBuilder span = new TagBuilder("i");
            span.MergeAttribute("class", className);
            return span;
        }
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
       


    }
    public class ReportHelper
    {
        public static readonly string ReportApiController = HtmlHelperExtensions.SiteRootPathUrl + "/api/reportsapi/";

        public static readonly string ReportTemplate = HtmlHelperExtensions.SiteRootPathUrl + "/Template/telerikReportViewerTemplate.html";
    }
}