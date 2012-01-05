using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Core.Taxonomy;

namespace JobZoom.Web.Models
{
    public class JobGraphViewModel
    {
        public JobGraphViewModel(Guid jobId)
        {
            Hierarchy hierarchy = new Hierarchy();
            Tag jobHierarchicalTree = hierarchy.GetHierarchicalTreeByObject(jobId);
            this.JobHierarchicalTree = jobHierarchicalTree;
            this.JobId = jobId;
        }
        public Guid JobId { get; set; }
        public Tag JobHierarchicalTree { get; set; }
    }
}