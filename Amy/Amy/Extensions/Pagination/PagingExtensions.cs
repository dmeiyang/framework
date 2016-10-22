using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Runtime.Serialization;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web;

namespace System
{
    public static class PagingExtensions
    {
        public static PageList<T> ToPageList<T>(this IQueryable<T> source, int page, int size)
        {
            if (page <= 0 || size <= 0)
            {
                return new PageList<T>(new List<T>().AsQueryable(), 1, 10);
            }
            else
            {
                return new PageList<T>(source, page, size);
            }
        }

        public static PageList<T> ToPageList<T>(this IList<T> source, int page, int size, int count)
        {
            if (page <= 0 || size <= 0)
            {
                return new PageList<T>(new List<T>().AsQueryable(), 1, 10);
            }
            else
            {
                return new PageList<T>(source, page, size, count);
            }
        }

        public static PageList<T> ToPageList<T>(this List<T> source, int page, int size, int count)
        {
            if (page <= 0 || size <= 0)
            {
                return new PageList<T>(new List<T>().AsQueryable(), 1, 10);
            }
            else
            {
                return new PageList<T>(source, page, size, count);
            }
        }

        public static PageList<T> ToPageList<T>(this IEnumerable<T> source, int page, int size, int count)
        {
            if (page <= 0 || size <= 0)
            {
                return new PageList<T>(new List<T>().AsQueryable(), 1, 10);
            }
            else
            {
                return new PageList<T>(source, page, size, count);
            }
        }
    }

    public static class HtmlPagingExtension
    {
        /// <summary>
        /// Pager
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pagedable"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagination model)
        {
            return RenderPager(helper, model);
        }

        /// <summary>
        /// Pager
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pagedable"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagination model, string template)
        {
            return RenderPager(helper, model, template);
        }

        /// <summary>
        /// 生成最终的分页Html代码
        /// </summary>
        private static MvcHtmlString RenderPager(HtmlHelper htmlHelper, IPagination model)
        {
            return RenderPager(htmlHelper, model, "Pagination");
        }

        /// <summary>
        /// 生成最终的分页Html代码
        /// </summary>
        private static MvcHtmlString RenderPager(HtmlHelper htmlHelper, IPagination model, string pagingTemplatePartialName)
        {
            var templateHtml = htmlHelper.Partial(pagingTemplatePartialName, model);

            if (templateHtml == null)
                throw new ArgumentException(pagingTemplatePartialName);
            return templateHtml;
        }
    }

    public static class RouteValueExtensions
    {
        /// <summary>
        /// 在当前的路由上添加querystring参数
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static RouteValueDictionary AddQueryStringParameters(this RouteValueDictionary dict)
        {
            var querystring = HttpContext.Current.Request.QueryString;

            foreach (var key in querystring.AllKeys)
                if (!dict.ContainsKey(key))
                    dict.Add(key, querystring.GetValues(key)[0]);
            return dict;
        }

        /// <summary>
        /// AppendOrReplace
        /// </summary>
        /// <param name="oldRouteValueDict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RouteValueDictionary AppendOrReplace(this RouteValueDictionary oldRouteValueDict, string key, object value)
        {
            if (oldRouteValueDict == null) return new RouteValueDictionary();

            var routeValueDict = new RouteValueDictionary();

            foreach (var k in oldRouteValueDict.Keys) routeValueDict.Add(k, oldRouteValueDict[k]);

            if (!routeValueDict.ContainsKey(key)) routeValueDict.Add(key, value);
            else routeValueDict[key] = value;

            return routeValueDict;
        }

        /// <summary>
        /// 获取当前url的RouteValues
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static RouteValueDictionary RouteValues(this UrlHelper url)
        {
            return url.RequestContext.RouteData.Route.GetRouteData(url.RequestContext.HttpContext).Values.AddQueryStringParameters();
        }

        /// <summary>
        /// 获取当前url的RouteValues
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static RouteValueDictionary RouteValues(this Uri url)
        {
            var routeDic = new RouteValueDictionary();
            var urlString = url.AbsolutePath;
            var urlArray = urlString.ToSplit('/');
            if (urlArray.Length > 0)
            {
                routeDic.Add("controller", urlArray[0]);
            }
            else
            {
                routeDic.Add("controller", "Home");
            }

            if (urlArray.Length > 1)
            {
                routeDic.Add("action", urlArray[1]);
            }
            else
            {
                routeDic.Add("action", "Index");
            }
            
            return routeDic;
        }
    }
}
