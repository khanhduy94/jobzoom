using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Web.JobZoomServiceReference;
using JobZoom.Business.Entites;

namespace JobZoom.Web.Models
{
    public class ProfileViewModel
    {
        public Jobseeker Jobseeker { get; set; }
        public List<Jobseeker_Experience> Education { get; set; }
        public List<Jobseeker_Experience> WorkExperience { get; set; }
        public List<Jobseeker_Skill> Skill { get; set; }
        public List<Jobseeker_Project> Project { get; set; }
        public List<Jobseeker_HonorAward> HonorAward { get; set; }

        public ProfileViewModel() { }

        public ProfileViewModel(string userID)
        {
            JobZoomServiceClient client = new JobZoomServiceClient();
            Jobseeker = client.GetJobseeker(userID);
            Education = client.GetAllEducation(userID);
            WorkExperience = client.GetAllExperience(userID);
            Skill = client.GetAllSkill(userID);
            HonorAward = client.GetAllHonorAward(userID);
            Project = client.GetAllProject(userID);
        }
    }
}