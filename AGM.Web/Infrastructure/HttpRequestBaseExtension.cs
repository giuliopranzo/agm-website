using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Infrastructure
{
    public static class HttpRequestBaseExtension
    {
        public static string GetCookieValue(this HttpRequestBase request, string name)
        {
            var cookies = request.Cookies;

            var httpCookie = cookies.Get(name);
            return (httpCookie != null) ? httpCookie.Value : null;
        }
    }
}