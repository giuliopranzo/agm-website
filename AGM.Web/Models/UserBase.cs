﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AGM.Web.Infrastructure;

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

        public string Name
        {
            get { return string.Format("{0} {1}", LastName, FirstName); }

        }

        public string Image
        {
            get { return (string.IsNullOrEmpty(_image))? string.Format("../images/default_avatar.jpg?size=160x160", Id) : _image; }
            set { _image = value; }
        }

        public string RetributionItemConfSerialized { get; set; }

        public List<RetributionItemConf> RetributionItemConfiguration
        {
            get
            {
                var currentRetItems = new List<RetributionItemConf>();
                if (!string.IsNullOrEmpty(RetributionItemConfSerialized))
                    currentRetItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RetributionItemConf>>(RetributionItemConfSerialized);

                foreach (var val in Enum.GetValues(typeof(RetributionItemType)))
                {
                    if (currentRetItems.All(i => i.Type != (RetributionItemType)val))
                        currentRetItems.Add(new RetributionItemConf() { Type = (RetributionItemType)val, EnableValue = 0 });
                }
                return currentRetItems;
            }
            set
            {
                if (string.IsNullOrEmpty(RetributionItemConfSerialized))
                    RetributionItemConfSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
        }
    }
}