using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace AGM.Web.Controllers
{
    public class MonthlyReportsController : ApiController
    {
        [HttpGet]
        public ApiResponse Get(string Id, string month)
        {
            var cultureIt = CultureInfo.GetCultureInfo("it-IT");
            var currentMonth = DateTime.Today.ToString("yyyy-MM-dd", cultureIt);
            if (!string.IsNullOrEmpty(month))
                currentMonth = DateTime.Parse(month, cultureIt).ToString("yyyy-MM-dd", cultureIt);

            return new ApiResponse()
            {
                Succeed = true,
                Data = new
                {
                    User = new
                    {
                        Id = Id,
                        Name = "test"
                    },
                    CurrentMonth = currentMonth,
                    Report = new List<object>(){
                        new { 
                            Date = (new DateTime(2014, 10, 1)).ToString("dddd d", cultureIt),
                            Hours = 8,
                            Notes = "test note",
                            WorkDay = ((new DateTime(2014, 10, 1)).DayOfWeek == DayOfWeek.Saturday || (new DateTime(2014, 10, 1)).DayOfWeek == DayOfWeek.Sunday)
                        },
                        new { 
                            Date = (new DateTime(2014, 10, 2)).ToString("dddd d", cultureIt),
                            Hours = 8,
                            Notes = "test note due",
                            WorkDay = ((new DateTime(2014, 10, 2)).DayOfWeek == DayOfWeek.Saturday || (new DateTime(2014, 10, 2)).DayOfWeek == DayOfWeek.Sunday)
                        },
                        new { 
                            Date = (new DateTime(2014, 10, 3)).ToString("dddd d", cultureIt),
                            Hours = 8,
                            Notes = "test note due",
                            WorkDay = ((new DateTime(2014, 10, 3)).DayOfWeek == DayOfWeek.Saturday || (new DateTime(2014, 10, 3)).DayOfWeek == DayOfWeek.Sunday)
                        },
                        new { 
                            Date = (new DateTime(2014, 10, 4)).ToString("dddd d", cultureIt),
                            Hours = 8,
                            Notes = "test note due",
                            WorkDay = ((new DateTime(2014, 10, 4)).DayOfWeek == DayOfWeek.Saturday || (new DateTime(2014, 10, 4)).DayOfWeek == DayOfWeek.Sunday)
                        },
                        new { 
                            Date = (new DateTime(2014, 10, 5)).ToString("dddd d", cultureIt),
                            Hours = 8,
                            Notes = "test note due",
                            WorkDay = ((new DateTime(2014, 10, 5)).DayOfWeek == DayOfWeek.Saturday || (new DateTime(2014, 10, 5)).DayOfWeek == DayOfWeek.Sunday)
                        }
                    }
                }
            };
        }
    }
}