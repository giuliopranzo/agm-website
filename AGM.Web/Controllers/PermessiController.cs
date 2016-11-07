using System.Configuration;
using AGM.Web.Infrastructure;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using User = AGM.Web.Models.User;

namespace AGM.Web.Controllers
{
    public class PermessiController : ApiController
    {
        [HttpGet]
        public ApiResponse DatabaseCheck()
        {
            var basePath = "~/DbUpdates/";
            var res = "";
            var path = HttpContext.Current.Server.MapPath(basePath);
            var exceptionText = string.Empty;
            
            if (!Directory.Exists(path))
                return new ApiResponse(false)
                {
                    Errors = new ApiResponseError[]{ new ApiResponseError(){ Message = "Path for db updates doesn't exists!" }}  //Directory.GetFiles(path, "AGM_???.sql", SearchOption.TopDirectoryOnly).ToArray()
                };

            var updateFiles = Directory.GetFiles(path, "AGM_???.sql", SearchOption.TopDirectoryOnly);
            var targetVersion = 0;
            var currentVersion = new Models.Version() { Code = "0", LastUpdateTryDate = SqlDateTime.MinValue.Value, UpdateDate = SqlDateTime.MinValue.Value };
            
            if (updateFiles.Any())
            {
                targetVersion = updateFiles.Select(f => int.Parse(Path.GetFileNameWithoutExtension(f).Substring(4))).Max();
            }

            using (var context = new AgmDataContext())
            {
                if (context.Versions.Any())
                {
                    currentVersion = context.Versions.ToList().OrderBy(v => int.Parse(v.Code)).Last();
                }
                else
                {
                    context.Versions.Add(currentVersion);
                }

                for (var v = int.Parse(currentVersion.Code) + 1; v <= targetVersion; v++)
                {
                    currentVersion.LastUpdateTryDate = DateTime.Now;
                    try
                    {
                        var filename = string.Format("{0}AGM_{1}.sql", basePath, v.ToString().PadLeft(3, '0'));
                        var completePath = HttpContext.Current.Server.MapPath(filename);
                        if (File.Exists(completePath))
                        {
                            var sqlText = string.Empty;
                            using (StreamReader sr = new StreamReader(completePath))
                            {
                                sqlText = sr.ReadToEnd();
                            }

                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AgmDataContext"].ConnectionString))
                            {
                                conn.Open();
                                SqlCommand command = new SqlCommand(sqlText, conn);
                                res = command.ExecuteNonQuery().ToString();
                            }

                            var newVersion = new Models.Version();
                            newVersion.Code = v.ToString();
                            newVersion.LastUpdateTryDate = currentVersion.LastUpdateTryDate;
                            newVersion.UpdateDate = DateTime.Now;
                            newVersion.UpdateSucceeded = true;

                            context.Versions.Add(newVersion);
                            context.Versions.Remove(currentVersion);
                        }
                    }
                    catch (Exception e)
                    {
                        currentVersion.UpdateSucceeded = false;
                        exceptionText = e.Message;
                    }
                    finally
                    {
                        context.SaveChanges();
                    }
                }


                return new ApiResponse(true)
                {
                    Data = context.Versions.ToList().OrderBy(v => int.Parse(v.Code)).Last(),
                    Errors = (string.IsNullOrEmpty(exceptionText)) ? null : new ApiResponseError[]{ new ApiResponseError(){ Message = exceptionText } }
                };
            }
        }

