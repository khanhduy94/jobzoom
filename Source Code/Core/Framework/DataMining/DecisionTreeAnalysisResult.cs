using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Entities;

namespace JobZoom.Core.Framework.DataMining
{
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
            while (!DecisionTreeAnalysis.isRootNode(node.DecisionTreeNode_Parent))
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
}
