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

        public List<Country> GetCountries()
        {            
            return repository.GetCountries();
        }

        public List<City> GetCities()
        {
            return repository.GetCities();
        }

        public List<Language> GetLanguages()
        {
            return repository.GetLanguages();
        }

        public Jobseeker GetJobseeker(string userID)
        {
            return repository.GetJobseeker(userID);
        }

        public bool SavePersonalInfo(Jobseeker model)
        {
            return repository.SavePersonalInfo(model);
        }

        public Jobseeker_Experience GetExperience(string id)
        {
            return repository.GetExperience(id);
        }

        public List<Jobseeker_Experience> GetEducation(string userID)
        {
            return repository.GetEducation(userID);
        }

        public bool AddEducation(Jobseeker_Experience model)
        {
            return repository.AddEducation(model);
        }

        public List<Jobseeker_Experience> GetWorkExperience(string userID)
        {
            return repository.GetWorkExperience(userID);
        }

        public bool AddWorkExperience(Jobseeker_Experience model)
        {
            return repository.AddWorkExperience(model);
        }

        public bool SaveExperience(Jobseeker_Experience model)
        {
            return repository.SaveExperience(model);
        }

        public bool DeleteExperience(string id)
        {
            return repository.DeleteExperience(id);
        }

        public List<Jobseeker_Skill> GetSkill(string userID)
        {
            return repository.GetSkill(userID);
        }

        public bool AddSkill(Jobseeker_Skill model)
        {
            return repository.AddSkill(model);
        }

        public bool DeleteSkill(string id)
        {
            return repository.DeleteSkill(id);
        }

        public List<Jobseeker_HonorAward> GetAllHonorAward(string userID)
        {
            return repository.GetAllHonorAward(userID);
        }

        public Jobseeker_HonorAward GetHonorAward(string userID)
        {
            return repository.GetHonorAward(userID);
        }

        public bool AddHonorAward(Jobseeker_HonorAward model)
        {
            return repository.AddHonorAward(model);
        }

        public bool SaveHonorAward(Jobseeker_HonorAward model)
        {
            return repository.SaveHonorAward(model);
        }

        public bool DeleteHonorAward(string id)
        {
            return repository.DeleteHonorAward(id);
        }

        public List<Jobseeker_Language> GetAllProfileLanguage(string userID)
        {
            return repository.GetAllProfileLanguage(userID);
        }

        public Jobseeker_Language GetProfileLanguage(string userID)
        {
            return repository.GetProfileLanguage(userID);
        }

        public bool AddProfileLanguage(Jobseeker_Language model)
        {
            return repository.AddProfileLanaguage(model);
        }

        public bool SaveProfileLanguage(Jobseeker_Language model)
        {
            return repository.SaveProfileLanguage(model);
        }

        public bool DeleteProfileLanguage(string id)
        {
            return repository.DeleteProfileLanguage(id);
        }

        public List<Jobseeker_Project> GetAllProject(string userID)
        {
            return repository.GetAllProject(userID);
        }

        public Jobseeker_Project GetProject(string userID)
        {
            return repository.GetProject(userID);
        }

        public bool AddProject(Jobseeker_Project model)
        {
            return repository.AddProject(model);
        }

        public bool SaveProject(Jobseeker_Project model)
        {
            return repository.SaveProject(model);
        }

        public bool DeleteProject(string id)
        {
            return repository.DeleteProject(id);
        }
    }
}
