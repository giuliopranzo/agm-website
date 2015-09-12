using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AGM.Web.Infrastructure;

namespace AGM.Web.Models
{
    public class Option
    {
        public int Id { get; set; }
        public OptionSection Section { get; set; }
        public string SerializedValue { get; set; }

        public object Value
        {
            get
            {
                if (string.IsNullOrEmpty(SerializedValue))
                    return null;

                switch (Section)
                {
                    case OptionSection.MealVoucher:
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<MealVoucherOptions>(SerializedValue);
                    default:
                        break;
                }

                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(SerializedValue))
                {
                    SerializedValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                }
            }
        }
    }
}