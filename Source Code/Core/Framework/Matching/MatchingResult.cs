using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Core.Entities;

namespace JobZoom.Core.Matching
{
    public class MatchingResult
    {
        public TagAttribute TagMatch { get; set; }
        public bool IsMatchAbsolute { get; set; }
    }
}