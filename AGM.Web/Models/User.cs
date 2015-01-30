using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class User : UserBase
    {
        public string Password { get; set; }
        public int _sectionUsersVisible { get; set; }
        public int _sectionJobAdsVisible { get; set; }
        public int _sectionMonthlyReportsVisible { get; set; }
        public int _sectionJobApplicantsVisible { get; set; }
        public int _isActive { get; set; }
        public bool _isDeleted { get; set; }

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

        public bool IsActive
        {
            get { return _isActive == 1; }
        }

        public bool IsDeleted
        {
            get { return _isDeleted; }
        }
    }
}