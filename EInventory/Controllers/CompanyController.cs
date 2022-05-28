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
    public class CompanyController : BaseController
    {
        //
        // GET: /Master/
        CompanyModels objCM = new CompanyModels();


        public ActionResult Index()
        {
            if (Session["objComp"] != null)
                Session.Remove("objComp");

            return View();
        }

        public ActionResult LoadCompaniesAjaxHandler()
        {


            string jsonData = JsonConvert.SerializeObject(objCM.GetCompanyList());

            DataTable dtCompnies = objCM.GetCompanyList();
            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtCompnies.AsEnumerable()
                         select new
                         {
                             CompID = dr["CompID"],
                             State = dr["State"],
                             City = dr["City"],
                             Addr = dr["Addr"],
                             Name = dr["Name"],
                             Pincode = dr["Pincode"],
                             Country = dr["Country"],
                             Contact1 = dr["Contact1"],
                             Contact2 = dr["Contact2"],
                             EmailID = dr["EmailID"],
                             WebURL = dr["WebURL"]
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtCompnies.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            CompanyModels objComp = objCM.GetNewCompany();

            Session["objComp"] = objComp;
            return View(objComp);
        }

        public ActionResult Edit(int ID)
        {
            CompanyModels objComp = null;
            if (Session["objComp"] != null)
            {
                objComp = Session["objComp"] as CompanyModels;
            }
            else
            {
                objComp = objCM.GetCompanyDetails(ID);
                Session["objComp"] = objComp;
            }
            return View(objComp);
        }

        [HttpPost]
        public ActionResult Add(CompanyModels objCompany)
        {
            CompanyModels objComp = null;
            objComp = Session["objComp"] as CompanyModels;
            if (ModelState.IsValid)
            {
                objCompany.listCompAuthorityPersons = objComp.listCompAuthorityPersons;
                objCompany.listCompSatutory = objComp.listCompSatutory;

                try
                {
                    objCompany.SaveCompanyDetail(objCompany, intUserID, strTerminal);
                }
                catch
                {
                    return View("Add", objCompany);
                }
            }
            else
            {
                return View("Add", objCompany);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CompanyModels objCompany)
        {
            CompanyModels objComp = null;
            objComp = Session["objComp"] as CompanyModels;
            if (ModelState.IsValid)
            {
                objCompany.listCompAuthorityPersons = objComp.listCompAuthorityPersons;
                objCompany.listCompSatutory = objComp.listCompSatutory;

                try
                {
                    objCompany.SaveCompanyDetail(objCompany, intUserID, strTerminal);
                }
                catch
                {
                    return View("Edit", objCompany);
                }
            }
            else
            {
                return View("Edit", objCompany);
            }

            return RedirectToAction("Index");
        }


        public ActionResult ContactList(List<CompanyAuthorityPerson> conactList)
        {
            List<CompanyAuthorityPerson> list = null;
            if (Session["objComp"] != null)
            {
                CompanyModels objComp = Session["objComp"] as CompanyModels;
                list = objComp.listCompAuthorityPersons;
            }
            else
            {
                list = conactList;
            }

            return PartialView("_ContactListPartial", list);
        }
        public ActionResult SatutoryList(List<CompanySatutory> satutoryList)
        {
            List<CompanySatutory> list = null;
            if (Session["objComp"] != null)
            {
                CompanyModels objComp = Session["objComp"] as CompanyModels;
                list = objComp.listCompSatutory;
            }
            else
            {
                list = satutoryList;
            }

            return PartialView("_SatutoryListPartial", list);
        }
        public ActionResult AddSatutory()
        {
            CompanySatutory objSatutory = new CompanySatutory();
            ViewBag.listSatutoryMaster = (Session["objComp"] as CompanyModels).listSatutoryMaster;
            CompanyModels objComp = Session["objComp"] as CompanyModels;
            if (objComp.listCompSatutory.Count > 0)
                objSatutory.SrNo = objComp.listCompSatutory.Max(X => X.SrNo) + 1;
            else
                objSatutory.SrNo = 1;
            return PartialView("_AddSatutory", objSatutory);
        }

        [HttpPost]
        public ActionResult AddSatutory(CompanySatutory objSatutory)
        {
            ViewBag.listSatutoryMaster = (Session["objComp"] as CompanyModels).listSatutoryMaster;

            if (ModelState.IsValid)
            {
                CompanyModels objComp = Session["objComp"] as CompanyModels;
                objSatutory.ParamVal = objComp.listSatutoryMaster.Where(x => x.ID == objSatutory.refStatutoryID_ParamSetup).FirstOrDefault().ParamVal;
                objComp.listCompSatutory.Add(objSatutory);
                Session["objComp"] = objComp;

                string url = Url.Action("SatutoryList", "Company");
                return Json(new { success = true, url = url });
                //return RedirectToAction("Edit", new { ID = objComp.CompID });

            }
            return PartialView("_AddSatutory", objSatutory);
        }


        public ActionResult EditSatutory(int srNo)
        {
            ViewBag.listSatutoryMaster = (Session["objComp"] as CompanyModels).listSatutoryMaster;
            CompanySatutory objSatutory = (Session["objComp"] as CompanyModels).listCompSatutory.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_EditSatutory", objSatutory);
        }

        [HttpPost]
        public ActionResult EditSatutory(CompanySatutory objSatutory)
        {
            ViewBag.listSatutoryMaster = (Session["objComp"] as CompanyModels).listSatutoryMaster;

            if (ModelState.IsValid)
            {
                CompanyModels objComp = Session["objComp"] as CompanyModels;
                objSatutory.ParamVal = objComp.listSatutoryMaster.Where(x => x.ID == objSatutory.refStatutoryID_ParamSetup).FirstOrDefault().ParamVal;
                List<CompanySatutory> objSatList = objComp.listCompSatutory as List<CompanySatutory>;
                objSatList.FirstOrDefault(X => X.SrNo == objSatutory.SrNo).refStatutoryID_ParamSetup = objSatutory.refStatutoryID_ParamSetup;
                objSatList.FirstOrDefault(X => X.SrNo == objSatutory.SrNo).ParamVal = objSatutory.ParamVal;
                objSatList.FirstOrDefault(X => X.SrNo == objSatutory.SrNo).StatutoryName = objSatutory.StatutoryName;

                Session["objComp"] = objComp;

                string url = Url.Action("SatutoryList", "Company");
                return Json(new { success = true, url = url });
                //return RedirectToAction("Edit", new { ID = objComp.CompID });

            }

            return PartialView("_EditSatutory", objSatutory);
        }

        public ActionResult DeleteSatutory(int srNo)
        {
            CompanySatutory objSatutory = (Session["objComp"] as CompanyModels).listCompSatutory.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_DeleteSatutory", objSatutory);
        }

        [HttpPost]
        public ActionResult DeleteSatutory(int? srNo)
        {
            CompanyModels objComp = Session["objComp"] as CompanyModels;
            CompanySatutory objSatutory = (Session["objComp"] as CompanyModels).listCompSatutory.Where(X => X.SrNo == srNo).FirstOrDefault();
            objComp.listCompSatutory.Remove(objSatutory);
            Session["objComp"] = objComp;

            string url = Url.Action("SatutoryList", "Company");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Edit", new { ID = objComp.CompID });
        }


        public ActionResult AddContactPerson()
        {
            CompanyAuthorityPerson objContact = new CompanyAuthorityPerson();
            ViewBag.listDesgMaster = (Session["objComp"] as CompanyModels).listDesgnMaster;

            CompanyModels objComp = Session["objComp"] as CompanyModels;

            if (objComp.listCompAuthorityPersons.Count > 0)
                objContact.SrNo = objComp.listCompAuthorityPersons.Max(X => X.SrNo) + 1;
            else
                objContact.SrNo = 1;

            return PartialView("_AddContactPerson", objContact);
        }

        [HttpPost]
        public ActionResult AddContactPerson(CompanyAuthorityPerson objContact)
        {
            ViewBag.listDesgMaster = (Session["objComp"] as CompanyModels).listDesgnMaster;

            if (ModelState.IsValid)
            {
                CompanyModels objComp = Session["objComp"] as CompanyModels;
                objContact.ParamVal = objComp.listDesgnMaster.Where(x => x.ID == objContact.refDesignID_ParamVal).FirstOrDefault().ParamVal;
                objComp.listCompAuthorityPersons.Add(objContact);
                Session["objComp"] = objComp;

                string url = Url.Action("ContactList", "Company");
                return Json(new { success = true, url = url });
                //return RedirectToAction("Edit", new { ID = objComp.CompID });

            }
            return PartialView("_AddContactPerson", objContact);
        }

        public ActionResult EditContactPerson(int srNo)
        {
            ViewBag.listDesgMaster = (Session["objComp"] as CompanyModels).listDesgnMaster;
            CompanyAuthorityPerson objContact = (Session["objComp"] as CompanyModels).listCompAuthorityPersons.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_EditContactPerson", objContact);
        }

        [HttpPost]
        public ActionResult EditContactPerson(CompanyAuthorityPerson objContact)
        {
            ViewBag.listDesgMaster = (Session["objComp"] as CompanyModels).listDesgnMaster;

            if (ModelState.IsValid)
            {
                CompanyModels objComp = Session["objComp"] as CompanyModels;
                objContact.ParamVal = objComp.listDesgnMaster.Where(x => x.ID == objContact.refDesignID_ParamVal).FirstOrDefault().ParamVal;
                List<CompanyAuthorityPerson> objContactList = objComp.listCompAuthorityPersons;
                objContactList.FirstOrDefault(X => X.SrNo == objContact.SrNo).refDesignID_ParamVal = objContact.refDesignID_ParamVal;
                objContactList.FirstOrDefault(X => X.SrNo == objContact.SrNo).ParamVal = objContact.ParamVal;
                objContactList.FirstOrDefault(X => X.SrNo == objContact.SrNo).ContactPerson = objContact.ContactPerson;

                Session["objComp"] = objComp;

                string url = Url.Action("ContactList", "Company");
                return Json(new { success = true, url = url });
            }

            return PartialView("_EditContactPerson", objContact);
        }



        public ActionResult DeleteContactPerson(int srNo)
        {
            CompanyAuthorityPerson objContact = (Session["objComp"] as CompanyModels).listCompAuthorityPersons.Where(X => X.SrNo == srNo).FirstOrDefault();
            return PartialView("_DeleteContactPerson", objContact);
        }

        [HttpPost]
        public ActionResult DeleteContactPerson(int? srNo)
        {
            CompanyModels objComp = Session["objComp"] as CompanyModels;
            CompanyAuthorityPerson objSatutory = (Session["objComp"] as CompanyModels).listCompAuthorityPersons.Where(X => X.SrNo == srNo).FirstOrDefault();
            objComp.listCompAuthorityPersons.Remove(objSatutory);
            Session["objComp"] = objComp;

            string url = Url.Action("ContactList", "Company");
            return Json(new { success = true, url = url });
        }

        public ActionResult Delete(int ID)
        {
            return PartialView("_Delete");
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            if (objCM.DeleteCompany(ID.Value) > 0)
                return View("Index");
            else
                return PartialView("_Delete");

        }

    }
}
