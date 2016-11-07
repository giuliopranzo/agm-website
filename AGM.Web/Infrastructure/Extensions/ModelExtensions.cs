using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AGM.Web.Models;

namespace AGM.Web.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static double GetTotalAmount(this MonthlyReportExpense o)
        {
            double resp = 0;
            switch (o.ReasonId)
            {
                case 1:
                    resp = o.Amount* 0.4;
                    break;
                case 2:
                    resp = o.Amount* 0.4;
                    break;
                default:
                    resp = o.Amount;
                    break;
            }
            return resp;
        }
    }
}