using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AGM.Web.Infrastructure;
using Newtonsoft.Json;

namespace AGM.Web.Models
{
    public class UserBase
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string _image { get; set; }

        public string Company { get { return "COMPANY"; } }
        public string RetributionItemConfSerialized { get; set; }

        public List<RetributionItemConf> RetributionItemConfiguration
        {
            get
            {
                if (!string.IsNullOrEmpty(RetributionItemConfSerialized))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<RetributionItemConf>>(RetributionItemConfSerialized);

                var res = new List<RetributionItemConf>();
                foreach (var val in Enum.GetValues(typeof (RetributionItemType)))
                {
                    res.Add(new RetributionItemConf() {Type = (RetributionItemType)val, EnableValue = 0});
                }
                return res;
            }
            set
            {
                if (string.IsNullOrEmpty(RetributionItemConfSerialized))
                    RetributionItemConfSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
        }

        public string Name
        {
            get { return string.Format("{0} {1}", LastName, FirstName); }

        }

        public string Image
        {
            get { return (string.IsNullOrEmpty(_image))? string.Format("http://robohash.org/bgset_bg1/agmuser_{0}?size=160x160", Id) : _image; }
            set { _image = value; }
        }
    }
}