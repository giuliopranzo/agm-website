using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AGM.Web.Infrastructure
{
    public class OperationException : Exception
    {
        public HttpStatusCode HttpStatus { get; set; }

        public OperationException(HttpStatusCode httpStatus, string message)
            : base(message)
        {
            HttpStatus = httpStatus;
        }
    }
}