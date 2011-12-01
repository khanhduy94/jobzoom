    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Business.Entities
{
    public class Industry
    {
        public List<string> GetIndustry
        {
            set
            {
            }
            get
            {
                return new List<string>
                {
                    "Banking",
                    "Finance",
                    "Marketing",
                    "Computer Software",
                    "Computer Hardware"
                };
            }
        }
    }
}
