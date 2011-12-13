using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JobZoom.Business.Entities;

namespace Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IJobZoomService
    {
        [OperationContract]       
        bool ValidateUser(string username, string password);

        [OperationContract(Name= "GetProfileBasicByUserId")]
        Profile_Basic GetProfileBasic(string userId);

        [OperationContract]
        void SaveProfileBasic(Profile_Basic profile_basic);
    }    
}
