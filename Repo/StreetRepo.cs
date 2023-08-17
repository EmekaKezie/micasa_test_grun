using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.HouseholdModels;
using TheEstate.Models.StreetModels;
using TheEstate.Models.ZoneModels;

namespace TheEstate.Repo
{
    public class StreetRepo : AppDbContext
    {
        public static async Task<List<StreetModelView>> GetStreets(string search, string streetType, string zoneId, string estateId, int pageSize, int pageNumber)
        {
            List<StreetModelView> data = new();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                streetType = string.IsNullOrEmpty(streetType) ? "%%" : $"%{streetType?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"%{estateId?.Trim()}%";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"%{zoneId?.Trim()}%";

                DataTable dt = await db.SQLFetchAsync(SQL.StreetsQry, new object[] { search, search, streetType, zoneId, estateId, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new StreetModelView
                        {
                            StreetId = i["street_id"].ToString(),
                            StreetCode = i["street_code"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            StreetType = i["street_type"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZipcodeId = i["zipcode_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneCode = i["zone_code"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ZoneType = i["zone_type"].ToString(),
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


        public static async Task<StreetModelView> GetStreetById(string streetId)
        {
            StreetModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.StreetByIdQry, new object[] { streetId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new StreetModelView
                        {
                            StreetId = i["street_id"].ToString(),
                            StreetCode = i["street_code"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            StreetType = i["street_type"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZipcodeId = i["zipcode_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            ZoneCode = i["zone_code"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            ZoneType = i["zone_type"].ToString(),
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



        public static async Task<int> GetStreetsCount(string search, string streetType, string zoneId, string estateId)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                streetType = string.IsNullOrEmpty(streetType) ? "%%" : $"%{streetType?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"%{estateId?.Trim()}%";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"%{zoneId?.Trim()}%";

                string result = await db.SQLSelectAsync(SQL.StreetsQryCount, new object[] { search, search, streetType, zoneId, estateId });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> CreateStreet(StreetModelCreate model, string? streetId = null)
        {
            string data;
            try
            {
                streetId ??= Guid.NewGuid().ToString();
                model.StreetCode = string.IsNullOrEmpty(model.StreetCode) ? Utility.GenerateRandomCharacters(3) : model.StreetCode;

                List<string> sql = new List<string>();
                List<object> param = new List<object>();

                sql.Add(SQL.StreetCreateQry);
                param.Add(new object[] { model.EstateId!, streetId, model.StreetCode, model.StreetName!, model.StreetType!, model.ProfileId!, DateTime.Now });

                sql.Add(SQL.ZipcodeCreateQry);
                param.Add(new object[] { model.EstateId!, Guid.NewGuid().ToString(), model.ZoneId!, streetId});

                int action = await db.SQLExecuteTransactionsAsync(sql.ToArray(), param.ToArray());
                if (action > 0) data = Constants.StatusResponses.SUCCESS;
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> UpdateStreet(StreetModelCreate model, string streetId)
        {
            string data;
            try
            {
                StreetModelView street = await GetStreetById(streetId);
                if (street != null)
                {
                    model.StreetCode = string.IsNullOrEmpty(model.StreetCode) ? street.StreetCode : model.StreetCode;

                    int action = await db.SQLExecuteAsync(SQL.StreetUpdateQry, new object[] { model.StreetCode!, model.StreetName!, model.StreetType!, model.ProfileId!, DateTime.Now, streetId });
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


        public static async Task<string> DeactivateStreet(string streetId)
        {
            string data;
            try
            {
                StreetModelView street = await GetStreetById(streetId);
                if (street != null)
                {
                    int action = await db.SQLExecuteAsync(SQL.StreetDeactivateQry, new object[] { streetId });
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
