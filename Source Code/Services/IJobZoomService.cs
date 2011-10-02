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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IJobZoomService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract(Name = "GetAllCountries")]
        List<Country> GetCountries();

        [OperationContract(Name = "GetAllCities")]
        List<City> GetCities();

        [OperationContract]
        Jobseeker GetJobseeker(string userID);

        [OperationContract]
        bool SavePersonalInfo(Jobseeker model);

        [OperationContract]
        Jobseeker_Experience GetExperience(string id);

        [OperationContract(Name = "GetAllEducation")]
        List<Jobseeker_Experience> GetEducation(string userID);

        [OperationContract(Name = "GetAllExperience")]
        List<Jobseeker_Experience> GetWorkExperience(string userID);

        [OperationContract]
        bool AddEducation(Jobseeker_Experience model);

        [OperationContract]
        bool AddWorkExperience(Jobseeker_Experience model);

        [OperationContract]
        bool SaveExperience(Jobseeker_Experience model);

        [OperationContract]
        bool DeleteExperience(string id);

        [OperationContract(Name = "GetAllSkill")]
        List<Jobseeker_Skill> GetSkill(string userID);

        [OperationContract]
        bool AddSkill(Jobseeker_Skill model);

        [OperationContract]
        bool DeleteSkill(string id);

        [OperationContract]
        List<Jobseeker_HonorAward> GetAllHonorAward(string userID);

        [OperationContract]
        Jobseeker_HonorAward GetHonorAward(string id);

        [OperationContract]
        bool AddHonorAward(Jobseeker_HonorAward model);

        [OperationContract]
        bool SaveHonorAward(Jobseeker_HonorAward model);

        [OperationContract]
        bool DeleteHonorAward(string id);

        [OperationContract]
        List<Jobseeker_Project> GetAllProject(string userID);

        [OperationContract]
        Jobseeker_Project GetProject(string id);

        [OperationContract]
        bool AddProject(Jobseeker_Project model);

        [OperationContract]
        bool SaveProject(Jobseeker_Project model);

        [OperationContract]
        bool DeleteProject(string id);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
