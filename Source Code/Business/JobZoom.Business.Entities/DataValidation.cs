using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JobZoom.Business.Entities
{
    #region Profile_Basic

    [MetadataType(typeof(Profile_Basic_Validation))]
    public partial class Profile_Basic
    {
        public class Profile_Basic_Validation
        {
            #region Properties

            [Required(ErrorMessage="Required")]
            [Display(Name="First Name")]
            public object FirstName
            {
                get;
                set;
            }

            [Required(ErrorMessage = "Required")]
            [Display(Name = "Last Name")]
            public object LastName
            {
                get;
                set;
            }

            [Required(ErrorMessage = "Required")]
            [DataType(DataType.Date)]
            [Display(Name = "Birthdate")]
            public object Birthdate
            {
                get;
                set;
            }            

            [Required(ErrorMessage = "Required")]
            [Display(Name = "Marital Status")]
            public object MaritalStatus
            {
                get;
                set;
            }
            #endregion
        }
    }
    #endregion
}

