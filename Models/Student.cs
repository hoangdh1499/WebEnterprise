//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebEnterprise.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Contents = new HashSet<Content>();
        }
        [DisplayName("ID")]
        [Required(ErrorMessage = "Field can't be empty")]
        public string StudentID { get; set; }
        [DisplayName("Student")]
        [Required(ErrorMessage = "Field can't be empty")]
        public string StudentName { get; set; }
        [DisplayName("Address")]
        public string StudentAddress { get; set; }

        //[DataType(DataType.Date)]
        [DisplayName("Birthday")]
        public System.DateTime DOB { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string UserName { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]  
        public string StudentEmail { get; set; }
        [DisplayName("Faculty")]
        public Nullable<int> FacultyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Content> Contents { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
