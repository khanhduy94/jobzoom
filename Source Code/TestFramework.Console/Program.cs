using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.FlexibleAttributes;
using JobZoom.Core.Taxonomy;

namespace TestFramework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Profile_Basic basic = new Profile_Basic
            {
                FirstName = "Phúc",
                LastName = "Lê Dương Công",
                Gender = "Nam",
                Birthdate = DateTime.Now,
                MaritalStatus = "Single",
                AddressLine1 = "123 Trần Hưng Đạo",
                AddressLine2 = "93 Cao Thắng",
                Country = "Vietnam",
                City = "848"
            };            

            //TagAttributeMappingManager test = new TagAttributeMappingManager();
            //test.AddAttributeObject(basic, basic.ProfileBasicId, "Profile", Guid.Empty);

            Hierarchy hierarchy = new Hierarchy();
            Tag tag = hierarchy.GetHierarchicalTreeByObject(new Guid("B121BDDF-7A43-4DE4-9048-7FF1C90EAD9B"), "JobSeekerProfile");

        }
    }
}
