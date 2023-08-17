using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AppModels;
using TheEstate.Models.BillingModels;
using TheEstate.Models.PaymentModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class BillingApi
    {
        public static void RegisterBillingApi(this WebApplication app)
        {
            app.MapGet("/billing/getbillings", GetFlatrateBillings);
            app.MapPost("/billing/payment", BillPaymentOnline);
            app.MapPost("/billing/offlinepayment", BillPaymentOffline);
            app.MapPost("/billing/uploadreceipt", UploadPaymentReceipt).Accepts<IFormFile>("multipart/form-data");
            app.MapPost("/billing/payment/verify", VerifyBillPayment);
        }


        [ProducesResponseType(typeof(PaginatedResponseModel<List<BillingModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetFlatrateBillings(string? invoiceId, string? householdId, string? billStatus, string? invoiceTargetFlag, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                List<BillingModelView> billings = await BillingRepo.GetBillings(invoiceId, householdId, billStatus, invoiceTargetFlag, pageSize, ++pageNumber);
                int total = await BillingRepo.GetBillingsCount(invoiceId, householdId, billStatus, invoiceTargetFlag);

                PaginatedResponseModel<List<BillingModelView>> ret = new()
                {
                    Data = billings,
                    Total = total
                };
                return Results.Ok(ret);
            }
            catch (Exception ex)
            {
                Log.Error($"[Billing][GetFlatrateBillings] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(TransactionModelResponse), StatusCodes.Status200OK)]
        public static async Task<IResult> BillPaymentOnline(PaymentModelCreate model)
        {
            try
            {
                TransactionModelResponse ret = await BillingRepo.BillPaymentOnline(model);

                if (ret == null) return Results.BadRequest("Failed. Please try again");
                else return Results.Ok(ret);
            }
            catch (Exception ex)
            {
                Log.Error($"[Billing][BillPayment] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> BillPaymentOffline(PaymentModelOffline model)
        {
            Log.Verbose(JsonConvert.SerializeObject(model));
            try
            {
                string ret = await BillingRepo.BillPaymentOffline(model);

                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                else if (ret == Constants.StatusResponses.NOTFOUND) return Results.NotFound("Bill not found");
                else if (ret == Constants.StatusResponses.SUCCESS) return Results.Ok("Success");
                else return Results.BadRequest("Failed. Please try again");
            }
            catch (Exception ex)
            {
                Log.Error($"[Billing][BillPayment] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(FileModelResponse), StatusCodes.Status200OK)]
        public static IResult UploadPaymentReceipt(IWebHostEnvironment webHostEnvironment, HttpRequest file)
        {
            try
            {
                IFormFile uploadedFile = file.Form.Files[0];
                string webRoot = webHostEnvironment.WebRootPath;
                FileModelResponse ret = Utility.FileUploader(webRoot, Constants.FileStorageLocations.PAYMENT_RECEIPT_PATH, uploadedFile);

                if (ret != null) return Results.Ok(ret);
                else return Results.NoContent();
            }
            catch (Exception ex)
            {
                Log.Error($"[Profile][UploadEstatePhoto] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> VerifyBillPayment(PaymentVerifyModel model)
        {
            try
            {
                string ret = await BillingRepo.VerifyBillPayment(model);

                if (ret == Constants.StatusResponses.FAILED)
                {
                    Log.Information($"Verifypayment - failed - {JsonConvert.SerializeObject(model)}");
                    return Results.BadRequest("Failed. Please try again");
                }
                else if (ret == Constants.StatusResponses.NOTFOUND)
                {
                    Log.Information($"Verifypayment - notfound - {JsonConvert.SerializeObject(model)}");
                    return Results.NotFound("Payment cannot be found");
                }
                else if (ret == Constants.StatusResponses.ERROR)
                {
                    Log.Information($"Verifypayment - error - {JsonConvert.SerializeObject(model)}");
                    PaymentModel res = await PaymentRepo.GetPaymetByReference(model.ReferenceCode!);
                    return Results.BadRequest(res.AcceptanceStatus);
                }
                else if (ret == Constants.StatusResponses.EXISTS)
                {
                    Log.Information($"Verifypayment - exist - {JsonConvert.SerializeObject(model)}");
                    return Results.Conflict("Payment already completed");
                }
                else if (ret == Constants.StatusResponses.SUCCESS)
                {
                    PaymentModel res = await PaymentRepo.GetPaymetByReference(model.ReferenceCode!);
                    return Results.Ok(res.AcceptanceStatus);
                }
                else return Results.BadRequest("Failed. Please try again");

            }
            catch (Exception ex)
            {
                Log.Error($"[Billing][VerifyBillPayment] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


    }
}
