using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Text;
using TheEstate.Data;
using TheEstate.Models.AppModels;
using TheEstate.Models.ResidentModels;
using TheEstate.Models.ResponseModels;
using TheEstate.Models.VisitorModels;
using TheEstate.Repo;

namespace TheEstate.Api
{
    public static class VisitorApi
    {
        public static void RegisterVisitorApi(this WebApplication app)
        {
            app.MapGet("/visitor/getbookings", GetBookings);
            app.MapPost("/visitor/createbooking", CreateBooking);
            app.MapPost("/visitor/createbooking/bulk", CreateBulkBooking);
            //app.MapPost("/visitor/createbooking/bulktest", CreateBulkBookingTest);
            app.MapPost("/visitor/exitbooking", ExitBooking);
        }


        [ProducesResponseType(typeof(PaginatedResponseModel<List<VisitorModelViewExtension>>), StatusCodes.Status200OK)]
        public static async Task<IResult> GetBookings(string? search, string? estateId, string? zoneId, string? residentId, string? isRecurring, string? isClearedtoExit, string? isActiveBooking, int pageSize = 50, int pageNumber = 0)
        {
            try
            {
                List<VisitorModelViewExtension> bookings = await VisitorRepo.GetVisitorBookings(search, estateId, zoneId, residentId, isRecurring, isClearedtoExit, pageSize, ++pageNumber);
                int total = await VisitorRepo.GetVisitorBookingsCount(search, estateId, zoneId, residentId, isRecurring, isClearedtoExit);

                if (!string.IsNullOrEmpty(isActiveBooking))
                {
                    bookings = bookings.Where(x => x.isActiveBooking == isActiveBooking.ToUpper()).ToList();
                    total = bookings.Count;
                }



                PaginatedResponseModel<List<VisitorModelViewExtension>> ret = new()
                {
                    Data = bookings,
                    Total = total
                };
                return Results.Ok(ret);
            }
            catch (Exception ex)
            {
                Log.Error($"[Visitor][GetBookings] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(VisitorModelCreateResponse), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateBooking(IWebHostEnvironment webHostEnvironment, VisitorModelCreate model)
        {
            try
            {
                string webRoot = webHostEnvironment.WebRootPath;
                FileModelResponse qrcodeImageRes = Utility.GenerateQRCode(model.QrCodeText!, webRoot, Constants.FileStorageLocations.QRCODE_PHOTO_PATH);
                string qrCodeImageUrl = qrcodeImageRes.FileData.FilePath;

                string visitorAccessCode = Utility.GenerateRandomCharacters(7);

                string ret = await VisitorRepo.VisitorBooking(model, visitorAccessCode, qrCodeImageUrl);
                if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                else if (ret == Constants.StatusResponses.ERROR) return Results.BadRequest("Recurring days were not provided");
                else if (ret == Constants.StatusResponses.SUCCESS)
                {
                    VisitorModelCreateResponse response = new()
                    {
                        AccessCode = visitorAccessCode,
                        QRCodeImageUrl = qrcodeImageRes.FileData.FilePath,
                    };
                    return Results.Ok(response);
                }

                else return Results.BadRequest("Failed. Please try again");
            }
            catch (Exception ex)
            {
                Log.Error($"[Visitor][CreateBooking] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public static async Task<IResult> CreateBulkBooking(List<VisitorModelCreate> model)
        //{
        //    try
        //    {
        //        string ret = await VisitorRepo.VisitorBulkBooking(model);
        //        if (ret == Constants.StatusResponses.SUCCESS) return Results.Ok(ret);
        //        else if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
        //        else return Results.BadRequest("Failed. Please try again");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"[Visitor][CreateBulkBooking] -> {ex.Message}");
        //        return Results.BadRequest(ex.Message);
        //    }
        //}


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateBulkBooking(IWebHostEnvironment webHostEnvironment, VisitorModelCreateBulk model)
        {
            try
            {
                string webRoot = webHostEnvironment.WebRootPath;


                string ret = await VisitorRepo.VisitorBulkBooking(model, webRoot);

                if (ret == Constants.StatusResponses.FAILED)
                {
                    return Results.BadRequest("Failed. Please try again");
                }
                else if (ret == Constants.StatusResponses.ERROR)
                {
                    return Results.BadRequest("Recurring days were not provided");
                }
                else if (ret == Constants.StatusResponses.SUCCESS)
                {
                    return Results.Ok("Success");
                }
                else
                {
                    return Results.BadRequest("Recurring days were not provided");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[Visitor][CreateBulkBooking] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> CreateBulkBookingTest(VisitorModelCreateBulk2 model)
        {

            Log.Verbose(JsonConvert.SerializeObject(model));
            Log.Verbose("verbose - Got here");
            try
            {
                var dd = Convert.FromBase64String(model.Visitor);
                var dd2 = Encoding.UTF8.GetString(dd);

                return Results.Ok(dd2);
            }
            catch (Exception ex)
            {
                Log.Error($"[Visitor][CreateBulkBooking] -> {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public static async Task<IResult> ExitBooking(string bookingId)
        {
            try
            {
                string ret = await VisitorRepo.ExitVisitorBooking(bookingId);
                if (ret == Constants.StatusResponses.SUCCESS) return Results.Ok(ret);
                else if (ret == Constants.StatusResponses.FAILED) return Results.BadRequest("Failed. Please try again");
                else return Results.BadRequest("Failed. Please try again");
            }
            catch (Exception ex)
            {
                Log.Error($"[Visitor][ExitBooking] -> {ex.Message}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
