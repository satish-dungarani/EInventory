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
    public class PartyDAL : BaseDAL
    {
        public DataSet GetParties(int? partyID)
        {
            DataSet dsResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetParties"))
                {

                    objDB.AddInParameter(objCMD, "@PartyID",
                                         DbType.Int32, partyID);

                    dsResult = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsResult;
        }

        public int DeleteParty(int partyID)
        {
            int rowCount = 0;
            try
            {

                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteParty"))
                {

                    objDB.AddInParameter(objCMD, "@PartyID",
                                         DbType.Int32, partyID);

                    rowCount = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rowCount;
        }

        public int SaveParty(int PartyID,
                             int companyID,
                             string PartyName,
                             string EmailID,
                             string WebURL,
                             int userID,
                             string terminal,
                             string CategoryLinkage,
                             DataTable dtAddress)
        {
            int result = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveParty"))
                {

                    objDB.AddInParameter(objCMD, "@PartyID",
                                                        DbType.Int32, PartyID);
                    objDB.AddInParameter(objCMD, "@refCompID_Company",
                                                        DbType.String, companyID);
                    objDB.AddInParameter(objCMD, "@PartyName",
                                                         DbType.String, PartyName);
                    objDB.AddInParameter(objCMD, "@EmailID",
                                                         DbType.String, EmailID);
                    objDB.AddInParameter(objCMD, "@WebURL",
                                                         DbType.String, WebURL);
                    objDB.AddInParameter(objCMD, "@InsUser",
                                                         DbType.Int32, userID);
                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                                         DbType.String, terminal);
                    objDB.AddInParameter(objCMD, "@CategoryLinkage",
                                                         DbType.String, CategoryLinkage);

                    SqlParameter dtSql1 = new SqlParameter("@PartyAddress", SqlDbType.Structured);
                    dtSql1.Value = dtAddress;

                    objCMD.Parameters.Add(dtSql1);

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
