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

    public partial class ContentAssign
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContentAssign()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int CTassignID { get; set; }
        
        public Nullable<int> CTID { get; set; }
        [DisplayName("MC")]

        public string MCID { get; set; }
        public Nullable<int> StatusID { get; set; }
        [DisplayName("Feedback")]
        public string CommentA { get; set; }
        [DisplayName("Topic")]
        public string TopicID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Content Content { get; set; }
        public virtual MarketingCoordinator MarketingCoordinator { get; set; }
        public virtual Status Status { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
