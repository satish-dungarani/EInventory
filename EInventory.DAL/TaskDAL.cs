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
    public class TaskDAL : BaseDAL
    {
        public DataSet GetTaskList(int? TaskID)
        {
            DataSet dsResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTaskList"))
                {

                    objDB.AddInParameter(objCMD, "@TranID",
                                           DbType.Int32, TaskID);


                    dsResult = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsResult;
        }

        public int SaveTask(int refTransID, int InsUser, string InsTerminal, DataTable dtTaskRemarks)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveTask"))
                {

                    objDB.AddInParameter(objCMD, "@refTransID",
                                      DbType.Int32, refTransID);

                    objDB.AddInParameter(objCMD, "@InsUser",
                                      DbType.Int32, InsUser);

                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                      DbType.String, InsTerminal);

                    SqlParameter dtSql2 = new SqlParameter("@TaskRemarks", SqlDbType.Structured);
                    dtSql2.Value = dtTaskRemarks;

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

        public int DeleteTask(int TaskID)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteTask"))
                {

                    objDB.AddInParameter(objCMD, "@TaskID",
                                         DbType.Int32, TaskID);

                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public DataSet GetTaskCheckList(int TranTypeID, int UserID)
        {
            DataSet dsResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetTaskCheckList"))
                {
                    objDB.AddInParameter(objCMD, "@TranTypeID",
                                         DbType.Int32, TranTypeID);

                    objDB.AddInParameter(objCMD, "@UserID",
                                         DbType.Int32, UserID);

                    dsResult = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsResult;
        }

    }

}
