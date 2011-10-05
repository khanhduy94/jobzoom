using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JobZoom.Business.Entites
{
    [MetadataType(typeof(JobseekerMetadata))]
    public partial class Jobseeker
    {
        public class JobseekerMetadata
        {
            [ScaffoldColumn(false)]
            public object ID { get; set; }

            [Required()]
            public object FirstName { get; set; }

            [Required()]
            public object LastName { get; set; }

            [Required()]
            public object Gender { get; set; }

            [Required()]
            [DataType(DataType.Date)]
            public object Birthdate { get; set; }

            [Required()]
            public object MaritalStatus { get; set; }

            [Required()]
            public object Citizenship { get; set; }

            [Required()]
            public object AddressLine1 { get; set; }

            [Required()]
            public object CityID { get; set; }

            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$")]
            public object Phone { get; set; }

            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$")]
            public object Mobile { get; set; }

            [DataType(DataType.Url)]
            public object Website { get; set; }
        }
    }
}
