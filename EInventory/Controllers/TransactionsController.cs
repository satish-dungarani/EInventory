using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EInventory.Models;
using Newtonsoft.Json;
using System.Data;
using EInventory.Helpers;
using System.IO;

namespace EInventory.Controllers
{
    public class TransactionsController : BaseController
    {

        TransactionModels objTranModel = new TransactionModels();
        TaskOperations objTaskOper = new TaskOperations();
        static List<TransactionItems> _ObjTransItem;
        public ActionResult Index(int ID)
        {
            Session["TranTypeID"] = ID;

            ViewBag.PageName = string.Format("{0} List", Enum.GetName(typeof(TransactionType), ID));

            return View();
        }

        public ActionResult LoadItemTransactionListAjaxHandler()
        {

            int TranTypeID = Convert.ToInt32(Session["TranTypeID"]);
            DataTable dtTransactionList = objTranModel.GetTransactionList(TranTypeID, intCompanyID);



            var aadata = from dr in dtTransactionList.AsEnumerable()
                         select new
                         {
                             TranID = dr["TranID"],
                             TranDispID = dr["TranDispID"],
                             TransDate = dr["TransDate"],

                             PartyName = dr["PartyName"],
                             Remark = dr["Remark"],
                             WithMaterial = dr["WithMaterial"],
                             ExpDueDate = dr["ExpDueDate"],
                             TotalQty = dr["TotalQty"],
                             TotalAmount = dr["TotalAmount"],
                             FollowUPKey = dr["FollowUPKey"],
                             TaskID = dr["TaskID"]
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtTransactionList.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public List<PartyMaster> GetPartyList(TransactionModels objTran)
        {
            if (objTran.refID_TransactionType == (int)TransactionType.Purchase ||
                objTran.refID_TransactionType == (int)TransactionType.PurchaseOrder ||
                objTran.refID_TransactionType == (int)TransactionType.PurchaseReturn)
            {
                return objTranModel.GetAllVendors(8);
            }
            else if (objTran.refID_TransactionType == (int)TransactionType.Sales ||
                  objTran.refID_TransactionType == (int)TransactionType.SalesReturn ||
                  objTran.refID_TransactionType == (int)TransactionType.Scrapesale)
            {
                objTran.IsDisplayExpDate = false;
                objTran.IsDisplayWithMaterial = false;
                return objTranModel.GetAllVendors(7);
            }
            else if (objTran.refID_TransactionType == (int)TransactionType.JobWorkIssue ||
                     objTran.refID_TransactionType == (int)TransactionType.JobWorkreturn)
            {
                return objTranModel.GetAllVendors(23);
            }
            else
            {
                return new List<PartyMaster>();
            }
        }

        public ActionResult AddTransaction()
        {
            //_ObjTransItem = new List<TransactionItems>();
            TransactionModels objTran = new TransactionModels();
            objTran.refID_TransactionType = Convert.ToInt32(Session["TranTypeID"]);
            objTran.TransDate = DateTime.Now;
            objTran.listTasks = objTaskOper.GetTaskCheckList(objTran.refID_TransactionType, intUserID);

            string _CategoryId = (Convert.ToInt32(MyHelpers.CategoryList.RawMaterials)).ToString() + "," + (Convert.ToInt32(MyHelpers.CategoryList.SubAssemble).ToString());
            ViewData["ItemList"] = MyHelpers.GetItemList(intCompanyID, _CategoryId);

            ViewBag.PageName = string.Format("Add {0}", Enum.GetName(typeof(TransactionType), objTran.refID_TransactionType));

            ViewBag.listParties = GetPartyList(objTran);

            TransactionSetupData objTranSetupData = objTran.GetTransactionSetupData(objTran.refID_TransactionType);
            objTran.TranDispID = objTranSetupData.TransactionNo;
            objTran.IsAllowedFollowUp = objTranSetupData.IsFollowUP;
            objTran.IsFollowUP = false;
            Session["objTran"] = objTran;

            return View(objTran);
        }

        [HttpPost]
        public ActionResult AddTransaction(TransactionModels objTran)
        {
            ViewBag.PageName = string.Format("Add {0}", Enum.GetName(typeof(TransactionType), objTran.refID_TransactionType));

            TransactionModels objTranSession = Session["objTran"] as TransactionModels;
            objTran.listTransactionItem = objTranSession.listTransactionItem;
            objTran.listTasks = objTranSession.listTasks;

            ModelState.Clear();

            if (this.TryValidateModel(objTran))
            {
                objTranModel.SaveTransaction(objTran, intUserID, intCompanyID, strTerminal);
                return RedirectToAction("Index", new { ID = objTran.refID_TransactionType });
            }
            else
            {
                ViewBag.listParties = GetPartyList(objTran);
            }


            return View(objTran);
        }

        public ActionResult EditTransaction(int ID)
        {
            TransactionModels objTran = objTranModel.GetTransactionDetails(ID);

            ViewBag.PageName = string.Format("Edit {0}", Enum.GetName(typeof(TransactionType), objTran.refID_TransactionType));

            // Get List of items for dropdownlist
            string _CategoryId = (Convert.ToInt32(MyHelpers.CategoryList.RawMaterials)).ToString() + "," + (Convert.ToInt32(MyHelpers.CategoryList.SubAssemble).ToString());
            ViewData["ItemList"] = MyHelpers.GetItemList(intCompanyID, _CategoryId);

            _ObjTransItem = objTran.listTransactionItem;

            if (objTran.IsAllowedFollowUp && objTran.IsFollowUP == false)
            {
                objTran.listTasks = objTaskOper.GetTaskCheckList(objTran.refID_TransactionType, intUserID);
            }

            ViewBag.listParties = GetPartyList(objTran);

            Session["objTran"] = objTran;

            return View(objTran);
        }
        [HttpGet]
        public string SetDataInItemDetail(TransactionModels objTran, string btnSubmit, string RowId)
        {
            //TransactionItems _ObjItems = new TransactionItems();
            try
            {
                ViewBag.listParties = GetPartyList(objTran);
                //_ObjTransItem = objTran.listTransactionItem;
                // Get List of items for dropdownlist
                int refID_TransactionType = 0;
                Enum.GetName(typeof(TransactionType), refID_TransactionType);
                string _CategoryId = (Convert.ToInt32(MyHelpers.CategoryList.RawMaterials)).ToString() + "," + (Convert.ToInt32(MyHelpers.CategoryList.SubAssemble).ToString());
                ViewData["ItemList"] = MyHelpers.GetItemList(intCompanyID, _CategoryId);

                if (btnSubmit.ToUpper() == "REMOVE" && Convert.ToInt32(RowId) != -1)
                {
                    //_ObjTransItem.RemoveAt(_Rowid);
                    //objTran.listTransactionItem = _ObjTransItem;
                    objTran.listTransactionItem.RemoveAt(Convert.ToInt32(RowId));

                    //objTran.listTransactionItem.RemoveRange(_Rowid, 1);
                }
                else if (btnSubmit.ToUpper() == "ADD")
                {
                    TransactionItems _ObjItems = new TransactionItems();
                    //_ObjItems.refItemID_ItemMas = 0;
                    //_ObjItems.TranQty = 0;
                    //_ObjItems.TranRate = 0;

                    objTran.listTransactionItem.Add(_ObjItems);
                }
                ModelState.Clear();
                return RenderRazorViewToString("_TransactionsDetailListPartial", objTran);
            }
            catch (Exception)
            {
                ModelState.Clear();
                return RenderRazorViewToString("_TransactionsDetailListPartial", objTran);
            }
        }
        [HttpPost]
        public ActionResult EditTransaction(TransactionModels objTran, string btnSubmit, string RowId)
        {

            if (btnSubmit.ToUpper() == "SAVE")
            {
                ViewBag.PageName = string.Format("Edit {0}", Enum.GetName(typeof(TransactionType), objTran.refID_TransactionType));


                TransactionModels objTranSession = Session["objTran"] as TransactionModels;
                //objTran.listTransactionItem = objTranSession.listTransactionItem;
                objTran.listTasks = objTranSession.listTasks;

                //ModelState.Clear();

                if (this.TryValidateModel(objTran))
                {
                    objTranModel.SaveTransaction(objTran, intUserID, intCompanyID, strTerminal);

                    return RedirectToAction("Index", new { ID = objTranSession.refID_TransactionType });
                }
                else
                {
                    ViewBag.listParties = GetPartyList(objTran);
                }

                Session["objTran"] = objTran;

                //var _PV = ;
                return Json(new { result = RenderRazorViewToString("_TransactionsDetailListPartial", objTran).ToString(), btn = btnSubmit }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //// Get List of items for dropdownlist
                //int refID_TransactionType = 0;
                //Enum.GetName(typeof(TransactionType), refID_TransactionType);
                //ViewData["ItemList"] = MyHelpers.GetItemList(intCompanyID, refID_TransactionType);

                //return SetDataInItemDetail(objTran, btnSubmit, RowId);
                var _PV = SetDataInItemDetail(objTran, btnSubmit, RowId);
                return Json(new { result = _PV, btn = btnSubmit }, JsonRequestBehavior.AllowGet);
                //string ActionName = string.Empty;
                //int _Rowid = 0;
                //if (btnSubmit.IndexOf(' ') > 0)
                //{
                //    ActionName = btnSubmit.Split(' ')[0].Trim();
                //    _Rowid = Convert.ToInt32(btnSubmit.Split(' ')[1].Trim());
                //}

                //if (ActionName.ToUpper() == "REMOVE" && _Rowid != -1)
                //{
                //    //objTran.listTransactionItem.RemoveAt(_Rowid);
                //    objTran.listTransactionItem.RemoveRange(_Rowid, 1);
                //}
                //else
                //{
                //    TransactionItems _ObjItems = new TransactionItems();
                //    _ObjItems.refItemID_ItemMas = 0;
                //    _ObjItems.TranQty = 0;
                //    _ObjItems.TranRate = 0;

                //    objTran.listTransactionItem.Add(_ObjItems);
                //}
                //return PartialView("_TransactionsDetailListPartial", objTran);
            }
            //_ObjTransItem = objTran.listTransactionItem;
            //return View(objTran);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public ActionResult DeleteTransaction(int ID)
        {
            TransactionModels objTransactionM = objTranModel.GetTransactionDetails(ID);

            return PartialView("_DeleteTransaction", objTransactionM);
        }

        [HttpPost]
        public ActionResult DeleteTransaction(int? ID)
        {
            objTranModel.DeleteTransaction(ID.Value);
            int TranTypeID = Convert.ToInt32(Session["TranTypeID"]);

            string url = Url.Action("Index", "Transactions", new { ID = TranTypeID });
            return Json(new { success = true, url = url });
            //return RedirectToAction("Index");
        }

        public string GetRowItems()
        {
            //var editList = from DataRow dr in objTranModel.GetItemsByType(intCompanyID, 8).AsEnumerable()
            //               select new
            //               {
            //                   refItemID_ItemMas = Convert.ToString(dr["ItemID"]),
            //                   Name = Convert.ToString(dr["ItemName"])
            //               };

            //return Json(editList, JsonRequestBehavior.AllowGet);

            //"1:Still Road;3:Still Seasor"

            string returnString = "";
            string _CategoryId = (Convert.ToInt32(MyHelpers.CategoryList.RawMaterials)).ToString() + "," + (Convert.ToInt32(MyHelpers.CategoryList.SubAssemble).ToString());
            DataRowCollection drSelected = objTranModel.GetItemsByType(intCompanyID, _CategoryId).Rows;
            int count = 1;

            foreach (DataRow dr in drSelected)
            {

                returnString += Convert.ToString(dr["ItemID"]) + ":" + Convert.ToString(dr["ItemName"]);

                if (drSelected.Count > count)
                {
                    returnString += ";";
                    count++;
                }
            }

            //returnString += "]";

            return returnString;
        }

        public string GetUsers()
        {

            string returnString = "";
            List<UserModels> drSelected = objTaskOper.GetUserList();
            int count = 1;

            foreach (UserModels objUser in drSelected)
            {

                returnString += Convert.ToString(objUser.UserID) + ":" + objUser.UserName;

                if (drSelected.Count > count)
                {
                    returnString += ";";
                    count++;
                }
            }

            //returnString += "]";

            return returnString;
        }

        public ActionResult GridDemoData(int page, int rows, string search, string sidx, string sord)
        {
            TransactionModels objTra = Session["objTran"] as TransactionModels;
            int currentPage = Convert.ToInt32(page) - 1;
            int totalRecords = 0;

            //var data = manageContacts.GetContactPaged(currentPage, rows, out totalRecords);
            //objTra.listTransactionItem.Add(new TransactionItems());

            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,

                rows = (
                           from m in objTra.listTransactionItem
                           select new
                           {
                               id = m.SrNo,
                               cell = new object[]
                                      {
                                          
                                           m.refItemID_ItemMas,
                                           m.TranQty,
                                           m.TranRate,
                                           m.TIID
                                      }
                           }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PerformCRUDAction(TransactionItems objTransactionItem)
        {
            TransactionModels objTra = Session["objTran"] as TransactionModels;
            bool result = false;
            switch (Request.Form["oper"])
            {
                case "add":
                    //contact.AddDate = DateTime.Now.Date;
                    //contact.ModifiedDate = DateTime.Now;
                    //result = manageContacts.AddContact(contact);
                    if (objTra.listTransactionItem.Count() > 0)
                    {
                        objTransactionItem.SrNo = objTra.listTransactionItem.Max(X => X.SrNo) + 1;
                    }
                    else
                    {
                        objTransactionItem.SrNo = 1;
                    }

                    objTra.listTransactionItem.Add(objTransactionItem);
                    Session["objTran"] = objTra;
                    result = true;
                    break;
                case "edit":
                    int id = Int32.Parse(Request.Form["id"]);
                    objTra.listTransactionItem.FirstOrDefault(X => X.SrNo == id).refItemID_ItemMas = objTransactionItem.refItemID_ItemMas;
                    objTra.listTransactionItem.FirstOrDefault(X => X.SrNo == id).TranQty = objTransactionItem.TranQty;
                    objTra.listTransactionItem.FirstOrDefault(X => X.SrNo == id).TranRate = objTransactionItem.TranRate;
                    Session["objTran"] = objTra;
                    //result = manageContacts.UpdateContact(contact, id);
                    result = true;
                    break;
                case "del":
                    id = Int32.Parse(Request.Form["id"]);
                    objTra.listTransactionItem.Remove(objTra.listTransactionItem.FirstOrDefault(X => X.SrNo == id));
                    Session["objTran"] = objTra;
                    //result = manageContacts.DeleteContact(id);
                    result = true;
                    break;
                default:
                    break;
            }
            return Json(result);
        }

        public ActionResult GridDemoDataChecklist(int page, int rows, string search, string sidx, string sord)
        {
            TransactionModels objTra = Session["objTran"] as TransactionModels;
            int currentPage = Convert.ToInt32(page) - 1;
            int totalRecords = 0;

            //var data = manageContacts.GetContactPaged(currentPage, rows, out totalRecords);
            //objTra.listTransactionItem.Add(new TransactionItems());

            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,

                rows = (
                           from m in objTra.listTasks
                           select new
                           {
                               id = m.SrNo,
                               cell = new object[]
                                      {
                                           m.TaskDesc,
                                           m.TaskDate,
                                           m.refToUserID_UserMas,
                                           m.TaskID
                                      }
                           }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PerformCRUDActionChecklist(TaskModels objTaskModel)
        {
            TransactionModels objTra = Session["objTran"] as TransactionModels;
            bool result = false;
            switch (Request.Form["oper"])
            {
                case "add":
                    //contact.AddDate = DateTime.Now.Date;
                    //contact.ModifiedDate = DateTime.Now;
                    //result = manageContacts.AddContact(contact);
                    if (objTra.listTransactionItem.Count() > 0)
                    {
                        objTaskModel.SrNo = objTra.listTasks.Max(X => X.SrNo) + 1;
                    }
                    else
                    {
                        objTaskModel.SrNo = 1;
                    }

                    objTra.listTasks.Add(objTaskModel);
                    Session["objTran"] = objTra;
                    result = true;
                    break;
                case "edit":
                    int id = Int32.Parse(Request.Form["id"]);
                    objTra.listTasks.FirstOrDefault(X => X.SrNo == id).TaskDate = objTaskModel.TaskDate;
                    objTra.listTasks.FirstOrDefault(X => X.SrNo == id).TaskDesc = objTaskModel.TaskDesc;
                    objTra.listTasks.FirstOrDefault(X => X.SrNo == id).refToUserID_UserMas = objTaskModel.refToUserID_UserMas;
                    Session["objTran"] = objTra;
                    //result = manageContacts.UpdateContact(contact, id);
                    result = true;
                    break;
                case "del":
                    id = Int32.Parse(Request.Form["id"]);
                    objTra.listTasks.Remove(objTra.listTasks.FirstOrDefault(X => X.SrNo == id));
                    Session["objTran"] = objTra;
                    //result = manageContacts.DeleteContact(id);
                    result = true;
                    break;
                default:
                    break;
            }
            return Json(result);
        }
    }


    public class TransactionCheckListController : BaseController
    {
        TransactionSetup objTranSetUp = new TransactionSetup();

        public ActionResult Index(int? ID)
        {
            TransactionSetup objTSetUp = new TransactionSetup();
            List<TransactionSetup> listTransactionSetup = objTSetUp.GetTransactionSetuplist();

            if (ID.HasValue)
            {
                objTSetUp.TranID = ID.Value;
                List<TransactionChecklist> listTransactionChkList = objTSetUp.GetTransactionCheckList(ID.Value);
                objTSetUp.listTransactionChkList = listTransactionChkList;
                Session["objTSetUp"] = objTSetUp;
            }

            else
            {
                if (listTransactionSetup.Count > 0)
                {
                    objTSetUp.TranID = listTransactionSetup[0].TranID;

                    List<TransactionChecklist> listTransactionChkList = objTSetUp.GetTransactionCheckList(objTSetUp.TranID);
                    objTSetUp.listTransactionChkList = listTransactionChkList;
                    Session["objTSetUp"] = objTSetUp;
                }
            }

            ViewBag.listTransactionSetup = listTransactionSetup;

            return View(objTSetUp);
        }

        [HttpPost]
        public ActionResult Index(TransactionSetup objTSetUp)
        {
            List<TransactionSetup> listTransactionSetup = objTSetUp.GetTransactionSetuplist();
            ViewBag.listTransactionSetup = listTransactionSetup;


            List<TransactionChecklist> listTransactionChkList = objTSetUp.GetTransactionCheckList(objTSetUp.TranID);
            objTSetUp.listTransactionChkList = listTransactionChkList;
            Session["objTSetUp"] = objTSetUp;

            return View(objTSetUp);
        }


        public ActionResult Add()
        {
            TransactionSetup objTSetUp = Session["objTSetUp"] as TransactionSetup;
            TransactionChecklist transactionChecklist = new TransactionChecklist();

            transactionChecklist.ID = -1;
            transactionChecklist.refTransID_TransSetup = objTSetUp.TranID;
            return PartialView("_Add", transactionChecklist);
        }

        [HttpPost]
        public ActionResult Add(TransactionChecklist transactionChecklist)
        {
            if (ModelState.IsValid)
                objTranSetUp.SaveTransactionCheckList(transactionChecklist, intUserID, strTerminal);
            else
                return PartialView("_Add", transactionChecklist);

            //return RedirectToAction("Index", new { ID = objParameterValue.refSetupID_ParameterSetup });
            string url = Url.Action("Index", "TransactionCheckList", new { ID = transactionChecklist.refTransID_TransSetup });
            return Json(new { success = true, url = url });
        }

        public ActionResult Edit(int ID)
        {
            TransactionSetup objTSetUp = Session["objTSetUp"] as TransactionSetup;
            TransactionChecklist transactionChecklist = objTSetUp.listTransactionChkList.FirstOrDefault(X => X.ID == ID);

            return PartialView("_Edit", transactionChecklist);
        }

        [HttpPost]
        public ActionResult Edit(TransactionChecklist transactionChecklist)
        {
            if (ModelState.IsValid)
                objTranSetUp.SaveTransactionCheckList(transactionChecklist, intUserID, strTerminal);
            else
                return PartialView("_Edit", transactionChecklist);

            //return RedirectToAction("Index", new { ID = objParameterValue.refSetupID_ParameterSetup });
            string url = Url.Action("Index", "TransactionCheckList", new { ID = transactionChecklist.refTransID_TransSetup });
            return Json(new { success = true, url = url });
        }

        public ActionResult Delete(int ID)
        {
            TransactionSetup objTSetUp = Session["objTSetUp"] as TransactionSetup;
            TransactionChecklist transactionChecklist = objTSetUp.listTransactionChkList.FirstOrDefault(X => X.ID == ID);

            return PartialView("_Delete", transactionChecklist);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            objTranSetUp.DeleteTransactionCheckList(ID.Value);
            TransactionSetup objParameter = Session["objTSetUp"] as TransactionSetup;

            //return RedirectToAction("Index", new { ID = objParameter.SetupID });

            string url = Url.Action("Index", "TransactionCheckList", new { ID = objParameter.TranID });
            return Json(new { success = true, url = url });
        }
    }
}
