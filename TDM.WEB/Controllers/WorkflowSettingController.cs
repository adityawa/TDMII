using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDM.BLL;
using TDM.BLL.Model;
using Newtonsoft.Json;
namespace TDM.WEB.Controllers
{
    public class WorkflowSettingController : BaseController
    {
        // GET: WorkflowSetting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return CheckSession();
        }

        public JsonResult RemoveSetting(string id)
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            string _errMsg = string.Empty;
            int result = new WorkflowSettingBLL().Delete(Convert.ToInt32(id), out _errMsg);
            if (result <= 0 || _errMsg != string.Empty)
                _status = MyEnums.enumStatus.ERROR.ToString();
            return Json(new { Status = _status, Mesg = _errMsg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListWFSetting()
        {
            IEnumerable<WorkflowSettingHeader> lists = new List<WorkflowSettingHeader>();
            lists = new WorkflowSettingBLL().GetHeader();
            var JsonResult = Json(new { Data = lists }, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }
        public JsonResult AddNewSetting()
        {
            string _status = MyEnums.enumStatus.SUCCESS.ToString();
            string _errMsg = string.Empty;
            int _result = 0;
            List<WorkflowSettingModel> lswfDetail = new List<WorkflowSettingModel>();
            WorkflowSettingHeader wfHdr = new WorkflowSettingHeader();
            if (Session["UserLogon"] != null)
            {
                List<WorkflowSettingModel> details = new List<WorkflowSettingModel>();
                details = JsonConvert.DeserializeObject<List<WorkflowSettingModel>>(Request.Form["WFApproval"]);
               
                try
                {

                     if(Request.Form["ID"]=="")
                     {
                         wfHdr.TypeID =Convert.ToInt32( Request.Form["DocType"].ToString());
                         wfHdr.Version = Convert.ToInt32(Request.Form["Version"].ToString());
                         wfHdr.ApprovalLevel = details.Count;
                         wfHdr.CreatedBy = "SYSTEM";
                         wfHdr.CreatedDate = DateTime.Now;
                         _result = new WorkflowSettingBLL().Insert(wfHdr, details, out _errMsg);
                     }
                     else
                     {

                     }
                }
                catch (Exception ex)
                {
                    _errMsg = ex.Message;
                }

            }
            return Json(new { Status = _status, Mesg=_errMsg }, JsonRequestBehavior.AllowGet);
        }
    }
}