using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using EInventory.DAL;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;

namespace EInventory.Models
{
    public class TransactionModels : BaseModels
    {

        TransactionDAL objTranDAL;
        ItemsDAL objItemDAL;

        public TransactionModels()
        {
            objTranDAL = new TransactionDAL();
            objItemDAL = new ItemsDAL();
            this.listTransactionItem = new List<TransactionItems>();
            this.listTasks = new List<TaskModels>();

            objItemDAL.ConnectionString = ConnectionString;
            objTranDAL.ConnectionString = ConnectionString;

            IsDisplayWithMaterial = true;
            IsDisplayParty = true;
            IsDisplayExpDate = true;

        }

        public int TranID { get; set; }

        public int? refID_Company { get; set; }

        [MaxLength(1000)]
        public string Remark { get; set; }

        [Display(Name = "Tran No.")]
        public string TranDispID { get; set; }

        [Display(Name = "Tran. Date")]
        [Required]
        public DateTime TransDate { get; set; }

        public int refID_TransactionType { get; set; }

        [Display(Name = "Party")]
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select party")]
        public int refPartyID_EntityMas { get; set; }

        public bool WithMaterial { get; set; }

        [Display(Name = "Exp. Due Date")]
        public DateTime? ExpDueDate { get; set; }

        [EnsureOneElement(ErrorMessage = "Please enter atleast one Item")]
        public List<TransactionItems> listTransactionItem { get; set; }

        public List<TaskModels> listTasks { get; set; }

        public bool IsDisplayWithMaterial { get; set; }
        public bool IsDisplayExpDate { get; set; }
        public bool IsDisplayParty { get; set; }

