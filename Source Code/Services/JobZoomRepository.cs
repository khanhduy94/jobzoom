using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;
using System.Data;

namespace Services
{
    public class JobZoomRepository
    {
        private JobZoomEntities _entities;

        public JobZoomRepository()
        {
            _entities = new JobZoomEntities();
        }

        public Profile_Basic GetProfileBasicByUserId(string userId)
        {
            return _entities.Profile_Basic.Single(x => x.UserId == userId);
        }

        public void SaveProfileBasic(Profile_Basic profile_basic)
        {
            _entities.Profile_Basic.Attach(profile_basic);
            _entities.ObjectStateManager.ChangeObjectState(profile_basic, EntityState.Modified);
            _entities.SaveChanges();
        }

        public bool ValidateUser(string username, string password)
        {
            int userCount =_entities.Users.Where(x => x.UserId == username && x.Password == password).Count();
            if (userCount == 0)
                return false;
            else
                return true;
        }
    }
}