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
    public class CompanyDAL : BaseDAL
    {
        public int SaveCompany(int CompID,
                                    string Name,
                                    string Addr,
                                    string City,
                                    string State,
                                    string Pincode,
                                    string Country,
                                    string Contact1,
                                    string Contact2,
                                    string EmailID,
                                    string WebURL,
                                    int userID,
                                    string terminal,
                                    DataTable dtCompanyAuthrorizePerson,
                                    DataTable dtCompanySatutory)
        {
            int result = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveCompany"))
                {

                    objDB.AddInParameter(objCMD, "@CompID",
                                         DbType.Int32, CompID);
                    objDB.AddInParameter(objCMD, "@Name",
                                                         DbType.String, Name);
                    objDB.AddInParameter(objCMD, "@Addr",
                                                         DbType.String, Addr);
                    objDB.AddInParameter(objCMD, "@City",
                                                         DbType.String, City);
                    objDB.AddInParameter(objCMD, "@State",
                                                         DbType.String, State);
                    objDB.AddInParameter(objCMD, "@Pincode",
                                                         DbType.String, Pincode);
                    objDB.AddInParameter(objCMD, "@Country",
                                                         DbType.String, Country);
                    objDB.AddInParameter(objCMD, "@Contact1",
                                                         DbType.String, Contact1);
                    objDB.AddInParameter(objCMD, "@Contact2",
                                                         DbType.String, Contact2);
                    objDB.AddInParameter(objCMD, "@EmailID",
                                                         DbType.String, EmailID);
                    objDB.AddInParameter(objCMD, "@WebURL",
                                                         DbType.String, WebURL);
                    objDB.AddInParameter(objCMD, "@InsUser",
                                                         DbType.Int32, userID);
                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                                         DbType.String, terminal);

                    SqlParameter dtSql1 = new SqlParameter("@CompanyAuthorizePerson", SqlDbType.Structured);
                    dtSql1.Value = dtCompanyAuthrorizePerson;
                    SqlParameter dtSql2 = new SqlParameter("@CompanyStatutory", SqlDbType.Structured);
                    dtSql2.Value = dtCompanySatutory;

                    objCMD.Parameters.Add(dtSql1);
                    objCMD.Parameters.Add(dtSql2);

                    result = Convert.ToInt32(objDB.ExecuteNonQuery(objCMD));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public DataSet GetCompanies(int? CompID)
        {
            DataSet dsCompanies = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetCompanies"))
                {

                    objDB.AddInParameter(objCMD, "@CompID",
                                         DbType.Int32, CompID);

                    dsCompanies = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCompanies;
        }

        public int DeleteCompany(int CompID)
        {
            int rowCount = 0;
            try
            {
                
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteCompany"))
                {

                    objDB.AddInParameter(objCMD, "@CompID",
                                         DbType.Int32, CompID);

                    rowCount = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rowCount;
        }
    }
}
