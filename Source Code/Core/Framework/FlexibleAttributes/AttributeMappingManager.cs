using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Entities;

namespace JobZoom.Core.FlexibleAttributes
{
    public class TagAttributeMappingManager
    {
        JobZoomCoreEntities db = new JobZoomCoreEntities();

        private List<TagMemberAttribute> _listTagMemberAttributes = new List<TagMemberAttribute>();

        public void AddAttributeObject(object attributeObject, Guid objectId, Guid parentId)
        {
            var listAttributeObjects = AttributeManager.GetProperiesFromTypeByAttribute<TagMemberAttribute>(attributeObject);            

            List<TagAttribute> lisTagAttributes = new List<TagAttribute>();

            foreach(var i in listAttributeObjects)
            {
                string tagValue = string.Empty;
                if (i.PropertyValue != null)
                {
                    tagValue = i.ToString();
                }

                TagAttribute tag = new TagAttribute();                
                
                tag.TagId = Guid.NewGuid();
                tag.TagName = i.Property.Name;
                tag.TagValue = tagValue;
                tag.ObjectId = objectId;
                tag.ParentId = parentId;
                tag.ModifiedDate = DateTime.Now;

                lisTagAttributes.Add(tag);
            }

            SaveChanges(lisTagAttributes);
        }

        public void UpdateAttributeObject(object attributeObject)
        {   
         
        }

        private void SaveChanges(List<TagAttribute> listTagAttributes)
        {
            foreach (var tag in listTagAttributes)
            {                
            }

            db.SaveChanges();

        }
    }    
}
