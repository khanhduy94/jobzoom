using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Core.Entities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    partial class TagAttribute : Attribute
    {        
        
    }
}
