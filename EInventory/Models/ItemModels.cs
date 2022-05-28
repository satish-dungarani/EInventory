using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EInventory.DAL;
using System.ComponentModel.DataAnnotations;


namespace EInventory.Models
{
    public class ItemModels : BaseModels
    {

        ItemsDAL objItemDAL = null;
        public ItemModels()
        {
            objItemDAL = new ItemsDAL();
            objItemDAL.ConnectionString = ConnectionString;


        }
        public int ItemID { get; set; }

        [Required]
        public int refCompID_Company { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Item Category")]
        [Display(Name = "Item Category")]
        public int refCatID_CatMas { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemDesc { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select UOM")]
        [Display(Name = "UOM")]
        public int refUOMID_ParamValue { get; set; }

        [Display(Name = "Re-Order Level")]
        public decimal ReOrdLevel { get; set; }

        [Display(Name = "Max. Delivery Days")]
        public int MaxDeliveryDays { get; set; }

        public string Size { get; set; }

        public string Material { get; set; }

        [Display(Name = "Is Finish Item")]
        public bool IsFinishedItem { get; set; }

        public List<ItemCategoryModel> listCategory { get; set; }

        public List<ParamValue> listUOM { get; set; }

        public List<FinalItemConsumption> listFinalItemConsumption { get; set; }
        public List<ItemVendor> listItemVendor { get; set; }

        public List<ItemModels> listRowMaterials { get; set; }
        public List<PartyMaster> listVendors { get; set; }

        public DataTable GetAllItems(int CompID)
        {
            return objItemDAL.GetItems(CompID, null).Tables[0];
        }

        public ItemModels GetItemDetails(int CompID, int ItemID)
        {
            DataSet dsItemDetails = objItemDAL.GetItems(CompID, ItemID);
            ItemModels objItem = null;

            if (ItemID > 0)
                objItem = dsItemDetails.Tables[0].ToList<ItemModels>().FirstOrDefault();
            else
                objItem = new ItemModels();

            if (dsItemDetails.Tables[1].Rows.Count > 0)
                objItem.listCategory = dsItemDetails.Tables[1].ToList<ItemCategoryModel>();
            else
                objItem.listCategory = new List<ItemCategoryModel>();

            if (dsItemDetails.Tables[2].Rows.Count > 0)
                objItem.listUOM = dsItemDetails.Tables[2].ToList<ParamValue>();
            else
                objItem.listUOM = new List<ParamValue>();

            if (dsItemDetails.Tables[3].Rows.Count > 0)
                objItem.listFinalItemConsumption = dsItemDetails.Tables[3].ToList<FinalItemConsumption>();
            else
                objItem.listFinalItemConsumption = new List<FinalItemConsumption>();

            if (dsItemDetails.Tables[4].Rows.Count > 0)
                objItem.listItemVendor = dsItemDetails.Tables[4].ToList<ItemVendor>();
            else
                objItem.listItemVendor = new List<ItemVendor>();


            ItemCategoryModel objNewParam = new ItemCategoryModel();
            objNewParam.CatID = -1;
            objNewParam.CategoryName = SELECTITEM;
            objItem.listCategory.Insert(0, objNewParam);

            ParamValue objNewParam1 = new ParamValue();
            objNewParam1.ID = -1;
            objNewParam1.ParamVal = SELECTITEM;
            objItem.listUOM.Insert(0, objNewParam1);

            return objItem;
        }

        public int SaveItem(ItemModels objItem, int CompID, int UserID, string Terminal)
        {
            DataTable dtFinalItemConsumption = objItem.listFinalItemConsumption.ToDataTable<FinalItemConsumption>();
            DataTable dtItemVendor = objItem.listItemVendor.ToDataTable<ItemVendor>();

            return objItemDAL.SaveItem(objItem.ItemID, CompID, objItem.refCatID_CatMas,
                                      objItem.ItemName, objItem.ItemDesc, objItem.refUOMID_ParamValue,
                                      objItem.ReOrdLevel, objItem.MaxDeliveryDays, objItem.Size, objItem.Material,
                                      UserID, Terminal, objItem.IsFinishedItem, dtFinalItemConsumption, dtItemVendor);

        }

        public int DeleteItem(int ItemID)
        {
            return objItemDAL.DeleteItem(ItemID);
        }

        public List<PartyMaster> GetAllVendors()
        {
            List<PartyMaster> listPartyMaster = objItemDAL.GetAllVendors(8).ToList<PartyMaster>();

            if (listPartyMaster == null)
                listPartyMaster = new List<PartyMaster>();

            PartyMaster objNewParam = new PartyMaster();
            objNewParam.PartyID = -1;
            objNewParam.PartyName = SELECTITEM;

            listPartyMaster.Insert(0, objNewParam);

            return listPartyMaster;
        }

        public List<ItemModels> GetRowMaterials()
        {
            List<ItemModels> listItemModel = objItemDAL.GetRowMaterials().ToList<ItemModels>();

            if (listItemModel == null)
                listItemModel = new List<ItemModels>();

            ItemModels objNewParam = new ItemModels();
            objNewParam.ItemID = -1;
            objNewParam.ItemName = SELECTITEM;

            listItemModel.Insert(0, objNewParam);

            return listItemModel;
        }
    }

    public class FinalItemConsumption
    {
        public int ID { get; set; }

        
        public int refItemID_ItemMas { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Item")]
        public int refrowItemID_ItemMas { get; set; }
        public string RowItemName { get; set; }
        public double Qty { get; set; }
        public int SrNo { get; set; }
    }

    public class ItemVendor
    {
        public int ID { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Vendor")]
        public int refPartyID_PartyMas { get; set; }
        public string PartyName { get; set; }
        public int refItemID_ItemMas { get; set; }
        public bool WithMaterial { get; set; }
        public int SrNo { get; set; }
    }

    public class ItemCategoryModel : BaseModels
    {
        ItemCategoryDAL objItemCatDAL;

        public ItemCategoryModel()
        {
            objItemCatDAL = new ItemCategoryDAL();
            this.listCategory = new List<ItemCategoryModel>();
            objItemCatDAL.ConnectionString = ConnectionString;
        }

        public int CatID { get; set; }

        [Required]
        [MaxLength(280)]
        public string CategoryName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public List<ItemCategoryModel> listCategory { get; set; }

        public DataTable GetCategories()
        {
            DataTable dtCategories = objItemCatDAL.GetCategories(null).Tables[0];
            return dtCategories;
        }

        public ItemCategoryModel GetCategoryDetail(int catID)
        {
            DataTable dtCategories = objItemCatDAL.GetCategories(catID).Tables[0];
            return dtCategories.ToList<ItemCategoryModel>()[0];
        }

        public int DeletCategory(int catID)
        {
            return objItemCatDAL.DeleteCategories(catID);
        }

        public int SaveCategory(ItemCategoryModel objItemCat, int UserID, string terminal)
        {
            return objItemCatDAL.SaveCategories(objItemCat.CatID, objItemCat.CategoryName, objItemCat.IsActive, UserID, terminal);
        }
    }


}