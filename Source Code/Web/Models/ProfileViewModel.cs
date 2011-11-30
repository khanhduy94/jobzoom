using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Models
{
    public class ProfileViewModel
    {
        public Profile_Basic Basic { get; set; }
        public IEnumerable<Profile_Education> Educations { get; set; }
        public IEnumerable<Profile_Work> Works { get; set; }
        public IEnumerable<AttributeTag> Skills { get; set; }
    }
}