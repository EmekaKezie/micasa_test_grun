using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.EstateModels;

namespace TheEstate.Repo
{
    public class EstateRepo : AppDbContext
    {
        public static async Task<List<EstateModelView>> GetEstates(string search, string status, string code, int pageSize, int pageNumber)
        {
            List<EstateModelView> data = new();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                status = string.IsNullOrEmpty(status) ? "%%" : $"{status?.Trim()?.ToUpper()}";
                code = string.IsNullOrEmpty(code) ? "%%" : $"{code?.Trim()?.ToUpper()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.EstatesQry, new object[] { search, status, code, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new EstateModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateImage = i["image_url"].ToString(),
                            EstateCode = i["estate_code"].ToString(),
                            Longitude = i["longitute"].ToString(),
                            Latitude = i["latitude"].ToString(),
                            EstateStatus = i["status"].ToString(),
                            NoOfZone = Convert.ToInt32(i["no_of_zone"]),
                            NoOfStreet = Convert.ToInt32(i["no_of_street"]),
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


        public static async Task<int> GetEstatesCount(string search, string status, string code)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                status = string.IsNullOrEmpty(status) ? "%%" : $"{status?.Trim()?.ToUpper()}";
                code = string.IsNullOrEmpty(code) ? "%%" : $"{code?.Trim()?.ToUpper()}";

                string result = await db.SQLSelectAsync(SQL.EstatesCountQry, new object[] { search, status, code });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<EstateModelView> GetEstateById(string estateId)
        {
            EstateModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.EstateByIdQry, new object[] { estateId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new EstateModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateImage = i["image_url"].ToString(),
                            EstateCode = i["estate_code"].ToString(),
                            Longitude = i["longitute"].ToString(),
                            Latitude = i["latitude"].ToString(),
                            EstateStatus = i["status"].ToString(),
                            NoOfZone = Convert.ToInt32(i["no_of_zone"]),
                            NoOfStreet = Convert.ToInt32(i["no_of_street"]),
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


        public static async Task<string> CreateEstate(EstateModelCreate model, string estateId)
        {
            string data;
            try
            {
                model.EstateCode = string.IsNullOrEmpty(model.EstateCode) ? Utility.GenerateRandomCharacters(8) : model.EstateCode;
                model.EstateStatus = string.IsNullOrEmpty(model.EstateStatus) ? Constants.ActiviyStatus.ACTIVE.ToString() : model.EstateStatus;

                decimal longitude = string.IsNullOrEmpty(model.Longitude) ? 0 : Convert.ToDecimal(model.Longitude);
                decimal latitude = string.IsNullOrEmpty(model.Latitude) ? 0 : Convert.ToDecimal(model.Latitude);

                List<string> sql = new List<string>();
                List<object> param = new List<object>();

                sql.Add(SQL.EstateCreateQry);
                param.Add(new object[] { estateId, model.EstateName!, model.EstateDesc!, longitude, latitude, model.EstateStatus?.ToUpper()!, model.EstateCode, model.EstateImage, model.ProfileId!, DateTime.Now });

                sql.Add(SQL.ZoneCreateQry);
                param.Add(new object[] { estateId, Constants.Defaults.DefaultZoneId, Utility.GenerateRandomCharacters(3), Constants.Defaults.DefaultZoneName, Constants.Defaults.DefaultZoneName, model.ProfileId!, DateTime.Now });


                //int action = await db.SQLExecuteAsync(SQL.EstateCreateQry, new object[] { estateId, model.EstateName!, model.EstateDesc!, longitude, latitude, model.EstateStatus?.ToUpper()!, model.EstateCode, model.EstateImage, model.ProfileId!, DateTime.Now });
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


        public static async Task<string> UpdateEstate(EstateModelCreate model, string estateId)
        {
            string data;
            try
            {
                EstateModelView estate = await GetEstateById(estateId);
                if (estate != null)
                {
                    model.EstateCode = string.IsNullOrEmpty(model.EstateCode) ? estate.EstateCode : model.EstateCode;
                    model.EstateStatus = string.IsNullOrEmpty(model.EstateStatus) ? estate.EstateStatus : model.EstateStatus;
                    model.EstateImage = string.IsNullOrEmpty(model.EstateImage) ? estate.EstateImage : model.EstateImage;

                    decimal longitude = string.IsNullOrEmpty(model.Longitude) ? 0 : Convert.ToDecimal(model.Longitude);
                    decimal latitude = string.IsNullOrEmpty(model.Latitude) ? 0 : Convert.ToDecimal(model.Latitude);


                    int action = await db.SQLExecuteAsync(SQL.EstateUpdateQry, new object[] { model.EstateName!, model.EstateDesc!, longitude, latitude, model.EstateStatus!, model.EstateCode, model.EstateImage, model.ProfileId!, DateTime.Now, estateId });
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


        public static async Task<string> DeactivateEstate(string estateId)
        {
            string data;
            try
            {
                EstateModelView estate = await GetEstateById(estateId);
                if (estate != null)
                {
                    int action = await db.SQLExecuteAsync(SQL.EstateDeactivateQry, new object[] { estateId });
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
