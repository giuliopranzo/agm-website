using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class HourReason
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(3)]
        public string CodeExport { get; set; }
    }
}