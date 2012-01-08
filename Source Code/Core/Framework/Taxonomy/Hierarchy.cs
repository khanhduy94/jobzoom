using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Entities;
using JobZoom.Core.DataMining;
using JobZoom.Core.Framework.DataMining;

namespace JobZoom.Core.Taxonomy
{
    public class Tag : ComplexTreeNode<Tag>
    {
        public TagAttribute TagAttribute { get; set; }

        private bool _Completed;
        public bool Completed
        {
            get { return _Completed; }
            set
            {
                // if this task is complete, then all child tasks must also be complete
                if (value)
                {
                    foreach (Tag task in Children)
                    {
                        task.Completed = true;
                    }
                }

                _Completed = value;
            }
        }

        public Tag(TagAttribute tagAttribute)
        {
            TagAttribute = tagAttribute;
            Completed = false;
        }
    }

    public class Hierarchy
    {
        JobZoomCoreEntities db = new JobZoomCoreEntities();

        public Tag GetHierarchicalTreeByObject(Guid objectId)
        {
            TagAttribute rootNode = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 1).SingleOrDefault();
            List<TagAttribute> classificationNodes = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 2).ToList();

            Tag tag = new Tag(rootNode);
            tag.Depth = 1;

            foreach (var i in classificationNodes)
            {
                Tag secondLevelTag = new Tag(i);
                tag.Children.Add(secondLevelTag);
                tag.Depth = 2;

                List<TagAttribute> thirdLevelNodes = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ParentId == i.TagId && x.ObjectDeepLevel == 3).ToList();

                foreach (var j in thirdLevelNodes)
                {
                    Tag thirdLevelTag = new Tag(j);
                    secondLevelTag.Children.Add(thirdLevelTag);
                    tag.Depth = 3;
                }
            }
            return tag;
        }

        public Tag GetHierarchicalTreeByDecisionTree(string modelName)
        {
            modelName = "PFDeveloperEvangelist";
            DecisionTreeManager dt = new DecisionTreeManager();

            DecisionTreeNode dtroot = dt.GetDecisionTree(modelName);

            TagAttribute att = new TagAttribute
            {
                TagId = Guid.NewGuid(),
                TagName = dtroot.NODE_CAPTION,
                TagValue = dtroot.NODE_CAPTION,
                ObjectDeepLevel = 1,
                ObjectType = "Decision Tree"
            };
            Tag root = new Tag(att);

            addChildNodes(ref root, dtroot, 2);
            return root;
        }

        public void addChildNodes(ref Tag ParentTag, DecisionTreeNode node, int ChildDeepLevel)
        {
            foreach (var child in node.DecisionTreeNode_Childs)
            {
                TagAttribute childAtt = new TagAttribute
                {
                    TagId = Guid.NewGuid(),
                    TagName = child.NODE_CAPTION,
                    TagValue = child.NODE_CAPTION,
                    ObjectDeepLevel = ChildDeepLevel,
                    ObjectType = "Decision Tree"
                };
                Tag tag = new Tag(childAtt);
                ParentTag.Children.Add(tag);

                if (child.NODE_TYPE != (int) DecisionTreeNodeType.Distribution)
                {
                    addChildNodes(ref tag, child, ChildDeepLevel + 1);
                }
            }

        }
    }

}
