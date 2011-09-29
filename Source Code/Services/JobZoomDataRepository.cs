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

        public enum ExperienceType 
        { 
            EducationExperience,
            WorkExperience
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

        public Jobseeker_Experience GetExperience(string id)
        {
            return dataContext.Jobseeker_Experience.First(j => j.ID == id);
        }

        public List<Jobseeker_Experience> GetEducation(string userID)
        {
            return dataContext.Jobseeker_Experience.
                Where(j => j.UserID == userID && j.ExperienceType == (int)ExperienceType.EducationExperience).
                OrderBy(e => e.StartDate).ToList();
        }

        public bool AddEducation(Jobseeker_Experience model)
        {
            try
            {
                model.ExperienceType = (int)ExperienceType.EducationExperience;
                model.ModifiedDate = DateTime.Now;                
                dataContext.Jobseeker_Experience.AddObject(model);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveExperience(Jobseeker_Experience model)
        {
            try
            {
                var education = dataContext.Jobseeker_Experience.First(j => j.ID == model.ID);
                education.Name = model.Name;
                education.Location = model.Location;
                education.Title = model.Title;
                education.Industry = model.Industry;
                education.StartDate = model.StartDate;
                education.EndDate = model.EndDate;
                education.Description = model.Description;
                education.ModifiedDate = DateTime.Now;
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Jobseeker_Experience> GetWorkExperience(string userID)
        {
            return dataContext.Jobseeker_Experience.
                Where(j => j.UserID == userID && j.ExperienceType == (int)ExperienceType.WorkExperience).
                OrderBy(e => e.StartDate).ToList();
        }

        public bool AddWorkExperience(Jobseeker_Experience model)
        {
            try
            {
                model.ExperienceType = (int)ExperienceType.WorkExperience;
                model.ModifiedDate = DateTime.Now;
                dataContext.Jobseeker_Experience.AddObject(model);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }       

        public bool DeleteExperience(string id)
        {
            try
            {
                var education = dataContext.Jobseeker_Experience.Where(e => e.ID == id).First();
                dataContext.DeleteObject(education);
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