using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Business.Entities
{
    public class MaritalStatus
    {
        public List<string> MaritalStatusList
        {
            set { }
            get
            {
                return new List<string>
                {
                    "Single",
                    "Married"                    
                };
            }
        }
    }
}
