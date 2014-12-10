using System.IO;
using AGM.Web.Infrastructure;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace AGM.Web.Controllers
{
    public class MonthlyReportController : ApiController
    {
        [HttpGet]
        public ApiResponse Get(int id, string month)
        {
            if (ConfigurationHelper.UseMockupData)
            {
                using (var re = new StreamReader(HttpContext.Current.Server.MapPath("~/App/Mockup/monthlyreports.js")))
                {
                    JsonTextReader reader = new JsonTextReader(re);
                    JsonSerializer se = new JsonSerializer();
                    object parsedData = se.Deserialize(reader);
                    return new ApiResponse(true)
                    {
                        Data = parsedData
                    };
                }
            }

            var cultureIt = CultureInfo.GetCultureInfo("it-IT");
            var currentMonthDate = DateTime.Today;
            var currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);

            if (!string.IsNullOrEmpty(month))
            {
                currentMonthDate = DateTime.Parse(month, cultureIt);
                currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);
            }

            var user = new User();
            var userHourReports = new List<MonthlyReportHour>();
            var userExpenseReports = new List<MonthlyReportExpense>();
            var userNoteReports = new List<MonthlyReportNote>();
            var hourReasons = new List<HourReason>();
            var expenseReasons = new List<ExpenseReason>();
            using (var context = new AgmDataContext())
            {
                user = context.Users.First(u => u.Id == id);
                userHourReports = context.MonthlyReportHours.Where(r => r.UserId == id && r.Month == currentMonthDate.Month).ToList();
                userExpenseReports = context.MonthlyReportExpenses.Where(e => e.UserId == id && e.Month == currentMonthDate.Month).ToList();
                userNoteReports = context.MonthlyReportNotes.Where(e => e.UserId == id && e.Month == currentMonthDate.Month).ToList();
                hourReasons = context.HourReasons.ToList();
                expenseReasons = context.ExpenseReasons.ToList();
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
                var hoursCollection = userHourReports.Where(r => r.Date == currentDate).ToList();
                var expensesCollection = userExpenseReports.Where(e => e.Date == currentDate).ToList();
                var notesCollection = userNoteReports.Where(e => e.Date == currentDate).ToList();
                report.Add(new
                {
                    Date = string.Format("{0} {1}", Char.ToUpper(currentDate.ToString("dddd", cultureIt)[0]), currentDate.ToString(" d", cultureIt)),
                    Hours = (hoursCollection.Any()) ? hoursCollection.Sum(r => r.HoursCount) : 0,
                    HoursCollection = hoursCollection,
                    Expenses = expensesCollection.Sum(e => e.GetTotalAmount()).ToString("N2", cultureIt),
                    ExpensesCollection = expensesCollection,
                    NotesCollection = notesCollection,
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
                        Id = id,
                        Name = string.Format("{0} {1}", user.FirstName, user.LastName)
                    },
                    CurrentMonth = currentMonthString,
                    Report = report,
                    HourReasons = hourReasons,
                    ExpenseReasons = expenseReasons
                }
            };
        }
    }
}