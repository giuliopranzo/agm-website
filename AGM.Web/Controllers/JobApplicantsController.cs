using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Infrastructure.Extensions;
using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));

            using (var context = new AgmDataContext())
            {
                var res = context.JobApplicants.Include("JobCategory").Include("Status").Include("StatusReason").Include("User").ToList().OrderByDescending(a => a.InterviewDate);
                return new ApiResponse(true)
                {
                    Data = res
                };
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse Set(JobApplicant objToSave)
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));
            objToSave.JobCategory = null;
            objToSave.Status = null;
            objToSave.StatusReason = null;
            objToSave.User = null;

            using (var context = new AgmDataContext())
            {
                var user = context.Users.First(u => u.Id == objToSave.UserId);
                if (user == null || !user.SectionJobApplicantsVisible)
                    objToSave.UserId = this.GetCurrentUser().Id;

                if (objToSave.Id == 0 || !context.JobApplicants.Any(j => j.Id == objToSave.Id))
                {
                    context.JobApplicants.Add(objToSave);
                }
                else
                {
                    context.JobApplicants.Attach(objToSave);
                    ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.ChangeObjectState(objToSave, EntityState.Modified);
                }
                context.SaveChanges();
            }

            return new ApiResponse(true);
        }

        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetStatus()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));

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

        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse GetJobCategory()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));

            using (var context = new AgmDataContext())
            {
                var res = context.JobCategories.Where(j => j.IsDeleted == false).ToList();
                return new ApiResponse(true)
                {
                    Data = res.OrderBy(i => i.Name)
                };
            }
        }

        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse GetLocation()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));

            using (var context = new AgmDataContext())
            {
                var res = context.Locations.ToList();
                return new ApiResponse(true)
                {
                    Data = res.OrderBy(i => i.Name)
                };
            }
        }

        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse GetLanguage()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));

            using (var context = new AgmDataContext())
            {
                var res = context.Languages.ToList();
                return new ApiResponse(true)
                {
                    Data = res.OrderBy(i => i.Name)
                };
            }
        }

        [AuthorizeAction]
        [DeflateCompression]
        [HttpGet]
        public ApiResponse GetInterviewer()
        {
            this.CheckCurrentUserPermission(((x) => x.SectionJobApplicantsVisible));

            using (var context = new AgmDataContext())
            {
                var users = context.Users.Where(u => u._sectionJobApplicantsVisible == 1).ToList();
                var res = users.Select(u => new {
                    Value = u.Id,
                    Description = u.Name,
                    IsCurrent = u.Id == this.GetCurrentUser().Id
                }).OrderBy(u => u.Description).ToList();

                return new ApiResponse(true)
                {
                    Data = res
                };
            }
        }
    }
}