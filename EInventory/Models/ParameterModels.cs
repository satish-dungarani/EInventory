using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EInventory.DAL;


namespace EInventory.Models
{

    public class ParameterSetup : BaseModels
    {
        ParameterDAL objParmDAL;

        public ParameterSetup()
        {
            objParmDAL = new ParameterDAL();
            objParmDAL.ConnectionString = ConnectionString;
        }

        [Display(Name = "Pameter Setup")]
        public int SetupID { get; set; }

        public bool IsSystemGene { get; set; }


        public List<ParameterValues> listParameterValues;

        [Display(Name = "Parameter Type")]
        public List<ParameterSetupMaster> listParameterSetup;

        public List<ParameterSetupMaster> GetParameterSetupList()
        {
            DataTable dtParameterSetup = objParmDAL.GetParameterSetupList();
            return dtParameterSetup.ToList<ParameterSetupMaster>();
        }

        public List<ParameterValues> GetParameterValueList(int setupID)
        {
            DataTable dtParameterValues = objParmDAL.GetParameterValueList(setupID);
            return dtParameterValues.ToList<ParameterValues>();
        }

        public int SaveParameterValues(ParameterValues objParamValue, int userID, string terminal)
        {
            return objParmDAL.SaveParameterValues(objParamValue.ID,
                                                  objParamValue.refCompID_Company,
                                                  objParamValue.refSetupID_ParameterSetup,
                                                  objParamValue.ParamVal,
                                                  userID,
                                                  terminal);
        }

        public int DeleteParameterValues(int ID)
        {
            return objParmDAL.DeleteParameterValues(ID);
        }

    }

    public class ParameterSetupMaster
    {
        public int SetupID { get; set; }

        public string ParamType { get; set; }

        public bool IsSystemGene { get; set; }

    }

    public class ParameterValues
    {
        public int ID { get; set; }

        public int refCompID_Company { get; set; }

        public int refSetupID_ParameterSetup { get; set; }

        [Required]
        [Display(Name = "Parameter Value")]
        [StringLength(200)]
        public string ParamVal { get; set; }

        public bool IsSystemGene { get; set; }

        public int SrNo { get; set; }

    }

}