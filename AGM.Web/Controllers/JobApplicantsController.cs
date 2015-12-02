using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AGM.Web.Controllers
{
    public class JobApplicantController : ApiController
    {
        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse Get()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));

            using (var context = new AgmDataContext())
            {
                var res = context.JobApplicants.Include("JobCategory").Include("Status").Include("StatusReason").ToList();
                return new ApiResponse(true)
                {
                    Data = res
                };
            }
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetStatus()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobAdsVisible));

            using (var context = new AgmDataContext())
            {
                var resStatus = context.JobApplicantStatuses.Select(s => new { id = s.Id, name = s.Name, type = "status"  }).ToList();
                var resStatusReason = context.JobApplicantStatusReasons.Select(s => new { id = s.Id, name = s.Name, type = "statusReason" }).ToList();
                return new ApiResponse(true)
                {
                    Data = resStatus.Union(resStatusReason).OrderBy(s => s.name)
                };
            }
        }
    }
}