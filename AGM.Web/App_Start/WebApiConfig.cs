using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AGM.Web.Core;

namespace AGM.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Use lower case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new LowercaseContractResolver();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "backoffice/api/{controller}/{action}"
            );

            
        }
    }
}
