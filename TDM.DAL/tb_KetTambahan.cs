//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class tb_KetTambahan
    {
        public int Id { get; set; }
        public int SPKId { get; set; }
        public bool IsKaroseri { get; set; }
        public string Karoseri { get; set; }
        public bool IsOnTheRoad { get; set; }
        public bool IsOffTheRoad { get; set; }
        public bool IsChooseNo { get; set; }
        public string PlatNo { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual tb_spkHdr tb_spkHdr { get; set; }
    }
}