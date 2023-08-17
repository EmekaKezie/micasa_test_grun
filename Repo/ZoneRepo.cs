using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.EstateModels;
using TheEstate.Models.ZoneModels;

namespace TheEstate.Repo
{
    public class ZoneRepo : AppDbContext
    {
        public static async Task<List<ZoneModelView>> GetZones(string search, string estateId, string zoneType, int pageSize, int pageNumber)
        {
            List<ZoneModelView> data = new();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneType = string.IsNullOrEmpty(zoneType) ? "%%" : $"{zoneType?.Trim()?.ToUpper()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.ZonesQry, new object[] { estateId, search, search, zoneType, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new ZoneModelView
                        {
                            ZoneId = i["zone_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
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


        public static async Task<int> GetZonesCount(string search, string estateId, string zoneType)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneType = string.IsNullOrEmpty(zoneType) ? "%%" : $"{zoneType?.Trim()?.ToUpper()}";

                string result = await db.SQLSelectAsync(SQL.ZonesQryCount, new object[] { estateId, search, search, zoneType });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<ZoneModelView> GetZoneById(string zoneId)
        {
            ZoneModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ZoneByIdQry, new object[] { zoneId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new ZoneModelView
                        {
                            ZoneId = i["zone_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
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


        public static async Task<string> CreateZone(ZoneModelCreate model, string? zoneId = null)
        {
            string data;
            try
            {
                zoneId ??= Guid.NewGuid().ToString();
                model.ZoneCode = string.IsNullOrEmpty(model.ZoneCode) ? Utility.GenerateRandomCharacters(3) : model.ZoneCode;

                if (model.ZoneCode.Length < 3)
                {
                    int action = await db.SQLExecuteAsync(SQL.ZoneCreateQry, new object[] { model.EstateId!, zoneId, model.ZoneCode, model.ZoneName, model.ZoneType, model.ProfileId, DateTime.Now });
                    if (action > 0) data = Constants.StatusResponses.SUCCESS;
                    else data = Constants.StatusResponses.FAILED;
                }
                else
                {
                    return Constants.StatusResponses.ERROR;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> UpdateZone(ZoneModelCreate model, string zoneId)
        {
            string data;
            try
            {
                ZoneModelView zone = await GetZoneById(zoneId);
                if (zone != null)
                {
                    model.ZoneCode = string.IsNullOrEmpty(model.ZoneCode) ? zone.ZoneCode : model.ZoneCode;

                    int action = await db.SQLExecuteAsync(SQL.ZoneUpdateQry, new object[] { model.EstateId!, model.ZoneCode!, model.ZoneName!, model.ZoneType!, model.ProfileId, DateTime.Now, zoneId });
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


        public static async Task<string> DeactivateZone(string zoneId)
        {
            string data;
            try
            {
                ZoneModelView zone = await GetZoneById(zoneId);
                if (zone != null)
                {
                    int action = await db.SQLExecuteAsync(SQL.ZoneDeactivateQry, new object[] { zoneId });
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
