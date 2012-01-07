using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Entities;

namespace JobZoom.Core.Framework.DataMining
{
    public class DecisionTreeAnalysis
    {
        protected static JobZoomCoreEntities entities = new JobZoomCoreEntities();

        private static void test()
        {
            //string[] att = new string[] { ".net" };
            //string[] ex_att = new string[] { "C#" };

            string[] att = new string[] { };
            string[] ex_att = new string[] { "C#" };

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

    }  
}
