﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HETHONGDANHGIAsaEntities : DbContext
    {
        public HETHONGDANHGIAsaEntities()
            : base("name=HETHONGDANHGIAsaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BOPHAN> BOPHANs { get; set; }
        public virtual DbSet<CANBO> CANBOes { get; set; }
        public virtual DbSet<GOPY> GOPies { get; set; }
        public virtual DbSet<KETQUADANHGIA> KETQUADANHGIAs { get; set; }
        public virtual DbSet<MAYDANHGIA> MAYDANHGIAs { get; set; }
        public virtual DbSet<MUCDODANHGIA> MUCDODANHGIAs { get; set; }
        public virtual DbSet<SOTHUTU> SOTHUTUs { get; set; }
        public virtual DbSet<SOTOIDA> SOTOIDAs { get; set; }
        public virtual DbSet<TAIKHOANADMIN> TAIKHOANADMINs { get; set; }
        public virtual DbSet<TRANGTHAIDANGNHAP> TRANGTHAIDANGNHAPs { get; set; }
    }
}
