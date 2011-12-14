using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Core.FlexibleAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class TagMemberAttribute: Attribute
    {
        public TagMemberType Type { get; set; }
    }
}