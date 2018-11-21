using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
using TDM.WEB.Models;
using Newtonsoft.Json;
using System.IO;
namespace TDM.WEB.Controllers
{
    public class SPKController : BaseController
    {
        // GET: SPK
        public ActionResult Index()
        {
            return CheckSession();
        }

        public ActionResult CreateSPK()
        {
            return CheckSession();
        }

        public JsonResult SubmitSPK()
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            string _retId = "0";
            string _errMsg = string.Empty;
            List<SPKAttachmentModel> _lsAttachment = new List<SPKAttachmentModel>();
            List<SPKAdditionalModel> _lsAdditional = new List<SPKAdditionalModel>();
            SPKHeaderModel _spk = new SPKHeaderModel();
            SPKEquipmentModel _misc = new SPKEquipmentModel();
            if (Session["UserLogon"] != null)
            {
                _spk.SPKDate = DateTime.Parse(Request.Form["TglSPK"].ToString());
                _spk.JanjiPenyerahan = DateTime.Parse(Request.Form["JanjiSerah"].ToString());
                _spk.Branch = Request.Form["Branch"].ToString();
                _spk.LOTNo = Request.Form["Lot"].ToString();
                _spk.Buyer = Request.Form["BuyerName"].ToString();
                _spk.BuyerAddress = Request.Form["BuyerAddress"].ToString();
                _spk.KTP = Request.Form["KTP"].ToString();
                _spk.Phone = Request.Form["Phone"].ToString();
                _spk.STNKName = Request.Form["STNKName"].ToString();
                _spk.STNKAddress = Request.Form["AlamatSTNK"].ToString();
                _spk.Email = Request.Form["Email"].ToString();
                _spk.Merk = Request.Form["Merk"].ToString();
                _spk.Warna = Request.Form["Warna"].ToString();
                _spk.Year = Request.Form["Tahun"] == "" ? DateTime.Now.Year : int.Parse(Request.Form["Tahun"].ToString());
                _spk.Karoseri = Request.Form["Karoseri"].ToString();
                _spk.MachineNo = Request.Form["MachineNo"].ToString();
                _spk.RangkaNo = Request.Form["RangkaNo"].ToString();
                _spk.Pembiayaan = Request.Form["Pembiayaan"].ToString();
                _spk.Via = Request.Form["Via"].ToString();
                _spk.PercentageBunga = float.Parse(Request.Form["Bunga"].ToString());
                _spk.OTRPrice = float.Parse(Request.Form["HargaOnTheRoad"].ToString());
                _spk.KaroseriPrice = float.Parse(Request.Form["HargaKaroseri"].ToString());
                _spk.TotalPrice = float.Parse(Request.Form["Total"].ToString());
                _spk.DP = float.Parse(Request.Form["DP"].ToString());
                _spk.TandaJadi = Request.Form["TandaJadi"].ToString();
                _spk.Pembayaran = Request.Form["Pembayaran"].ToString();
                _spk.TransferVia = Request.Form["TransferVia"].ToString();
                _spk.AlamatKirim = Request.Form["AlamatKirim"].ToString();
                _spk.City = int.Parse(Request.Form["City"].ToString());
                _spk.CreatedDate = DateTime.Now;
                _spk.CreatedBy = Utilities.GetUserNameLogon((UserAppsModel)Session["UserLogon"]);
                if (Request.Form["IsKaroseri"].ToString() == "true")
                {
                    _spk.IsKaroseri = true;
                }
                else
                {
                    _spk.IsKaroseri = false;
                }

                if (Request.Form["IsOntheRoad"].ToString() == "true")
                {
                    _spk.IsOnTR = true;
                }
                else
                {
                    _spk.IsOnTR = false;
                }

                if (Request.Form["IsOffTheRoad"].ToString() == "true")
                {
                    _spk.IsOffTR = true;
                }
                else
                {
                    _spk.IsOffTR = false;
                }

                if (Request.Form["IsNoPilihan"].ToString() == "true")
                {
                    _spk.IsChooseNo = true;
                }
                else
                {
                    _spk.IsChooseNo = false;
                }

                _spk.KaroseriDesc = Request.Form["KaroseriDesc"].ToString();
                _spk.ChooseNo = Request.Form["NoPilihan"].ToString();

                _lsAdditional = JsonConvert.DeserializeObject<List<SPKAdditionalModel>>(Request.Form["Additional"]);

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        var fileName = Path.GetFileName(fileContent.FileName);
                        var reader = new System.IO.BinaryReader(stream);
                        var Content = reader.ReadBytes(fileContent.ContentLength);

                        _lsAttachment.Add(new SPKAttachmentModel
                        {
                            DocType = new MasterBLL().FindByValue("SPK", MyEnums.enumMaster.DocumentType.ToString()),
                            AttachmentName = fileName,
                            Attachment = Content,
                        });
                    }
                }

