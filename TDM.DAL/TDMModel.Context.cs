﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TDM.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TDMDBEntities : DbContext
    {
        public TDMDBEntities()
            : base("name=TDMDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tb_Attachment> tb_Attachment { get; set; }
        public virtual DbSet<tb_Master> tb_Master { get; set; }
        public virtual DbSet<tb_spkHdr> tb_spkHdr { get; set; }
        public virtual DbSet<tb_userApps> tb_userApps { get; set; }
        public virtual DbSet<tb_Worklist> tb_Worklist { get; set; }
        public virtual DbSet<tb_employee> tb_employee { get; set; }
        public virtual DbSet<tb_role> tb_role { get; set; }
        public virtual DbSet<tb_userRole> tb_userRole { get; set; }
        public virtual DbSet<tb_Worklist_Archive> tb_Worklist_Archive { get; set; }
        public virtual DbSet<tb_workflowSettingHdr> tb_workflowSettingHdr { get; set; }
        public virtual DbSet<tb_workflowSetting> tb_workflowSetting { get; set; }
        public virtual DbSet<tb_KetTambahan> tb_KetTambahan { get; set; }
        public virtual DbSet<tb_PerlTambahan> tb_PerlTambahan { get; set; }
    }
}
