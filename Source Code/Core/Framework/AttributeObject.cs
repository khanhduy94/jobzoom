using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace JobZoom.Core
{
    public class AttributeObject<T>
    {
        public Type Type { get; set; }
        public T Attribute { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
