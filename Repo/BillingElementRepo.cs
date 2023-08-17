using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.BillingElementModels;

namespace TheEstate.Repo
{
    public class BillingElementRepo : AppDbContext
    {
        public static async Task<List<BillingElementModelView>> GetBillingElements(string search, string estateId, string zoneId, string householdClassification, int pageSize, int pageNumber)
        {
            List<BillingElementModelView> data = new List<BillingElementModelView>();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                householdClassification = string.IsNullOrEmpty(householdClassification) ? "%%" : $"{householdClassification?.Trim()?.ToUpper()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.BillingElementsQry, new object[] { estateId, zoneId, search, householdClassification, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new BillingElementModelView()
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ElementId = i["element_id"].ToString(),
                            ElementTitle = i["element_title"].ToString(),
                            ElementStatus = i["status"].ToString(),
                            HouseholdClassification = i["classification_code"].ToString(),
                            ElementCost = string.IsNullOrEmpty(i["cost"].ToString()) ? 0 : Convert.ToDecimal(i["cost"]),
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


        public static async Task<int> GetBillingElementsCount(string search, string estateId, string zoneId, string householdClassification)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                householdClassification = string.IsNullOrEmpty(householdClassification) ? "%%" : $"{householdClassification?.Trim()?.ToUpper()}";

                string result = await db.SQLSelectAsync(SQL.BillingElementsQryCount, new object[] { estateId, zoneId, search, householdClassification });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<BillingElementModelView> GetBillingElementById(string elementId)
        {
            BillingElementModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.BillingElementByIdQry, new object[] { elementId});
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new BillingElementModelView()
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ElementId = i["element_id"].ToString(),
                            ElementTitle = i["element_title"].ToString(),
                            ElementStatus = i["status"].ToString(),
                            HouseholdClassification = i["classification_code"].ToString(),
                            ElementCost = string.IsNullOrEmpty(i["cost"].ToString()) ? 0 : Convert.ToDecimal(i["cost"]),
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




        public static async Task<string> CreateBillingElement(BillingElementModelCreate model, string? elementId = null)
        {
            string data;
            try
            {
                if (string.IsNullOrEmpty(model.HouseholdClassification))
                {
                    throw new Exception("Household classification must be selected");
                }
                else
                {
                    elementId ??= Guid.NewGuid().ToString();
                    model.ZoneId = string.IsNullOrEmpty(model.ZoneId) ? Constants.Defaults.DefaultZoneId : model.ZoneId;
                    model.ElementStatus = string.IsNullOrEmpty(model.ElementStatus) ? Constants.ActiviyStatus.ACTIVE.ToString() : model.ElementStatus;

                    List<string> sql = new();
                    List<object> param = new();

                    sql.Add(SQL.BillingElementCreateQry);
                    param.Add(new object[] { model.EstateId!, model.ZoneId, elementId, model.ElementTitle!, model.ElementStatus });

                    sql.Add(SQL.BillingElementCostCreateQry);
                    param.Add(new object[] { elementId, model.HouseholdClassification!, model.ElementCost });

                    int action = await db.SQLExecuteTransactionsAsync(sql.ToArray(), param.ToArray());
                    if (action > 0) data = Constants.StatusResponses.SUCCESS;
                    else data = Constants.StatusResponses.FAILED;
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
