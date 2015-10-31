using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Http;
using AGM.Web.Infrastructure;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Models;

namespace AGM.Web.Controllers
{
    public class ExportController : ApiController
    {
        [AuthorizeAction]
        [HttpGet]
        public ApiResponse Get(string month)
        {
            this.CheckCurrentUserPermission(((x) => x.SectionUsersVisible));
            using (var db = new AgmDataContext())
            {
                if (db.Exports.All(e => e.Month != month))
                    return new ApiResponse(true) {Data = null };
                return new ApiResponse(true) {Data = db.Exports.First(e => e.Month == month)};
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Calculate([FromBody]string month)
        {
            this.CheckCurrentUserPermission(((x) => x.SectionUsersVisible));
            using (var db = new AgmDataContext())
            {
                var export = new Export();
                if (db.Exports.Any(e => e.Month == month))
                {
                    export = db.Exports.First(e => e.Month == month);
                }
                else
                {
                    export.Month = month;
                    db.Exports.Add(export);
                }

                var exportMH = ExportMH(month);
                export.MHFileName = exportMH.Key;
                export._hourReport = null;
                export.HourReport = exportMH.Value;
                var exportRI = ExportRI(month);
                export.RIFileName = exportRI.Key;
                export._retributionItems = null;
                export.RetributionItems = exportRI.Value;
                export.CalculateDate = DateTime.Now;
                export.UsersMax = db.Users.Count(u => !u._isDeleted && u._isActive == 1 && u.IdExport.HasValue);
                db.SaveChanges();

                return new ApiResponse(true) {
                    Data = new Export()
                    {
                        Month = export.Month,
                        CalculateDate = export.CalculateDate,
                        UsersCount = export.UsersCount,
                        UsersMax = export.UsersMax
                    }
                };
            }
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse StartExport()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionUsersVisible));
            using (var db = new AgmDataContext())
            {
                var tokenId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                db.Tokens.Add(new Token() {Id = tokenId, ExpirationDate = DateTime.Now.AddSeconds(30)});
                db.SaveChanges();
                return new ApiResponse(true) { Data = tokenId };
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExportMH(string tokenId, string month)
        {
            var guid = string.Empty;
            using (var db = new AgmDataContext())
            {
                if (db.Tokens.All(t => t.Id != tokenId || t.ExpirationDate < DateTime.Now) || db.Exports.All(e => e.Month != month))
                    return new HttpResponseMessage(HttpStatusCode.NoContent);

                if (db.Tokens.Any(t => t.Id == tokenId))
                {
                    db.Tokens.Remove(db.Tokens.First(t => t.Id == tokenId));
                }
                var expirationSearch = DateTime.Now.AddSeconds(30);
                if (db.Tokens.Any(t => t.ExpirationDate < expirationSearch))
                { 
                    db.Tokens.RemoveRange(db.Tokens.Where(t => t.ExpirationDate < expirationSearch));
                }

                db.SaveChanges();

                guid = db.Exports.First(e => e.Month == month).MHFileName;
            }
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath(string.Format("~/Exports/{0}", guid));

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(mappedPath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "export.txt"
            };
            return result;
        }

        [HttpGet]
        public HttpResponseMessage GetExportRI(string tokenId, string month)
        {
            var guid = string.Empty;
            using (var db = new AgmDataContext())
            {
                if (db.Tokens.All(t => t.Id != tokenId || t.ExpirationDate < DateTime.Now) || db.Exports.All(e => e.Month != month))
                    return new HttpResponseMessage(HttpStatusCode.NoContent);

                if (db.Tokens.Any(t => t.Id == tokenId))
                {
                    db.Tokens.Remove(db.Tokens.First(t => t.Id == tokenId));
                }
                var expirationSearch = DateTime.Now.AddSeconds(30);
                if (db.Tokens.Any(t => t.ExpirationDate < expirationSearch))
                {
                    db.Tokens.RemoveRange(db.Tokens.Where(t => t.ExpirationDate < expirationSearch));
                }

                db.SaveChanges();

                guid = db.Exports.First(e => e.Month == month).RIFileName;
            }
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath(string.Format("~/Exports/{0}", guid));

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(mappedPath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "voci.txt"
            };
            return result;
        }

        private KeyValuePair<string, Dictionary<int, Dictionary<string, double>>> ExportMH(string month)
        {
            var reasonCode = new Dictionary<string, string>()
            {
                {"ordinarie", "   "},
                {"ferie", "FP"},
                {"r.o.l.", "P1"},
                {"straordinarie (solo se approvate)", "S1"},
                {"malattia", "M1"},
                {"infortunio", "I1"},
                {"donazione sangue", "DS"},
                {"congedo matrimoniale", "CM"},
                {"D.Lgs. 151", "M8 "},
                {"Permessi ex-festività", "P2"}
            };
            var mhReport = new Dictionary<int, Dictionary<string, double>>();

            Thread.CurrentPrincipal = new CustomPrincipal("nandowalter@gmail.com$Fernando Walter Gagni");
            List<string> res = new List<string>();
            using (var context = new AgmDataContext())
            {
                var users = context.Users.OrderBy(u => u.LastName).ToList();
                foreach (var user in users.Where(u => !u.IsDeleted && u.IsActive && u.IdExport.HasValue))
                {
                    var userMhReport = new Dictionary<string, double>();
                    dynamic completeReport = (new MonthlyReportController()).ExtractMonthlyReport(user.Id, $"{new string(month.Take(4).ToArray())}-{new string(month.Skip(4).ToArray())}");
                    var hourReport = completeReport.Data.Report;
                    var userName = user.Name;
                    foreach (var itemParent in hourReport)
                    {
                        if (itemParent.CompleteDate.ToString("MM") == new string(month.Skip(4).ToArray()))
                        {
                            if ((itemParent.HoursCollection as IEnumerable<MonthlyReportHour>).Any())
                            {
                                foreach (var item in itemParent.HoursCollection)
                                {
                                    var hours = ((int)Math.Truncate((item as MonthlyReportHour).HoursCount * 100)).ToString();
                                    var reason = (item as MonthlyReportHour).Reason;
                                    var reasonCurr = (reasonCode.Any(r => r.Key == reason) ? reasonCode[reason] : reason);
                                    res.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                        "00000",
                                        "00",
                                        user.IdExport.ToString().PadLeft(4, '0'),
                                        item.Date.ToString("ddMMyy"),
                                        reasonCurr.PadRight(3, ' '),
                                        hours,
                                        (reasonCurr == "   " || reasonCurr == "S1") ? hours : "0000",
                                        "0",
                                        "0"));

                                    if (!userMhReport.ContainsKey(reason))
                                    {
                                        userMhReport.Add(reason, 0.00);
                                    }
                                    userMhReport[reason] += (item as MonthlyReportHour).HoursCount;
                                }
                            }
                            else
                            {
                                res.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", "00000", "00",
                                    user.IdExport.ToString().PadLeft(4, '0'),
                                    itemParent.CompleteDate.ToString("ddMMyy"), "   ", "0000", "0000",
                                    (itemParent.WorkDay) ? "0" : (!itemParent.IsHoliday) ? "1" : "2", "0"));
                            }
                        }
                    }
                    mhReport.Add(user.Id, userMhReport);
                }
            }

