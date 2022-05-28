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
    public class PartyController : BaseController
    {
        //
        // GET: /Party/
        PartyMaster objPM = new PartyMaster();

        public ActionResult Index()
        {
            if (Session["objParty"] != null)
            {
                Session.Remove("objParty");
            }

            return View();

        }

        public ActionResult LoadCompaniesAjaxHandler()
        {
            DataTable dtParties = objPM.GetPartyList();
            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtParties.AsEnumerable()
                         select new
                         {
                             PartyID = dr["PartyID"],
                             refCompID_Company = dr["refCompID_Company"],
                             PartyName = dr["PartyName"],
                             EmailID = dr["EmailID"],
                             WebURL = dr["WebURL"],
                             InsDate = dr["InsDate"],
                             InsTerminal = dr["InsTerminal"]
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtParties.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            PartyMaster objPartyMaster = objPM.CreateNewParty();
            ViewBag.listPartyCategory = objPartyMaster.listPartyCategory;

            Session["objParty"] = objPartyMaster;
            return View(objPartyMaster);
        }

        [HttpPost]
        public ActionResult Add(PartyMaster objPartyMaster)
        {
            PartyMaster objParty = null;
            objParty = Session["objParty"] as PartyMaster;
            ViewBag.listPartyCategory = objParty.listPartyCategory;

            if (ModelState.IsValid)
            {
                objPartyMaster.PartyAddressList = objParty.PartyAddressList;

                try
                {
                    objPartyMaster.refCompID_Company = intCompanyID;
                    objPartyMaster.SaveParty(objPartyMaster, intUserID, strTerminal);
                }
                catch
                {
                    ViewBag.listPartyCategory = objParty.listPartyCategory;
                    return View("Add", objPartyMaster);
                }
            }
            else
            {
                return View("Add", objPartyMaster);
            }

            return RedirectToAction("Index");
        }


        public ActionResult Edit(int ID)
        {
            PartyMaster objParty = null;
            if (Session["objParty"] != null)
            {
                objParty = Session["objParty"] as PartyMaster;
            }
            else
            {
                objParty = objPM.GetPartyDetails(ID);
                Session["objParty"] = objParty;
            }
            ViewBag.listPartyCategory = objParty.listPartyCategory;
            return View(objParty);
        }


        [HttpPost]
        public ActionResult Edit(PartyMaster objPartyMaster)
        {
            PartyMaster objParty = null;
            objParty = Session["objParty"] as PartyMaster;
            if (ModelState.IsValid)
            {
                objPartyMaster.PartyAddressList = objParty.PartyAddressList;

                try
                {
                    objPartyMaster.refCompID_Company = intCompanyID;
                    objPartyMaster.SaveParty(objPartyMaster, intUserID, strTerminal);
                }
                catch
                {
                    ViewBag.listPartyCategory = objParty.listPartyCategory;
                    return View("Edit", objPartyMaster);
                }
            }
            else
            {
                ViewBag.listPartyCategory = objParty.listPartyCategory;
                return View("Edit", objPartyMaster);
            }

            return RedirectToAction("Index");
        }

        public ActionResult PartyAddressList(List<PartyAddress> addressList)
        {
            List<PartyAddress> list = null;

            if (Session["objParty"] != null)
            {
                PartyMaster objParty = Session["objParty"] as PartyMaster;
                list = objParty.PartyAddressList;
            }
            else
            {
                list = addressList;
            }

            return PartialView("_PartyAddressList", list);
        }

        public ActionResult AddPartyAddress()
        {
            PartyAddress objPartyAddress = new PartyAddress();

            ViewBag.listPartyLocation = (Session["objParty"] as PartyMaster).listPartyLocation;
            PartyMaster objParty = Session["objParty"] as PartyMaster;

            if (objParty.PartyAddressList.Count > 0)
                objPartyAddress.SrNo = objParty.PartyAddressList.Max(X => X.SrNo) + 1;
            else
                objPartyAddress.SrNo = 1;
            return PartialView("_AddPartyAddress", objPartyAddress);
        }

        [HttpPost]
        public ActionResult AddPartyAddress(PartyAddress objPartyAddress)
        {
            ViewBag.listPartyLocation = (Session["objParty"] as PartyMaster).listPartyLocation;

            if (ModelState.IsValid)
            {
                PartyMaster objParty = Session["objParty"] as PartyMaster;

                objPartyAddress.ParamVal = objParty.listPartyLocation.Where(x => x.ID == objPartyAddress.refLocID_ParamVal).FirstOrDefault().ParamVal;
                objParty.PartyAddressList.Add(objPartyAddress);
                Session["objParty"] = objParty;

                string url = Url.Action("PartyAddressList", "Party");
                return Json(new { success = true, url = url });
                //return RedirectToAction("Edit", new { ID = objComp.CompID });

            }
            return PartialView("_AddPartyAddress", objPartyAddress);
        }

        public ActionResult EditPartyAddress(int srNo)
        {
            ViewBag.listPartyLocation = (Session["objParty"] as PartyMaster).listPartyLocation;
            PartyAddress objPartyAddress = (Session["objParty"] as PartyMaster).PartyAddressList.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_EditPartyAddress", objPartyAddress);
        }

        [HttpPost]
        public ActionResult EditPartyAddress(PartyAddress objPartyAddress)
        {
            ViewBag.listPartyLocation = (Session["objParty"] as PartyMaster).listPartyLocation;

            if (ModelState.IsValid)
            {
                PartyMaster objParty = Session["objParty"] as PartyMaster;
                objPartyAddress.ParamVal = objParty.listPartyLocation.Where(x => x.ID == objPartyAddress.refLocID_ParamVal).FirstOrDefault().ParamVal;

                List<PartyAddress> objAddressList = objParty.PartyAddressList as List<PartyAddress>;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).AddrID = objPartyAddress.AddrID;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).refLocID_ParamVal = objPartyAddress.refLocID_ParamVal;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).Addr = objPartyAddress.Addr;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).City = objPartyAddress.City;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).State = objPartyAddress.State;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).Pincode = objPartyAddress.Pincode;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).Country = objPartyAddress.Country;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).ContactPerson1 = objPartyAddress.ContactPerson1;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).Mobile1 = objPartyAddress.Mobile1;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).ContactPerson2 = objPartyAddress.ContactPerson2;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).Mobile2 = objPartyAddress.Mobile2;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).WorkContact1 = objPartyAddress.WorkContact1;
                objAddressList.FirstOrDefault(X => X.SrNo == objPartyAddress.SrNo).WorkContact2 = objPartyAddress.WorkContact2;


                Session["objParty"] = objParty;

                string url = Url.Action("PartyAddressList", "Party");
                return Json(new { success = true, url = url });
            }

            return PartialView("_EditPartyAddress", objPartyAddress);
        }

        public ActionResult DeletePartyAddress(int srNo)
        {
            PartyAddress objPartyAddress = (Session["objParty"] as PartyMaster).PartyAddressList.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_DeletePartyAddress", objPartyAddress);
        }

        [HttpPost]
        public ActionResult DeletePartyAddress(int? srNo)
        {
            PartyMaster objParty = Session["objParty"] as PartyMaster;
            PartyAddress objPartyAdress = (Session["objParty"] as PartyMaster).PartyAddressList.Where(X => X.SrNo == srNo).FirstOrDefault();
            objParty.PartyAddressList.Remove(objPartyAdress);
            Session["objParty"] = objParty;

            string url = Url.Action("PartyAddressList", "Party");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Edit", new { ID = objComp.CompID });
        }


        public ActionResult Delete(int ID)
        {
            PartyMaster objParty = objPM.GetPartyDetails(ID);

            return PartialView("_Delete", objParty);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            objPM.DeleteParty(ID.Value);

            string url = Url.Action("Index", "Party");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Index");
        }

    }
}
