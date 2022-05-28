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
    public class ParameterDAL : BaseDAL
    {
        //GetParameterSetupList
        //GetParameterValueList
        //SaveParameterValues

        public DataTable GetParameterSetupList()
        {
            DataTable dtParameterSetup = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetParameterSetupList"))
                {

                    dtParameterSetup = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtParameterSetup;
        }

        public DataTable GetParameterValueList(int SetupID)
        {

            DataTable dtParameter = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetParameterValueList"))
                {

                    objDB.AddInParameter(objCMD, "@SetupID",
                                         DbType.Int32, @SetupID);

                    dtParameter = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtParameter;
        }

        public int SaveParameterValues(int ID,
                                       int refCompID_Company,
                                       int refSetupID_ParameterSetup,
                                       string ParamVal,
                                       int userID,
                                       string terminal)
        {
            int result = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveParameterValues"))
                {
                    objDB.AddInParameter(objCMD, "@ID",
                                        DbType.Int32, ID);
                    objDB.AddInParameter(objCMD, "@refCompID_Company",
                                        DbType.Int32, refCompID_Company);
                    objDB.AddInParameter(objCMD, "@refSetupID_ParameterSetup",
                                        DbType.Int32, refSetupID_ParameterSetup);
                    objDB.AddInParameter(objCMD, "@ParamVal",
                                        DbType.String, ParamVal);
                    objDB.AddInParameter(objCMD, "@InsUser",
                                         DbType.Int32, userID);
                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                        DbType.String, terminal);

                    result = Convert.ToInt32(objDB.ExecuteNonQuery(objCMD));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public int DeleteParameterValues(int ID)
        {
            int result = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteParameterValues"))
                {
                    objDB.AddInParameter(objCMD, "@ID",
                                       DbType.Int32, ID);
                    result = Convert.ToInt32(objDB.ExecuteNonQuery(objCMD));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
