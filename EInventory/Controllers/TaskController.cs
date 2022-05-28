using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EInventory.Models;
using System.Data;

namespace EInventory.Controllers
{
    public class TaskController : BaseController
    {
        TaskOperations objTaskModel = new TaskOperations();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult LoadTasksAjaxHandler()
        {
            DataTable dtTasks = objTaskModel.GetTaskList();

            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtTasks.AsEnumerable()
                         select new
                         {
                             refTransID = dr["refTransID"],
                             TranDispID = dr["TranDispID"],
                             IsCompleted = dr["IsCompleted"],
                             NoOfFollowUp = dr["NoOfFollowUp"],
                             CompletedFollowUp = dr["CompletedFollowUp"],
                             TransactionType = dr["TransactionType"]
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtTasks.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddTask(int ID)
        {
            TaskModels objTask = objTaskModel.GetTaskCheckList(ID, -1).FirstOrDefault();
            objTask.TaskDate = DateTime.Now;
            Session["objTask"] = objTask;

            return PartialView(objTask);
        }

        [HttpPost]
        public ActionResult AddTask(TaskModels objTaskA)
        {

            TaskModels objTask = Session["objTask"] as TaskModels;
            objTaskA.listTaskRemarks = objTask.listTaskRemarks;

            if (ModelState.IsValid)
            {
                objTaskModel.SaveTask(objTaskA, intUserID, strTerminal);
                Session["objTask"] = objTask;

                string url = Url.Action("Index", "Transactions", new { ID = objTask.TranTypeID });
                return Json(new { success = true, url = url });
                //return RedirectToAction("Index", "Transactions", new { ID = objTask.TranTypeID });

            }
            Session["objTask"] = objTask;
            objTaskA.listUsers = objTask.listUsers;
            objTaskA.listTaskStatus = objTask.listTaskStatus;

            return PartialView(objTaskA);
        }


        public ActionResult GridDemoData(int page, int rows, string search, string sidx, string sord)
        {
            TaskModels objTask = Session["objTask"] as TaskModels;
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
                           from m in objTask.listTaskRemarks
                           select new
                           {
                               id = m.SrNo,
                               cell = new object[]
                                      {
                                          
		                                   m.TaskDate,
		                                   m.TaskDesc,
		                                   m.UserName,
		                                   m.NextTaskDueDate,
                                           m.Remarks,
                                           m.refTaskStatusID_ParamVal,
                                           m.ID,
                                           m.TaskID
                                      }
                           }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PerformCRUDAction(TaskRemarks objTaskRemarks)
        {
            TaskModels objTask = Session["objTask"] as TaskModels;
            bool result = false;
            switch (Request.Form["oper"])
            {
                case "add":
                    //contact.AddDate = DateTime.Now.Date;
                    //contact.ModifiedDate = DateTime.Now;
                    //result = manageContacts.AddContact(contact);
                    if (objTask.listTaskRemarks.Count() > 0)
                    {
                        objTaskRemarks.SrNo = objTask.listTaskRemarks.Max(X => X.SrNo) + 1;
                    }
                    else
                    {
                        objTaskRemarks.SrNo = 1;
                    }

                    objTask.listTaskRemarks.Add(objTaskRemarks);
                    Session["objTask"] = objTask;
                    result = true;
                    break;
                case "edit":
                    int id = Int32.Parse(Request.Form["id"]);
                    objTask.listTaskRemarks.FirstOrDefault(X => X.SrNo == id).Remarks = objTaskRemarks.Remarks;
                    objTask.listTaskRemarks.FirstOrDefault(X => X.SrNo == id).NextTaskDueDate = objTaskRemarks.NextTaskDueDate;
                    objTask.listTaskRemarks.FirstOrDefault(X => X.SrNo == id).refTaskStatusID_ParamVal = objTaskRemarks.refTaskStatusID_ParamVal;
                    Session["objTask"] = objTask;

                    result = true;
                    break;
                case "del":
                    id = Int32.Parse(Request.Form["id"]);
                    objTask.listTaskRemarks.Remove(objTask.listTaskRemarks.FirstOrDefault(X => X.SrNo == id));
                    Session["objTask"] = objTask;
                    //result = manageContacts.DeleteContact(id);
                    result = true;
                    break;
                default:
                    break;
            }
            return Json(result);
        }

        public ActionResult EditTask(int ID)
        {
            ViewBag.PageName = "Edit FollowUp";
            TaskModels objTask = objTaskModel.GetTaskList(ID);
            Session["objTask"] = objTask;

            return PartialView(objTask);
        }

        [HttpPost]
        public ActionResult EditTask(TaskModels objTaskA)
        {

            TaskModels objTask = Session["objTask"] as TaskModels;
            objTaskA.listTaskRemarks = objTask.listTaskRemarks;

            if (ModelState.IsValid)
            {
                objTaskModel.SaveTask(objTaskA, intUserID, strTerminal);
                Session["objTask"] = objTask;

                //string url = Url.Action("Index", "Transactions", new { ID = objTask.TranTypeID });
                //return Json(new { success = true, url = url });
                return RedirectToAction("Index");

            }
            Session["objTask"] = objTask;
            objTaskA.listUsers = objTask.listUsers;
            objTaskA.listTaskStatus = objTask.listTaskStatus;

            return PartialView(objTaskA);
        }


        public ActionResult DeleteTask(int ID)
        {
            TaskModels objTask = objTaskModel.GetTaskList(ID);

            return PartialView(objTask);
        }

        [HttpPost]
        public ActionResult DeleteTask(int? ID)
        {
            objTaskModel.DeleteTask(ID.Value);

            string url = Url.Action("Index", "Task");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Index");
        }


        public string GetTaskStatus()
        {

            string returnString = "";
            List<ParameterValues> drSelected = objTaskModel.GetParameterValueList(7);
            int count = 1;

            foreach (ParameterValues objStatus in drSelected)
            {

                returnString += Convert.ToString(objStatus.ID) + ":" + objStatus.ParamVal;

                if (drSelected.Count > count)
                {
                    returnString += ";";
                    count++;
                }
            }

            //returnString += "]";

            return returnString;
        }


    }
}
