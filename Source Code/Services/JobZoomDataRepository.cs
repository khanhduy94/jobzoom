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

        public List<Language> GetLanguages()
        {
            return dataContext.Languages.ToList();
        }


        public Jobseeker GetJobseeker(string userID)
        {
            return dataContext.Jobseekers.First(j => j.UserID == userID);
        }

        public bool SavePersonalInfo(Jobseeker model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                var jobseeker = dataContext.Jobseekers.First(j => j.UserID == model.UserID);
                dataContext.Attach(jobseeker);
                dataContext.ApplyCurrentValues(jobseeker.EntityKey.EntitySetName, model);
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
                model.ModifiedDate = DateTime.Now;
                var education = dataContext.Jobseeker_Experience.First(j => j.ID == model.ID);
                dataContext.Attach(education);
                dataContext.ApplyCurrentValues(education.EntityKey.EntitySetName, model);                
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
                var education = dataContext.Jobseeker_Experience.First(e => e.ID == id);
                dataContext.DeleteObject(education);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Jobseeker_Skill> GetSkill(string userID)
        {
            return dataContext.Jobseeker_Skill.Where(s => s.UserID == userID).ToList();
        }

        public bool AddSkill(Jobseeker_Skill model)
        {
            try
            {                
                model.ModifiedDate = DateTime.Now;
                dataContext.Jobseeker_Skill.AddObject(model);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSkill(string id)
        {
            try
            {
                var skill = dataContext.Jobseeker_Skill.First(e => e.ID == id);
                dataContext.DeleteObject(skill);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Jobseeker_HonorAward GetHonorAward(string id)
        {
            return dataContext.Jobseeker_HonorAward.First(s => s.ID == id);
        }

        public List<Jobseeker_HonorAward> GetAllHonorAward(string userID)
        {
            return dataContext.Jobseeker_HonorAward.Where(s => s.UserID == userID).ToList();
        }

        public bool AddHonorAward(Jobseeker_HonorAward model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                dataContext.Jobseeker_HonorAward.AddObject(model);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveHonorAward(Jobseeker_HonorAward model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                var honorAward = dataContext.Jobseeker_HonorAward.First(j => j.ID == model.ID);
                dataContext.Attach(honorAward);
                dataContext.ApplyCurrentValues(honorAward.EntityKey.EntitySetName, model);                   
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteHonorAward(string id)
        {
            try
            {
                var skill = dataContext.Jobseeker_HonorAward.First(e => e.ID == id);
                dataContext.DeleteObject(skill);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Jobseeker_Language GetProfileLanguage(string id)
        {
            return dataContext.Jobseeker_Language.First(s => s.ID == id);
        }

        public List<Jobseeker_Language> GetAllProfileLanguage(string userID)
        {
            return dataContext.Jobseeker_Language.Where(s => s.UserID == userID).ToList();
        }

        public bool AddProfileLanaguage(Jobseeker_Language model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                dataContext.Jobseeker_Language.AddObject(model);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveProfileLanguage(Jobseeker_Language model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                var language = dataContext.Jobseeker_Language.First(j => j.ID == model.ID);
                dataContext.Attach(language);
                dataContext.ApplyCurrentValues(language.EntityKey.EntitySetName, model);    
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProfileLanguage(string id)
        {
            try
            {
                var language = dataContext.Jobseeker_Language.First(e => e.ID == id);
                dataContext.DeleteObject(language);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Jobseeker_Project GetProject(string id)
        {
            return dataContext.Jobseeker_Project.First(s => s.ID == id);
        }

        public List<Jobseeker_Project> GetAllProject(string userID)
        {
            return dataContext.Jobseeker_Project.Where(s => s.UserID == userID).ToList();
        }

        public bool AddProject(Jobseeker_Project model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                dataContext.Jobseeker_Project.AddObject(model);
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveProject(Jobseeker_Project model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                var project = dataContext.Jobseeker_Project.First(j => j.ID == model.ID);
                dataContext.Attach(project);
                dataContext.ApplyCurrentValues(project.EntityKey.EntitySetName, model);    
                dataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProject(string id)
        {
            try
            {
                var project = dataContext.Jobseeker_Project.First(e => e.ID == id);
                dataContext.DeleteObject(project);
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