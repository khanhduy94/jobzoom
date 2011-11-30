using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Business.Entities
{
    public class Genders
    {
        public List<string> GetGenders 
        {
            set { }
            get
            {
                return new List<string>
                {
                    "Male",
                    "Female"                    
                };
            }
        }
    }
}
