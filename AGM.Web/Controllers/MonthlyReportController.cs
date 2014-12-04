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
    public class MonthlyReportController : ApiController
    {
        [HttpGet]
        public ApiResponse Get(int Id, string month)
        {
            var cultureIt = CultureInfo.GetCultureInfo("it-IT");
            var currentMonthDate = DateTime.Today;
            var currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);

            if (!string.IsNullOrEmpty(month))
            {
                currentMonthDate = DateTime.Parse(month, cultureIt);
                currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);
            }

            var userReports = new List<MonthlyReportHour>();
            using (var context = new AgmDataContext())
            {
                userReports = context.MonthlyReportHours.Where(r => r.UserId == Id && r.Month == currentMonthDate.Month).ToList();
            }

            var startDate = new DateTime(currentMonthDate.Year, currentMonthDate.Month, 1);
            while (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(-1);
            }
            var endDate = new DateTime(currentMonthDate.Year, currentMonthDate.Month, DateTime.DaysInMonth(currentMonthDate.Year, currentMonthDate.Month));
            while (endDate.DayOfWeek != DayOfWeek.Sunday)
            {
                endDate = endDate.AddDays(1);
            }
            
            var currentDate = startDate;
            var report = new List<object>();

            while (currentDate <= endDate)
            {
                report.Add(new
                {
                    Date = string.Format("{0} {1}", Char.ToUpper(currentDate.ToString("dddd", cultureIt)[0]), currentDate.ToString(" d", cultureIt)),
                    Hours = (userReports.Any(r => r.Date == currentDate))? userReports.Where(r => r.Date == currentDate).Sum(r => r.HoursCount) : 0,
                    HoursCollection = new { },
                    Expenses = ((decimal)0).ToString("C", cultureIt),
                    ExpensesCollection = new { },
                    NotesCollection = new { },
                    WorkDay = currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday,
                    IsCurrentMonth = (currentDate.Month == currentMonthDate.Month),
                    DayOfWeek = (int)currentDate.DayOfWeek
                });

                currentDate = currentDate.AddDays(1);
            }


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
                    CurrentMonth = currentMonthString,
                    Report = report
                }
            };
        }
    }
}