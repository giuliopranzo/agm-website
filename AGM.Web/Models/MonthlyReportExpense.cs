using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AGM.Web.Infrastructure.Extensions;

namespace AGM.Web.Models
{
    public class MonthlyReportExpense
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReasonId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string AmountRaw { get; set; }

        public double Amount
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                double o;
                double.TryParse(AmountRaw,NumberStyles.Any, cultureIt, out o);
                return Math.Round(o, 2);
            }
        }

        public DateTime Date
        {
            get { return new DateTime(Year, Month, Day); }
        }

        public string Reason
        {
            get
            {
                if (AgmStaticDataContext.ExpenseReasons.Any(r => r.Id == ReasonId))
                    return AgmStaticDataContext.ExpenseReasons.First(r => r.Id == ReasonId).Name;
                return null;
            }
        }

        public string CompleteDescription
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                switch (ReasonId)
                {
                    case 1:
                        return string.Format("{0}-{1}", Reason, (Amount * 0.40).ToString("N2", cultureIt));
                        break;
                    case 2:
                        return string.Format("{0}-{1}", Reason, (Amount * 0.40).ToString("N2", cultureIt));
                        break;
                    default:
                        return string.Format("{0}-{1}", Reason, Amount.ToString("N2", cultureIt));
                        break;
                }
            }
        }
    }
}