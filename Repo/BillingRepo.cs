using PayStack.Net;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;
using TheEstate.Models.BillingModels;
using TheEstate.Models.InvoiceModels;
using TheEstate.Models.PaymentModels;

namespace TheEstate.Repo
{
    public class BillingRepo : AppDbContext
    {
        public static async Task<List<BillingModelView>> GetBillings(string invoiceId, string householdId, string billStatus, string invoiceTargetFlag, int pageSize, int pageNumber)
        {
            List<BillingModelView> data = new();
            try
            {
                invoiceId = string.IsNullOrEmpty(invoiceId) ? "%%" : $"{invoiceId?.Trim()}";
                householdId = string.IsNullOrEmpty(householdId) ? "%%" : $"{householdId?.Trim()}";
                billStatus = string.IsNullOrEmpty(billStatus) ? "%%" : $"{billStatus?.Trim().ToUpper()}";
                invoiceTargetFlag = string.IsNullOrEmpty(invoiceTargetFlag) ? "%%" : $"{invoiceTargetFlag?.Trim().ToUpper()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.BillingsQry, new object[] { invoiceId, householdId, billStatus, invoiceTargetFlag, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new BillingModelView
                        {
                            BillId = i["bill_id"].ToString(),
                            BillStatus = i["bill_status"].ToString(),
                            BillStatusDate = string.IsNullOrEmpty(i["bill_status_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["bill_status_date"]),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            InvoiceId = i["invoice_id"].ToString(),
                            InvoiceDate = string.IsNullOrEmpty(i["invoice_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["invoice_date"]),
                            InvoiceTitle = i["invoice_title"].ToString(),
                            InvoiceStatus = i["invoice_status"].ToString(),
                            InvoiceTargetFlag = i["target_flag"].ToString(),
                            BillAmount = string.IsNullOrEmpty(i["bill_amount"].ToString()) ? 0 : Convert.ToDecimal(i["bill_amount"]),
                            ZoneId = i["zone_id"].ToString(),
                            HouseholdId = i["household_id"].ToString(),
                            HouseholdLabel = i["household_label"].ToString(),
                            HouseholdUsageCategory = i["usage_category"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            BillItems = await GetBillIngItems(i["bill_id"].ToString()!),
                            Currency = "NGN",
                            PaymentId = i["payment_id"].ToString(),
                            PaymentMethod = i["payment_method"].ToString(),
                            AmountPaid = string.IsNullOrEmpty(i["payment_amount"].ToString()) ? 0 : Convert.ToDecimal(i["payment_amount"]),
                            PaymentAcceptanceStatus = i["acceptance_status"].ToString(),
                            PaymentReceipt = i["receipt"].ToString(),
                            PaymentNote = i["note"].ToString(),
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


        public static async Task<int> GetBillingsCount(string invoiceId, string householdId, string billStatus, string invoiceTargetFlag)
        {
            int data;
            try
            {
                invoiceId = string.IsNullOrEmpty(invoiceId) ? "%%" : $"{invoiceId?.Trim()}";
                householdId = string.IsNullOrEmpty(householdId) ? "%%" : $"%{householdId?.Trim().ToUpper()}%";
                billStatus = string.IsNullOrEmpty(billStatus) ? "%%" : $"%{billStatus?.Trim().ToUpper()}%";
                invoiceTargetFlag = string.IsNullOrEmpty(invoiceTargetFlag) ? "%%" : $"%{invoiceTargetFlag?.Trim().ToUpper()}%";

                string result = await db.SQLSelectAsync(SQL.BillingsCountQry, new object[] { invoiceId, householdId, billStatus, invoiceTargetFlag });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<List<BillingItemModel>> GetBillIngItems(string billId)
        {
            List<BillingItemModel> data = new();
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.BillingItems, new object[] { billId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new BillingItemModel()
                        {
                            BillId = i["bill_id"].ToString(),
                            ItemId = i["item_id"].ToString(),
                            ItemDesc = i["item_description"].ToString(),
                            ItemCost = Convert.ToDecimal(i["item_cost"]),
                            ItemCurrency = "NGN"
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


        public static async Task<BillingModelView> GetBillById(string billId)
        {
            BillingModelView data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.BillingByIdQry, new object[] { billId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new BillingModelView
                        {
                            BillId = i["bill_id"].ToString(),
                            BillStatus = i["bill_status"].ToString(),
                            BillStatusDate = string.IsNullOrEmpty(i["bill_status_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["bill_status_date"]),
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            InvoiceId = i["invoice_id"].ToString(),
                            InvoiceDate = string.IsNullOrEmpty(i["invoice_date"].ToString()) ? Utility.NullDate() : Convert.ToDateTime(i["invoice_date"]),
                            InvoiceTitle = i["invoice_title"].ToString(),
                            InvoiceStatus = i["invoice_status"].ToString(),
                            InvoiceTargetFlag = i["target_flag"].ToString(),
                            BillAmount = string.IsNullOrEmpty(i["bill_amount"].ToString()) ? 0 : Convert.ToDecimal(i["bill_amount"]),
                            ZoneId = i["zone_id"].ToString(),
                            HouseholdId = i["household_id"].ToString(),
                            HouseholdLabel = i["household_label"].ToString(),
                            HouseholdUsageCategory = i["usage_category"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            BillItems = await GetBillIngItems(i["bill_id"].ToString()!),
                            Currency = "NGN",
                            PaymentId = i["payment_id"].ToString(),
                            PaymentMethod = i["payment_method"].ToString(),
                            AmountPaid = string.IsNullOrEmpty(i["payment_amount"].ToString()) ? 0 : Convert.ToDecimal(i["payment_amount"]),
                            PaymentAcceptanceStatus = i["acceptance_status"].ToString(),
                            PaymentReceipt = i["receipt"].ToString(),
                            PaymentNote = i["note"].ToString(),
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


        public static async Task<bool> CreateBilling(List<string> householdIds, string invoiceId)
        {
            bool data = false;

            try
            {
                InvoiceModelView invoice = await InvoiceRepo.GetInvoiceById(invoiceId);
                if (invoice != null)
                {
                    List<string> billSql = new();
                    List<object> billParam = new();

                    List<string> billItemSql = new();
                    List<object> billItemParam = new();


                    householdIds?.ForEach(householdId =>
                    {
                        string billId = Guid.NewGuid().ToString();
                        billSql.Add(SQL.BillingCreateQry);
                        billParam.Add(new object[] { billId, invoiceId, householdId, Constants.ActiviyStatus.OPEN.ToString(), DateTime.Now });

                        invoice.InvoiceItems?.ForEach(billItems =>
                        {
                            string billItemId = Guid.NewGuid().ToString();
                            billItemSql.Add(SQL.BillingItemCreateQry);
                            billItemParam.Add(new object[] { billId, billItemId, billItems.InvoiceItemTitle!, billItems.InvoiceFlatRateCost });
                        });

                    });

                    int createBillaction = db.SQLExecuteTransactions(billSql.ToArray(), billParam.ToArray());
                    int createBillItemAction = db.SQLExecuteTransactions(billItemSql.ToArray(), billItemParam.ToArray());
                    if (createBillaction > 0) data = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }


        public static async Task<bool> CreateBillingTarget(List<string> householdIds, string invoiceId)
        {
            bool data = false;
            try
            {
                InvoiceModelView invoice = await InvoiceRepo.GetInvoiceById(invoiceId);
                if (invoice != null)
                {
                    List<string> sql = new();
                    List<object> param = new();

                    householdIds?.ForEach(householdId =>
                    {
                        sql.Add(SQL.BillingTargetCreateQry);
                        param.Add(new object[] { invoiceId, householdId});
                    });
                    int action = db.SQLExecuteTransactions(sql.ToArray(), param.ToArray());
                    if (action > 0) data = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<TransactionModelResponse> BillPaymentOnline(PaymentModelCreate model)
        {
            TransactionModelResponse? data = null;

            try
            {
                BillingModelView bill = await GetBillById(model.BillId!);

                if (bill != null)
                {
                    TransactionModel tranxModel = new()
                    {
                        EstateId = model.EstateId,
                        Amount = bill.BillAmount,
                        Currency = bill.Currency,
                        Email = bill.ResidentEmail,
                        Description = bill.InvoiceTitle,
                        BillId = model.BillId,
                        HouseholdId = model.HouseholdId,

                    };

                    if (model.PaymentGateway!.Equals(Constants.PaymentGateways.PAYSTACK.ToString()))
                    {
                        TransactionModelResponse paystack = await Paystack.InitializePayment(tranxModel);
                        if (paystack != null) data = paystack;
                    }
                    else if (model.PaymentGateway.Equals(Constants.PaymentGateways.GTPAY.ToString())) {
                        bool insertPayment = await BillingRepo.MakePaymentFXN(estateId: model.EstateId!, householdId: model.HouseholdId!, amount: tranxModel.Amount, description: tranxModel.Description!, gateway: "ONLINE", method: Constants.PaymentGateways.GTPAY.ToString(), gatewayRef: model.ReferenceCode, billId: model.BillId, accessCode:model.AccessCode);
                        data = new TransactionModelResponse
                        {
                            AccessCode = model.AccessCode,
                            Reference = model.ReferenceCode,
                            SecretKey = "",
                            PublicKey = "",
                            AuthourizedUrl = "",
                            Amount = tranxModel.Amount,
                            Currency = tranxModel.Currency
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



        public static async Task<string> BillPaymentOffline(PaymentModelOffline model)
        {
            string? data = null;

            try
            {
                BillingModelView bill = await GetBillById(model.BillId!);

                if (bill != null)
                {
                    bool insertPayment = await BillingRepo.MakePaymentFXN(estateId: model.EstateId!, householdId: model.HouseholdId!, amount: model.Amount, description: bill.InvoiceTitle!, gateway: "OFFLINE", method: "OFFLINE", gatewayRef: Guid.NewGuid().ToString(), billId: model.BillId, receipt: model.Receipt, note: model.Note);
                    if (insertPayment) data = Constants.StatusResponses.SUCCESS;
                    else data = Constants.StatusResponses.FAILED;
                }
                else
                {
                    data = Constants.StatusResponses.NOTFOUND;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }



        public static async Task<string> VerifyBillPayment(PaymentVerifyModel model)
        {
            string data;
            bool canVerify = false;
            string acceptanceStatus = Constants.ActiviyStatus.PENDING.ToString();
            try
            {
                PaymentModel checkExistsTransactionReference = await PaymentRepo.GetPaymetByReference(model.ReferenceCode!);
                if (checkExistsTransactionReference != null)
                {
                    if (checkExistsTransactionReference.PaymentGateway!.Equals(Constants.PaymentGateways.PAYSTACK.ToString()))
                    {
                        TransactionVerifyResponse? paystackVerifyTrnx = await Paystack.VerifyPayment(model.ReferenceCode);
                        if (paystackVerifyTrnx.Data.Status.Equals("success"))
                        {
                            acceptanceStatus = Constants.ActiviyStatus.ACCEPTED.ToString();
                            canVerify = true;
                        }
                        else
                        {
                            acceptanceStatus = paystackVerifyTrnx.Data.Status.ToUpper();
                        }
                        
                    }
                    else if (checkExistsTransactionReference.PaymentGateway!.Equals(Constants.PaymentGateways.GTPAY.ToString()))
                    {
                        if (model.Status!.Equals(Constants.ActiviyStatus.ACCEPTED.ToString()))
                        {
                            acceptanceStatus = Constants.ActiviyStatus.ACCEPTED.ToString();
                            canVerify = true;
                        }
                        else
                        {
                            acceptanceStatus = model.Status.ToUpper();
                        }
                    }
                    else
                    {
                        data = Constants.StatusResponses.ERROR;
                        throw new Exception("Failed to verify. Payment channel is unrecognized");
                    }

                    
                    if (canVerify)
                    {
                        bool isAccepted = await CheckIsPaymentAccepted(model.ReferenceCode);
                        if (!isAccepted)
                        {
                            string billId = await GetBillByGatewayRef(model.ReferenceCode!);

                            List<string> sql = new();
                            List<object> param = new();

                            sql.Add(SQL.VerifyPayment);
                            param.Add(new object[] { acceptanceStatus, DateTime.Now, model.ReferenceCode! });


                            sql.Add(SQL.CloseHouseholdBill);
                            param.Add(new object[] { Constants.ActiviyStatus.CLOSED.ToString(), DateTime.Now, billId });

                            int action = await db.SQLExecuteTransactionsAsync(sql.ToArray(), param.ToArray());
                            if (action > 0) data = Constants.StatusResponses.SUCCESS;
                            else data = Constants.StatusResponses.FAILED;
                        }
                        else data = Constants.StatusResponses.EXISTS;
                    }
                    else
                    {
                        int action = await db.SQLExecuteAsync(SQL.VerifyPayment, new object[] { acceptanceStatus, DateTime.Now, model.ReferenceCode! });
                        //if (action > 0) data = Constants.StatusResponses.SUCCESS;
                        //else data = Constants.StatusResponses.FAILED;
                        data = Constants.StatusResponses.ERROR;
                    }
                    //else data = Constants.StatusResponses.ERROR;

                }
                else data = Constants.StatusResponses.NOTFOUND;

            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }


        public static async Task<bool> CheckExistsGatewayRef(string gatewayRef)
        {
            bool data;
            try
            {
                string result = await db.SQLSelectAsync(SQL.CheckExistsGatewayRef, new object[] { gatewayRef });
                int count = Convert.ToInt32(result);
                if (count > 0) data = true;
                else data = false;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> GetBillByGatewayRef(string gatewayRef)
        {
            string data;
            try
            {
                data = await db.SQLSelectAsync(SQL.GetBillByGatewayRef, new object[] { gatewayRef });

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<bool> CheckIsPaymentAccepted(string gatewayRef)
        {
            bool data;
            try
            {
                string result = await db.SQLSelectAsync(SQL.CheckAcceptedPaymentByGatewayRef, new object[] { Constants.ActiviyStatus.ACCEPTED.ToString(), gatewayRef });
                int count = Convert.ToInt32(result);
                if (count > 0) data = true;
                else data = false;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }














        public static async Task<bool> MakePaymentFXN(string estateId, string householdId, decimal amount, string description, string gateway, string method, string gatewayRef, string? paymentId = null, string? billId = null, string? receipt = null, string? note = null, string? accessCode = null)
        {
            bool data = false;

            try
            {
                paymentId ??= Guid.NewGuid().ToString();
                billId ??= Guid.NewGuid().ToString();
                receipt ??= string.Empty;
                note ??= string.Empty;

                int action = await db.SQLExecuteAsync(SQL.MakePaymentsQry, new object[] { estateId, paymentId, householdId, billId, amount, description, gateway, method, DateTime.Now, gatewayRef, receipt, note, accessCode });
                if (action > 0)
                {
                    Log.Information($"Payment [{description}] successfully inserted into the database");
                    data = true;
                }
                else Log.Information($"Payment [{description}] failed to insert into the database");
            }
            catch (Exception)
            {

                throw;
            }

            return data;

        }




    }
}
