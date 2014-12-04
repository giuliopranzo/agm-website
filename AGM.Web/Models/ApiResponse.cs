using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AGM.Web.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {  }

        public ApiResponse(bool succeed)
        {
            Succeed = succeed;
        }

        public bool Succeed { get; set; }
        public ApiResponseError[] Errors { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}