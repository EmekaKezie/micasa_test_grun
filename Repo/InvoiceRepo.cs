using Newtonsoft.Json;
using System.Buffers.Text;
using System.Data;
using System.Drawing.Printing;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.InvoiceModels;
using TheEstate.Models.VisitorModels;

namespace TheEstate.Repo
{
    public class InvoiceRepo : AppDbContext
    {
        public static async Task<List<InvoiceModelView>> GetInvoices(string search, string estateId, string zoneId, string invoiceStatus, string invoiceTargetFlag, int pageSize, int pageNumber)
        {
            List<InvoiceModelView> data = new();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                invoiceStatus = string.IsNullOrEmpty(invoiceStatus) ? "%%" : $"{invoiceStatus?.Trim()?.ToUpper()}";
                invoiceTargetFlag = string.IsNullOrEmpty(invoiceTargetFlag) ? "%%" : $"{invoiceTargetFlag?.ToUpper()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.InvoicesQry, new object[] { estateId, zoneId, search, invoiceStatus, invoiceTargetFlag, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new InvoiceModelView()
                        {
                            InvoiceId = i["invoice_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            InvoiceDate = string.IsNullOrEmpty(i["invoice_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["invoice_date"]),
                            InvoiceTitle = i["invoice_title"].ToString(),
                            InvoiceStatus = i["status"].ToString(),
                            InvoiceTargetFlag = i["target_flag"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateCode = i["estate_code"].ToString(),
                            EstateImage = i["estate_image"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            InvoiceItems = await GetInvoiceItems(i["invoice_id"].ToString()!)
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


        public static async Task<InvoiceModelView> GetInvoiceById(string invoiceId)
        {
            InvoiceModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.InvoiceByIdQry, new object[] { invoiceId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new InvoiceModelView()
                        {
                            InvoiceId = i["invoice_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            InvoiceDate = string.IsNullOrEmpty(i["invoice_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["invoice_date"]),
                            InvoiceTitle = i["invoice_title"].ToString(),
                            InvoiceStatus = i["status"].ToString(),
                            InvoiceTargetFlag = i["target_flag"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateCode = i["estate_code"].ToString(),
                            EstateImage = i["estate_image"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            InvoiceItems = await GetInvoiceItems(i["invoice_id"].ToString()!)
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


        public static async Task<int> GetInvoiceCount(string search, string estateId, string zoneId, string invoiceStatus, string invoiceTargetFlag)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim()?.ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"{zoneId?.Trim()}";
                invoiceStatus = string.IsNullOrEmpty(invoiceStatus) ? "%%" : $"{invoiceStatus?.Trim()?.ToUpper()}";
                invoiceTargetFlag = string.IsNullOrEmpty(invoiceTargetFlag) ? "%%" : $"{invoiceTargetFlag?.ToUpper()}";

                string result = await db.SQLSelectAsync(SQL.InvoicesCountQry, new object[] { search, estateId, zoneId, invoiceStatus, invoiceTargetFlag });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<List<InvoiceItemModelView>> GetInvoiceItems(string invoiceId)
        {
            List<InvoiceItemModelView> data = new();
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.InvoiceItemsQryByInvoiceId, new object[] { invoiceId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new InvoiceItemModelView()
                        {
                            InvoiceId = i["invoice_id"].ToString(),
                            InvoiceItemId = i["invoice_item_id"].ToString(),
                            InvoiceItemTitle = i["invoice_item_title"].ToString(),
                            InvoiceCostType = i["cost_type"].ToString(),
                            InvoiceFlatRateCost = string.IsNullOrEmpty(i["flat_rate_cost"].ToString()) ? 0 : Convert.ToDecimal(i["flat_rate_cost"]),
                            BillingElementId = i["billing_element_id"].ToString(),
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


        public static async Task<string> CreateInvoice(InvoiceModelCreate model, string? invoiceId = null)
        {
            string data = string.Empty;
            try
            {
                model.ZoneId = string.IsNullOrEmpty(model.ZoneId) ? Constants.Defaults.DefaultZoneId : model.ZoneId;
                bool createInvoice = await CreateInvoiceFXN(model, invoiceId);
                if (createInvoice)
                {
                    List<string>? eligibleHouseholds = null;
                    if (model.InvoiceTargetFlag!.Equals(Constants.ActiviyStatus.ALL.ToString()))
                    {
                        if (model.ZoneId!.Equals(Constants.Defaults.DefaultZoneId) || string.IsNullOrEmpty(model.ZoneId))
                        {
                            //get all householdId that is in the estate
                            eligibleHouseholds = HouseholdRepo.GetHouseholdIdsByEstateId(model.EstateId!);
                        }
                        else
                        {
                            //get all householdId that is in the zone inside the estate
                            eligibleHouseholds = HouseholdRepo.GetHouseholdIdsByZoneId(model.EstateId!, model.ZoneId!);
                        }

                        if (eligibleHouseholds!.Count > 0)
                        {
                            await BillingRepo.CreateBilling(eligibleHouseholds, invoiceId!);
                        }
                    }
                    else if (model.InvoiceTargetFlag!.Equals(Constants.ActiviyStatus.SELECT.ToString()))
                    {
                        eligibleHouseholds = model.targetHouseholds;
                        if (eligibleHouseholds!.Count > 0)
                        {
                            await BillingRepo.CreateBilling(eligibleHouseholds, invoiceId!);
                            await BillingRepo.CreateBillingTarget(eligibleHouseholds, invoiceId!);
                        }
                    }

                    data = Constants.StatusResponses.SUCCESS;
                }
                else data = Constants.StatusResponses.FAILED;

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }




        public static async Task<bool> CreateInvoiceFXN(InvoiceModelCreate model, string? invoiceId = null)
        {
            bool data = false;
            try
            {
                invoiceId ??= Guid.NewGuid().ToString();

                List<InvoiceItemModel> invoiceItems = Utility.DecodeAnyTypeFromBase64<List<InvoiceItemModel>>(model.InvoiceItemBase64!);

                List<string> sql = new List<string>();
                List<object> param = new List<object>();

                model.ZoneId = string.IsNullOrEmpty(model.ZoneId) ? Constants.Defaults.DefaultZoneId : model.ZoneId;
                model.InvoiceStatus = string.IsNullOrEmpty(model.InvoiceStatus) ? Constants.ActiviyStatus.ACTIVE.ToString() : model.InvoiceStatus;


                sql.Add(SQL.InvoiceCreateQry);
                param.Add(new object[] { model.EstateId!, model.ZoneId, invoiceId, DateTime.Now, model.InvoiceTitle!, model.InvoiceStatus, model.InvoiceTargetFlag! });

                invoiceItems?.ForEach(item =>
                {
                    sql.Add(SQL.InvoiceItemCreateQry);
                    param.Add(new object[] { invoiceId, Guid.NewGuid().ToString(), item.InvoiceItemTitle, item.InvoiceCostType, item.InvoiceFlatRateCost, item.BillingElementId });
                });
                int action = await db.SQLExecuteTransactionsAsync(sql.ToArray(), param.ToArray());
                if (action > 0) data = true;

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}
