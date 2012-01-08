using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;
using JobZoom.Core.Matching;

namespace JobZoom.Web.Models
{
    public class CandidateViewModel
    {
		public Profile_Basic Basic { get; set; }
        public IEnumerable<Profile_Education> Education { get; set; }
        public IEnumerable<Profile_Work> Experience { get; set; }
        public int MatchPercent { get; set; }
        public IEnumerable<MatchingResult> MatchResult { get; set; }
    }
}