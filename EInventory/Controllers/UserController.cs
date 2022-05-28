using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EInventory.Models;
using Newtonsoft.Json;
using System.Data;
using System.IO;

namespace EInventory.Controllers
{
    public class UserController : BaseController
    {
        //
        UserModels objUser = new UserModels();

        public ActionResult Index()
        {
            if (Session["objUserModel"] != null)
            {
                Session.Remove("objUserModel");
            }

            return View();
        }

        public ActionResult LoadUsersAjaxHandler()
        {
            DataTable dtUsers = objUser.GetAllUsers();
            //var aaData = dtCompnies.AsEnumerable().Select(d => new string[] { d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString() }).ToArray();

            var aadata = from dr in dtUsers.AsEnumerable()
                         select new
                         {
                             UserID = dr["UserID"],
                             UserName = dr["UserName"],
                             FirstName = dr["FirstName"],
                             LastName = dr["LastName"],
                             Gender = dr["Gender"],
                             MobileNo = dr["MobileNo"],
                             RoleName = dr["RoleName"],
                             IsActive = dr["IsActive"]
                         };

            return Json(new

            {
                draw = "",
                data = aadata,
                recordsFiltered = "",
                recordsTotal = dtUsers.Rows.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(int? ID)
        {
            UserModels objUserModel = null;


            if (ID.HasValue)
            {
                objUserModel = objUser.GetUserDetails(ID.Value);
                objUserModel.ConfirmedPassword = objUserModel.Password;
            }
            else
            {
                objUserModel = objUser.GetUserDetails(0);
            }

            ViewBag.listCompnies = objUserModel.listCompnies;
            ViewBag.listRoles = objUserModel.listRoles;

            Session["objUserModel"] = objUserModel;

            return View(objUserModel);
        }

        [HttpPost]
        public ActionResult Save(UserModels objUserModel)
        {
            UserModels objUserSession = null;
            objUserSession = Session["objUserModel"] as UserModels;

            ViewBag.listCompnies = objUserSession.listCompnies;
            ViewBag.listRoles = objUserSession.listRoles;

            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (objUserModel.ImageUpload != null && objUserModel.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(objUserModel.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (objUserModel.ImageUpload != null && objUserModel.ImageUpload.ContentLength > 1048576)
            {
                ModelState.AddModelError("ImageUpload", "Please choose image file < 1 MB.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (objUserModel.ImageUpload != null && objUserModel.ImageUpload.ContentLength > 0)
                    {
                        var uploadDir = "~/Shared/UserImages";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), objUserModel.ImageUpload.FileName);
                        objUserModel.ImageUpload.SaveAs(imagePath);
                        objUserModel.PhotoPath = objUserModel.ImageUpload.FileName;
                    }

                    objUser.SaveUser(objUserModel);
                }
                catch
                {
                    return View("Save", objUserModel);
                }
            }
            else
            {
                return View("Save", objUserModel);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int ID)
        {
            UserModels objUserModel = objUser.GetUserDetails(ID);

            return PartialView("_Delete", objUserModel);
        }

        [HttpPost]
        public ActionResult Delete(int? ID)
        {
            objUser.DeleteUser(ID.Value);

            string url = Url.Action("Index", "User");
            return Json(new { success = true, url = url });
            //return RedirectToAction("Index");
        }

        public ActionResult SaveProfile(int ID)
        {
            UserModels objUserModel = null;

            objUserModel = objUser.GetUserDetails(ID);
            objUserModel.ConfirmedPassword = objUserModel.Password;

            ViewBag.listCompnies = objUserModel.listCompnies;
            ViewBag.listRoles = objUserModel.listRoles;

            Session["objUserModel"] = objUserModel;

            return View(objUserModel);
        }

        [HttpPost]
        public ActionResult SaveProfile(UserModels objUserModel)
        {
            UserModels objUserSession = null;
            objUserSession = Session["objUserModel"] as UserModels;

            ViewBag.listCompnies = objUserSession.listCompnies;
            ViewBag.listRoles = objUserSession.listRoles;

            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (objUserModel.ImageUpload != null && objUserModel.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(objUserModel.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (objUserModel.ImageUpload != null && objUserModel.ImageUpload.ContentLength > 1048576)
            {
                ModelState.AddModelError("ImageUpload", "Please choose image file < 1 MB.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (objUserModel.ImageUpload != null && objUserModel.ImageUpload.ContentLength > 0)
                    {
                        var uploadDir = "~/Shared/UserImages";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), objUserModel.ImageUpload.FileName);
                        objUserModel.ImageUpload.SaveAs(imagePath);
                        objUserModel.PhotoPath = objUserModel.ImageUpload.FileName;
                    }

                    objUser.SaveUser(objUserModel);
                }
                catch
                {
                    return View("SaveProfile", objUserModel);
                }
            }
            else
            {
                return View("SaveProfile", objUserModel);
            }

            return RedirectToAction("SaveProfile");
        }

    }
}
