using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Business.Entities
{
    public class City
    {
        public List<string> GetCities
        {
            set
            {
            }
            get
            {
                return new List<string>
                {
                    "Tp.Hồ Chí Minh",
                    "Hà Nội"
                };
            }
        }
    }
}
