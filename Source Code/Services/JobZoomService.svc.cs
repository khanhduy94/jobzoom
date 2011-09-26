using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JobZoom.Business.Entites;

namespace JobZoom.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class JobZoomService : IJobZoomService
    {
        JobZoomDataRepository repository;
        public JobZoomService()
        {
            repository = new JobZoomDataRepository();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


        public List<Business.Entites.Country> GetCountries()
        {            
            //return new List<Country>{new Country{ID="Vietnam", Name="Vietnam"},
            //    new Country{ID="US", Name="US"}};  

            return repository.GetCountries();
        }       
    }
}
