using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionTreeDataStructure
{
    class DecisionTreeAnalysis
    {
        protected static CoreEntities entities = new CoreEntities();
        static void Main(string[] args)
        {
            test();
            Console.ReadLine();
        }

        private static void test()
        {
            //string[] att = new string[] { ".net" };
            //string[] ex_att = new string[] { "C#" };

            string[] att = new string[] { };
            string[] ex_att = new string[] { };

            List<DecisionTreeAnalysisResult> results = new List<DecisionTreeAnalysisResult>();
            //results = getAnalysisResults("Developer Evangelist", CompareType.GreaterThanOrEqualTo, 0.5);
            results = getAnalysisResults(convertJobTitleNameToModelName("Developer Evangelist", "PF"), att, ex_att, CompareType.GreaterThanOrEqualTo, 0.5);

            foreach (var result in results)
            {
                Console.WriteLine("NODE_ID = " + result.Node.NODE_ID);
                foreach (var caption in result.NodeDescription.NodeCaptions)
                {
                    Console.WriteLine(caption.Name + " = " + caption.Value + " with Probability = " + result.getDetailProbability());
                }
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Is root node?
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>True if the node is a root node and reverse</returns>
        public static bool isRootNode(DecisionTreeNode node)
        {
            try
            {
                if (node == null)
                    return false;
                return (node.NODE_ID == "0" && node.NODE_TYPE == (int) DecisionTreeNodeType.Model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Is root node?
        /// </summary>
        /// <param name="node">NODE_ID to check</param>
        /// <returns>True if the node is a root node and reverse</returns>
        public static bool isRootNode(string NODE_ID)
        {
            try
            {
                DecisionTreeNode node = entities.DecisionTreeNodes.First(n => n.NODE_ID == NODE_ID);
                return isRootNode(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Is leaf node?
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>True if the node is a leaf node and reverse</returns>
        public static bool isLeafNode(DecisionTreeNode node)
        {
            try
            {
                if (node == null)
                    return false;
                return (node.NODE_TYPE == (int) DecisionTreeNodeType.Distribution && node.CHILDREN_CARDINALITY == 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Is leaf node?
        /// </summary>
        /// <param name="NODE_ID">NODE_ID to check</param>
        /// <returns>True if the node is a leaf node and reverse</returns>
        public static bool isLeafNode(string NODE_ID)
        {
            try
            {
                DecisionTreeNode node = entities.DecisionTreeNodes.First(n => n.NODE_ID == NODE_ID);
                return isLeafNode(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Is Model Name node
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>True if the node is a model name node and reverse</returns>
        public static bool isModelNameNode(DecisionTreeNode node)
        {
            try
            {
                if (node == null)
                    return false;
                return (node.NODE_TYPE == (int) DecisionTreeNodeType.Tree && node.PARENT_ID == "0");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Is Model name node
        /// </summary>
        /// <param name="NODE_ID">NODE_ID to check</param>
        /// <returns>True if the node is a model name node and reverse</returns>
        public static bool isModelNameNode(string NODE_ID)
        {
            try
            {
                DecisionTreeNode node = entities.DecisionTreeNodes.First(n => n.NODE_ID == NODE_ID);
                return isModelNameNode(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get child nodes
        /// </summary>
        /// <param name="node">The node you want to get its childs</param>
        /// <returns>Array of node childs</returns>
        public static DecisionTreeNode[] getChildNodes(DecisionTreeNode node)
        {
            try
            {
                if (!isLeafNode(node))
                    return node.DecisionTreeNode_Childs.ToArray();
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get child nodes
        /// </summary>
        /// <param name="NODE_ID">The node with NODE_ID you want to get its childs</param>
        /// <returns>Array of node childs</returns>
        public static DecisionTreeNode[] getChildNodes(string NODE_ID)
        {
            try
            {
                DecisionTreeNode node = entities.DecisionTreeNodes.First(n => n.NODE_ID == NODE_ID);
                return getChildNodes(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get parent node
        /// </summary>
        /// <param name="node">The node you want to get its parent</param>
        /// <returns>The parent node</returns>
        public static DecisionTreeNode getParentNode(DecisionTreeNode node)
        {
            try
            {
                if (!isRootNode(node))
                    return node.DecisionTreeNode_Parent;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get parent node
        /// </summary>
        /// <param name="NODE_ID">The node with NODE_ID you want to get its parent</param>
        /// <returns>The parent node</returns>
        public static DecisionTreeNode getParentNode(string NODE_ID)
        {
            try
            {
                DecisionTreeNode node = entities.DecisionTreeNodes.First(n => n.NODE_ID == NODE_ID);
                return getParentNode(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get all attributes with probability comparison
        /// </summary>
        /// <param name="strJobTitle">Job title to get attributes</param>
        /// <param name="compareType">Compare Type</param>
        /// <param name="compareValue">Value to compare</param>
        /// <param name="compareValue2">Value to compare 2 [optional]</param>
        /// <returns>The attributes appropriate probability comparison</returns>
        public static List<DecisionTreeAnalysisResult> getAnalysisResults(string strModelName, CompareType compareType, double compareValue, double compareValue2 = -1)
        {
            if (compareValue < 0 || compareValue > 1)
            {
                throw new Exception("compareValue variable value must be between 0-1");
            }
            if ((compareType == CompareType.Between || compareType == CompareType.NotBetween) && (compareValue2 > 1 || compareValue2 < 0))
            {
                throw new Exception("compareValue2 variable value must be between 0-1");
            }

            if (!existsModelName(strModelName))
            {
                throw new Exception("Model Name doesn't exists!");
            }
            try
            {
                //Get all leaves with Probability and Model Name
                List<DecisionTreeNode> nodes = getDecisionTreeNodeWithProbability(strModelName, compareType, compareValue, compareValue2);
                List<DecisionTreeAnalysisResult> results = new List<DecisionTreeAnalysisResult>();
                foreach (DecisionTreeNode node in nodes)
                {
                    DecisionTreeAnalysisResult result = new DecisionTreeAnalysisResult(new NodeDescription(node), node);
                    results.Add(result);
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all attributes with "true" probability comparison
        /// </summary>
        /// <param name="strJobTitle">Job title to get attributes</param>
        /// <param name="strAttributes">The attributes has already been typed</param>
        /// <param name="exceptAttributes">The unexpected attributes</param>
        /// <param name="compareType">Compare Type</param>
        /// <param name="compareValue">Value to compare</param>
        /// <param name="compareValue2">Value to compare 2 [optional]</param>
        /// <returns></returns>
        public static List<DecisionTreeAnalysisResult> getAnalysisResults(string strModelName, string[] strAttributes, string[] exceptAttributes, CompareType compareType, double compareValue, double compareValue2 = -1)
        {
            try
            {
                List<DecisionTreeAnalysisResult> results = getAnalysisResults(strModelName, compareType, compareValue, compareValue2);
                if (results != null)
                {
                    if (strAttributes != null && strAttributes.Length > 0)
                        results = results.Where(r => r.NodeDescription.isContainOneOfListAttributes(strAttributes)).ToList();
                    if (exceptAttributes != null && exceptAttributes.Length > 0)
                        results = results.Where(r => (!r.NodeDescription.isContainOneOfListAttributes(exceptAttributes))).ToList();
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all DecisionTreeNodes in ModelName with "true" probability comparison
        /// </summary>
        /// <param name="ModelName">Model name</param>
        /// <param name="compareType">CompareType</param>
        /// <param name="compareValue">compareValue</param>
        /// <param name="compareValue2">Use when CompareType = Between or NotBetween</param>
        /// <returns>List of DecisionTreeNodes</returns>
        public static List<DecisionTreeNode> getDecisionTreeNodeWithProbability(string ModelName, CompareType compareType, double compareValue, double compareValue2 = -1)
        {
            try
            {
                string[] nodeids = entities.DecisionTreeNodes.Where(n => n.NODE_TYPE == (int)DecisionTreeNodeType.Distribution && n.MODEL_NAME == ModelName).Select(n => n.NODE_ID).ToArray();
                string[] dis_nodeids;
                switch (compareType)
                {
                    case CompareType.LessThanOrEqualTo:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY <= compareValue).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.GreaterThan:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY > compareValue).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.LessThan:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY < compareValue).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.Between:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY > compareValue
                                                                                && nd.PROBABILITY < compareValue2).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.NotBetween:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && (nd.PROBABILITY < compareValue
                                                                                || nd.PROBABILITY > compareValue2)).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.EqualTo:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY == compareValue).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.NotEqualTo:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY != compareValue).Select(nd => nd.NODE_ID).ToArray();
                        break;
                    case CompareType.GreaterThanOrEqualTo:
                    default:
                        dis_nodeids = entities.DecisionTreeNodeDistributions.Where(nd => nodeids.Contains(nd.NODE_ID)
                                                                                && nd.ATTRIBUTE_VALUE == "True"
                                                                                && nd.PROBABILITY >= compareValue).Select(nd => nd.NODE_ID).ToArray();
                        break;
                }
                return entities.DecisionTreeNodes.Where(n => dis_nodeids.Contains(n.NODE_ID)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Convert a job title name to model name
        /// </summary>
        /// <param name="JobTitle">The job title name you want to convert</param>
        /// <param name="prefix">Prefix</param>
        /// <returns>The model name</returns>
        public static string convertJobTitleNameToModelName(string JobTitle, string prefix = "PF")
        {
            return prefix + JobTitle.Replace(" ", "");
        }

        /// <summary>
        /// Exists mining data with the model name?
        /// </summary>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public static bool existsModelName(string ModelName)
        {
            try
            {
                DecisionTreeNode node = entities.DecisionTreeNodes.First(n => n.MODEL_NAME == ModelName && n.NODE_TYPE == (int)DecisionTreeNodeType.Tree);
                if (node != null)
                {
                    return true;
                }
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public class NodeCaption
        {
            public NodeCaption(string Caption)
            {
                string[] s = Caption.Split(new string[] { " not = " }, StringSplitOptions.None);
                if (s.Length == 2)
                {
                    _Name = s[0];
                    _Value = !Convert.ToBoolean(s[1]);
                }
                else
                {
                    s = Caption.Split('=');
                    if (s.Length == 2)
                    {
                        _Name = s[0];
                        _Value = Convert.ToBoolean(s[1]);
                    }
                    else
                    {
                        _Name = s[0];
                    }
                }
            }

            private string _Name;
            /// <summary>
            /// Get the attribute name
            /// </summary>
            public string Name { get { return _Name; } }

            private bool _Value = true;
            /// <summary>
            /// Get the attribute value
            /// </summary>
            public bool Value { get { return _Value; } }
        }

        public class NodeDescription
        {
            public NodeDescription(string Description)
            {
                //Loc chuoi, tranh nham lan giua chu "and" trong mo ta thuoc tinh 
                Description = Description.Replace(" False and ", " False @nd ");
                Description = Description.Replace(" True and ", " True @nd ");
                //Tach caption
                string[] captions = Description.Split(new string[] { " @nd " }, StringSplitOptions.None);
                _NodeCaptions = new List<NodeCaption>();

                foreach (string caption in captions)
                {
                    _NodeCaptions.Add(new NodeCaption(caption));
                }
            }

            public NodeDescription(DecisionTreeNode node)
            {
                _NodeCaptions = new List<NodeCaption>();
                while(!isRootNode(node.DecisionTreeNode_Parent))
                {
                    _NodeCaptions.Add(new NodeCaption(node.NODE_CAPTION));
                    node = node.DecisionTreeNode_Parent;
                }
            }

            private List<NodeCaption> _NodeCaptions;
            /// <summary>
            /// List of NodeCaptions
            /// </summary>
            public List<NodeCaption> NodeCaptions { get { return _NodeCaptions; } }

            public List<NodeCaption> getNodeCaptionsWithValue(bool value = true)
            {
                return NodeCaptions.Where(i => i.Value == value).ToList();
            }

            public bool isContainAttribute(string attribute)
            {
                List<NodeCaption> captions = getNodeCaptionsWithValue(true);
                foreach (NodeCaption caption in captions)
                {
                    if (caption.Name.Trim().ToLower() == attribute.Trim().ToLower())
                        return true;
                }
                return false;
            }

            public bool isContainOneOfListAttributes(string[] attributes)
            {
                if (attributes == null)
                    throw new Exception("Attributes is null!");
                else if (attributes.Length == 0)
                    throw new Exception("Attributes have no element!");
                foreach (string attribute in attributes)
                {
                    if (isContainAttribute(attribute))
                        return true;
                }
                return false;
            }
        }

        public class DecisionTreeAnalysisResult
        {

            private NodeDescription _NodeDescription;
            /// <summary>
            /// Get NodeDescription Instance
            /// </summary>
            public NodeDescription NodeDescription { get { return _NodeDescription; } }

            private DecisionTreeNode _Node;
            /// <summary>
            /// Get node details
            /// </summary>
            public DecisionTreeNode Node { get { return _Node; } }

            public DecisionTreeAnalysisResult(NodeDescription NodeDescription, DecisionTreeNode Node)
            {
                _NodeDescription = NodeDescription;
                _Node = Node;
            }

            public double getDetailProbability(AttributeValue ATTRIBUTE_VALUE = AttributeValue.IsTrue)
            {
                string value;
                switch (ATTRIBUTE_VALUE)
                {
                    case AttributeValue.IsFalse:
                        value = "False";
                        break;
                    case AttributeValue.IsMissing:
                        value = "Missing";
                        break;
                    case AttributeValue.IsNull:
                        value = "0";
                        break;
                    case AttributeValue.IsTrue:
                    default:
                        value = "True";
                        break;
                }
                try
                {
                    return Node.DecisionTreeNodeDistributions.First(nd => nd.ATTRIBUTE_VALUE == value).PROBABILITY;
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            public List<NodeCaption> getNodeCaptionsWithValue(bool value = true)
            {
                return NodeDescription.getNodeCaptionsWithValue(value);
            }
        }

        public enum CompareType
        {
            Between,
            NotBetween,
            EqualTo,
            NotEqualTo,
            GreaterThan,
            LessThan,
            GreaterThanOrEqualTo,
            LessThanOrEqualTo
        }

        public enum DecisionTreeNodeType
        {
            Model = 1, //Root node for model.
            Tree = 2, //Parent node for classification trees in the model. Labeled "Prefix+JobTitle".
            Interior = 3, //Head of interior branch, found within in a classification tree or regression tree.
            Distribution = 4, //Leaf node, found within a classification tree or regression tree.
            RegressionTree = 25 //Parent node for regression tree within the model. Labeled as "Prefix+JobTitle".
        }

        public enum AttributeValue
        {
            IsTrue,
            IsFalse,
            IsMissing,
            IsNull,
        }
    }  
}
