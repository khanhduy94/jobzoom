using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Models
{
    public class ResumeViewModel
    {
        public Profile_Basic Basic { get; set; }
        public IEnumerable<Profile_Education> Education { get; set; }
        public IEnumerable<Profile_Work> Experience { get; set; }       
    }
}