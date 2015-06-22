using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using AGM.Web.Infrastructure;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Models;

namespace AGM.Web.Controllers
{
    public class SettingsController : ApiController
    {
        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetHourReasons()
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);

                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                var hourReasons = context.HourReasons.Where(h => h.IsDeleted == false).ToList();
                return new ApiResponse(true)
                {
                    Data = hourReasons
                };
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse InsertHourReason(HourReason newHourReason)
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);

                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                if (context.HourReasons.Any(r => r.Name == newHourReason.Name))
                    return new ApiResponse(false)
                    {
                        Errors = new ApiResponseError[] {new ApiResponseError() {Message = "Causale già esistente!"}}
                    };

                context.HourReasons.Add(new HourReason() {Id = 10, Name = newHourReason.Name});
                context.SaveChanges();

                return new ApiResponse(true);
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse DeleteHourReason(HourReason oldHourReason)
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);

                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                if (context.HourReasons.Any(r => r.Id == oldHourReason.Id))
                {
                    context.HourReasons.Single(r => r.Id == oldHourReason.Id).IsDeleted = true;
                } 
                context.SaveChanges();

                return new ApiResponse(true);
            }
        }
    }
}