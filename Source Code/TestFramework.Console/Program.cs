using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.FlexibleAttributes;

namespace TestFramework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Profile_Basic basic = new Profile_Basic();
            basic.FirstName = "Phúc";

            TagAttributeMappingManager test = new TagAttributeMappingManager();
            test.AddAttributeObject(basic, basic.ProfileBasicId, Guid.Empty);
            
            
        }
    }
}
