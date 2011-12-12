//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobZoom.Core.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class DecisionTreeNode
    {
        public string NODEID { get; set; }
        public Nullable<int> NODE_TYPE { get; set; }
        public string NODE_CAPTION { get; set; }
        public Nullable<int> CHILDREN_CARDINALITY { get; set; }
        public string PARENTID { get; set; }
        public string NODE_DESCRIPTION { get; set; }
        public string NODE_RULE { get; set; }
        public string MARGINAL_RULE { get; set; }
        public Nullable<double> NODE_PROBABILITY { get; set; }
        public Nullable<double> MARGINAL_PROBABILITY { get; set; }
        public Nullable<double> NODE_SUPPORT { get; set; }
        public string MSOLAP_MODEL_COLUMN { get; set; }
        public Nullable<double> MSOLAP_NODE_SCORE { get; set; }
        public string MSOLAP_NODE_SHORT_CAPTION { get; set; }
        public string ATTRIBUTE_NAME { get; set; }
    
        public virtual DecisionTreeNodeDistribution DecisionTreeNodeDistribution { get; set; }
    }
}
