using AGM.Web.Infrastructure;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

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
                sqlreader.Close();

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

        [HttpGet]
        public ApiResponse GetAll()
        {
            if (ConfigurationHelper.UseMockupData)
            {
                using (var re = new StreamReader(HttpContext.Current.Server.MapPath("~/App/Mockup/users.js")))
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

            using (var context = new AgmDataContext())
            {
                var users = context.Users.OrderBy(u => u.LastName).ToList(); 
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
    }
}