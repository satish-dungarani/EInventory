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
    public class RoleDAL : BaseDAL
    {
        public DataTable GetRoleList(int? RoleID)
        {
            DataTable dtRoles = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetRoleList"))
                {
                    objDB.AddInParameter(objCMD, "@RoleID",
                                      DbType.Int32, RoleID);

                    dtRoles = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtRoles;
        }

        public int SaveRole(int roleID,
                            string roleName,
                            int userID,
                            string terminal)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveRole"))
                {

                    objDB.AddInParameter(objCMD, "@RoleID",
                                      DbType.Int32, roleID);

                    objDB.AddInParameter(objCMD, "@RoleName",
                                      DbType.String, roleName);

                    objDB.AddInParameter(objCMD, "@InsUser",
                                      DbType.Int32, userID);

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


        public int DeleteRole(int roleID)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteRole"))
                {

                    objDB.AddInParameter(objCMD, "@RoleID",
                                      DbType.Int32, roleID);

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

    public class RoleWiseMenuDAL : BaseDAL
    {
        public DataTable GetRoleWiseMenus(int RoleID)
        {
            DataTable dtRoleWiseMenu = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetRoleWiseMenus"))
                {
                    objDB.AddInParameter(objCMD, "@RoleID",
                                      DbType.Int32, RoleID);

                    dtRoleWiseMenu = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtRoleWiseMenu;
        }

        public int SaveRoleWiseMenus(int RoleID,
                                     DataTable dtRoleWisMenu)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveRoleWiseMenu"))
                {

                    objDB.AddInParameter(objCMD, "@RoleID",
                                    DbType.Int32, RoleID);

                    SqlParameter dtSql1 = new SqlParameter("@RoleWiseMenu", SqlDbType.Structured);
                    dtSql1.Value = dtRoleWisMenu;

                    objCMD.Parameters.Add(dtSql1);

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