                int _idocType = new MasterBLL().FindByValue("SPK", MyEnums.enumMaster.DocumentType.ToString());
                int result = new SPKBll().Insert(_spk, _lsAdditional, _misc, _lsAttachment, _idocType, out _errMsg);
                if (result <= 0 || _errMsg != string.Empty)
                {
                    _status = MyEnums.enumStatus.ERROR.ToString();
                }
                else
                {
                    _retId = result.ToString();
                }
            }
            else
            {
                _status = MyEnums.enumStatus.ERROR.ToString();
                _errMsg = "Your session Expired";
            }
            return Json(new { Status = _status, RetId = _retId, ErrMsg = _errMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SPKApproval()
        {
            string _spkId = "";
            if (Request.QueryString["DocId"] != null)
            {
                _spkId = Request.QueryString["DocId"].ToString();
                ViewBag.IDSpk = _spkId;
            }

            if (Request.QueryString["Type"] != null)
            {
                ViewBag.TypeID = Request.QueryString["Type"].ToString();
            }

            if (Request.QueryString["Level"] != null)
            {
                ViewBag.Level = Request.QueryString["Level"].ToString();
            }
            return CheckSession();
        }

        public JsonResult DeleteAttachment(string id)
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            
            int result_remove = new SPKBll().DeleteAttachment(Convert.ToInt32( id));
            if (result_remove <= 0)
            {
                _status = MyEnums.enumStatus.ERROR.ToString();
            }
            return Json(new { Status = _status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadAttachment(string id)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            SPKBll _spkbllobj = new SPKBll();
         
                SPKAttachmentModel _attFile = _spkbllobj.GetDocAttachment(Convert.ToInt32(id));
                Response.ClearContent();
                // adding the headers to file stream
                Response.AddHeader("Content-Disposition", "attachment; filename=" + _attFile.AttachmentName);
                //Streaming out the data(file) from database to client(browser)
                BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                // converting data into bytes
                byte[] file = (byte[])_attFile.Attachment;

                bw.Write(file);
                bw.Close();
                Response.End();
                return View();
           
        }

        public JsonResult Detail(string id)
        {
            SPKHeaderModel _spkModel = new SPKHeaderModel();
           
            _spkModel = new SPKBll().Detail(Convert.ToInt32( id));
            return Json(new
            {
                Id = _spkModel.Id
                ,
                SPKDate = _spkModel.SPKDate.ToString("yyyy-MM-dd")
                ,
                JanjiSerah = _spkModel.JanjiPenyerahan.ToString("yyyy-MM-dd")
                ,
                LOTNo = _spkModel.LOTNo
                ,
                Buyer = _spkModel.Buyer
                ,
                BuyerAddress = _spkModel.BuyerAddress
                ,
                KTP = _spkModel.KTP
                ,
                Phone = _spkModel.Phone
                ,
                STNKName = _spkModel.STNKName
                ,
                STNKAddress = _spkModel.STNKAddress
                ,
                Email = _spkModel.Email
                ,
                Branch = _spkModel.Branch
                ,
                Merk = _spkModel.Merk
                ,
                Warna = _spkModel.Warna
                ,
                Karoseri = _spkModel.Karoseri
                ,
                MachineNo = _spkModel.MachineNo
                ,
                RangkaNo = _spkModel.RangkaNo
                ,
                Pembiayaan = _spkModel.Pembiayaan
                ,
                Via = _spkModel.Via
                ,
                PercentageBunga = _spkModel.PercentageBunga
                ,
                OTRPrice = _spkModel.OTRPrice
                ,
                KaroseriPrice = _spkModel.KaroseriPrice
                ,
                Year = _spkModel.Year
                ,
                TotalPrice = _spkModel.TotalPrice
                ,
                DP = _spkModel.DP
                ,
                TandaJadi = _spkModel.TandaJadi
                ,
                Pembayaran = _spkModel.Pembayaran
                ,
                TransferVia = _spkModel.TransferVia
                ,
                AlamatKirim = _spkModel.AlamatKirim
                ,
                City = _spkModel.City
                ,
                IsKaroseri = _spkModel.IsKaroseri
                ,
                KaroseriDesc = _spkModel.KaroseriDesc
                ,
                IsOnTR = _spkModel.IsOnTR
                ,
                IsOffTR = _spkModel.IsOffTR
                ,
                IsChooseNo = _spkModel.IsChooseNo
                ,
                PlatNo = _spkModel.ChooseNo
                ,
                Additional = _spkModel.lsadditional
                ,
                Attachment = _spkModel.lsspkattachment
            }, JsonRequestBehavior.AllowGet);
        }
    }
}