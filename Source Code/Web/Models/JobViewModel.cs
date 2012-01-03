using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Models
{
    public class JobViewModel
    {
        public Job_Posting Job_Posting { get; set; }
        public IEnumerable<Profile_Basic> ApplyList { get; set; }
    }
}