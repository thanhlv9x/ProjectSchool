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
    
    public partial class SOTHUTU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SOTHUTU()
        {
            this.KETQUADANHGIAs = new HashSet<KETQUADANHGIA>();
        }
    
        public int MASTT { get; set; }
        public Nullable<int> STT { get; set; }
        public Nullable<int> MACB { get; set; }
        public Nullable<System.DateTime> BD { get; set; }
        public Nullable<System.DateTime> KT { get; set; }
    
        public virtual CANBO CANBO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KETQUADANHGIA> KETQUADANHGIAs { get; set; }
    }
}
