using System.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AuthModels;

namespace TheEstate.RepoUtil
{
    public class AuthUtil : AppDbContext
    {
        //public static async Task<AuthUserModel> AuthUser(string username)
        //{
        //    AuthUserModel? data = null;

        //    try
        //    {
        //        DataTable dt = await db.SQLFetchAsync(SQL.AuthUserQry, new object[] { username });
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow i in dt.Rows)
        //            {
        //                data = new AuthUserModel
        //                {
        //                    Id = i["profile_id"].ToString(),
        //                    Username = i["username"].ToString(),
        //                    Password = i["password"].ToString(),
        //                    LoginMethod = i["login_method"].ToString(),
        //                    Email = i["emailaddress"].ToString(),
        //                    MobileNo = i["mobileno"].ToString(),
        //                    Firstname = i["firstname"].ToString(),
        //                    Lastname = i["lastname"].ToString(),
        //                    Status = i["status"].ToString(),
        //                    EstateId = i["estate_id"].ToString(),
        //                    EstateName = i["estate_name"].ToString(),
        //                    ResidentId = i["resident_id"].ToString(),
        //                    ResidentCategory = i["resident_category"].ToString(),
        //                    ResidentCode = i["resident_code"].ToString(),
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return data;
        //}


        //public static async Task<bool> CheckExistsUsername(string username)
        //{
        //    bool data = false;
        //    try
        //    {
        //        string result = await db.SQLSelectAsync(SQL.CheckExistsUsernameQry, new[] { username });
        //        int count = Convert.ToInt32(result);
        //        if (count > 0) data = true;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return data;
        //}
    }
}
