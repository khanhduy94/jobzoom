using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Core.Framework.DataMining
{
    public enum DecisionTreeNodeType
    {
        Model = 1, //Root node for model.
        Tree = 2, //Parent node for classification trees in the model. Labeled "Prefix+JobTitle".
        Interior = 3, //Head of interior branch, found within in a classification tree or regression tree.
        Distribution = 4, //Leaf node, found within a classification tree or regression tree.
        RegressionTree = 25 //Parent node for regression tree within the model. Labeled as "Prefix+JobTitle".
    }
}
