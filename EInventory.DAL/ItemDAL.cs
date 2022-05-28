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
    public class ItemsDAL : BaseDAL
    {
        public DataSet GetItems(int CompID,
                                  int? ItemID)
        {
            DataSet dsResult = null;
            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetItems"))
                {

                    objDB.AddInParameter(objCMD, "@CompID",
                                         DbType.Int32, CompID);


                    objDB.AddInParameter(objCMD, "@ItemID",
                                         DbType.Int32, ItemID);

                    dsResult = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsResult;
        }

        public int SaveItem(int ItemID, int CompID, int CatID, string ItemName, string ItemDesc, int UOMID, decimal ReOrderLevel,
                            int MaxDeliveryDays, string Size, string Material, int UserID, string Terminal, bool IsFinalProduct, DataTable dtConsumption, DataTable dtItemVendor)
        {
            int returnValue = 0;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveItem"))
                {

                    objDB.AddInParameter(objCMD, "@ItemID",
                                         DbType.Int32, ItemID);
                    objDB.AddInParameter(objCMD, "@refCompID_Company",
                                         DbType.Int32, CompID);
                    objDB.AddInParameter(objCMD, "@refCatID_CatMas",
                                         DbType.Int32, CatID);
                    objDB.AddInParameter(objCMD, "@ItemName",
                                         DbType.String, ItemName);
                    objDB.AddInParameter(objCMD, "@ItemDesc",
                                         DbType.String, ItemDesc);
                    objDB.AddInParameter(objCMD, "@refUOMID_ParamValue",
                                         DbType.Int32, UOMID);
                    objDB.AddInParameter(objCMD, "@ReOrdLevel",
                                         DbType.Decimal, ReOrderLevel);
                    objDB.AddInParameter(objCMD, "@MaxDeliveryDays",
                                         DbType.Int32, MaxDeliveryDays);

                    objDB.AddInParameter(objCMD, "@Size",
                                         DbType.String, Size);
                    objDB.AddInParameter(objCMD, "@Material",
                                         DbType.String, Material);

                    objDB.AddInParameter(objCMD, "@InsUser",
                                         DbType.Int32, UserID);
                    objDB.AddInParameter(objCMD, "@InsTerminal",
                                         DbType.String, Terminal);
                    objDB.AddInParameter(objCMD, "@IsFinalProduct",
                                         DbType.Boolean, IsFinalProduct);

                    SqlParameter dtSql1 = new SqlParameter("@FinishItemConsumption", SqlDbType.Structured);
                    dtSql1.Value = dtConsumption;


                    SqlParameter dtSql2 = new SqlParameter("@ItemVendor", SqlDbType.Structured);
                    dtSql2.Value = dtItemVendor;

                    objCMD.Parameters.Add(dtSql1);
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


        public int DeleteItem(int ItemID)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteItem"))
                {

                    objDB.AddInParameter(objCMD, "@ItemID",
                                         DbType.Int32, ItemID);

                    returnValue = objDB.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public DataTable GetRowMaterials()
        {
            DataTable dtRowMaterials = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetRowMaterials"))
                {
                    dtRowMaterials = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtRowMaterials;
        }

        public DataTable GetAllVendors(int TypeID)
        {
            DataTable dtVendorList = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetPartiesByType"))
                {
                    objDB.AddInParameter(objCMD, "@PartyType",
                                                    DbType.Int32, TypeID);

                    dtVendorList = objDB.ExecuteDataSet(objCMD).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtVendorList;
        }
    }

    public class ItemCategoryDAL : BaseDAL
    {
        public DataSet GetCategories(int? catID)
        {
            DataSet dsResult = null;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("GetCategories"))
                {

                    objDB.AddInParameter(objCMD, "@CatID",
                                         DbType.Int32, catID);

                    dsResult = objDB.ExecuteDataSet(objCMD);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsResult;
        }


        public int SaveCategories(int catID,
                                      string CatName,
                                      bool IsActive,
                                      int UserID,
                                      string terminal)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("SaveCategories"))
                {

                    objDB.AddInParameter(objCMD, "@CatID",
                                         DbType.Int32, catID);
                    objDB.AddInParameter(objCMD, "@CategoryName",
                                        DbType.String, CatName);
                    objDB.AddInParameter(objCMD, "@IsActive",
                                        DbType.Boolean, IsActive);
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

        public int DeleteCategories(int catID)
        {
            int returnValue = -1;

            try
            {
                //Use the database connection string here...
                Database objDB = new SqlDatabase(ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("DeleteCategories"))
                {

                    objDB.AddInParameter(objCMD, "@CatID",
                                         DbType.Int32, catID);

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
