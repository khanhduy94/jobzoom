//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobZoom.Business.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Profile_Basic = new HashSet<Profile_Basic>();
            this.Profile_Education = new HashSet<Profile_Education>();
            this.Profile_Work = new HashSet<Profile_Work>();
        }
    
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Profile_Basic> Profile_Basic { get; set; }
        public virtual ICollection<Profile_Education> Profile_Education { get; set; }
        public virtual ICollection<Profile_Work> Profile_Work { get; set; }
    }
}
