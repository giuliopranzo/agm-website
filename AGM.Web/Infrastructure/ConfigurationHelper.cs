using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AGM.Web.Infrastructure
{
    public static class ConfigurationHelper
    {
        public static bool UseMockupData = bool.Parse(ConfigurationManager.AppSettings["MockupData"]);
    }
}