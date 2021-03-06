﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MonthlyReportNote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Note { get; set; }

        public DateTime Date
        {
            get { return new DateTime(Year, Month, Day); }
        }
    }
}