//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebServerAPI.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class KETQUADANHGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KETQUADANHGIA()
        {
            this.GOPies = new HashSet<GOPY>();
        }
    
        public int MADG { get; set; }
        public Nullable<int> MUCDO { get; set; }
        public Nullable<System.DateTime> TG { get; set; }
        public int MASTT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GOPY> GOPies { get; set; }
        public virtual SOTHUTU SOTHUTU { get; set; }
        public virtual MUCDODANHGIA MUCDODANHGIA { get; set; }
    }
}
