using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AGM.Web.Infrastructure;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using System.Threading;

namespace AGM.Web.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        public ApiResponse TableDefs()
        {
            Dictionary<string, object> defs = new Dictionary<string, object>();
            List<string> cols = new List<string>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("Data Source=hostingmssql02;Initial Catalog=agmsolutions_net_site;Integrated Security=False;User Id=agmsolutions_net_user;Password=C0nsu1t:v0_A:rD070m:TiSOCA_;MultipleActiveResultSets=True");
            try
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand command =
                    new System.Data.SqlClient.SqlCommand("select * from utenti where idutente=21", conn);
                System.Data.SqlClient.SqlDataReader sqlreader = command.ExecuteReader();
                var schemaTable = sqlreader.GetSchemaTable();
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

                //command = new System.Data.SqlClient.SqlCommand("ALTER TABLE utenti ADD image VARCHAR(250)", conn);
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
                foreach (System.Data.DataRow col in schemaTable.Rows)
                {
                    cols.Add(string.Format("{0} - {1}", col["ColumnName"], col["IsIdentity"]));
                }
                defs.Add("rappfestivi", cols);
                sqlreader.Read();
                defs.Add("rappfestivi example", sqlreader[1].ToString());
                sqlreader.Close();

                conn.Close();

            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                return new ApiResponse(false) { Errors = new ApiResponseError[]{ new ApiResponseError(){ Message = e.Message}}};
            }

            return new ApiResponse(true)
            {
                Data = defs
            };
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetAll()
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);

                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                var users = context.Users.Where(u => u.Email != email && !u._isDeleted).OrderBy(u => u.LastName).ToList(); 
                return new ApiResponse(true)
                {
                    Data = users.Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u.Image,
                        u.Username
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

                var user = context.Users.FirstOrDefault(u => u.Id == id && !u._isDeleted);
                if (user == null && id == 0)
                    user = new User();
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
            this.CheckCurrentUserPermission(user.Id, ((x) => x.SectionUsersVisible));

            using (var context = new AgmDataContext())
            {
                if (user._image.Contains("/Temp"))
                {
                    File.Move(HttpContext.Current.Server.MapPath(user._image), HttpContext.Current.Server.MapPath(user._image.Replace("/Temp", string.Empty)));
                    user.Image = user._image.Replace("/Temp", string.Empty);
                }

                if (user.Id != 0 && context.Users.Any(u => u.Id == user.Id && !u._isDeleted))
                {
                    context.Users.Attach(user);
                    ((IObjectContextAdapter) context).ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                }
                else
                {
                    if (user.Id != 0)
                        user.Id = 0;
                    context.Users.Add(user);
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

    }
}