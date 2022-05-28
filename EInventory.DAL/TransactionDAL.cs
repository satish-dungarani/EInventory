using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInventory.DAL
{
    public class TransactionDAL : BaseDAL
    {
        public DataTable GetTransactionList(int TranTypeID, int CompID)
        {
            DataTable dtTransactionList = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTransactionList"))
                {

                    objDB.AddInParameter(objCMD, "@TransactionTypeID",
                                                  DbType.Int32, TranTypeID);
                    objDB.AddInParameter(objCMD, "@CompID",
                                        DbType.Int32, CompID);

                    dtTransactionList = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtTransactionList;
        }

        public DataSet GetTransactionDetails(int TranID)
        {
            DataSet dsTranDetails = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTransactionDetails"))
                {

                    objDB.AddInParameter(objCMD, "@TranID",
                                                  DbType.Int32, TranID);

                    dsTranDetails = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsTranDetails;
        }

        public int DeleteTransaction(int TranID)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteTransaction"))
                {

                    objDB.AddInParameter(objCMD, "@TranID",
                                                  DbType.Int32, TranID);

                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }
        public int SaveTransaction(int TranID,
                                     int refID_Company,
                                     string Remark,
                                     string TranDispID,
                                     DateTime TransDate,
                                     int refID_TransactionType,
                                     int refPartyID_EntityMas,
                                     bool WithMaterial,
                                     DateTime? ExpDueDate,
                                     int UserID,
                                     string temrinal,
                                     DataTable dtTransactionItems,
                                     bool IsFollowUp,
                                     DataTable dtTasks)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveTransaction"))
                {

                    objDB.AddInParameter(objCMD, "@TranID",
                                                  DbType.Int32, TranID);
                    objDB.AddInParameter(objCMD, "@refID_Company",
                                                  DbType.Int32, refID_Company);
                    objDB.AddInParameter(objCMD, "@Remark",
                                                  DbType.String, Remark);
                    objDB.AddInParameter(objCMD, "@TranDispID",
                                                  DbType.String, TranDispID);
                    objDB.AddInParameter(objCMD, "@TransDate",
                                                  DbType.DateTime, TransDate);
                    objDB.AddInParameter(objCMD, "@refID_TransactionType",
                                                  DbType.Int32, refID_TransactionType);
                    objDB.AddInParameter(objCMD, "@refPartyID_EntityMas",
                                                  DbType.Int32, refPartyID_EntityMas);
                    objDB.AddInParameter(objCMD, "@WithMaterial",
                                                  DbType.Boolean, WithMaterial);
                    objDB.AddInParameter(objCMD, "@ExpDueDate",
                                                  DbType.Date, ExpDueDate);
                    objDB.AddInParameter(objCMD, "@InsUser",
                                                 DbType.Int32, UserID);
                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                                 DbType.String, temrinal);

                    SqlParameter dtSql1 = new SqlParameter("@TransactionItem", SqlDbType.Structured);
                    dtSql1.Value = dtTransactionItems;
                    objCMD.Parameters.Add(dtSql1);

                    objDB.AddInParameter(objCMD, "@IsFollowUP",
                                                 DbType.String, IsFollowUp);
                    SqlParameter dtSql2 = new SqlParameter("@Tasks", SqlDbType.Structured);
                    dtSql2.Value = dtTasks;
                    objCMD.Parameters.Add(dtSql2);


                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }


        public DataTable GetTransactionSetupData(int TranTypeID)
        {
            DataTable returnValue = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTransactionNo"))
                {

                    objDB.AddInParameter(objCMD, "@TranTypeID",
                                                  DbType.Int32, TranTypeID);

                    returnValue = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public DataTable GetItemsByType(int CompID, string CatID)
        {
            DataTable returnValue = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetItemsByType"))
                {

                    objDB.AddInParameter(objCMD, "@CompID",
                                                  DbType.Int32, CompID);


                    objDB.AddInParameter(objCMD, "@CatID",
                                                  DbType.String, CatID);

                    returnValue = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }
    }

    public class TransactionCheckListDAL : BaseDAL
    {
        public DataTable GetTransactionSetuplist()
        {
            DataTable dtTransactionSetupList = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTransactionSetuplist"))
                {
                    dtTransactionSetupList = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtTransactionSetupList;
        }

        public DataTable GetTransactionCheckList(int TransID)
        {
            DataTable dtResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTransactionCheckList"))
                {

                    objDB.AddInParameter(objCMD, "@TransID",
                                         DbType.Int32, TransID);

                    dtResult = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtResult;
        }

        public int SaveTransactionCheckList(int ID,
                                              int refTransID_TransSetup,
                                              decimal OrdNo,
                                              string ChklstDesc,
                                              int FllowDays,
                                              int UserID,
                                              string terminal)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveTransactionCheckList"))
                {

                    objDB.AddInParameter(objCMD, "@ID",
                                         DbType.Int32, ID);
                    objDB.AddInParameter(objCMD, "@refTransID_TransSetup",
                                        DbType.Int32, refTransID_TransSetup);
                    objDB.AddInParameter(objCMD, "@OrdNo",
                                        DbType.Decimal, OrdNo);
                    objDB.AddInParameter(objCMD, "@ChklstDesc",
                                        DbType.String, ChklstDesc);
                    objDB.AddInParameter(objCMD, "@FllowDays",
                               DbType.Int32, FllowDays);
                    objDB.AddInParameter(objCMD, "@InsUser",
                                        DbType.Int32, UserID);
                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                        DbType.String, terminal);

                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }


        public int DeleteTransactionCheckList(int ID)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteTransactionCheckList"))
                {

                    objDB.AddInParameter(objCMD, "@ID",
                                         DbType.Int32, ID);

                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

    }
}
