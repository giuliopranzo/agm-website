using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Infrastructure.Helpers;
using AGM.Web.Models;

namespace AGM.Web.Controllers
{
    public class JobAdController: ApiController
    {
        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse Get()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));

            using (var context = new AgmDataContext())
            {
                var res = context.JobAds.ToList().OrderByDescending(j => j.DateFrom).ToList();
                res.Add(new JobAd(){ DateFrom = DateTime.Today, DateTo = DateTime.Today});
                return new ApiResponse(true)
                {
                    Data = res
                };
            }
        }

        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse GetText(int id)
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));
            var completePath = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}.txt", id.ToString()));
            if (!File.Exists(completePath))
                return new ApiResponse(false);

            using (StreamReader sr = new StreamReader(new FileStream(completePath, FileMode.Open), Encoding.GetEncoding(1252)))
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
                if (jobAd.Id == 0 || !context.JobAds.Any(j => j.Id == jobAd.Id))
                {
                    context.JobAds.Add(jobAd);
                }
                else
                { 
                    context.JobAds.Attach(jobAd);
                    ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.ChangeObjectState(jobAd, EntityState.Modified);
                }
                context.SaveChanges();
            }

            var completePath = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}.txt", jobAd.Id.ToString()));

            if (File.Exists(completePath))
            {
                var newName = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}_{1}.txt", jobAd.Id.ToString(), DateTime.Now.ToFileTimeUtc().ToString()));
                File.Move(completePath, newName);
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(completePath, FileMode.Create), Encoding.GetEncoding(1252)))
            {
                sw.Write(objToSave.JobAdText);
                sw.Flush();
            }

            return new ApiResponse(true);
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Delete(List<JobAd> objCollectionToDelete)
        {
            using (var context = new AgmDataContext())
            {
                foreach (var item in objCollectionToDelete)
                {
                    if (context.JobAds.Any(j => j.Id == item.Id))
                    {
                        context.JobAds.Remove(context.JobAds.First(j => j.Id == item.Id));
                    }

                    var completePath = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}.txt", item.Id.ToString()));
                    if (File.Exists(completePath))
                    {
                        var newName = HttpContext.Current.Server.MapPath(string.Format("../../../annunci/{0}_{1}.txt", item.Id.ToString(), DateTime.Now.ToFileTimeUtc().ToString()));
                        File.Move(completePath, newName);
                    }
                }

                context.SaveChanges();
            }

            return new ApiResponse(true);
        }
    }
}