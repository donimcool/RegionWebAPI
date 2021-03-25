using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegionWebAPI.Models
{
    public class BaseResponse
    {
        public BaseResponse(bool h, string e)
        {
            HasError = h;
            ErrorMsg = e;
        }
        public bool HasError { get; set; }
        public string ErrorMsg { get; set; }
    }
}