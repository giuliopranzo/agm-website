using AGM.Web.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class User : UserBase
    {
        public int? IdExport { get; set; }
        public string Password { get; set; }
        public int _sectionUsersVisible { get; set; }
        public int _sectionJobAdsVisible { get; set; }
        public int _sectionMonthlyReportsVisible { get; set; }
        public int _sectionJobApplicantsVisible { get; set; }
        public int? _sectionExportVisible { get; set; }
        public int? _canDeleteJobApplicants { get; set; }
        public int? _canSendMessage { get; set; }
        public int _isActive { get; set; }
        public bool _isDeleted { get; set; }
        public int? _isAdmin { get; set; }

        public bool SectionUsersVisible
        {
            get { return _sectionUsersVisible == 1; }
        }

        public bool SectionJobAdsVisible
        {
            get { return _sectionJobAdsVisible == 1; }
        }

        public bool SectionMonthlyReportsVisible
        {
            get { return _sectionMonthlyReportsVisible == 1; }
        }

        public bool SectionJobApplicantsVisible
        {
            get { return _sectionJobApplicantsVisible == 1; }
        }

        public bool SectionExportVisible
        {
            get { return (_sectionExportVisible.HasValue) ? (_sectionExportVisible == 1) : false; }
        }

        public bool CanDeleteJobApplicants
        {
            get { return (_canDeleteJobApplicants.HasValue) ? (_canDeleteJobApplicants == 1) : false; }
        }

        public bool CanSendMessage
        {
            get { return (_canSendMessage.HasValue) ? (_canSendMessage == 1) : false; }
        }

        public bool IsActive
        {
            get { return _isActive == 1; }
        }

        public bool IsDeleted
        {
            get { return _isDeleted; }
        }

        public bool IsAdmin
        {
            get { return (_isAdmin.HasValue) ? (_isAdmin == 1) : false; }
        }
    }
}