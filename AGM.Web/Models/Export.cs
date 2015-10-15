using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AGM.Web.Infrastructure;

namespace AGM.Web.Models
{
    public class Export
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string MHFileName { get; set; }
        public string RIFileName { get; set; }
        public int UsersCount { get; set; }
        public int UsersMax { get; set; }
        public DateTime CalculateDate { get; set; }
        public string _hourReport { get; set; }
        public string _retributionItems { get; set; }


        public Dictionary<int, Dictionary<string, double>> HourReport
        {
            get
            {
                if (string.IsNullOrEmpty(_hourReport))
                    return null;

                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, double>>>(_hourReport);
            }
            set
            {
                if (string.IsNullOrEmpty(_hourReport))
                {
                    _hourReport = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                }
            }
        }

        public Dictionary<int, Dictionary<RetributionItemType, RetributionItem>> RetributionItems
        {
            get
            {
                if (string.IsNullOrEmpty(_retributionItems))
                    return null;

                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, Dictionary<RetributionItemType, RetributionItem>>>(_retributionItems);
            }
            set
            {
                if (string.IsNullOrEmpty(_retributionItems))
                {
                    _retributionItems = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                }
            }
        }
    }
}