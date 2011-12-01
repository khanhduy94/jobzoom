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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class JobZoomService: IJobZoomService
    {
        private JobZoomRepository _repository;
        public JobZoomService()
        {
            _repository = new JobZoomRepository();
        }

        public bool ValidateUser(string username, string password)
        {
            return _repository.ValidateUser(username, password);
        }

        public Profile_Basic GetProfileBasic(string userId)
        {
            return _repository.GetProfileBasicByUserId(userId);
        }

        public void SaveProfileBasic(Profile_Basic profile_basic)
        {
            _repository.SaveProfileBasic(profile_basic);
        }
    }
}
