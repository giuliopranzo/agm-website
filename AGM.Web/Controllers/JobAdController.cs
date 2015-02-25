using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Models;

namespace AGM.Web.Controllers
{
    public class JobAdController: ApiController
    {
        [AuthorizeAction]
        [HttpGet]
        public ApiResponse Get()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));

            using (var context = new AgmDataContext())
            {
                return new ApiResponse(true)
                {
                    Data = context.JobAds.ToList()
                };
            }
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetText(int id)
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));
            var completePath = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}.txt", id.ToString()));
            if (!File.Exists(completePath))
                return new ApiResponse(false);

            using (StreamReader sr = new StreamReader(new FileStream(completePath, FileMode.Open)))
            {
                string text = sr.ReadToEnd();

                return new ApiResponse(true) {Data = text};
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Set(JobAdSaveIn objToSave)
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));
            var jobAd = objToSave.JobAd;
            using (var context = new AgmDataContext())
            {
                context.JobAds.Attach(jobAd);
                ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.ChangeObjectState(jobAd, EntityState.Modified);
                context.SaveChanges();
            }

            var completePath = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}.txt", jobAd.Id.ToString()));
            using (StreamWriter sw = new StreamWriter(new FileStream(completePath, FileMode.Create)))
            {
                sw.Write(objToSave.JobAdText);
                sw.Flush();
            }

            return new ApiResponse(true);
        }
    }
}