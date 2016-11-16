using System;
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
            get {
                List<string> lnames = LastName.TrimStart().TrimEnd().Split(' ').ToList().ConvertAll(x => x = x.TrimStart().TrimEnd()).Where(x => x != "").ToList();
                List<string> fnames = FirstName.TrimStart().TrimEnd().Split(' ').ToList().ConvertAll(x => x = x.TrimStart().TrimEnd()).Where(x => x != "").ToList();
                
                string lname = string.Join(" ", lnames);
                string fname = string.Join(" ", fnames);

                if (lname.Length + fname.Length <= 25)
                    return string.Format("{0} {1}", lname, fname);
                else if (fnames.Count > 0 && lname.Length + 2 <= 25)
                {
                    if (lname.Length + (fnames.Count * 3) - 1 <= 25)
                    {
                        return string.Format("{0} {1}", lname, string.Join(" ", fnames.ConvertAll(x => x.Substring(0, 1) + ".")));
                    }
                    else
                    {
                        return string.Format("{0} {1}", lname, fnames.First().Substring(0, 1) + ".");
                    }
                }
                else
                {
                    return string.Format("{0}", lname.Substring(0, 25) + ".");
                }
            }
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