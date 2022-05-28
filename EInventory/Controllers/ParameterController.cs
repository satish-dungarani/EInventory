using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EInventory.Models;
using Newtonsoft.Json;
using System.Data;

namespace EInventory.Controllers
{
    public class ParameterController : BaseController
    {
        //
        // GET: /Parameter/
        ParameterSetup objPSetup = new ParameterSetup();

        public ActionResult Index(int? ID)
        {
            ParameterSetup objParameter = new ParameterSetup();
            List<ParameterSetupMaster> listParameterSetup = objPSetup.GetParameterSetupList();

            if (ID.HasValue)
            {
                objParameter.SetupID = ID.Value;
                List<ParameterValues> listParameterValues = objPSetup.GetParameterValueList(ID.Value);
                objParameter.listParameterValues = listParameterValues;
                Session["objParameter"] = objParameter;
            }
            else
            {
                if (listParameterSetup.Count > 0)
                {
                    objParameter.SetupID = listParameterSetup[0].SetupID;
                    objParameter.IsSystemGene = listParameterSetup[0].IsSystemGene;

                    List<ParameterValues> listParameterValues = objPSetup.GetParameterValueList(objParameter.SetupID);
                    objParameter.listParameterValues = listParameterValues;

                    Session["objParameter"] = objParameter;
                }
            }

            ViewBag.listParameterSetup = listParameterSetup;

            return View(objParameter);
        }

        [HttpPost]
        public ActionResult Index(ParameterSetup objParameter)
        {
            List<ParameterSetupMaster> listParameterSetup = objPSetup.GetParameterSetupList();
            ViewBag.listParameterSetup = listParameterSetup;

            objParameter.IsSystemGene = listParameterSetup.FirstOrDefault(X => X.SetupID == objParameter.SetupID).IsSystemGene;

            List<ParameterValues> listParameterValues = objPSetup.GetParameterValueList(objParameter.SetupID);
            objParameter.listParameterValues = listParameterValues;
            Session["objParameter"] = objParameter;

            return View(objParameter);
        }

        public ActionResult Add()
        {
            ParameterSetup objParameter = Session["objParameter"] as ParameterSetup;
            ParameterValues objParameterValue = new ParameterValues();
            objParameterValue.ID = -1;
            objParameterValue.refCompID_Company = intCompanyID;
            objParameterValue.refSetupID_ParameterSetup = objParameter.SetupID;

            return PartialView("_Add", objParameterValue);
        }

        [HttpPost]
        public ActionResult Add(ParameterValues objParameterValue)
        {
            if (ModelState.IsValid)
                objPSetup.SaveParameterValues(objParameterValue, intUserID, strTerminal);
            else
                return PartialView("_Add", objParameterValue);

            //return RedirectToAction("Index", new { ID = objParameterValue.refSetupID_ParameterSetup });
            string url = Url.Action("Index", "Parameter", new { ID = objParameterValue.refSetupID_ParameterSetup });
            return Json(new { success = true, url = url });
        }

        public ActionResult Edit(int ID)
        {
            ParameterSetup objParameter = Session["objParameter"] as ParameterSetup;
            ParameterValues objParameterValue = objParameter.listParameterValues.FirstOrDefault(X => X.ID == ID);

            return PartialView("_Edit", objParameterValue);
        }

        [HttpPost]
        public ActionResult Edit(ParameterValues objParameterValue)
        {
            if (ModelState.IsValid)
                objPSetup.SaveParameterValues(objParameterValue, intUserID, strTerminal);
            else
                return PartialView("_Edit", objParameterValue);

            //return RedirectToAction("Index", new { ID = objParameterValue.refSetupID_ParameterSetup });
            string url = Url.Action("Index", "Parameter", new { ID = objParameterValue.refSetupID_ParameterSetup });
            return Json(new { success = true, url = url });
        }

        public ActionResult Delete(int ID)
        {
            ParameterSetup objParameter = Session["objParameter"] as ParameterSetup;
            ParameterValues objParameterValue = objParameter.listParameterValues.FirstOrDefault(X => X.ID == ID);

            return PartialView("_Delete", objParameterValue);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            objPSetup.DeleteParameterValues(ID.Value);
            ParameterSetup objParameter = Session["objParameter"] as ParameterSetup;

            //return RedirectToAction("Index", new { ID = objParameter.SetupID });

            string url = Url.Action("Index", "Parameter", new { ID = objParameter.SetupID });
            return Json(new { success = true, url = url });
        }
    }
}
