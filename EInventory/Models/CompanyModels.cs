using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EInventory.DAL;
using System.ComponentModel.DataAnnotations;

namespace EInventory.Models
{
    public class CompanyModels : BaseModels
    {
        CompanyDAL objCompany = null;

        public CompanyModels()
        {
            objCompany = new CompanyDAL();
            this.listCompAuthorityPersons = new List<CompanyAuthorityPerson>();
            this.listCompSatutory = new List<CompanySatutory>();
            objCompany.ConnectionString = ConnectionString;
        }

        public int CompID { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 3)]
        [Display(Name = "Company Name")]

        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        [Display(Name = "Company Address")]
        public string Addr { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        [Display(Name = "Pincode")]
        public string Pincode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Contact 1")]
        [StringLength(20, MinimumLength = 3)]
        public string Contact1 { get; set; }

        [Display(Name = "Contact 2")]
        [StringLength(20, MinimumLength = 3)]
        public string Contact2 { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(120)]
        public string EmailID { get; set; }

        [Display(Name = "Web URL")]
        [StringLength(120)]
        public string WebURL { get; set; }

        public List<CompanyAuthorityPerson> listCompAuthorityPersons { get; set; }

        public List<CompanySatutory> listCompSatutory { get; set; }

        public List<ParamValue> listDesgnMaster { get; set; }
        public List<ParamValue> listSatutoryMaster { get; set; }

        public DataTable GetCompanyList()
        {
            return objCompany.GetCompanies(null).Tables[0];
        }

        public CompanyModels GetCompanyDetails(int id)
        {
            DataSet dsCompany = objCompany.GetCompanies(id);
            CompanyModels objCompanyModel = dsCompany.Tables[0].ToList<CompanyModels>().FirstOrDefault();
            objCompanyModel.listCompAuthorityPersons = dsCompany.Tables[1].ToList<CompanyAuthorityPerson>();
            objCompanyModel.listCompSatutory = dsCompany.Tables[2].ToList<CompanySatutory>();
            objCompanyModel.listSatutoryMaster = dsCompany.Tables[3].ToList<ParamValue>();
            objCompanyModel.listDesgnMaster = dsCompany.Tables[4].ToList<ParamValue>();

            if (objCompanyModel.listSatutoryMaster == null)
                objCompanyModel.listSatutoryMaster = new List<ParamValue>();

            if (objCompanyModel.listDesgnMaster == null)
                objCompanyModel.listDesgnMaster = new List<ParamValue>();

            ParamValue objNewParam = new ParamValue();
            objNewParam.ID = -1;
            objNewParam.ParamVal = SELECTITEM;

            objCompanyModel.listSatutoryMaster.Insert(0, objNewParam);
            objCompanyModel.listDesgnMaster.Insert(0, objNewParam);


            return objCompanyModel;
        }

        public CompanyModels GetNewCompany()
        {
            DataSet dsCompany = objCompany.GetCompanies(0);
            CompanyModels objCompanyModel = new CompanyModels();
            objCompanyModel.listSatutoryMaster = dsCompany.Tables[3].ToList<ParamValue>();
            objCompanyModel.listDesgnMaster = dsCompany.Tables[4].ToList<ParamValue>();

            if (objCompanyModel.listSatutoryMaster == null)
                objCompanyModel.listSatutoryMaster = new List<ParamValue>();

            if (objCompanyModel.listDesgnMaster == null)
                objCompanyModel.listDesgnMaster = new List<ParamValue>();

            ParamValue objNewParam = new ParamValue();
            objNewParam.ID = -1;
            objNewParam.ParamVal = SELECTITEM;

            objCompanyModel.listSatutoryMaster.Insert(0, objNewParam);
            objCompanyModel.listDesgnMaster.Insert(0, objNewParam);

            return objCompanyModel;
        }

        public int SaveCompanyDetail(CompanyModels objComp, int userid, string terminal)
        {
            DataTable dtContact = objComp.listCompAuthorityPersons.ToDataTable<CompanyAuthorityPerson>();
            DataTable dtSatutory = objComp.listCompSatutory.ToDataTable<CompanySatutory>();

            return objCompany.SaveCompany(objComp.CompID,
                                          objComp.Name,
                                          objComp.Addr,
                                          objComp.City,
                                          objComp.State,
                                          objComp.Pincode,
                                          objComp.Country,
                                          objComp.Contact1,
                                          objComp.Contact2,
                                          objComp.EmailID,
                                          objComp.WebURL,
                                          userid,
                                          terminal,
                                          dtContact,
                                          dtSatutory);
        }

        public int DeleteCompany(int CompID)
        {
            return objCompany.DeleteCompany(CompID);
        }
    }


    public class CompanyAuthorityPerson
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Designation")]
        public int refDesignID_ParamVal { get; set; }
        public string ParamVal { get; set; }
        public int SrNo { get; set; }

    }

    public class CompanySatutory
    {
        public int SatutoryID { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Satutory Param.")]
        public int refStatutoryID_ParamSetup { get; set; }

        [Required]
        [StringLength(100)]
        public string StatutoryName { get; set; }
        public string ParamVal { get; set; }
        public int SrNo { get; set; }
    }
}