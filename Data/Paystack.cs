using PayStack.Net;
using Serilog;
using TheEstate.Models.AppModels;
using TheEstate.Models.PaymentModels;
using TheEstate.Repo;

namespace TheEstate.Data
{
    public class Paystack
    {
        public static async Task<TransactionModelResponse> InitializePayment(TransactionModel model)
        {
            TransactionModelResponse? data = null;

            try
            {
                List<SettingModel> paystack = await Utility.GetSettingByGroup(Constants.SettingGroups.PAYSTACK) ?? throw new Exception("Payment key error");
                string secretKey = paystack.Where(x => x.SettingId!.Equals(Constants.SettingIds.PAYSTACK_SECRET_KEY)).Select(x => x.SettingValue).FirstOrDefault()!;
                string publicKey = paystack.Where(x => x.SettingId!.Equals(Constants.SettingIds.PAYSTACK_PUBLIC_KEY)).Select(x => x.SettingValue).FirstOrDefault()!;
                //string callbackUrl = paystack.Where(x => x.SettingId!.Equals(Constants.SettingIds.PAYSTACK_CALLBACK_URL)).Select(x => x.SettingValue).FirstOrDefault()!;

                if (!string.IsNullOrEmpty(secretKey))
                {
                    TransactionInitializeRequest req = new()
                    {
                        AmountInKobo = Convert.ToInt32(model.Amount * 100), //convert amount to kobo
                        Email = model.Email,
                        Reference = Guid.NewGuid().ToString(),
                        Currency = model.Currency,
                        //CallbackUrl = callbackUrl,
                    };

                    PayStackApi payStackApi = new(secretKey);
                    TransactionInitializeResponse res = payStackApi.Transactions.Initialize(req);

                    bool insertPayment = await BillingRepo.MakePaymentFXN(estateId: model.EstateId!, householdId: model.HouseholdId!, amount: model.Amount, description: model.Description!, gateway: "ONLINE", method: "PAYSTACK", gatewayRef: req.Reference, billId: model.BillId, accessCode: res.Data.AccessCode);
                    if (insertPayment)
                    {
                        if (res.Status)
                        {
                            data = new TransactionModelResponse
                            {
                                AccessCode = res.Data.AccessCode,
                                Reference = res.Data.Reference,
                                SecretKey = secretKey,
                                PublicKey = publicKey,
                                AuthourizedUrl = res.Data.AuthorizationUrl,
                                Amount = Convert.ToInt32(req.AmountInKobo / 100), //revert from kobo to normal amount
                                Currency = model.Currency
                            };
                        }
                        else Log.Verbose($"Payment [{model.Description}] failed to initialize at Paystack");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<TransactionVerifyResponse> VerifyPayment(string reference)
        {
            TransactionVerifyResponse? data = null;
            try
            {
                List<SettingModel> paystack = await Utility.GetSettingByGroup(Constants.SettingGroups.PAYSTACK) ?? throw new Exception("Payment key error");
                string secretKey = paystack.Where(x => x.SettingId!.Equals(Constants.SettingIds.PAYSTACK_SECRET_KEY)).Select(x => x.SettingValue).FirstOrDefault()!;
                PayStackApi payStackApi = new(secretKey);
                data  = payStackApi.Transactions.Verify(reference);

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}
