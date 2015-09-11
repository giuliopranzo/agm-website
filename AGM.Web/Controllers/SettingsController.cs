using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using AGM.Web.Infrastructure;
using AGM.Web.Infrastructure.Attributes;
using AGM.Web.Models;
using System.Web.Http.ModelBinding;
using AGM.Web.Infrastructure.Helpers;

namespace AGM.Web.Controllers
{
    public class SettingsController : ApiController
    {
        #region HourReason
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

                var hourReasons = context.HourReasons.Where(h => !h.IsDeleted).ToList();
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

                if (context.HourReasons.Any(r => r.CodeExport == newHourReason.CodeExport))
                    return new ApiResponse(false)
                    {
                        Errors = new ApiResponseError[] { new ApiResponseError() { Message = "Codice export causale già utilizzato!" } }
                    };

                context.HourReasons.Add(newHourReason);
                context.SaveChanges();

                return new ApiResponse(true);
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse UpdateHourReason(HourReason newHourReason)
        {
            if (ModelState.IsValid)
            {
                using (var context = new AgmDataContext())
                {
                    var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                    var user = context.Users.Single(u => u.Email == email);

                    if (!user.SectionUsersVisible)
                        return new ApiResponse(false);

                    if (!context.HourReasons.Any(r => r.Id == newHourReason.Id))
                        return new ApiResponse(false)
                        {
                            Errors =
                                new ApiResponseError[] {new ApiResponseError() {Message = "Causale non esistente!"}}
                        };

                    if (context.HourReasons.Any(r => r.Id != newHourReason.Id && r.Name == newHourReason.Name))
                        return new ApiResponse(false)
                        {
                            Errors =
                                new ApiResponseError[]
                                {new ApiResponseError() {Message = "Nome causale già utilizzato!"}}
                        };

                    if (
                        context.HourReasons.Any(
                            r => r.Id != newHourReason.Id && r.CodeExport == newHourReason.CodeExport))
                        return new ApiResponse(false)
                        {
                            Errors =
                                new ApiResponseError[]
                                {new ApiResponseError() {Message = "Codice export causale già utilizzato!"}}
                        };

                    context.HourReasons.Attach(newHourReason);
                    ((IObjectContextAdapter) context).ObjectContext.ObjectStateManager.ChangeObjectState(newHourReason,
                        EntityState.Modified);
                    context.SaveChanges();

                    return new ApiResponse(true);
                }
            }
            return new ApiResponse(false);
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse DeleteHourReason(List<HourReason> objCollectionToDelete)
        {
            using (var context = new AgmDataContext())
            {
                foreach (var item in objCollectionToDelete)
                {
                    var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                    var user = context.Users.Single(u => u.Email == email);

                    if (!user.SectionUsersVisible)
                        return new ApiResponse(false);

                    if (context.HourReasons.Any(r => r.Id == item.Id))
                    {
                        context.HourReasons.Single(r => r.Id == item.Id).IsDeleted = true;
                    }
                }
                context.SaveChanges();
                return new ApiResponse(true);
            }
        }
        #endregion

        #region Festivity
        [AuthorizeAction]
        [HttpGet]
        public ApiResponse GetFestivities()
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);

                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                var festivities = context.Festivities.Where(f => !f.IsDeleted);
                return new ApiResponse(true)
                {
                    Data = (festivities.Any())? festivities.ToList() : null
                };
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse DeleteFestivity(List<Festivity> objCollectionToDelete)
        {
            using (var context = new AgmDataContext())
            {
                foreach (var item in objCollectionToDelete)
                {
                    var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                    var user = context.Users.Single(u => u.Email == email);

                    if (!user.SectionUsersVisible)
                        return new ApiResponse(false);

                    if (context.Festivities.Any(r => r.Id == item.Id))
                    {
                        context.Festivities.Single(r => r.Id == item.Id).IsDeleted = true;
                    }
                }
                context.SaveChanges();
                return new ApiResponse(true);
            }
        }

        [AuthorizeAction]
        [HttpPost]
        public ApiResponse InsertFestivity(Festivity newFestivity)
        {
            using (var context = new AgmDataContext())
            {
                var email = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var user = context.Users.Single(u => u.Email == email);

                if (!user.SectionUsersVisible)
                    return new ApiResponse(false);

                if (context.Festivities.Any(r => r.Date == newFestivity.Date))
                    return new ApiResponse(false)
                    {
                        Errors = new ApiResponseError[] { new ApiResponseError() { Message = "Festività già esistente!" } }
                    };

                context.Festivities.Add(newFestivity);
                context.SaveChanges();

                return new ApiResponse(true);
            }
        }
        #endregion
    }
}