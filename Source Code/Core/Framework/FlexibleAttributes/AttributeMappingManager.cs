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

        public void AddAttributeObject(object attributeObject, Guid objectId, string objectType, Guid classificationId, string classificationName)
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

                switch (i.TaggingType)
                {
                    case TaggingType.ColumnNameAsTag:
                        tag.TagName = i.Property.Name;
                        tag.TagValue = i.PropertyValue.ToString();
                        break;
                    case TaggingType.ValueAsTag:
                        tag.TagName = i.PropertyValue.ToString();
                        tag.TagValue = i.PropertyValue.ToString();
                        break;
                }
                
                tag.TagId = Guid.NewGuid();                                
                tag.ObjectId = objectId;
                tag.ObjectType = objectType;
                tag.ParentId = classificationId;
                tag.ParentName = classificationName;
                tag.ModifiedDate = DateTime.Now;                

                lisTagAttributes.Add(tag);
            }

            SaveChanges(lisTagAttributes);
        }

        private void SaveChanges(List<TagAttribute> listTagAttributes)
        {            
            foreach (var tag in listTagAttributes)
            {
                db.TagAttributes.Add(tag);                
            }

            db.SaveChanges();
        }
    }    
}
