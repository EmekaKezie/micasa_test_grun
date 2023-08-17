using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.PropertyModels;
using TheEstate.Models.PropertyModels;

namespace TheEstate.Repo
{
    public class PropertyRepo : AppDbContext
    {
        public static async Task<List<PropertyModelView>> GetProperties(string estateId, string propertyType, string propertyCategory, string zoneId, string streetId, int pageSize, int pageNumber)
        {
            List<PropertyModelView> data = new();
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                streetId = string.IsNullOrEmpty(streetId) ? "%%" : $"{streetId?.Trim()}";
                propertyType = string.IsNullOrEmpty(propertyType) ? "%%" : $"{propertyType?.Trim()?.ToUpper()}";
                propertyCategory = string.IsNullOrEmpty(propertyCategory) ? "%%" : $"{propertyCategory?.Trim()?.ToUpper()}";

                DataTable dt = await db.SQLFetchAsync(SQL.PropertiesQry, new object[] { estateId, propertyType, propertyCategory, zoneId, streetId, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new PropertyModelView
                        {
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
                            PropertyStatus = i["status"].ToString(),
                            PropertyRemark = i["remarks"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZipcodeId = i["zipcode_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneCode = i["zone_code"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ZoneType = i["zone_type"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetCode = i["street_code"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            StreetType = i["street_type"].ToString(),
                            PropertyOwnerFirstname = i["owners_firstname"].ToString(),
                            PropertyOwnerLastname = i["owners_lastname"].ToString(),
                            PropertyOwnerMobileNo = i["owners_mobileno"].ToString(),
                            PropertyOwnerEmail = i["owners_email"].ToString(),
                            PropertyOwnerGender = i["owners_gender"].ToString(),
                            PropertyOwnerOtherAddress = i["owners_other_address"].ToString(),
                            PropertyImage = i["property_image"].ToString(),
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


        public static async Task<int> GetPropertiesCount(string estateId, string propertyType, string propertyCategory, string zoneId, string streetId)
        {
            int data;
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                streetId = string.IsNullOrEmpty(streetId) ? "%%" : $"{streetId?.Trim()}";
                propertyType = string.IsNullOrEmpty(propertyType) ? "%%" : $"{propertyType?.Trim()?.ToUpper()}";
                propertyCategory = string.IsNullOrEmpty(propertyCategory) ? "%%" : $"{propertyCategory?.Trim()?.ToUpper()}";

                string result = await db.SQLSelectAsync(SQL.PropertiesQryCount, new object[] { estateId, propertyType, propertyCategory, zoneId, streetId });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<PropertyModelView> GetPropertyById(string propertyId)
        {
            PropertyModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.PropertyByIdQry, new object[] { propertyId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new PropertyModelView
                        {
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
                            PropertyStatus = i["status"].ToString(),
                            PropertyRemark = i["remarks"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZipcodeId = i["zipcode_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneCode = i["zone_code"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ZoneType = i["zone_type"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetCode = i["street_code"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            StreetType = i["street_type"].ToString(),
                            PropertyOwnerFirstname = i["owners_firstname"].ToString(),
                            PropertyOwnerLastname = i["owners_lastname"].ToString(),
                            PropertyOwnerMobileNo = i["owners_mobileno"].ToString(),
                            PropertyOwnerEmail = i["owners_email"].ToString(),
                            PropertyOwnerGender = i["owners_gender"].ToString(),
                            PropertyOwnerOtherAddress = i["owners_other_address"].ToString(),
                            EntryBy = i["entry_by"].ToString(),
                            PropertyImage = i["property_image"].ToString(),
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


        public static async Task<string> CreateProperty(PropertyModelCreate model, string? propertyId = null)
        {
            string data;
            try
            {
                propertyId ??= Guid.NewGuid().ToString();
                model.PropertyStatus = string.IsNullOrEmpty(model.PropertyStatus) ? Constants.ActiviyStatus.ACTIVE.ToString() : model.PropertyStatus;

                int action = await db.SQLExecuteAsync(SQL.PropertyCreateQry, new object[] { model.EstateId!, propertyId, model.ZipcodeId!, model.PropertyNo!, model.PropertyType!, model.PropertyCategory!, model.PropertyStatus, model.PropertyRemark ?? "", model.PropertyOwnerFirstname ?? "", model.PropertyOwnerLastname ?? "", model.PropertyOwnerMobileNo ?? "", model.PropertyOwnerEmail ?? "", model.PropertyOwnerGender ?? "", model.PropertyOwnerOtherAddress ?? "", model.ProfileId!, DateTime.Now, model.PropertyImage ?? "" });
                if (action > 0) data = Constants.StatusResponses.SUCCESS;
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> UpdateProperty(PropertyModelCreate model, string propertyId)
        {
            string data;
            try
            {
                PropertyModelView property = await GetPropertyById(propertyId);
                if (property != null)
                {
                    model.PropertyType = string.IsNullOrEmpty(model.PropertyType) ? property.PropertyNo : model.PropertyType;
                    model.PropertyCategory = string.IsNullOrEmpty(model.PropertyCategory) ? property.PropertyCategory : model.PropertyCategory;
                    model.PropertyStatus = string.IsNullOrEmpty(model.PropertyStatus) ? property.PropertyStatus : model.PropertyStatus;
                    model.PropertyImage = string.IsNullOrEmpty(model.PropertyImage) ? property.PropertyImage : model.PropertyImage;

                    int action = await db.SQLExecuteAsync(SQL.PropertyUpdateQry, new object[] { model.EstateId!, model.ZipcodeId!, model.PropertyNo!, model.PropertyType!, model.PropertyCategory!, model.PropertyStatus!, model.PropertyRemark ?? "", model.PropertyOwnerFirstname ?? "", model.PropertyOwnerLastname ?? "", model.PropertyOwnerMobileNo ?? "", model.PropertyOwnerEmail ?? "", model.PropertyOwnerGender ?? "", model.PropertyOwnerOtherAddress ?? "", model.ProfileId!, DateTime.Now, model.PropertyImage!, propertyId });
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


        public static async Task<string> DeactivateProperty(string propertyId)
        {
            string data;
            try
            {
                PropertyModelView property = await GetPropertyById(propertyId);
                if (property != null)
                {
                    int action = await db.SQLExecuteAsync(SQL.PropertyDeactivateQry, new object[] { propertyId });
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
    }
}

