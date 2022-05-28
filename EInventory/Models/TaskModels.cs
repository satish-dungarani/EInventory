using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EInventory.DAL;

namespace EInventory.Models
{

    public class TaskOperations : BaseModels
    {
        TaskDAL objTaskDAL;
        UserDAL objUserDAL;
        ParameterDAL objParamVALDAL;

        public TaskOperations()
        {
            objTaskDAL = new TaskDAL();
            objTaskDAL.ConnectionString = ConnectionString;
            objUserDAL = new UserDAL();
            objUserDAL.ConnectionString = ConnectionString;
            objParamVALDAL = new ParameterDAL();
            objParamVALDAL.ConnectionString = ConnectionString;

        }

        public List<TaskModels> GetTaskCheckList(int TranID, int UserID)
        {
            //TaskModels objTask = null;

            //try
            //{
            //    DataSet dsResult = objTaskDAL.GetTaskCheckList(TranID);

            //    if (dsResult.Tables[0].Rows.Count > 0)
            //    {
            //        objTask = dsResult.Tables[0].ToList<TaskModels>().FirstOrDefault();
            //        if (objTask == null)
            //            objTask = new TaskModels();

            //        objTask.listTaskRemarks = dsResult.Tables[1].ToList<TaskRemarks>();
            //    }
            //    else
            //    {
            //        objTask = new TaskModels();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return objTask;

            List<TaskModels> listTasks = null;

            try
            {
                DataSet dsResult = objTaskDAL.GetTaskCheckList(TranID, UserID);

                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    listTasks = dsResult.Tables[0].ToList<TaskModels>();
                }
                else
                {
                    listTasks = new List<TaskModels>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listTasks;
        }

        public TaskModels GetTaskList(int TaskID)
        {
            TaskModels objTaskModel = null;

            try
            {
                //Use the database connection string here...
                DataSet dsTask = objTaskDAL.GetTaskList(TaskID);

                objTaskModel = dsTask.Tables[0].ToList<TaskModels>().FirstOrDefault();
                objTaskModel.listTaskRemarks = dsTask.Tables[1].ToList<TaskRemarks>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objTaskModel;
        }

        public DataTable GetTaskList()
        {
            DataTable dtTasks = null;

            try
            {
                //Use the database connection string here...
                dtTasks = objTaskDAL.GetTaskList(null).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtTasks;

        }
        public int SaveTask(TaskModels objTask, int userID, string terminal)
        {
            int returnValue = 0;

            try
            {

                DataTable dtTaskRemarks = objTask.listTaskRemarks.ToDataTable<TaskRemarks>();

                if (objTask.IsTaksCompleted == true)
                    objTask.TaskCompletedOn = DateTime.Now;

                dtTaskRemarks = dtTaskRemarks.DefaultView.ToTable(false,
                                                                   "ID",
                                                                   "Remarks",
                                                                   "NextTaskDueDate",
                                                                   "SrNo",
                                                                   "refTaskStatusID_ParamVal",
                                                                   "TaskID");

                returnValue = objTaskDAL.SaveTask(  objTask.refTransID,
                                                    userID,
                                                    terminal,
                                                    dtTaskRemarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public int DeleteTask(int TaskID)
        {
            int returnValue = 0;

            try
            {
                returnValue = objTaskDAL.DeleteTask(TaskID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public List<UserModels> GetUserList()
        {
            DataTable dtUsers = objUserDAL.GetUsers(null).Tables[0];
            return dtUsers.ToList<UserModels>();
        }
        public List<ParameterValues> GetParameterValueList(int setupID)
        {
            DataTable dtParameterValues = objParamVALDAL.GetParameterValueList(setupID);
            return dtParameterValues.ToList<ParameterValues>();
        }
    }

    public class TaskModels : BaseModels
    {
        TaskOperations objTOper = null;
        public TaskModels()
        {
            objTOper = new TaskOperations();
            this.listTaskRemarks = new List<TaskRemarks>();
            this.listUsers = objTOper.GetUserList();
            this.listTaskStatus = objTOper.GetParameterValueList(7);
        }

        public int TaskID { get; set; }

        [Display(Name = "Task Date")]
        public DateTime TaskDate { get; set; }

        [Display(Name = "Task Desc.")]
        public string TaskDesc { get; set; }

        [Display(Name = "Assign User")]
        public int refToUserID_UserMas { get; set; }

        [Display(Name = "Task Status")]
        public int refTaskStatusID_ParamVal { get; set; }

        public DateTime? TaskCompletedOn { get; set; }

        [Display(Name = "Is Completed")]
        public bool IsTaksCompleted { get; set; }

        public int refTransID { get; set; }

        [Display(Name = "Tran. No")]
        public string TranDispID { get; set; }

        public int TranTypeID { get; set; }

        public long SrNo { get; set; }

        public List<TaskRemarks> listTaskRemarks { get; set; }

        public List<UserModels> listUsers { get; set; }

        public List<ParameterValues> listTaskStatus { get; set; }
    }

    public class TaskRemarks
    {
        public int ID { get; set; }

        public string Remarks { get; set; }

        public DateTime? NextTaskDueDate { get; set; }

        public long SrNo { get; set; }

        public int TaskID { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskDesc { get; set; }
        public string UserName { get; set; }
        public int refTaskStatusID_ParamVal { get; set; }
        public int refToUserID_UserMas { get; set; }
    }
}