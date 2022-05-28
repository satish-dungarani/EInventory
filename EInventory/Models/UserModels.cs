using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using EInventory.DAL;
using System.Data;
using System.Linq;
using System.Web;


namespace EInventory.Models
{
    public class UserModels : BaseModels
    {

        UserDAL objUserDAL;

        public UserModels()
        {
            objUserDAL = new UserDAL();
            objUserDAL.ConnectionString = ConnectionString;
        }
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(500)]
        public string PhotoPath { get; set; }

        [StringLength(4)]
        public string Gender { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(30)]
        public string MobileNo { get; set; }


        [StringLength(100)]
        public string EmaiAddress { get; set; }

        public bool IsActive { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Company")]
        public int refCompID_Company { get; set; }

        public int RoleID { get; set; }

        public List<RoleModels> listRoles { get; set; }

        public List<CompanyModels> listCompnies { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public DataTable GetAllUsers()
        {
            return objUserDAL.GetUsers(null).Tables[0];
        }

        public UserModels GetUserDetails(int UserID)
        {
            DataSet dsUserDetails = objUserDAL.GetUsers(UserID);

            UserModels objUserModel = null;

            if (dsUserDetails.Tables[0].Rows.Count == 0)
                objUserModel = new UserModels();
            else
                objUserModel = dsUserDetails.Tables[0].ToList<UserModels>().FirstOrDefault();

            objUserModel.listCompnies = dsUserDetails.Tables[1].ToList<CompanyModels>();
            
            if (objUserModel.listCompnies == null)
                objUserModel.listCompnies = new List<CompanyModels>();

            CompanyModels objNewParam = new CompanyModels();
            objNewParam.CompID = -1;
            objNewParam.Name = SELECTITEM;

            objUserModel.listCompnies.Insert(0, objNewParam);

            objUserModel.listRoles = dsUserDetails.Tables[2].ToList<RoleModels>();
            return objUserModel;
        }

        public int SaveUser(UserModels objUser)
        {
            return objUserDAL.SaveUser(objUser.UserID,
                                       objUser.UserName,
                                       objUser.Password,
                                       objUser.FirstName,
                                       objUser.LastName,
                                       objUser.PhotoPath,
                                       objUser.Gender,
                                       objUser.DOB,
                                       objUser.MobileNo,
                                       objUser.EmaiAddress,
                                       objUser.IsActive,
                                       objUser.refCompID_Company,
                                       objUser.RoleID);
        }

        public int DeleteUser(int UserID)
        {
            return objUserDAL.DeleteUser(UserID);
        }

    }
}