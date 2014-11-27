using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace AGM.Web.Infrastructure
{
    public static class HttpRequestHeadersExtension
    {
        public static string GetCookieValue(this HttpRequestHeaders headers, string cookieName)
        {
            var cookies = headers.GetCookies();

            if (cookies == null || !cookies.Any(c => c.Cookies.Any(k => k.Name == cookieName)))
                return null;

            return cookies.First(c => c[cookieName] != null)[cookieName].Value;
        }

        public static string GetHeaderValue(this HttpRequestHeaders headers, string name)
        {
            IEnumerable<string> resColl;
            headers.TryGetValues(name, out resColl);

            var enumerable = resColl as IList<string> ?? resColl.ToList();
            if (resColl == null || !enumerable.Any())
                return null;

            return enumerable.First();
        }
    }
}