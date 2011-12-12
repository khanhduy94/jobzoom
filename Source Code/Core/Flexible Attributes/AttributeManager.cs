using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using JobZoom.Core.Entities;

namespace JobZoom.Core.FlexibleAttributes
{
    public class AttributeManager
    {
        public void GetAllTypesFromAssemblyByAttribute<TAttribute>(Assembly[] assemblies)
            where TAttribute : Attribute
        {
            List<AttributeObject<TAttribute>> typesWithMyAttribute =
                (from assembly in assemblies
                 from type in assembly.GetTypes()
                 let attributes = type.GetCustomAttributes(typeof(TAttribute), true)
                 where attributes != null && attributes.Length > 0
                 select new AttributeObject<TAttribute> { Type = type, Attribute = attributes.Cast<TAttribute>().FirstOrDefault() }).
                 ToList();
        }

        public void GetProperiesFromTypeByAttribute<TAttribute>(Assembly[] assemblies)
            where TAttribute : Attribute
        {
            List<AttributeObject<TAttribute>> methodsWithAttributes =
                (from assembly in assemblies
                 from type in assembly.GetTypes()                 
                 from property in type.GetProperties()
                 let attributes = property.GetCustomAttributes(typeof(TagAttribute), true)
                 where attributes != null && attributes.Length > 0
                 select new AttributeObject<TAttribute> { Type = type, Property = property, Attribute = attributes.Cast<TAttribute>().FirstOrDefault() })
                .ToList();
        }        
    }
}
