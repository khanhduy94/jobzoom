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

            TagAttributeMappingManager test = new TagAttributeMappingManager();
            test.AddAttributeObject(basic, basic.ProfileBasicId, "Profile", Guid.Empty);
        }
    }
}
