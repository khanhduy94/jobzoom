using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobZoom.Core
{
    public class MatchingResult
    {
        public string TargetTagID { get; set; }
        public bool IsExists { get; set; }
        public bool IsMatch { get; set; }
        public int Point { get; set; }        
    }
}