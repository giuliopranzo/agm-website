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

                var users = context.Users.Where(u => u.Email != email).OrderBy(u => u.LastName).ToList(); 
                return new ApiResponse(true)
                {
                    Data = users.Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u.Picture,
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
                var user = context.Users.First(u => u.Id == id);
                return new ApiResponse(true)
                {
                    Data = user
                };
            }
        }
    }
}