        public bool IsFollowUP { get; set; }
        public bool IsAllowedFollowUp { get; set; }
        public TransactionSetupData GetTransactionSetupData(int TranTypeID)
        {
            TransactionSetupData returnValue = null;

            try
            {
                DataTable dtResult = objTranDAL.GetTransactionSetupData(TranTypeID);
                returnValue = dtResult.ToList<TransactionSetupData>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public DataTable GetTransactionList(int TranTypeID, int CompID)
        {
            DataTable dtTransactionList = null;

            try
            {
                dtTransactionList = objTranDAL.GetTransactionList(TranTypeID, CompID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtTransactionList;
        }

        public TransactionModels GetTransactionDetails(int TranID)
        {
            DataSet dsTranDetails = null;
            TransactionModels objTransation = null;

            try
            {
                dsTranDetails = objTranDAL.GetTransactionDetails(TranID);

                objTransation = dsTranDetails.Tables[0].ToList<TransactionModels>().FirstOrDefault();
                objTransation.listTransactionItem = dsTranDetails.Tables[1].ToList<TransactionItems>();
                objTransation.listTasks = dsTranDetails.Tables[2].ToList<TaskModels>();

                if (dsTranDetails.Tables[2].Rows.Count > 0)
                    objTransation.IsFollowUP = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objTransation;
        }


        public int DeleteTransaction(int TranID)
        {
            int returnValue = -1;

            try
            {
                returnValue = objTranDAL.DeleteTransaction(TranID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public int SaveTransaction(TransactionModels objTransaction,
                                   int UserID,
                                   int CompID,
                                   string temrinal)
        {
            int returnValue = -1;

            try
            {
                DataTable dtTransactionItems = objTransaction.listTransactionItem.ToDataTable<TransactionItems>();

                DataTable dtTasks = objTransaction.listTasks.ToDataTable<TaskModels>();
                dtTasks = dtTasks.DefaultView.ToTable(false,
                                                        "TaskID",
                                                        "TaskDesc",
                                                        "TaskDate",
                                                        "refToUserID_UserMas",
                                                        "SrNo");

                returnValue = objTranDAL.SaveTransaction(objTransaction.TranID,
                                                         CompID,
                                                         objTransaction.Remark,
                                                         objTransaction.TranDispID,
                                                         objTransaction.TransDate,
                                                         objTransaction.refID_TransactionType,
                                                         objTransaction.refPartyID_EntityMas,
                                                         objTransaction.WithMaterial,
                                                         objTransaction.ExpDueDate,
                                                         UserID,
                                                         temrinal,
                                                         dtTransactionItems,
                                                         objTransaction.IsFollowUP,
                                                         dtTasks);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }


        public List<PartyMaster> GetAllVendors(int TypeID)
        {
            List<PartyMaster> listParty = objItemDAL.GetAllVendors(TypeID).ToList<PartyMaster>();

            if (listParty == null)
                listParty = new List<PartyMaster>();

            PartyMaster objNew = new PartyMaster();
            objNew.PartyID = -1;
            objNew.PartyName = SELECTITEM;

            listParty.Insert(0, objNew);
            return listParty;
        }

        public DataTable GetItemsByType(int CompID, string CatID)
        {
            DataTable returnValue = null;

            try
            {

                returnValue = objTranDAL.GetItemsByType(CompID, CatID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }


    }

    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
    public class TransactionItems
    {
        //public TransactionItems()
        //{
        //}
        public int TIID { get; set; }

        public int refTranID_TranMas { get; set; }

        public int? SrNo { get; set; }

        public int refID_Company { get; set; }

        public int? refItemID_ItemMas { get; set; }

        public int? refMachineID_MachineMas { get; set; }

        public decimal? TranQty { get; set; }

        public decimal? TranRate { get; set; }

        public decimal? TranAmount { get; set; }

        public int? refPurchase_ID { get; set; }

    }

    public class TransactionChecklist : BaseModels
    {

        public int ID { get; set; }

        public int refTransID_TransSetup { get; set; }

        [Required]
        [Display(Name = "Ord. No")]
        public decimal OrdNo { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Checklist Description")]
        public string ChklstDesc { get; set; }

        public string ShortDesc { get; set; }

        [Required]
        [Display(Name = "Followup Days")]
        public int FllowDays { get; set; }
    }

    public class TransactionSetup : BaseModels
    {

        TransactionCheckListDAL objChkLstDAL;

        public TransactionSetup()
        {
            objChkLstDAL = new TransactionCheckListDAL();
            objChkLstDAL.ConnectionString = ConnectionString;
        }

        [Display(Name = "Transaction Type")]
        public int TranID { get; set; }

        public string TransactionType { get; set; }

        public bool? StockEffect { get; set; }

        public bool AllowMegativeBal { get; set; }

        public List<TransactionSetup> GetTransactionSetuplist()
        {
            return objChkLstDAL.GetTransactionSetuplist().ToList<TransactionSetup>();
        }

        public List<TransactionChecklist> listTransactionChkList
        {
            get;
            set;
        }

        public int SaveTransactionCheckList(TransactionChecklist objChkList, int UserID, string Terminal)
        {
            return objChkLstDAL.SaveTransactionCheckList(objChkList.ID,
                                                         objChkList.refTransID_TransSetup,
                                                         objChkList.OrdNo,
                                                         objChkList.ChklstDesc,
                                                         objChkList.FllowDays,
                                                         UserID,
                                                         Terminal);
        }

        public int DeleteTransactionCheckList(int ID)
        {
            return objChkLstDAL.DeleteTransactionCheckList(ID);
        }

        public List<TransactionChecklist> GetTransactionCheckList(int TransID)
        {
            return objChkLstDAL.GetTransactionCheckList(TransID).ToList<TransactionChecklist>();
        }

    }

    public class TransactionSetupData
    {
        public string TransactionNo { get; set; }
        public bool IsFollowUP { get; set; }
    }

    public enum TransactionType
    {
        PurchaseOrder = 1,
        Purchase = 2,
        PurchaseReturn = 3,
        DemoItemReceived = 4,
        DemoItemReturn = 5,
        Sales = 6,
        SalesReturn = 7,
        JobWorkIssue = 8,
        JobWorkreturn = 9,
        Found = 10,
        Less = 11,
        DemoItemIssueMachine = 12,
        DemoItemReturnMachine = 13,
        Scrapesale = 14,
        MachineMaster = 15,
        MachineOrder = 16,
        Followup = 17,
        Quatation = 18
    }


}