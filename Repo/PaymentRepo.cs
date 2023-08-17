using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.PaymentModels;

namespace TheEstate.Repo
{
    public class PaymentRepo : AppDbContext
    {

        public static async Task<PaymentModel> GetPaymetByReference(string referenceCode)
        {
            PaymentModel? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.PaymentByReferenceQry, new object[] { referenceCode });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new PaymentModel
                        {
                            EstateId = i["estate_id"].ToString(),
                            BillId = i["billId"].ToString(),
                            HouseholdId = i["household_id"].ToString(),
                            PaymentGateway = i["payment_gateway"].ToString(),
                            PaymentMethod = i["payment_method"].ToString(),
                            AcceptanceStatus = i["acceptance_status"].ToString(),
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


    }
}
