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
    public class ItemCategoryController : BaseController
    {
        //
        // GET: /Role/
        ItemCategoryModel objItemCategory = new ItemCategoryModel();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadItemCategoryAjaxHandler()
        {

            DataTable dtCategories = objItemCategory.GetCategories();
            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtCategories.AsEnumerable()
                         select new
                         {
                             CatID = dr["CatID"],
                             CategoryName = dr["CategoryName"],
                             IsActive = dr["IsActive"],
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtCategories.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            ItemCategoryModel objItemCat = new ItemCategoryModel();
            return PartialView("_Add", objItemCat);
        }

        [HttpPost]
        public ActionResult Add(ItemCategoryModel objItemCat)
        {
            if (ModelState.IsValid)
                objItemCategory.SaveCategory(objItemCat, intUserID, strTerminal);
            else
            {
                return PartialView("_Add", objItemCat);
            }

            string url = Url.Action("Index", "ItemCategory");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Index");
        }

        public ActionResult Edit(int ID)
        {
            ItemCategoryModel objItemCat = objItemCategory.GetCategoryDetail(ID);

            return PartialView("_Edit", objItemCat);
        }

        [HttpPost]
        public ActionResult Edit(ItemCategoryModel objItemCat)
        {
            if (ModelState.IsValid)
                objItemCategory.SaveCategory(objItemCat, intUserID, strTerminal);
            else
                return PartialView("_Edit", objItemCat);

            string url = Url.Action("Index", "ItemCategory");
            return Json(new { success = true, url = url });

            //return RedirectToAction("Index");
        }

        public ActionResult Delete(int ID)
        {
            ItemCategoryModel objItemCat = objItemCategory.GetCategoryDetail(ID);

            return PartialView("_Delete", objItemCat);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            objItemCategory.DeletCategory(ID.Value);

            string url = Url.Action("Index", "ItemCategory");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Index");
        }
    }

    public class ItemController : BaseController
    {
        ItemModels objItemModel = new ItemModels();
        public ActionResult Index()
        {
            if (Session["objItem"] != null)
            {
                Session.Remove("objItem");
            }

            return View();
        }
        public ActionResult LoadItemAjaxHandler()
        {

            DataTable dtItems = objItemModel.GetAllItems(intCompanyID);
            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtItems.AsEnumerable()
                         select new
                         {
                             ItemID = dr["ItemID"],
                             CategoryName = dr["CategoryName"],
                             ItemName = dr["ItemName"],
                             UnitName = dr["UnitName"],
                             ReOrdLevel = dr["ReOrdLevel"],
                             MaxDeliveryDays = dr["MaxDeliveryDays"],
                             Size = dr["Size"],
                             Material = dr["Material"],
                             IsFinishedItem = dr["IsFinishedItem"]
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtItems.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            ItemModels objItem = null;
            objItem = objItemModel.GetItemDetails(intCompanyID, 0);
            Session["objItem"] = objItem;

            return View(objItem);
        }

        [HttpPost]
        public ActionResult Add(ItemModels objItemEdit)
        {

            if (Session["objItem"] != null)
            {
                ItemModels objItem = Session["objItem"] as ItemModels;

                objItemEdit.listFinalItemConsumption = objItem.listFinalItemConsumption;
                objItemEdit.listItemVendor = objItem.listItemVendor;
                objItemEdit.listCategory = objItem.listCategory;
                objItemEdit.listUOM = objItem.listUOM;
            }

            if (ModelState.IsValid)
            {
                objItemModel.SaveItem(objItemEdit, intCompanyID, intUserID, strTerminal);
                return RedirectToAction("Index");
            }

            return View(objItemEdit);
        }
        public ActionResult Edit(int ID)
        {
            ItemModels objItem = null;
            if (Session["objItem"] != null)
            {
                objItem = Session["objItem"] as ItemModels;
            }
            else
            {
                objItem = objItemModel.GetItemDetails(intCompanyID, ID);
                Session["objItem"] = objItem;
            }
            return View(objItem);
        }

        [HttpPost]
        public ActionResult Edit(ItemModels objItemEdit)
        {

            if (Session["objItem"] != null)
            {
                ItemModels objItem = Session["objItem"] as ItemModels;

                objItemEdit.listFinalItemConsumption = objItem.listFinalItemConsumption;
                objItemEdit.listItemVendor = objItem.listItemVendor;
                objItemEdit.listCategory = objItem.listCategory;
                objItemEdit.listUOM = objItem.listUOM;
            }

            if (ModelState.IsValid)
            {
                objItemModel.SaveItem(objItemEdit, intCompanyID, intUserID, strTerminal);
                return RedirectToAction("Index");
            }

            return View(objItemEdit);
        }

        public ActionResult ItemVendorList(List<ItemVendor> listItemVendor)
        {
            List<ItemVendor> list = null;
            if (Session["objItem"] != null)
            {
                ItemModels objItem = Session["objItem"] as ItemModels;
                list = objItem.listItemVendor;
            }
            else
            {
                list = listItemVendor;
            }

            return PartialView("_ItemVendorList", list);
        }
        public ActionResult SaveConsumption(int? SrNo)
        {
            FinalItemConsumption objFinalItem = null;

            ItemModels objItem = Session["objItem"] as ItemModels;
            objItem.listRowMaterials = objItem.GetRowMaterials();

            ViewBag.listRowMaterials = objItem.listRowMaterials;

            Session["objItem"] = objItem;

            if (SrNo.HasValue)
            {
                objFinalItem = objItem.listFinalItemConsumption.FirstOrDefault(X => X.SrNo == SrNo);
            }
            else
            {
                objFinalItem = new FinalItemConsumption();
                if (objItem.listFinalItemConsumption.Count() == 0)
                {
                    objFinalItem.SrNo = 1;
                }
                else
                {
                    objFinalItem.SrNo = objItem.listFinalItemConsumption.Max(X => X.SrNo) + 1;
                }
            }

            return PartialView("_SaveCosumption", objFinalItem);
        }

        [HttpPost]
        public ActionResult SaveConsumption(FinalItemConsumption objFinalItem)
        {
            ItemModels objItem = Session["objItem"] as ItemModels;

            if (ModelState.IsValid)
            {
                FinalItemConsumption tempFinalItemConsumption = objItem.listFinalItemConsumption.FirstOrDefault(X => X.SrNo == objFinalItem.SrNo);
                if (tempFinalItemConsumption != null)
                    objItem.listFinalItemConsumption.Remove(tempFinalItemConsumption);

                objFinalItem.RowItemName = objItem.listRowMaterials.FirstOrDefault(X => X.ItemID == objFinalItem.refrowItemID_ItemMas).ItemName;
                objItem.listFinalItemConsumption.Add(objFinalItem);
                Session["objItem"] = objItem;

                string url = Url.Action("ConstumptionList", "Item");
                return Json(new { success = true, url = url });
            }
            else
            {
                ViewBag.listRowMaterials = objItem.GetRowMaterials();
            }

            return PartialView("_SaveCosumption", objFinalItem);
        }

        public ActionResult ConstumptionList(List<FinalItemConsumption> listFinalItemConsumption)
        {
            List<FinalItemConsumption> list = null;
            if (Session["objItem"] != null)
            {
                ItemModels objItem = Session["objItem"] as ItemModels;
                list = objItem.listFinalItemConsumption;
            }
            else
            {
                list = listFinalItemConsumption;
            }

            return PartialView("_ConstumptionList", list);
        }
        public ActionResult SaveItemVendors(int? SrNo)
        {
            ItemVendor objItemVendror = null;

            ItemModels objItem = Session["objItem"] as ItemModels;
            objItem.listVendors = objItem.GetAllVendors();
            ViewBag.listVendors = objItem.listVendors;

            if (SrNo.HasValue)
            {
                objItemVendror = objItem.listItemVendor.FirstOrDefault(X => X.SrNo == SrNo);
            }
            else
            {
                objItemVendror = new ItemVendor();

                if (objItem.listItemVendor.Count() == 0)
                {
                    objItemVendror.SrNo = 1;
                }
                else
                {
                    objItemVendror.SrNo = objItem.listItemVendor.Max(X => X.SrNo) + 1;
                }
            }

            return PartialView("_SaveItemVendor", objItemVendror);
        }

        [HttpPost]
        public ActionResult SaveItemVendors(ItemVendor objItemVendror)
        {
            ItemModels objItem = Session["objItem"] as ItemModels;

            if (ModelState.IsValid)
            {
                objItemVendror.PartyName = objItem.listVendors.FirstOrDefault(X => X.PartyID == objItemVendror.refPartyID_PartyMas).PartyName;

                ItemVendor tempItemVendor = objItem.listItemVendor.FirstOrDefault(X => X.SrNo == objItemVendror.SrNo);
                if (tempItemVendor != null)
                    objItem.listItemVendor.Remove(tempItemVendor);

                objItem.listItemVendor.Add(objItemVendror);
                Session["objItem"] = objItem;

                string url = Url.Action("ItemVendorList", "Item");
                return Json(new { success = true, url = url });
            }
            else
            {
                ViewBag.listVendors = objItem.listVendors;
            }

            return PartialView("_SaveItemVendor", objItemVendror);
        }

        public ActionResult DeleteItemVendor(int srNo)
        {
            ItemVendor objItemVendor = (Session["objItem"] as ItemModels).listItemVendor.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_DeleteItemVendor", objItemVendor);
        }

        [HttpPost]
        public ActionResult DeleteItemVendor(int? srNo)
        {
            ItemModels objItem = Session["objItem"] as ItemModels;
            ItemVendor objItemVendor = objItem.listItemVendor.Where(X => X.SrNo == srNo).FirstOrDefault();
            objItem.listItemVendor.Remove(objItemVendor);
            Session["objItem"] = objItem;

            string url = Url.Action("ItemVendorList", "Item");
            return Json(new { success = true, url = url });
        }


        public ActionResult DeleteConsumption(int srNo)
        {
            FinalItemConsumption objItemConsumption = (Session["objItem"] as ItemModels).listFinalItemConsumption.Where(X => X.SrNo == srNo).FirstOrDefault();

            return PartialView("_DeleteConsumution", objItemConsumption);
        }

        [HttpPost]
        public ActionResult DeleteConsumption(int? srNo)
        {
            ItemModels objItem = Session["objItem"] as ItemModels;
            FinalItemConsumption objItemConsumption = objItem.listFinalItemConsumption.Where(X => X.SrNo == srNo).FirstOrDefault();
            objItem.listFinalItemConsumption.Remove(objItemConsumption);
            Session["objItem"] = objItem;

            string url = Url.Action("ConstumptionList", "Item");
            return Json(new { success = true, url = url });
        }

        public ActionResult Delete(int ID)
        {
            ItemModels objItem = objItemModel.GetItemDetails(intCompanyID, ID);
            return PartialView("_Delete", objItem);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            if (objItemModel.DeleteItem(ID.Value) > 0)
                return View("Index");
            else
                return PartialView("_Delete");

        }

    }
}