            Guid newGuid = Guid.NewGuid();
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath(string.Format("~/Exports/{0}", newGuid));
            using (FileStream f = new FileStream(mappedPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(f))
                {
                    foreach (var item in res)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Flush();
                }
            }

            return new KeyValuePair<string, Dictionary<int, Dictionary<string, double>>>(newGuid.ToString(), mhReport);
        }

        private KeyValuePair<string, Dictionary<int, Dictionary<RetributionItemType, RetributionItem>>>  ExportRI(string month)
        {
            var exportCode = new Dictionary<int, string>()
            {
                {0, "020"},
                {1, "805"},
                {2, "100"},
                {3, "102"},
                {4, "104"},
                {5, "101"},
                {6, "103"},
                {7, "105"},
                {8, "290"},
                {9, "004"},
                {10, "001"},
                {11, "002"}
            };

            var res = new List<string>();
            var retItemsReport = new Dictionary<int, Dictionary<RetributionItemType, RetributionItem>>();
            using (var context = new AgmDataContext())
            {
                var users = context.Users.OrderBy(u => u.LastName).ToList();
                foreach (var user in users.Where(u => !u.IsDeleted && u.IsActive && u.IdExport.HasValue))
                {
                    var userRetItems = new Dictionary<RetributionItemType, RetributionItem>();
                    var retItems = context.RetributionItems.Where(r => r.UserId == user.Id && r.Month == month);
                    if (retItems.Any())
                    {
                        foreach (var item in retItems)
                        {
                            string pattern = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}";
                            res.Add(string.Format(pattern, "00000", "00", user.IdExport.Value.ToString().PadLeft(4, '0'),
                                "    ",
                                exportCode[(int)item.Type], "                              ",
                                (item.Qty * 1000).ToString().PadLeft(7, '0'),
                                ((int)(item.Amount * 100000)).ToString().PadLeft(11, '0'),
                                ((int)(item.Total * 100)).ToString().PadLeft(9, '0'), new string(month.Take(4).ToArray()), new string(month.Skip(4).ToArray()), "0"));

                            userRetItems.Add(item.Type, item);
                        }
                    }
                    retItemsReport.Add(user.Id, userRetItems);
                }
            }

            Guid newGuid = Guid.NewGuid();
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath(string.Format("~/Exports/{0}", newGuid));
            using (FileStream f = new FileStream(mappedPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(f))
                {
                    foreach (var item in res)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Flush();
                }
            }

            return new KeyValuePair<string, Dictionary<int, Dictionary<RetributionItemType, RetributionItem>>>(newGuid.ToString(), retItemsReport);
        }
    }
}
