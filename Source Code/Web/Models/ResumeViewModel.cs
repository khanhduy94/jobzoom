using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;
using JobZoom.Core.Framework.DataMining;
using JobZoom.Core.Framework;
using JobZoom.Core.Taxonomy;
using JobZoom.Core.Entities;

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

        public List<Job_Posting> AppliedJobs { get; set; }
        public List<TagAttributeDic> TagAttributeDics { get; set; }
    }

    public class TagAttributeDic
    {
        public Guid JobId { get; set; }

        public List<TagAttribute> TagAttributes { get; set; }
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