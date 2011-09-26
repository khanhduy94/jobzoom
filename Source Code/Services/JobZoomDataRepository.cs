using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entites;

namespace JobZoom.Services
{
    public class JobZoomDataRepository
    {
        JobZoomEntities dataContext;
        public JobZoomDataRepository()
        {
            dataContext = new JobZoomEntities();
        }

        public List<Country> GetCountries()
        {
            return dataContext.Countries.ToList();
        }
    }
}