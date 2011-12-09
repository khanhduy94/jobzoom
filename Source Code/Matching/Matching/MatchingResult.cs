using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matching
{
    public class MatchingResult
    {
        public string TargetTagID { get; set; }
        // Chỉ match tag 
        public bool IsExists { get; set; }
        // Match tag và level
        public bool IsMatch { get; set; }
        public int Point { get; set; }        
    }
}