using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;
using JobZoom.Core.Framework.DataMining;

namespace JobZoom.Web.Models
{
    public class ResumeViewModel
    {
        public List<RequiredTagName> RequiredTagNames { get; set; }
        public string[] JobTitles { get; set; }
        public string[] ExceptAttributes { get; set; }
        public string[] TypedAttributes { get; set; }
        public double Probability = 0.8; // 80%
        public CompareType CompareType = CompareType.GreaterThanOrEqualTo; // >= Probability
        public string Prefix = "PF";
    }

    public class RequiredTagName
    {
        public string JobTitle {get; set;}
        public List<string> Attributes { get; set; }

        public RequiredTagName(string JobTitle, List<string> Attributes)
        {
            this.JobTitle = JobTitle;
            this.Attributes = Attributes;
        }
    }

}