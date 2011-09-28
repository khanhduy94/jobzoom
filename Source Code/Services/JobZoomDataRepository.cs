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

        public List<City> GetCities()
        {
            return dataContext.Cities.ToList();
        }

        public Jobseeker GetJobseeker(string userID)
        {
            return dataContext.Jobseekers.Where(j => j.UserID == userID).First();
        }

        public bool SavePersonalInfo(Jobseeker model)
        {
            try
            {
                var jobseeker = dataContext.Jobseekers.First(j => j.UserID == model.UserID);                
                jobseeker.FirstName = model.FirstName;
                jobseeker.LastName = model.LastName;
                jobseeker.Gender = model.Gender;
                jobseeker.Birthdate = model.Birthdate;
                jobseeker.Citizenship = model.Citizenship;
                jobseeker.Country = model.Country;
                jobseeker.CityID = model.CityID;
                jobseeker.MaritalStatus = model.MaritalStatus;
                jobseeker.Phone = model.Phone;
                jobseeker.Mobile = model.Mobile;
                jobseeker.Picture = model.Picture;
                jobseeker.AddressLine1 = model.AddressLine1;
                jobseeker.AddressLine2 = model.AddressLine2;
                jobseeker.AdditionalInfo = model.AdditionalInfo;
                jobseeker.Website = model.Website;
                jobseeker.ModifiedDate = DateTime.Now;
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}