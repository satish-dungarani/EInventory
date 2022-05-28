using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EInventory.Models;
using Newtonsoft.Json;
using System.Data;

namespace EInventory.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Role/
        RoleModels objRole = new RoleModels();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadRolesAjaxHandler()
        {

            DataTable dtRoles = objRole.GetRoleList(null);
            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtRoles.AsEnumerable()
                         select new
                         {
                             RoleID = dr["RoleID"],
                             RoleName = dr["RoleName"],
                             IsSystemRole = dr["IsSystemRole"],
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtRoles.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            RoleModels objRoleM = new RoleModels();
            return PartialView("_Add", objRoleM);
        }

        [HttpPost]
        public ActionResult Add(RoleModels objRoleM)
        {
            if (ModelState.IsValid)
                objRole.SaveRole(objRoleM, intUserID, strTerminal);
            else
                return PartialView("_Add", objRoleM);


            string url = Url.Action("Index", "Role");
            return Json(new { success = true, url = url });
        }

        public ActionResult Edit(int ID)
        {
            RoleModels objRoleM = objRole.GetRoleList(ID).ToList<RoleModels>().FirstOrDefault();

            return PartialView("_Edit", objRoleM);
        }

        [HttpPost]
        public ActionResult Edit(RoleModels objRoleM)
        {
            if (ModelState.IsValid)
                objRole.SaveRole(objRoleM, intUserID, strTerminal);
            else
                return PartialView("_Edit", objRoleM);

            string url = Url.Action("Index", "Role");
            return Json(new { success = true, url = url });
        }

        public ActionResult Delete(int ID)
        {
            RoleModels objRoleM = objRole.GetRoleList(ID).ToList<RoleModels>().FirstOrDefault();

            return PartialView("_Delete", objRoleM);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            objRole.DeleteRole(ID.Value);

            string url = Url.Action("Index", "Role");
            return Json(new { success = true, url = url });
        }

        public ActionResult RoleWiseMenu(int? RoleID)
        {
            RoleWiseMenusModel objRWMM = new RoleWiseMenusModel();
            List<RoleModels> listRoles = objRWMM.GetRoleList();
            ViewBag.listRoleMaster = listRoles;

            if (RoleID.HasValue)
            {
                objRWMM.RoleID = RoleID.Value;
                objRWMM.listMenus = objRWMM.GetRoleWiseMenus(objRWMM.RoleID);
            }
            else
            {
                if (listRoles.Count > 0)
                {
                    objRWMM.RoleID = Convert.ToInt32(listRoles[0].RoleID);
                    objRWMM.listMenus = objRWMM.GetRoleWiseMenus(objRWMM.RoleID);
                }
            }

            return View(objRWMM);
        }

        [HttpPost]
        public ActionResult RoleWiseMenu(RoleWiseMenusModel objRWMM, int loadRoleID)
        {
            List<RoleModels> listRoles = objRWMM.GetRoleList();
            ViewBag.listRoleMaster = listRoles;

            if (objRWMM.RoleID == loadRoleID)
            {
                objRWMM.SaveRoleWiseMenus(objRWMM);
            }
            else
            {
                objRWMM.listMenus = objRWMM.GetRoleWiseMenus(objRWMM.RoleID);
            }

            return RedirectToAction("RoleWiseMenu", new { RoleID = objRWMM.RoleID });
        }

    }
}
