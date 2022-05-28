using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EInventory.DAL;

namespace EInventory.Models
{
    public class RoleModels : BaseModels
    {

        RoleDAL objRoleDAL;

        public RoleModels()
        {
            objRoleDAL = new RoleDAL();
            objRoleDAL.ConnectionString = ConnectionString;
        }
        public int RoleID { get; set; }

        [Required]
        [MaxLength(60)]
        public string RoleName { get; set; }

        public bool IsSystemRole { get; set; }

        public DataTable GetRoleList(int? RoleID)
        {
            DataTable dtRoles = objRoleDAL.GetRoleList(RoleID);

            return dtRoles;
        }

        public int SaveRole(RoleModels objRole,
                            int UserdID,
                            string Terminal)
        {
            return objRoleDAL.SaveRole(objRole.RoleID, objRole.RoleName, UserdID, Terminal);
        }

        public int DeleteRole(int RoleID)
        {
            return objRoleDAL.DeleteRole(RoleID);
        }
    }

    public class RoleWiseMenusModel : BaseModels
    {

        RoleWiseMenuDAL objRWMMDAL;

        public RoleWiseMenusModel()
        {
            objRWMMDAL = new RoleWiseMenuDAL();
            objRWMMDAL.ConnectionString = ConnectionString;
        }

        public int RoleID { get; set; }

        public List<Menu> listMenus { get; set; }


        public List<Menu> GetRoleWiseMenus(int RoleID)
        {
            return objRWMMDAL.GetRoleWiseMenus(RoleID).ToList<Menu>();
        }

        public int SaveRoleWiseMenus(RoleWiseMenusModel objRWMM)
        {
            DataTable dtRoleWiseMenus = objRWMM.listMenus.ToDataTable<Menu>();

            return objRWMMDAL.SaveRoleWiseMenus(objRWMM.RoleID,
                                                dtRoleWiseMenus);
        }

        public List<RoleModels> GetRoleList()
        {
            RoleModels objRoleDAL = new RoleModels();
            DataTable dtRoleList = objRoleDAL.GetRoleList(null);
            return dtRoleList.ToList<RoleModels>();
        }
    }

    public class Menu
    {
        public int MenuID { get; set; }

        public string MenuName { get; set; }

        public string Name { get; set; }

        public bool IsView { get; set; }

        public bool IsAdd { get; set; }

        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }

        public string MenuPath { get; set; }
    }
}