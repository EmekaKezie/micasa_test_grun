using System.Data;
using TheEstate.Data.Database;
using TheEstate.Models.CfgModels;

namespace TheEstate.Repo
{
    public class CfgRepo : AppDbContext
    {
        public static async Task<List<UsageCategoryModelView>> GetUsageCategories(string estateId)
        {
            List<UsageCategoryModelView> data = new();
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";

                DataTable dt = await db.SQLFetchAsync(SQL.UsageCategoriesQry, new object[] { estateId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new UsageCategoryModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            UsageCategoryCode = i["usage_category_code"].ToString(),
                            UsageCategoryName = i["usage_category_name"].ToString(),
                            Status = i["status"].ToString(),
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


        public static async Task<List<PropertyTypeModelView>> GetPropertyTypes(string estateId)
        {
            List<PropertyTypeModelView> data = new();
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";

                DataTable dt = await db.SQLFetchAsync(SQL.PropertyTypesQry, new object[] { estateId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new PropertyTypeModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            PropertyTypeCode = i["property_type_code"].ToString(),
                            PropertyTypeName = i["property_type_name"].ToString(),
                            Status = i["status"].ToString(),
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



        public static async Task<List<HouseholdClassificationModelView>> GetHouseholdClassifications(string estateId, string zoneId)
        {
            List<HouseholdClassificationModelView> data = new();
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";

                DataTable dt = await db.SQLFetchAsync(SQL.HouseholdClassificationQry, new object[] { estateId, zoneId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new HouseholdClassificationModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            HouseholdClassificationCode = i["classification_code"].ToString(),
                            HouseholdClassificationName = i["classification_name"].ToString(),
                            //Status = i["status"].ToString(),
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



        //public static async Task<>
    }
}
