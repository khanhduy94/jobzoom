using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Core.FlexibleAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TagContractAttribute: Attribute
    {        
    }
}
