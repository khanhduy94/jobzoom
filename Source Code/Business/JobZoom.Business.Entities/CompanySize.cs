    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Business.Entities
{
    public class CompanySize
    {
        public List<string> GetCompanySizes
        {
            set
            {
            }
            get
            {
                return new List<string>
                {
                    "1-10",
                    "11-50",
                    "51-200",
                    "201-500",
                    "501-1000",
                    "1001-5000",
                    "5001-10,000",
                    "10,000+"
                };
            }
        }
    }
}
