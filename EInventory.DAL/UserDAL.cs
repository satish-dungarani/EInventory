using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInventory.DAL
{
    public class UserDAL : BaseDAL
    {

        public string AuthenticateUser(string username, string password)
        {
            string result = string.Empty;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("AuthenticateUser"))
                {
                    objDB.AddInParameter(objCMD, "@UserName",
                                         DbType.String, username);

                    objDB.AddInParameter(objCMD, "@Password",
                                         DbType.String, password);

                    result = Convert.ToString(objDB.ExecuteScalar(objCMD));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public DataSet GetSessionData(string sessionID)
        {
            DataSet dsResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetSessionData"))
                {
                    objDB.AddInParameter(objCMD, "@SessionID",
                                         DbType.String, sessionID);

                    dsResult = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsResult;
        }
        public int SetLogout(string sessionID)
        {
            int rowCount = 0;
            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SetLogout"))
                {
                    objDB.AddInParameter(objCMD, "@SessionID",
                                         DbType.String, sessionID);

                    rowCount = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rowCount;
        }
        public DataSet GetUsers(int? UserID)
        {
            DataSet dsResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetUsers"))
                {
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
        public int DeleteUser(int UserID)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteUser"))
                {
                    objDB.AddInParameter(objCMD, "@UserID",
                                         DbType.Int32, UserID);

                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }
        public int SaveUser(int UserID, string UserName, string Password, string FirstName, string LastName, string PhotoPath, string Gender, DateTime? DOB,
                            string MobileNo, string EmailAddress, bool IsActive, int CompanyID, int RoleID)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveUser"))
                {
                    objDB.AddInParameter(objCMD, "@UserID",
                                         DbType.Int32, UserID);
                    objDB.AddInParameter(objCMD, "@UserName",
                                       DbType.String, UserName);
                    objDB.AddInParameter(objCMD, "@Password",
                                      DbType.String, Password);
                    objDB.AddInParameter(objCMD, "@FirstName",
                                      DbType.String, FirstName);
                    objDB.AddInParameter(objCMD, "@LastName",
                                      DbType.String, LastName);
                    objDB.AddInParameter(objCMD, "@PhotoPath",
                                      DbType.String, PhotoPath);
                    objDB.AddInParameter(objCMD, "@Gender",
                                      DbType.String, Gender);
                    objDB.AddInParameter(objCMD, "@DOB",
                                      DbType.Date, DOB);
                    objDB.AddInParameter(objCMD, "@MobileNo",
                                      DbType.String, MobileNo);
                    objDB.AddInParameter(objCMD, "@EmaiAddress",
                                      DbType.String, EmailAddress);
                    objDB.AddInParameter(objCMD, "@IsActive",
                                      DbType.Boolean, IsActive);
                    objDB.AddInParameter(objCMD, "@refCompID_Company",
                                      DbType.Int32, CompanyID);
                    objDB.AddInParameter(objCMD, "@RoleID",
                                      DbType.Int32, RoleID);

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
