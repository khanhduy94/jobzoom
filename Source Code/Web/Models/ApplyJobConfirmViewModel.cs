using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobZoom.Web.Models
{
    public class JobApplyConfirmViewModel
    {
        public string UserId { get; set; }
        public Guid JobId { get; set; }
    }
}