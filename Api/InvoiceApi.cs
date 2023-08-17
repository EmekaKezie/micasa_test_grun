using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TheEstate.Data;
using TheEstate.Models.AuthModels;
using TheEstate.Models.EstateModels;
using TheEstate.Models.InvoiceModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class InvoiceApi
    {
        public static void RegisterInvoiceApi(this WebApplication app)
        {
            app.MapGet("/invoice/get", GetInvoice);
            app.MapGet("/invoice/get/{id}", GetInvoiceById);
            app.MapPost("/invoice/create", CreateInvoice);
            //app.MapGet("/invoice/get/{id}/householdbill", GetInvoiceForHouseholds);
            //app.MapPost("/invoice/assign/{id}/householdbill", AssignInvoiceToHouseholds);

        }


        [Authorize]
        [ProducesResponseType(typeof(PaginatedResponseModel<List<InvoiceModelView>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetInvoice(HttpContext httpContext, string? search, string? estateId, string? zoneId, string? invoiceStatus, string? invoiceTargetFlag, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    pageNumber = pageNumber == 0 ? 1 : pageNumber;
                    List<InvoiceModelView> data = await InvoiceRepo.GetInvoices(search, estateId, zoneId, invoiceStatus, invoiceTargetFlag, pageSize, pageNumber);
                    int total = await InvoiceRepo.GetInvoiceCount(search, estateId, zoneId, invoiceStatus, invoiceTargetFlag);
                    PaginatedResponseModel<List<InvoiceModelView>> ret = new PaginatedResponseModel<List<InvoiceModelView>>()
                    {
                        Total = total,
                        Data = data
                    };
                    return Results.Ok(ret);
                }
                else return Results.Unauthorized();

            }
            catch (Exception ex)
            {
                Log.Error($"[Invoice][GetInvoice] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        [ProducesResponseType(typeof(InvoiceModelView), StatusCodes.Status200OK)]
        public static async Task<IResult> GetInvoiceById(HttpContext httpContext, string id)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!);
                if (isValidToken)
                {
                    InvoiceModelView ret = await InvoiceRepo.GetInvoiceById(id);

                    if (ret == null) return Results.NotFound("Not Found");
                    else return Results.Ok(ret);
                }
                else return Results.Unauthorized();

            }
            catch (Exception ex)
            {
                Log.Error($"[Invoice][GetInvoiceById] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        public static async Task<IResult> CreateInvoice(HttpContext httpContext, InvoiceModelCreate model)
        {
            try
            {
                ClaimsIdentityModel tokenClaims = Jwt.ClaimsCredentials(httpContext);
                bool isValidToken = Jwt.ValidateToken(httpContext, tokenClaims.Id!) && tokenClaims.Id == model.ProfileId;
                if (isValidToken)
                {
                    string invoiceId = Guid.NewGuid().ToString();
                    string ret = await InvoiceRepo.CreateInvoice(model, invoiceId);
                    if (ret == Constants.StatusResponses.FAILED)
                    {
                        Log.Error($"[Invoice][CreateInvoice] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                    else if (ret == Constants.StatusResponses.SUCCESS)
                    {
                        InvoiceModelView invoice = await InvoiceRepo.GetInvoiceById(invoiceId);
                        return Results.Ok(invoice);
                    }
                    else
                    {
                        Log.Error($"[Invoice][CreateInvoice] -> Failed -> {JsonConvert.SerializeObject(model)}");
                        return Results.BadRequest("Failed. Please Try again");
                    }
                }
                else return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error($"[Invoice][CreateInvoice] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        public static async Task<IResult> GetInvoiceForHouseholds()
        {
            try
            {
                return Results.Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<IResult> AssignInvoiceToHouseholds()
        {
            try
            {
                return Results.Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
