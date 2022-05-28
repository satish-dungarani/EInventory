using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EInventory.DAL;

namespace EInventory.Models
{
    public class PartyMaster : BaseModels
    {

        PartyDAL objPartyDAL;

        public PartyMaster()
        {
            objPartyDAL = new PartyDAL();
            this.PartyAddressList = new List<PartyAddress>();
            objPartyDAL.ConnectionString = ConnectionString;
        }
        public int PartyID { get; set; }

        public int refCompID_Company { get; set; }

        [Required]
        [StringLength(70)]
        [Display(Name = "Party Name")]
        public string PartyName { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(120)]
        //[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$]")]
        public string EmailID { get; set; }

        [Display(Name = "Web URL")]
        [StringLength(120)]
        [RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+[.com]+(/[/?%&=]*)?")]
        public string WebURL { get; set; }

        [Required]
        [Display(Name = "Party Category")]
        public string[] CategoryLinkage { get; set; }

        public List<PartyAddress> PartyAddressList
        { get; set; }

        public List<ParamValue> listPartyCategory { get; set; }
        public List<ParamValue> listPartyLocation { get; set; }

        public DataTable GetPartyList()
        {
            return objPartyDAL.GetParties(null).Tables[0];
        }
        public PartyMaster GetPartyDetails(int id)
        {
            DataSet dsPartyList = objPartyDAL.GetParties(id);
            PartyMaster objParty = dsPartyList.Tables[0].ToList<PartyMaster>().FirstOrDefault();
            objParty.CategoryLinkage = Convert.ToString(dsPartyList.Tables[1].Rows[0]["CategoryLinkage"]).Split(',');
            objParty.PartyAddressList = dsPartyList.Tables[2].ToList<PartyAddress>();

            objParty.listPartyCategory = dsPartyList.Tables[3].ToList<ParamValue>();

            objParty.listPartyLocation = dsPartyList.Tables[4].ToList<ParamValue>();

            if (objParty.listPartyLocation == null)
                objParty.listPartyLocation = new List<ParamValue>();

            ParamValue objNewParam = new ParamValue();
            objNewParam.ID = -1;
            objNewParam.ParamVal = SELECTITEM;

            objParty.listPartyLocation.Insert(0, objNewParam);

            return objParty;
        }

        public int SaveParty(PartyMaster objParty, int userID, string terminal)
        {
            DataTable dtPartyAddress = objParty.PartyAddressList.ToDataTable<PartyAddress>();
            return objPartyDAL.SaveParty(objParty.PartyID
                                        , objParty.refCompID_Company
                                        , objParty.PartyName
                                        , objParty.EmailID
                                        , objParty.WebURL
                                        , userID
                                        , terminal
                                        , string.Join(",", objParty.CategoryLinkage) //objParty.CategoryLinkage.Join(',')
                                        , dtPartyAddress);
        }

        public PartyMaster CreateNewParty()
        {
            DataSet dsParty = objPartyDAL.GetParties(0);
            PartyMaster objPartyMaster = new PartyMaster();

            objPartyMaster.listPartyCategory = dsParty.Tables[3].ToList<ParamValue>();
            objPartyMaster.listPartyLocation = dsParty.Tables[4].ToList<ParamValue>();

            if (objPartyMaster.listPartyLocation == null)
                objPartyMaster.listPartyLocation = new List<ParamValue>();

            ParamValue objNewParam = new ParamValue();
            objNewParam.ID = -1;
            objNewParam.ParamVal = SELECTITEM;

            objPartyMaster.listPartyLocation.Insert(0, objNewParam);


            return objPartyMaster;
        }

        public int DeleteParty(int PartyID)
        {
            return objPartyDAL.DeleteParty(PartyID);
        }

    }

    public class PartyAddress
    {
        public int AddrID { get; set; }

        public int refPartyID_PartyMas { get; set; }

        [Required]
        [Display(Name = "Location")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Location")]
        public int refLocID_ParamVal { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Address")]
        public string Addr { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "State")]
        public string State { get; set; }

        [RegularExpression(@"^[0-9]*$")]
        [Display(Name = "PinCode")]
        [StringLength(10)]
        public string Pincode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[0-9]*$")]
        public string WorkContact1 { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[0-9]*$")]
        public string WorkContact2 { get; set; }

        [StringLength(50)]
        public string ContactPerson1 { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[7-9]\d{9}$")]
        public string Mobile1 { get; set; }

        [StringLength(50)]
        public string ContactPerson2 { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[7-9]\d{9}$")]
        public string Mobile2 { get; set; }

        public int SrNo { get; set; }

        [Display(Name = "Location Name")]
        public string ParamVal { get; set; }

    }

}