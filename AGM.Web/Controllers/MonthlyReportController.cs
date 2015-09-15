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
using AGM.Web.Infrastructure.Attributes;

namespace AGM.Web.Controllers
{
    public class MonthlyReportController : ApiController
    {
        [AuthorizeAction]
        [HttpGet]
        public ApiResponse Get(int id, string month)
        {
            this.CheckCurrentUserPermission(id, ((x) => x.SectionUsersVisible));
            return ExtractMonthlyReport(id, month);
        }

        [AuthorizeAction]
        public ApiResponse ExtractMonthlyReport(int id, string month)
        {
            var cultureIt = CultureInfo.GetCultureInfo("it-IT");
            var currentMonthDate = DateTime.Today;
            var currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);
            var currentMonthStringCompact = currentMonthDate.ToString("yyyyMM", cultureIt);

            if (!string.IsNullOrEmpty(month))
            {
                currentMonthDate = DateTime.Parse(month, cultureIt);
                currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);
                currentMonthStringCompact = currentMonthDate.ToString("yyyyMM", cultureIt);
            }

            var user = new User();
            var currentUser = this.GetCurrentUser();
            var prevUserId = -1;
            var nextUserId = -1;
            var userHourReports = new List<MonthlyReportHour>();
            var userExpenseReports = new List<MonthlyReportExpense>();
            var userNoteReports = new List<MonthlyReportNote>();
            var hourReasons = new List<HourReason>();
            var expenseReasons = new List<ExpenseReason>();
            var holidays = new List<Festivity>();
            var totalHours = 0d;
            var totalOrdinaryHours = 0d;
            var totalExpenses = 0d;
            var totalHolidays = 0d;
            var summaryHours = new Dictionary<string, string>();
            var retributionItems = new List<RetributionItem>();
            using (var context = new AgmDataContext())
            {
                user = context.Users.First(u => u.Id == id);
                var currentUserId = this.GetCurrentUser().Id;
                var users = (currentUserId != id) ? context.Users.Where(u => u.Id != currentUserId && u._isDeleted == false).OrderBy(u => u.LastName).ToList() : new List<User>();
                var currentUserIndex = users.IndexOf(user);

                prevUserId = (currentUser.SectionUsersVisible && currentUserIndex > 0) ? users[currentUserIndex - 1].Id : -1;
                nextUserId = (currentUser.SectionUsersVisible && currentUserIndex < (users.Count - 1)) ? users[currentUserIndex + 1].Id : -1;
                userHourReports = context.MonthlyReportHours.Where(r => r.UserId == id && r.Month == currentMonthDate.Month).ToList();
                userExpenseReports = context.MonthlyReportExpenses.Where(e => e.UserId == id && e.Month == currentMonthDate.Month).ToList();
                userNoteReports = context.MonthlyReportNotes.Where(e => e.UserId == id && e.Month == currentMonthDate.Month).ToList();
                hourReasons = context.HourReasons.Where(h => h.IsDeleted == false).ToList();
                expenseReasons = context.ExpenseReasons.ToList();
                holidays = context.Festivities.ToList();
                retributionItems = (currentUser.SectionUsersVisible /*&& currentUserIndex > 0*/)? context.RetributionItems.Where(r => r.UserId == id && r.Month == currentMonthStringCompact).ToList() : new List<RetributionItem>();
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
                    CompleteDate = currentDate,
                    DateShort = string.Format("{0} {1}", Char.ToUpper(currentDate.ToString("dddd", cultureIt)[0]), currentDate.ToString(" d", cultureIt)),
                    Date = string.Format("{0} {1}", currentDate.ToString("dddd", cultureIt), currentDate.ToString(" d", cultureIt)),
                    Hours = (hoursCollection.Any()) ? hoursCollection.Sum(r => r.HoursCount).ToString("N2", cultureIt) : 0.ToString("N2", cultureIt),
                    HoursCollection = hoursCollection,
                    Expenses = expensesCollection.Sum(e => e.GetTotalAmount()).ToString("N2", cultureIt),
                    ExpensesCollection = expensesCollection,
                    NotesCollection = notesCollection,
                    WorkDay = currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday && holidays.All(h => h.Date != currentDate),
                    IsHoliday = currentDate.DayOfWeek == DayOfWeek.Sunday || holidays.Any(h => h.Date == currentDate),
                    IsCurrentMonth = (currentDate.Month == currentMonthDate.Month),
                    DayOfWeek = (int)currentDate.DayOfWeek
                });

                if (currentDate.Month == currentMonthDate.Month)
                {
                    totalHours += hoursCollection.Sum(r => r.HoursCount);

                    if (hoursCollection.Any(r => r.ReasonId == 1))
                        totalOrdinaryHours += hoursCollection.Where(r => r.ReasonId == 1).Sum(r => r.HoursCount);

                    totalExpenses += expensesCollection.Sum(e => e.GetTotalAmount());

                    if (hoursCollection.Any(r => r.Reason == "ferie"))
                    {
                        totalHolidays += hoursCollection.Where(r => r.Reason == "ferie").Sum(r => r.HoursCount);
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            var summary = from t in userHourReports.Where(r => r.Date >= startDate && r.Date <= endDate && r.Date.Month == currentMonthDate.Month).ToList()
                          group t by t.Reason
                              into g
                              select new
                              {
                                  id = g.First().ReasonId,
                                  reason = g.Key,
                                  count = g.Sum(x => x.HoursCount).ToString("N2", cultureIt),
                                  days = (g.Sum(x => x.HoursCount) / 8).ToString("N2", cultureIt)
                              };

            if (currentUser.SectionUsersVisible)
            {
                foreach(var item in user.RetributionItemConfiguration)
                {
                    if (item.EnableValue == 1 && retributionItems.All(i => i.Type != item.Type))
                    {
                        var newRetributionItem = new RetributionItem();
                        newRetributionItem.Month = currentMonthStringCompact;
                        newRetributionItem.UserId = id;
                        newRetributionItem.Type = item.Type;
                        newRetributionItem.Qty = 0;
                        newRetributionItem.Amount = (item.Type == RetributionItemType.MealVoucher) ? 5.29 : 0.00;
                        newRetributionItem.Total = 0;
                        retributionItems.Add(newRetributionItem);
                    }
                }
            }

            return new ApiResponse()
            {
                Succeed = true,
                Data = new
                {
                    User = new
                    {
                        Id = id,
                        Name = user.Name
                    },
                    CurrentMonth = currentMonthString,
                    SelectedInsertDate = ((currentMonthDate.Month == DateTime.Today.Month && currentMonthDate.Year == DateTime.Today.Year) ? DateTime.Today : new DateTime(currentMonthDate.Year, currentMonthDate.Month, 1)).ToString("yyyy-MM-dd"),
                    MinMonthDate = (new DateTime(currentMonthDate.Year, currentMonthDate.Month, 1)).AddDays(-1).ToString("yyyy-MM-dd"),
                    MaxMonthDate = (new DateTime(currentMonthDate.Year, currentMonthDate.Month, DateTime.DaysInMonth(currentMonthDate.Year, currentMonthDate.Month))).ToString("yyyy-MM-dd"),
                    Report = report,
                    HourReasons = hourReasons,
                    ExpenseReasons = expenseReasons,
                    TotalHours = totalHours.ToString("N2", cultureIt),
                    TotalDays = (totalHours / 8).ToString("N2", cultureIt),
                    TotalOrdinaryHours = totalOrdinaryHours.ToString("N2", cultureIt),
                    TotalOrdinaryDays = (totalOrdinaryHours / 8).ToString("N2", cultureIt),
                    TotalExpenses = totalExpenses.ToString("N2", cultureIt),
                    TotalHolidays = totalHolidays.ToString("N2", cultureIt),
                    TotalHolidaysDays = (totalHolidays / 8).ToString("N2", cultureIt),
                    Summary = summary.OrderBy(s => s.id),
                    RetributionItems = retributionItems,
                    PrevUserId = prevUserId,
                    NextUserId = nextUserId
                }
            };
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Insert([FromBody]dynamic reportIn)
        {
            var userId = (int)reportIn.UserId;
            this.CheckCurrentUserPermission(userId, ((x) => x.SectionUsersVisible));
            var cultureIt = CultureInfo.GetCultureInfo("it-IT");

            using (var context = new AgmDataContext())
            {
                var res = new List<object>();
                if (reportIn.Hours != null && reportIn.Hours.HoursCount != null)
                {
                    var objHoursIn = context.MonthlyReportHours.Add(new MonthlyReportHour()
                    {
                        UserId = reportIn.UserId,
                        HoursRaw = ((double)reportIn.Hours.HoursCount).ToString("N2", cultureIt),
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
                        AmountRaw = ((double)reportIn.Expenses.Amount).ToString("N2", cultureIt),
                        ReasonId = reportIn.Expenses.ReasonId,
                        Day = ((DateTime) reportIn.Date).Day,
                        Month = ((DateTime) reportIn.Date).Month,
                        Year = ((DateTime) reportIn.Date).Year
                    });
                    res.Add(objExpensesIn);
                }

                if (reportIn.Note != null && !string.IsNullOrEmpty(reportIn.Note.ToString()))
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

        [AuthorizeAction]
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
                    this.CheckCurrentUserPermission(o.UserId, ((x) => x.SectionUsersVisible));
                    context.MonthlyReportHours.Remove(o);
                }
                else if (type == "Expense" && context.MonthlyReportExpenses.Any(h => h.Id == id))
                {
                    var o = context.MonthlyReportExpenses.First(h => h.Id == id);
                    this.CheckCurrentUserPermission(o.UserId, ((x) => x.SectionUsersVisible));
                    context.MonthlyReportExpenses.Remove(o);
                }
                else if (type == "Note" && context.MonthlyReportNotes.Any(h => h.Id == id))
                {
                    var o = context.MonthlyReportNotes.First(h => h.Id == id);
                    this.CheckCurrentUserPermission(o.UserId, ((x) => x.SectionUsersVisible));
                    context.MonthlyReportNotes.Remove(o);
                }
                
                context.SaveChanges();
                return new ApiResponse(true){ Data=type };
            }
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetRecurringNotes(int id, string value)
        {
            this.CheckCurrentUserPermission(id, ((x) => x.SectionUsersVisible));

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

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Autocomplete(dynamic objIn)
        {
            this.CheckCurrentUserPermission((int)objIn.id, ((x) => x.SectionUsersVisible));

            try
            {
                var userId = (int)objIn.id;
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                string month = objIn.month;

                var currentMonthDate = DateTime.Parse(month, cultureIt);
                var currentMonthString = currentMonthDate.ToString("yyyy-MM-dd", cultureIt);

                using (var context = new AgmDataContext())
                {
                    var user = context.Users.First(u => u.Id == userId);
                    if (user != null)
                    {
                        var userHourReports = context.MonthlyReportHours.Where(r => r.UserId == userId && r.Month == currentMonthDate.Month).ToList();
                        var hourReasons = context.HourReasons.ToList();
                        var holidays = context.Festivities.ToList();
                        var currentDate = new DateTime(currentMonthDate.Year, currentMonthDate.Month, 1);
                        var endDate = new DateTime(currentMonthDate.Year, currentMonthDate.Month, DateTime.DaysInMonth(currentMonthDate.Year, currentMonthDate.Month));
                        while (currentDate <= endDate)
                        {
                            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday && holidays.All(h => h.Date != currentDate) && !userHourReports.Any(r => r.Date == currentDate))
                            {
                                context.MonthlyReportHours.Add(new MonthlyReportHour() { UserId = user.Id, Day = currentDate.Day, Month = currentDate.Month, Year = currentDate.Year, HoursRaw = "8", ReasonId = hourReasons.First(r => r.Name == "ordinarie").Id });
                            }

                            currentDate = currentDate.AddDays(1);
                        }
                        context.SaveChanges();
                    }
                }

                return new ApiResponse(true);
            }
            catch(Exception e)
            {
                return new ApiResponse(false)
                {
                    Errors = (new List<ApiResponseError>() { new ApiResponseError() { Message = e.Message } }).ToArray()
                };

            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse UpdateRetributionItems(List<RetributionItem> objIn)
        {
            var userId = objIn[0].UserId;
            this.CheckCurrentUserPermission(userId, ((x) => x.SectionUsersVisible));

            try
            { 
                using (var db = new AgmDataContext())
                {
                    foreach (var item in objIn)
                    {
                        if (item.Total == 0.00 &&
                            db.RetributionItems.Any(
                                r => r.Month == item.Month && r.Type == item.Type && r.UserId == item.UserId))
                        {
                            db.RetributionItems.Remove(item);
                        }

                        if (item.Total != 0.00 &&
                            !db.RetributionItems.Any(
                                r => r.Month == item.Month && r.Type == item.Type && r.UserId == item.UserId))
                        {
                            db.RetributionItems.Add(item);
                        }

                        if (item.Total != 0.00 &&
                            db.RetributionItems.Any(
                                r => r.Month == item.Month && r.Type == item.Type && r.UserId == item.UserId))
                        {
                            var dbItem = db.RetributionItems.First(
                                r => r.Month == item.Month && r.Type == item.Type && r.UserId == item.UserId);
                            dbItem.Qty = item.Qty;
                            dbItem.Amount = item.Amount;
                            dbItem.Total = item.Total;
                        }

                        db.SaveChanges();
                    }
                }

                return new ApiResponse(true);
            }
            catch(Exception e)
            {
                return new ApiResponse(false)
                {
                    Errors = (new List<ApiResponseError>() { new ApiResponseError() { Message = e.Message } }).ToArray()
                };

            }
        }
    }
}