        [HttpGet]
        public ApiResponse TableDefs()
        {
            DataSet dsSchemaExport = new DataSet();
            Dictionary<string, object> defs = new Dictionary<string, object>();
            List<string> cols = new List<string>();
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AgmDataContext"].ConnectionString);
            try
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand command =
                    new System.Data.SqlClient.SqlCommand("select * from utenti where idutente=21", conn);
                System.Data.SqlClient.SqlDataReader sqlreader = command.ExecuteReader();
                var schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "utenti"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("utenti", cols);
                sqlreader.Read();
                object[] resUtenti = new object[cols.Count];
                sqlreader.GetValues(resUtenti);
                defs.Add("utenti example", JsonConvert.SerializeObject(resUtenti));
                sqlreader.Close();

                command = new System.Data.SqlClient.SqlCommand("UPDATE utenti set utente=email where utente is NULL;", conn);
                var resupd = command.ExecuteNonQuery();
                defs.Add("update result1", resupd);

                //command = new System.Data.SqlClient.SqlCommand("insert into rappcausali (idcausale,nome) values (10,'Permessi ex-festività')", conn);
                //var resAlter = command.ExecuteNonQuery();
                //defs.Add("update result1", resAlter);

                //command = new System.Data.SqlClient.SqlCommand("ALTER TABLE utenti ADD isDeleted bit DEFAULT 0 NOT NULL", conn);
                //var resAlter = command.ExecuteNonQuery();
                //defs.Add("update result1", resAlter);

                //command = new System.Data.SqlClient.SqlCommand("update utenti set utenti=1 where idutente=21", conn);
                //var resAlter = command.ExecuteNonQuery();
                //defs.Add("update result1", resAlter);

                //command = new System.Data.SqlClient.SqlCommand("update utenti set email='nandowalter@gmail.com' where idutente=21", conn);
                //var resAlter = command.ExecuteNonQuery();
                //defs.Add("update result1", resAlter);

                //command = new System.Data.SqlClient.SqlCommand("update utenti set email='davide.cavalli@agmsolutions.net' where idutente=4", conn);
                //resAlter = command.ExecuteNonQuery();
                //defs.Add("update result2", resAlter);

                //command = new System.Data.SqlClient.SqlCommand("update utenti set email='michela.merlo@agmsolutions.net' where idutente=3", conn);
                //resAlter = command.ExecuteNonQuery();
                //defs.Add("update result3", resAlter);

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select TOP 1 * from rappore where idutente=38", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappore"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappore", cols);
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select * from rappcausali", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappcausali"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappcausali", cols);
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select * from rappcausalispese", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappcausalispese"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappcausalispese", cols);
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select TOP 1 * from rappspese where idutente=38", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappspese"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappspese", cols);
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select TOP 1 * from rappdescrizioni where idutente=38", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappdescrizioni"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappdescrizioni", cols);
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select TOP 1 * from rappfestivi", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappfestivi"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappfestivi", cols);
                sqlreader.Read();
                defs.Add("rappfestivi example", sqlreader[1].ToString());
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select TOP 1 * from annunci", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "annunci"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} [{1}({2})] - {3}", col["ColumnName"], col["DataTypeName"], col["ColumnSize"], col["IsIdentity"]));
                }
                defs.Add("annunci", cols);
                sqlreader.Read();
                defs.Add("annunci example", sqlreader[4].ToString());
                sqlreader.Close();

                cols = new List<string>();
                command = new System.Data.SqlClient.SqlCommand("select TOP 1 * from rappvociretributive", conn);
                sqlreader = command.ExecuteReader();
                schemaTable = sqlreader.GetSchemaTable();
                dsSchemaExport.Tables.Add(Add(conn, "rappvociretributive"));
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} [{1}({2})] - {3}", col["ColumnName"], col["DataTypeName"], col["ColumnSize"], col["IsIdentity"]));
                }
                defs.Add("rappvociretributive", cols);
                sqlreader.Close();


                dsSchemaExport.WriteXml(HttpContext.Current.Server.MapPath("~/App_Data/tables.xml"), XmlWriteMode.WriteSchema);
                dsSchemaExport.WriteXmlSchema(HttpContext.Current.Server.MapPath("~/App_Data/tables_schema.xsd"));

                conn.Close();

            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                return new ApiResponse(false) { Errors = new ApiResponseError[] { new ApiResponseError() { Message = e.Message } } };
            }

            return new ApiResponse(true)
            {
                Data = defs
            };
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse Export(string year, string month)
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
                {"D.Lgs. 151", "AL "},
                {"Permessi ex-festività", "P2"}
            };
            
            Thread.CurrentPrincipal = new CustomPrincipal("nandowalter@gmail.com$Fernando Walter Gagni");
            List<string> res = new List<string>();
            using (var context = new AgmDataContext())
            {
                var users = context.Users.ToList();
                foreach (var user in users.Where(u => !u.IsDeleted && u.IsActive && u.IdExport.HasValue))
                {
                    dynamic completeReport = (new MonthlyReportController()).ExtractMonthlyReport(user.Id, string.Format("{0}-{1}", year, month.PadLeft(2, '0')));
                    var hourReport = completeReport.Data.Report;
                    var userName = user.Name;
                    foreach (var itemParent in hourReport)
                    {
                        if (itemParent.CompleteDate.ToString("MM") == month.PadLeft(2,'0'))
                        {
                            if ((itemParent.HoursCollection as IEnumerable<MonthlyReportHour>).Any())
                            {
                                foreach (var item in itemParent.HoursCollection)
                                {
                                    TimeSpan timespan = TimeSpan.FromHours(2.75);
                                    var hours = TimeSpan.FromHours((item as MonthlyReportHour).HoursCount).ToString("hhmm");
                                    var reason = (item as MonthlyReportHour).Reason;
                                    var reasonCurr = (reasonCode.Any(r => r.Key == reason)? reasonCode[reason] : reason);
                                    res.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", 
                                        "00000", 
                                        "00",
                                        user.IdExport.ToString().PadLeft(4,'0'),
                                        item.Date.ToString("ddMMyy"),
                                        reasonCurr.PadRight(3, ' '),
                                        hours, 
                                        (reasonCurr == "   " || reasonCurr == "S1") ? hours : "0000", 
                                        "0", 
                                        "0"));
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

            return new ApiResponse(true)
            {
                Data = newGuid.ToString()
            };
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse ExportRI(string year, string month)
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
            };

            var res = new List<string>();
            string monthSearch = year + month;
            using (var context = new AgmDataContext())
            {
                var users = context.Users.ToList();
                foreach (var user in users.Where(u => !u.IsDeleted && u.IsActive && u.IdExport.HasValue))
                {
                    var retItems = context.RetributionItems.Where(r => r.UserId == user.Id && r.Month == monthSearch);
                    if (retItems.Any())
                    {
                        foreach (var item in retItems)
                        {
                            string pattern = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}";
                            res.Add(string.Format(pattern, "00000", "00", user.IdExport.Value.ToString().PadLeft(4, '0'),
                                "    ",
                                exportCode[(int) item.Type], "                              ",
                                (item.Qty*1000).ToString().PadLeft(7, '0'),
                                ((int) (item.Amount*100000)).ToString().PadLeft(11, '0'),
                                ((int) (item.Total*100)).ToString().PadLeft(9, '0'), year.Substring(2), month, "0"));
                        }
                    }
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

            return new ApiResponse(true)
            {
                Data = newGuid.ToString()
            };
        }


        private DataTable Add(SqlConnection cnn, string tablename)
        {
            DataTable dt = new DataTable();
            dt.TableName = tablename;
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM[" + tablename +"];", cnn))
            {
                da.FillSchema(dt, SchemaType.Source);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                da.Fill(dt);
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.AutoIncrement)
                    {
                        col.AutoIncrementSeed = 1;
                        col.AutoIncrementStep = 1;
                    }
                }
            }
            return dt;
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetAll()
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);
                var currentMonth = string.Format("{0}{1}", DateTime.Today.Year.ToString(), DateTime.Today.Month.ToString().PadLeft(2, '0'));
                var mhReportLocks = context.MHReportLocks.Where(l => l.Month == currentMonth).ToList();
                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                var users = context.Users.Where(u => u.Email != email && !u._isDeleted).OrderBy(u => u.LastName).ToList(); 
                return new ApiResponse(true)
                {
                    Data = users.Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u._isActive,
                        u.Image,
                        u.Username,
                        u.IdExport,
                        currentMHReportLocked = mhReportLocks.Any(l => l.UserId == u.Id)
                    })
                };
            }
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetDetail(int id)
        {
            this.CheckCurrentUserPermission(id, ((x) => x.SectionUsersVisible));

            using (var context = new AgmDataContext())
            {
                var currentUser = this.GetCurrentUser();

                var user = context.Users.FirstOrDefault(u => u.Id == id && !u._isDeleted);
                if (user == null && id == 0)
                    user = new User();

                if (!currentUser.SectionUsersVisible)
                    user.IdExport = -1;

                return new ApiResponse(true)
                {
                    Data = user
                };
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Set(User user)
        {
            this.CheckCurrentUserPermission(user.Id, ((x) => x.SectionUsersVisible || x.IsAdmin));
            var currentUser = this.GetCurrentUser();

            using (var context = new AgmDataContext())
            {
                if (user._image.Contains("/Temp"))
                {
                    File.Move(HttpContext.Current.Server.MapPath(user._image), HttpContext.Current.Server.MapPath(user._image.Replace("/Temp", string.Empty)));
                    user.Image = user._image.Replace("/Temp", string.Empty);
                }

                if (user.IdExport.HasValue && context.Users.Any(u => u.IdExport == user.IdExport && u.Id != user.Id && !u._isDeleted))
                {
                    var suggestedId = (context.Users.Any(u => u.IdExport != null && !u._isDeleted)) ? context.Users.Where(u => u.IdExport != null && !u._isDeleted).Max(u => u.IdExport).Value + 1 : 1;
                    return new ApiResponse(false)
                    {
                        Errors = new List<ApiResponseError>() { new ApiResponseError() { Code = -2, Message = string.Format("ID Export già utilizzato. ID Export suggerito:{0}", suggestedId) } }.ToArray()
                    };
                }

                if (user.Id != 0 && context.Users.Any(u => u.Id == user.Id && !u._isDeleted))
                {
                    context.Users.Attach(user);
                    ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);

                    if (!currentUser.SectionUsersVisible && !currentUser.IsAdmin)
                    {
                        context.Entry(user).Property(x => x.IdExport).IsModified = false;
                        context.Entry(user).Property(x => x._isActive).IsModified = false;
                        context.Entry(user).Property(x => x._sectionMonthlyReportsVisible).IsModified = false;
                    }

                    if (!currentUser.IsAdmin)
                    {
                        context.Entry(user).Property(x => x._sectionJobAdsVisible).IsModified = false;
                        context.Entry(user).Property(x => x._sectionJobApplicantsVisible).IsModified = false;
                        context.Entry(user).Property(x => x._sectionUsersVisible).IsModified = false;
                        context.Entry(user).Property(x => x._sectionExportVisible).IsModified = false;
                        context.Entry(user).Property(x => x._canSendMessage).IsModified = false;
                        context.Entry(user).Property(x => x.RetributionItemConfSerialized).IsModified = false;
                    }
                }
                else
                {
                    if (context.Users.Any(u => u.Email.ToLower() == user.Email.ToLower() && !u._isDeleted))
                        return new ApiResponse(false)
                        {
                            Errors = new List<ApiResponseError>(){ new ApiResponseError() {Code = -1, Message = "Utente già esistente"}}.ToArray()
                        };

                    if (user.Id != 0)
                        user.Id = 0;
                    var insNewUser = context.Users.Add(user);
                    insNewUser._image = null;
                }

                var res = context.SaveChanges();

                if (res > 0)
                    return new ApiResponse(true);

                return new ApiResponse(false);
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Delete(dynamic inId)
        {
            int id = (int) inId;
            this.CheckCurrentUserPermission(id, ((x) => x.SectionUsersVisible));

            using (var context = new AgmDataContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return new ApiResponse(false);

                user._isDeleted = true;
                var res = context.SaveChanges();

                if (res > 0)
                    return new ApiResponse(true);

                return new ApiResponse(false);
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public async Task<object> UploadAvatarImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string serverImgBaseUrl = "/images/Avatars/Temp";
            var provider = new MultipartFormDataStreamProvider(HttpContext.Current.Server.MapPath(serverImgBaseUrl));
            List<object> files = new List<object>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    if (file.Headers.ContentType.MediaType.StartsWith("image/"))
                    {
                        string originalFilename = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                        string filename = string.Format("{0}{1}", file.LocalFileName, Path.GetExtension(originalFilename));
                        File.Move(file.LocalFileName, filename);
                        files.Add(new
                        {
                            Index = provider.FormData["index"],
                            File = new
                            {
                                ImageUrl = string.Format("{0}/{1}", serverImgBaseUrl, Path.GetFileName(filename))
                            }
                        });
                    }
                }


                var response = new ApiResponse(true)
                {
                    Data = new
                    {
                        files = files
                    }
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [AuthorizeAction]
        public ApiResponse UserExists(string email)
        {
            using (var context = new AgmDataContext())
            {
                 return new ApiResponse(true){ Data = context.Users.Any(u => u.Email.ToLower().Equals(email.ToLower()))};
            }
        }

        [HttpGet]
        [AuthorizeAction]
        public ApiResponse GetMessages()
        {
            var userId = this.GetCurrentUser().Id;
            using (var context = new AgmDataContext())
            {
                var users = context.Users.ToList();
                var res = context.MessageReceivers.Where(r => r.ToUserId == userId && !r.IsDeleted).Include("Message").OrderByDescending(r => r.Message.InsertDate).ToList();
                res.ForEach(i => i.Message.Sender = users.Find(u => u.Id == i.Message.FromUserId).Name);
                return new ApiResponse(true) { Data = res };
            }
        }

        [HttpGet]
        [AuthorizeAction]
        public ApiResponse GetSentMessages()
        {
            if (!this.GetCurrentUser().CanSendMessage || !this.GetCurrentUser().IsAdmin)
                return new ApiResponse(true) { Data = null };

            var userId = this.GetCurrentUser().Id;
            using (var context = new AgmDataContext())
            {
                var res = context.Messages.Where(m => m.FromUserId == userId && !m.IsDeleted).OrderByDescending(m => m.InsertDate).ToList();
                res.ForEach(i => i.ReceiverIds = context.MessageReceivers.Where(m => m.MessageId == i.Id).Select(m => m.ToUserId).ToList());
                res.ForEach(i => i.Receivers = string.Join(",", context.Users.Where(u => i.ReceiverIds.Contains(u.Id)).OrderBy(u => u.LastName).Select(u => u.LastName + " " + u.FirstName).ToList()));
                return new ApiResponse(true) { Data = context.Messages.Where(m => m.FromUserId == userId && !m.IsDeleted).OrderByDescending(m => m.InsertDate).ToList() };
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public ApiResponse SetMessage([FromBody]MessageIn msgIn)
        {
            this.CheckCurrentUserPermission((x) => x.CanSendMessage || x.IsAdmin);
            var userId = this.GetCurrentUser().Id;
            using (var context = new AgmDataContext())
            {
                var msgToAdd = new Message()
                {
                    InsertDate = DateTime.Now,
                    Subject = msgIn.Subject,
                    Text = msgIn.Text,
                    FromUserId = userId
                };
                var msg = context.Messages.Add(msgToAdd);

                var messageReceivers = new List<MessageReceiver>();
                if (msgIn.SendToAll == 1)
                {
                    context.Users.Where(u => !u._isDeleted && u.Id != userId).ToList().ForEach((u) => messageReceivers.Add(new MessageReceiver() { MessageId = msgToAdd.Id, ToUserId = u.Id }));
                }
                else
                {
                    msgIn.ToUserIds.ToList().ForEach(u => messageReceivers.Add(new MessageReceiver() { MessageId = msgToAdd.Id, ToUserId = u }));
                }
                context.MessageReceivers.AddRange(messageReceivers);

                var res = context.SaveChanges();

                if (res > 0)
                    return new ApiResponse(true) { Data = msg };

                return new ApiResponse(false);
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public ApiResponse DeleteMessage(dynamic idIn)
        {
            this.CheckCurrentUserPermission((x) => x.CanSendMessage || x.IsAdmin);
            int id = (int)idIn;
            var userId = this.GetCurrentUser().Id;
            using (var context = new AgmDataContext())
            {
                var messageReceivers = context.MessageReceivers.Where(i => i.MessageId == id && !i.IsDeleted);
                if (messageReceivers != null && messageReceivers.Count() > 0)
                {
                    messageReceivers.ToList().ForEach(m => m.IsDeleted = true);
                }

                var message = context.Messages.FirstOrDefault(i => i.Id == id);
                if (message == null)
                    return new ApiResponse(false);

                message.IsDeleted = true;
                var res = context.SaveChanges();

                if (res > 0)
                    return new ApiResponse(true);

                return new ApiResponse(false);
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public ApiResponse DeleteSentMessage(dynamic idIn)
        {
            this.CheckCurrentUserPermission((x) => x.CanSendMessage || x.IsAdmin);
            int id = (int)idIn;
            var userId = this.GetCurrentUser().Id;
            using (var context = new AgmDataContext())
            {
                var messageReceiver = context.MessageReceivers.FirstOrDefault(i => i.Id == id && !i.IsDeleted);
                if (messageReceiver == null )
                    return new ApiResponse(false);

                messageReceiver.IsDeleted = true;
                var res = context.SaveChanges();

                if (res > 0)
                    return new ApiResponse(true);

                return new ApiResponse(false);
            }
        }
    }
}