//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestFramework.Console
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job_WorkExpRequirement
    {
        public System.Guid JobWorkExpRequirementId { get; set; }
        public System.Guid JobPostingId { get; set; }
        public string JobAttributeName { get; set; }
        public string JobAttributeValue { get; set; }
    
        public virtual Job_Posting Job_Posting { get; set; }
    }
}
