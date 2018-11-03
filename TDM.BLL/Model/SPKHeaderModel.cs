using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TDM.BLL.Model
{
    public class SPKHeaderModel : BaseModel
    {
        
        [Required]
        [DisplayName("Tanggal SPK")]
        public DateTime SPKDate { get; set; }
        [DisplayName("Janji Penyerahan")]
        public DateTime JanjiPenyerahan { get; set; }
        [DisplayName("No.LOT")]
        public string LOTNo { get; set; }
        [Required]
        [DisplayName("Nama Pembeli")]
        public string Buyer { get; set; }
        [Required]
        [DisplayName("Alamat")]
        public string BuyerAddress { get; set; }
        [Required]
        [DisplayName("No.KTP")]
        public string KTP { get; set; }
        [DisplayName("Telp/HP")]
        public string Phone { get; set; }
        [Required]
        [DisplayName("Nama di STNK")]
        public string STNKName { get; set; }
        [DisplayName("Alamat")]
        public string STNKAddress { get; set; }

        public string Email { get; set; }
        public string Branch { get; set; }
        [Required]
        public string Merk { get; set; }

        public string Warna { get; set; }
        [Required]
        public string Karoseri { get; set; }
        [Required]
        [DisplayName("Tahun")]
        public int Year { get; set; }
        [Required]
        [DisplayName("Nomor Mesin")]
        public string MachineNo { get; set; }
        [Required]
        [DisplayName("Nomor Rangka")]
        public string RangkaNo { get; set; }
        [Required]
        public int Pembiayaan { get; set; }

        public string Via { get; set; }

        [DisplayName("Bunga")]
        public float PercentageBunga { get; set; }
        [DisplayName("Harga On The Road")]
        public float OTRPrice { get; set; }

        [DisplayName("Harga Karoseri")]
        public float KaroseriPrice { get; set; }

        [DisplayName("Total")]
        public float TotalPrice { get; set; }

        [DisplayName("DP/Uang Muka")]
        public float DP { get; set; }

        [DisplayName("Tanda Jadi")]
        public string TandaJadi { get; set; }

        [DisplayName("Pembayaran")]
        public string Pembayaran { get; set; }

        [DisplayName("Transfer Via")]
        public string TransferVia { get; set; }

        [DisplayName("Alamat Pengiriman")]
        public string AlamatKirim { get; set; }
        [DisplayName("Kota")]
        public int City { get; set; }
    }

    public class SPKEquipment :BaseModel
    {
        public int SPKId{get;set;}
        public bool IsKaroseri { get; set; }
        public string Karoseri { get; set; }
        public bool IsOnTheRoad { get; set; }
        public bool IsOffTheRoad { get; set; }
        public bool IsChooseNo { get; set; }
        public string PlatNo { get; set; }
    }

    public class SPKAttachment :BaseModel
    {
        public int DocId { get; set; }
        public string AttachmentName { get; set; }
        public byte[] Attachment { get; set; }
        public int DocType { get; set; }
    }
}
