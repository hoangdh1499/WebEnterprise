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
    
    public partial class Guest
    {
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public string GuestAddress { get; set; }
        public string GuestPhone { get; set; }
        public Nullable<int> FacultyID { get; set; }
        public string UserName { get; set; }
        public string GuestEmail { get; set; }
    
        public virtual Faculty Faculty { get; set; }
    }
}
