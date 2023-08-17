using System.Data;
using TheEstate.Data.Database;
using TheEstate.Models.ResidentModels;
using TheEstate.Models.HouseholdModels;
using TheEstate.Data;
using TheEstate.Models.ZoneModels;

namespace TheEstate.Repo
{
    public class HouseholdRepo : AppDbContext
    {

        public static async Task<List<HouseholdModelView>> GetHouseholds(string search, string estateId, string propertyId, string zoneId, string streetId, int pageSize, int pageNumber)
        {
            List<HouseholdModelView> data = new List<HouseholdModelView>();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                propertyId = string.IsNullOrEmpty(propertyId) ? "%%" : $"{propertyId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                streetId = string.IsNullOrEmpty(streetId) ? "%%" : $"{streetId?.Trim()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.HouseholdsQry, new object[] { estateId, propertyId, search, search, search, zoneId, streetId, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new HouseholdModelView
                        {
                            HouseholdId = i["household_id"].ToString(),
                            HouseholdLabel = i["household_label"].ToString(),
                            HouseholdStatus = i["household_status"].ToString(),
                            UsageCategory = i["usage_category"].ToString(),
                            HouseholdClassification = i["household_classification"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZipcodeId = i["zipcode_id"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetCode = i["street_code"].ToString(),
                            StreetType = i["street_type"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneCode = i["zone_code"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ZoneType = i["zone_type"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
                            PropertyOwnerFirstname = i["owners_firstname"].ToString(),
                            PropertyOwnerLastname = i["owners_lastname"].ToString(),
                            PropertyOwnerMobileNo = i["owners_mobileno"].ToString(),
                            PropertyOwnerEmail = i["owners_email"].ToString(),
                            PropertyOwnerGender = i["owners_gender"].ToString(),
                            PropertyOwnerOtherAddress = i["owners_other_address"].ToString(),
                            //PrimaryResidentCode = i["resident_code"].ToString(),
                            //PrimaryResidentCategory = i["resident_category"].ToString(),
                            //PrimaryResidentFirstname = i["resident_firstname"].ToString(),
                            //PrimaryResidentLastname = i["resident_lastname"].ToString(),
                            //PrimaryResidentMobileNo = i["resident_mobileno"].ToString(),
                            //PrimaryResidentEmail = i["resident_emailaddress"].ToString(),
                            //PrimaryResidentStatus = i["resident_status"].ToString(),
                            EntryBy = i["entry_by"].ToString(),
                            EntryDate = string.IsNullOrEmpty(i["entry_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["entry_date"]),
                            LastModifiedBy = i["last_modified_by"].ToString(),
                            LastModifedDate = string.IsNullOrEmpty(i["last_modified_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["last_modified_date"]),

                        });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<int> GetHouseholdCount(string search, string estateId, string propertyId, string zoneId, string streetId)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                propertyId = string.IsNullOrEmpty(propertyId) ? "%%" : $"{propertyId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                streetId = string.IsNullOrEmpty(streetId) ? "%%" : $"{streetId?.Trim()}";

                string result = await db.SQLSelectAsync(SQL.HouseholdsQryCount, new object[] { estateId, propertyId, search, search, search, zoneId, streetId });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<HouseholdModelView> GetHouseholdById(string householdId)
        {
            HouseholdModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.HouseholdByIdQry, new object[] { householdId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new HouseholdModelView
                        {
                            HouseholdId = i["household_id"].ToString(),
                            HouseholdLabel = i["household_label"].ToString(),
                            HouseholdStatus = i["household_status"].ToString(),
                            UsageCategory = i["usage_category"].ToString(),
                            HouseholdClassification = i["household_classification"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZipcodeId = i["zipcode_id"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetCode = i["street_code"].ToString(),
                            StreetType = i["street_type"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneCode = i["zone_code"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ZoneType = i["zone_type"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
                            PropertyOwnerFirstname = i["owners_firstname"].ToString(),
                            PropertyOwnerLastname = i["owners_lastname"].ToString(),
                            PropertyOwnerMobileNo = i["owners_mobileno"].ToString(),
                            PropertyOwnerEmail = i["owners_email"].ToString(),
                            PropertyOwnerGender = i["owners_gender"].ToString(),
                            PropertyOwnerOtherAddress = i["owners_other_address"].ToString(),
                            //PrimaryResidentCode = i["resident_code"].ToString(),
                            //PrimaryResidentCategory = i["resident_category"].ToString(),
                            //PrimaryResidentFirstname = i["resident_firstname"].ToString(),
                            //PrimaryResidentLastname = i["resident_lastname"].ToString(),
                            //PrimaryResidentMobileNo = i["resident_mobileno"].ToString(),
                            //PrimaryResidentEmail = i["resident_emailaddress"].ToString(),
                            //PrimaryResidentStatus = i["resident_status"].ToString(),
                            EntryBy = i["entry_by"].ToString(),
                            EntryDate = string.IsNullOrEmpty(i["entry_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["entry_date"]),
                            LastModifiedBy = i["last_modified_by"].ToString(),
                            LastModifedDate = string.IsNullOrEmpty(i["last_modified_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["last_modified_date"]),

                        };
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> CreateHousehold(HouseholdModelCreate model, string? householdId = null)
        {
            string data;
            try
            {
                householdId ??= Guid.NewGuid().ToString();
                model.HouseholdStatus = string.IsNullOrEmpty(model.HouseholdStatus) ? Constants.ActiviyStatus.ACTIVE.ToString() : model.HouseholdStatus;

                int action = await db.SQLExecuteAsync(SQL.HouseholdCreateQry, new object[] { model.EstateId!, model.PropertyId!, householdId, model.HouseholdLabel!, model.HouseholdStatus!, model.HouseholdRemark ?? "", model.UsageCategory!, model.HouseholdClassification ?? "", model.ProfileId!, DateTime.Now });
                if (action > 0) data = Constants.StatusResponses.SUCCESS;
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> UpdateHousehold(HouseholdModelCreate model, string householdId)
        {
            string data;
            try
            {
                HouseholdModelView household = await GetHouseholdById(householdId);
                if (household != null)
                {
                    model.HouseholdStatus = string.IsNullOrEmpty(model.HouseholdStatus) ? household.HouseholdStatus : model.HouseholdStatus;
                    model.HouseholdRemark = string.IsNullOrEmpty(model.HouseholdRemark) ? household.HouseholdRemark : model.HouseholdRemark;
                    model.UsageCategory = string.IsNullOrEmpty(model.UsageCategory) ? household.UsageCategory : model.UsageCategory;
                    model.HouseholdClassification = string.IsNullOrEmpty(model.HouseholdClassification) ? household.HouseholdClassification : model.HouseholdClassification;

                    int action = await db.SQLExecuteAsync(SQL.HouseholdUpdateQry, new object[] { model.EstateId!, model.PropertyId!, model.HouseholdLabel!, model.HouseholdStatus!, model.HouseholdRemark ?? "", model.UsageCategory!, model.HouseholdClassification ?? "", model.ProfileId!, DateTime.Now, householdId });
                    if (action > 0) data = Constants.StatusResponses.SUCCESS;
                    else data = Constants.StatusResponses.FAILED;
                }
                else return Constants.StatusResponses.NOTFOUND;

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> DeactivateHousehold(string householdId)
        {
            string data;
            try
            {
                HouseholdModelView household = await GetHouseholdById(householdId);
                if (household != null)
                {
                    int action = await db.SQLExecuteAsync(SQL.HouseholdDeactivateQry, new object[] { householdId });
                    if (action > 0) data = Constants.StatusResponses.SUCCESS;
                    else data = Constants.StatusResponses.FAILED;
                }
                else data = Constants.StatusResponses.NOTFOUND;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }



        public static List<string> GetHouseholdIdsByEstateId(string estateId)
        {
            List<string> data = new List<string>();
            try
            {
                DataTable dt =  db.SQLFetch(SQL.GetHouseholdIdsByEstateId, new object[] { estateId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(i["household_id"].ToString()!);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static List<string> GetHouseholdIdsByZoneId(string estateId, string zoneId)
        {
            List<string> data = new List<string>();
            try
            {
                DataTable dt = db.SQLFetch(SQL.GetHouseholdIdsByZoneId, new object[] { estateId, zoneId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(i["household_id"].ToString()!);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}

