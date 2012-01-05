using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace JobZoom.Core.FlexibleAttributes
{
    public class AttributeObject<T>
    {
        public Type Type { get; set; }
        public T Attribute { get; set; }
        public PropertyInfo Property { get; set; }
        public object PropertyValue { get; set; }
        public TaggingType TaggingType
        {
            get { return (this.Attribute as TagMemberAttribute).TaggingType; }            
        }

        public string ReferenceValueProperty
        {
            get { return (this.Attribute as TagMemberAttribute).ReferenceValueProperty; }
        }
    }
}