using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Business.Entities
{
    public class Countries
    {
        public List<string> GetCountries
        {
            set { }
            get
            {
                return new List<string>
                {
                    "Vietnam",
                    "United State"
                };
            }
        }
    }
}
