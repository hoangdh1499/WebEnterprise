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
    
    public partial class Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comment()
        {
            this.ConTents = new HashSet<ConTent>();
            this.Replies = new HashSet<Reply>();
        }
    
        public string CommentID { get; set; }
        public string Comment1 { get; set; }
        public string StudentID { get; set; }
        public string MCID { get; set; }
    
        public virtual MarketingCoordinator MarketingCoordinator { get; set; }
        public virtual Student Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConTent> ConTents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
