using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Entities;
using JobZoom.Core.Framework.DataMining;

namespace JobZoom.Core.DataMining
{
    public class DecisionTreeManager
    {
        JobZoomCoreEntities db = new JobZoomCoreEntities();
        public DecisionTreeNode GetDecisionTree(string modelName)
        {            
            return db.DecisionTreeNodes.First(n => n.MODEL_NAME == modelName && n.NODE_TYPE == (int) DecisionTreeNodeType.Tree);
        }
    }
}
