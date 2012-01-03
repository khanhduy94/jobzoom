using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Entities;

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

        public Tag GetHierarchicalTreeByObject(Guid objectId, string objectType)
        {
            TagAttribute rootNode = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 1).SingleOrDefault();
            List<TagAttribute> classificationNodes = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 2).ToList();

            Tag tag = new Tag(rootNode);

            foreach (var i in classificationNodes)
            {
                Tag secondLevelTag = new Tag(i);

                List<TagAttribute> thirdLevelNodes = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ParentId == i.ObjectId && x.ObjectDeepLevel == 3).ToList();

                foreach (var j in thirdLevelNodes)
                {
                    Tag thirdLevelTag = new Tag(j);

                    secondLevelTag.Children.Add(thirdLevelTag);
                }
            }
            return tag;            
        }
    }
}
