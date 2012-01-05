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

        public Guid RootId { get; set; }
        public Guid ClassificationId { get; set; }
        public string ClassificationName { get; set; }

        private List<TagMemberAttribute> _listTagMemberAttributes = new List<TagMemberAttribute>();

        public void AddRootAttribute(Guid objectId, string tagName, string objectType)
        {
            if (db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 1).Count() != 1)
            {
                TagAttribute rootNode = new TagAttribute
                {
                    TagId = Guid.NewGuid(),
                    TagName = tagName,
                    ObjectId = objectId,
                    ObjectDeepLevel = 1,
                    ObjectType = objectType
                };

                db.TagAttributes.Add(rootNode);
                db.SaveChanges();                
            }
            this.RootId = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 1).SingleOrDefault().TagId;
        }

        public void AddSecondLevelAttribute(Guid objectId, string tagName, string objectType)
        {
            if (db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 2 && x.TagName == tagName).Count() != 1)
            {
                TagAttribute classificationNode = new TagAttribute
                {
                    TagId = Guid.NewGuid(),
                    TagName = tagName,
                    TagValue = tagName,
                    ParentId = this.RootId,
                    ObjectId = objectId,
                    ObjectDeepLevel = 2,
                    ObjectType = objectType                    
                };
                db.TagAttributes.Add(classificationNode);
                db.SaveChanges();                
            }
            this.ClassificationId = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 2 && x.TagName == tagName).SingleOrDefault().TagId;
            this.ClassificationName = db.TagAttributes.Where(x => x.ObjectId == objectId && x.ObjectDeepLevel == 2 && x.TagName == tagName).SingleOrDefault().TagName;
        }                

        public void AddThirdLevelAttribute(object attributeObject, Guid objectId, string objectType, string tableReference)
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
                    case TaggingType.ReferenceColumnNameAsTag:
                        string referenceValueProperty = i.ReferenceValueProperty;
                        string value = listAttributeObjects.Where(x=>x.Property.Name == referenceValueProperty).FirstOrDefault().PropertyValue.ToString();
                        tag.TagName = i.PropertyValue.ToString();
                        tag.TagValue = value;
                        break;
                    case TaggingType.ReferenceValueAsTag:
                        break;                    
                }
                
                tag.TagId = Guid.NewGuid();                                
                tag.ObjectId = objectId;
                tag.ObjectType = objectType;
                tag.ObjectDeepLevel = 3;
                tag.ParentId = this.ClassificationId;
                tag.ParentName = this.ClassificationName;
                tag.ModifiedDate = DateTime.Now;
                tag.TableReference = tableReference;

                if (i.TaggingType != TaggingType.ReferenceValueAsTag)
                {
                    lisTagAttributes.Add(tag);
                }                
            }            

            ///Save TagAttribute List
            foreach (var tag in lisTagAttributes)
            {
                db.TagAttributes.Add(tag);
            }
            db.SaveChanges();
        }
    }    
}
