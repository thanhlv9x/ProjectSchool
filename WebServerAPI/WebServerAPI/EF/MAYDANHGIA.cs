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
    
    public partial class MAYDANHGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAYDANHGIA()
        {
            this.TRANGTHAIDANGNHAPs = new HashSet<TRANGTHAIDANGNHAP>();
        }
    
        public int MAMAY { get; set; }
        public string MAC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRANGTHAIDANGNHAP> TRANGTHAIDANGNHAPs { get; set; }
    }
}