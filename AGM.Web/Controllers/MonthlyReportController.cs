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
            var totalHours = 0d;
            var totalOrdinaryHours = 0d;
            var totalExpenses = 0d;
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
                    DateShort = string.Format("{0} {1}", Char.ToUpper(currentDate.ToString("dddd", cultureIt)[0]), currentDate.ToString(" d", cultureIt)),
                    Date = string.Format("{0} {1}", currentDate.ToString("dddd", cultureIt), currentDate.ToString(" d", cultureIt)),
                    Hours = (hoursCollection.Any()) ? hoursCollection.Sum(r => r.HoursCount) : 0,
                    HoursCollection = hoursCollection,
                    Expenses = expensesCollection.Sum(e => e.GetTotalAmount()).ToString("N2", cultureIt),
                    ExpensesCollection = expensesCollection,
                    NotesCollection = notesCollection,
                    WorkDay = currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday,
                    IsCurrentMonth = (currentDate.Month == currentMonthDate.Month),
                    DayOfWeek = (int)currentDate.DayOfWeek
                });

                if (currentDate.Month == currentMonthDate.Month)
                {
                    totalHours += hoursCollection.Sum(r => r.HoursCount);
                    
                    if (hoursCollection.Any(r => r.ReasonId == 1))
                        totalOrdinaryHours += hoursCollection.Where(r => r.ReasonId == 1).Sum(r => r.HoursCount);
                    
                    totalExpenses += expensesCollection.Sum(e => e.GetTotalAmount());
                }

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
                    SelectedInsertDate = ((currentMonthDate.Month == DateTime.Today.Month && currentMonthDate.Year == DateTime.Today.Year)? DateTime.Today : new DateTime(currentMonthDate.Year, currentMonthDate.Month, 1)).ToString("yyyy-MM-dd 00:00:00"),
                    MinMonthDate = (new DateTime(currentMonthDate.Year, currentMonthDate.Month, 1)).ToString("yyyy-MM-dd 00:00:00"),
                    MaxMonthDate = (new DateTime(currentMonthDate.Year, currentMonthDate.Month, DateTime.DaysInMonth(currentMonthDate.Year, currentMonthDate.Month))).ToString("yyyy-MM-dd 00:00:00"),
                    Report = report,
                    HourReasons = hourReasons,
                    ExpenseReasons = expenseReasons,
                    TotalHours = totalHours.ToString("N2", cultureIt),
                    TotalDays = (totalHours / 8).ToString("N2", cultureIt),
                    TotalOrdinaryHours =totalOrdinaryHours.ToString("N2", cultureIt),
                    TotalOrdinaryDays = (totalOrdinaryHours / 8).ToString("N2", cultureIt),
                    TotalExpenses = totalExpenses.ToString("N2", cultureIt)
                }
            };
        }

        [HttpPost]
        public ApiResponse Insert([FromBody]dynamic reportIn)
        {
            using (var context = new AgmDataContext())
            {
                var res = new List<object>();
                if (reportIn.Hours != null && reportIn.Hours.HoursCount != null)
                {
                    var objHoursIn = context.MonthlyReportHours.Add(new MonthlyReportHour()
                    {
                        UserId = reportIn.UserId,
                        HoursRaw = reportIn.Hours.HoursCount,
                        ReasonId = reportIn.Hours.ReasonId,
                        Day = ((DateTime) reportIn.Date).Day,
                        Month = ((DateTime) reportIn.Date).Month,
                        Year = ((DateTime) reportIn.Date).Year
                    });
                    res.Add(objHoursIn);
                }

                if (reportIn.Expenses != null && reportIn.Expenses.Amount != null)
                {
                    var objExpensesIn = context.MonthlyReportExpenses.Add(new MonthlyReportExpense()
                    {
                        UserId = reportIn.UserId,
                        AmountRaw = reportIn.Expenses.Amount,
                        ReasonId = reportIn.Expenses.ReasonId,
                        Day = ((DateTime) reportIn.Date).Day,
                        Month = ((DateTime) reportIn.Date).Month,
                        Year = ((DateTime) reportIn.Date).Year
                    });
                    res.Add(objExpensesIn);
                }

                if (reportIn.Note != null)
                {
                    var objNoteIn = context.MonthlyReportNotes.Add(new MonthlyReportNote()
                    {
                        UserId = reportIn.UserId,
                        Note = reportIn.Note,
                        Day = ((DateTime) reportIn.Date).Day,
                        Month = ((DateTime) reportIn.Date).Month,
                        Year = ((DateTime) reportIn.Date).Year
                    });
                    res.Add(objNoteIn);
                }

                context.SaveChanges();
                return new ApiResponse(true) {Data = res};
            }
            return new ApiResponse(true);
        }

        [HttpPost]
        public ApiResponse Delete([FromBody]dynamic objIn)
        {
            var id = (int)objIn.Id;
            var type = objIn.Type.ToString();
            using (var context = new AgmDataContext())
            {
                if (type == "Hour" && context.MonthlyReportHours.Any(h => h.Id == id))
                {
                    var o = context.MonthlyReportHours.First(h => h.Id == id);
                    context.MonthlyReportHours.Remove(o);
                }
                else if (type == "Expense" && context.MonthlyReportExpenses.Any(h => h.Id == id))
                {
                    var o = context.MonthlyReportExpenses.First(h => h.Id == id);
                    context.MonthlyReportExpenses.Remove(o);
                }
                else if (type == "Note" && context.MonthlyReportNotes.Any(h => h.Id == id))
                {
                    var o = context.MonthlyReportNotes.First(h => h.Id == id);
                    context.MonthlyReportNotes.Remove(o);
                }
                
                context.SaveChanges();
                return new ApiResponse(true){ Data=type };
            }
        }

        [HttpGet]
        public ApiResponse GetRecurringNotes(int id, string value)
        {
            var res = new List<string>();

            using (var context = new AgmDataContext())
            {
                if (context.MonthlyReportNotes.Any(n => n.UserId == id && n.Note.Contains(value)))
                    res = context.MonthlyReportNotes.Where(n => n.UserId == id && n.Note.Contains(value)).Select(n => n.Note).Distinct().ToList();
            }

            return new ApiResponse(true)
            {
                Data = res
            };
        }

        [HttpPost]
        public ApiResponse Autocomplete(dynamic objIn)
        {
            var cultureIt = CultureInfo.GetCultureInfo("it-IT");
            string month = objIn.month;

            var currentMonthDate = DateTime.Parse(month, cultureIt);
            var currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);

            var user = new User();
            var userHourReports = new List<MonthlyReportHour>();
            var userExpenseReports = new List<MonthlyReportExpense>();
            var userNoteReports = new List<MonthlyReportNote>();
            var hourReasons = new List<HourReason>();
            var expenseReasons = new List<ExpenseReason>();
            var totalHours = 0d;
            var totalOrdinaryHours = 0d;
            var totalExpenses = 0d;
            using (var context = new AgmDataContext())
            {
                user = context.Users.First(u => u.Id == objIn.id);
                userHourReports = context.MonthlyReportHours.Where(r => r.UserId == objIn.id && r.Month == currentMonthDate.Month).ToList();
                userExpenseReports = context.MonthlyReportExpenses.Where(e => e.UserId == objIn.id && e.Month == currentMonthDate.Month).ToList();
                userNoteReports = context.MonthlyReportNotes.Where(e => e.UserId == objIn.id && e.Month == currentMonthDate.Month).ToList();
                hourReasons = context.HourReasons.ToList();
                expenseReasons = context.ExpenseReasons.ToList();
            }
        }
    }
}