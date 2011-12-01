using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Models
{
    public class ProfileWorkViewModel
    {
        public IEnumerable<Profile_Work> ProfileWorks { get; set; }
        public Profile_Work CurrentProfileWork { get; set; }
    }
}