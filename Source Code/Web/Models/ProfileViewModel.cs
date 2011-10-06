using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entites;
using JobZoom.Web.JobZoomServiceReference;
using System.Web.Mvc;

namespace JobZoom.Web.Models
{
    public class ProfileViewModel
    {
        public Jobseeker Jobseeker { get; set; }
        public List<Jobseeker_Experience> Education { get; set; }
        public List<Jobseeker_Language> Language { get; set; }
        public List<Jobseeker_Experience> WorkExperience { get; set; }
        public List<Jobseeker_Skill> Skill { get; set; }
        public List<Jobseeker_Project> Project { get; set; }
        public List<Jobseeker_HonorAward> HonorAward { get; set; }
        public List<SelectListItem> Gender { get; set; }
        public List<SelectListItem> MaritalStatus { get; set; }
        public List<SelectListItem> Proficiency { get; set; }

        public ProfileViewModel() { }

        public ProfileViewModel(string userID)
        {
            JobZoomServiceClient client = new JobZoomServiceClient();
            Jobseeker = client.GetJobseeker(userID);
            Education = client.GetAllEducation(userID);
            Language = client.GetAllProfileLanguage(userID);
            WorkExperience = client.GetAllExperience(userID);
            Skill = client.GetAllSkill(userID);
            HonorAward = client.GetAllHonorAward(userID);
            Project = client.GetAllProject(userID);

            Gender = new List<SelectListItem>();
            Gender.Add(new SelectListItem { Text = "Male", Value = "M" });
            Gender.Add(new SelectListItem { Text = "Female", Value = "F" });

            MaritalStatus = new List<SelectListItem>();
            MaritalStatus.Add(new SelectListItem { Text = "Single", Value = "S" });
            MaritalStatus.Add(new SelectListItem { Text = "Married", Value = "M" });

            Proficiency = new List<SelectListItem>();
            Proficiency.Add(new SelectListItem { Text = "Elementary", Value = "Elementary" });
            Proficiency.Add(new SelectListItem { Text = "Limited working", Value = "Limited working" });
            Proficiency.Add(new SelectListItem { Text = "Professional working", Value = "Professional working" });
            Proficiency.Add(new SelectListItem { Text = "Full professional working", Value = "Full professional working" });
            Proficiency.Add(new SelectListItem { Text = "Native or bilingual", Value = "Native or bilingual" });  
        }
    }
}