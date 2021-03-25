using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegionWebAPI.Models
{
    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int? ParentRegionId { get; set; }
    }
}