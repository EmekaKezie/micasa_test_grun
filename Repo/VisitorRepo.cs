
using Newtonsoft.Json;
using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;
using TheEstate.Models.VisitorModels;

namespace TheEstate.Repo
{
    public class VisitorRepo : AppDbContext
    {

        public static async Task<List<VisitorModelViewExtension>> GetVisitorBookings(string search, string estateId, string zoneId, string residentId, string isRecurring, string isClearedtoExit, int pageSize, int pageNumber)
        {
            List<VisitorModelViewExtension> data = new();
            try
            {
                List<VisitorModelView> bookings = await GetVisitorBookingsFXN(search, estateId, zoneId, residentId, isRecurring, isClearedtoExit, pageSize, pageNumber);
                bookings?.ForEach(o =>
                {
                    data.Add(new VisitorModelViewExtension
                    {
                        BookingId = o.BookingId,
                        EstateId = o.EstateId,
                        ZoneId = o.ZoneId,
                        EstateName = o.EstateName,
                        ZoneName = o.ZoneName,
                        EstateDesc = o.EstateDesc,
                        BookingTitle = o.BookingTitle,
                        BookingDate = o.BookingDate,
                        ResidentId = o.ResidentId,
                        ResidentFirstname = o.ResidentFirstname,
                        ResidentLastname = o.ResidentLastname,
                        ResidentMobileNo = o.ResidentMobileNo,
                        ResidentEmail = o.ResidentEmail,
                        VisitorFullname = o.VisitorFullname,
                        VisitorMobileNo = o.VisitorMobileNo,
                        VisitorEmail = o.VisitorEmail,
                        IsAccompanied = o.IsAccompanied,
                        VisitorCompanions = o.VisitorCompanions,
                        AccessCode = o.AccessCode,
                        ValidityStartDate = o.ValidityStartDate,
                        ValidityEndDate = o.ValidityEndDate,
                        AccessStartTime = o.AccessStartTime,
                        AccessEndTime = o.AccessEndTime,
                        PassageMode = o.PassageMode,
                        IsRecurring = o.IsRecurring,
                        RecurringDays = RecuringDaysView(o),
                        IsExitClearanceRequired = o.IsExitClearanceRequired,
                        IsClearedToExit = o.IsClearedToExit,
                        isActiveBooking = o.isActiveBooking,
                        QrCodeText = o.QrCodeText,
                        QrCodeImage = o.QrCodeImage
                    });
                });

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<List<VisitorModelViewExtension>> GetVisitorBookingByBookingGroupNo(string bookingGroupNo)
        {
            List<VisitorModelViewExtension> data = new();
            try
            {
                List<VisitorModelView> bookings = await GetVisitorBookingByBookingGroupNoFXN(bookingGroupNo);
                bookings?.ForEach(o =>
                {
                    data.Add(new VisitorModelViewExtension
                    {
                        BookingId = o.BookingId,
                        EstateId = o.EstateId,
                        ZoneId = o.ZoneId,
                        EstateName = o.EstateName,
                        ZoneName = o.ZoneName,
                        EstateDesc = o.EstateDesc,
                        BookingTitle = o.BookingTitle,
                        BookingDate = o.BookingDate,
                        ResidentId = o.ResidentId,
                        ResidentFirstname = o.ResidentFirstname,
                        ResidentLastname = o.ResidentLastname,
                        ResidentMobileNo = o.ResidentMobileNo,
                        ResidentEmail = o.ResidentEmail,
                        VisitorFullname = o.VisitorFullname,
                        VisitorMobileNo = o.VisitorMobileNo,
                        VisitorEmail = o.VisitorEmail,
                        IsAccompanied = o.IsAccompanied,
                        VisitorCompanions = o.VisitorCompanions,
                        AccessCode = o.AccessCode,
                        ValidityStartDate = o.ValidityStartDate,
                        ValidityEndDate = o.ValidityEndDate,
                        AccessStartTime = o.AccessStartTime,
                        AccessEndTime = o.AccessEndTime,
                        PassageMode = o.PassageMode,
                        IsRecurring = o.IsRecurring,
                        RecurringDays = RecuringDaysView(o),
                        IsExitClearanceRequired = o.IsExitClearanceRequired,
                        IsClearedToExit = o.IsClearedToExit,
                        isActiveBooking = o.isActiveBooking,
                        QrCodeText = o.QrCodeText,
                        QrCodeImage = o.QrCodeImage
                    });
                });

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<VisitorModelViewExtension> GetVisitorBookingByAccessCode(string accessCode)
        {
            VisitorModelViewExtension data;
            try
            {
                VisitorModelView o = await GetVisitorBookingByAccessCodeFXN(accessCode);
                data = new VisitorModelViewExtension
                {
                    BookingId = o.BookingId,
                    EstateId = o.EstateId,
                    ZoneId = o.ZoneId,
                    EstateName = o.EstateName,
                    ZoneName = o.ZoneName,
                    EstateDesc = o.EstateDesc,
                    BookingTitle = o.BookingTitle,
                    BookingDate = o.BookingDate,
                    ResidentId = o.ResidentId,
                    ResidentFirstname = o.ResidentFirstname,
                    ResidentLastname = o.ResidentLastname,
                    ResidentMobileNo = o.ResidentMobileNo,
                    ResidentEmail = o.ResidentEmail,
                    VisitorFullname = o.VisitorFullname,
                    VisitorMobileNo = o.VisitorMobileNo,
                    VisitorEmail = o.VisitorEmail,
                    IsAccompanied = o.IsAccompanied,
                    VisitorCompanions = o.VisitorCompanions,
                    AccessCode = o.AccessCode,
                    ValidityStartDate = o.ValidityStartDate,
                    ValidityEndDate = o.ValidityEndDate,
                    AccessStartTime = o.AccessStartTime,
                    AccessEndTime = o.AccessEndTime,
                    PassageMode = o.PassageMode,
                    IsRecurring = o.IsRecurring,
                    RecurringDays = RecuringDaysView(o),
                    IsExitClearanceRequired = o.IsExitClearanceRequired,
                    IsClearedToExit = o.IsClearedToExit,
                    isActiveBooking = o.isActiveBooking,
                    QrCodeText = o.QrCodeText,
                    QrCodeImage = o.QrCodeImage
                };

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<VisitorModelViewExtension> GetVisitorBookingById(string id)
        {
            VisitorModelViewExtension data;
            try
            {
                VisitorModelView o = await GetVisitorBookingByIdFXN(id);

                data = new VisitorModelViewExtension
                {
                    BookingId = o.BookingId,
                    EstateId = o.EstateId,
                    ZoneId = o.ZoneId,
                    EstateName = o.EstateName,
                    ZoneName = o.ZoneName,
                    EstateDesc = o.EstateDesc,
                    BookingTitle = o.BookingTitle,
                    BookingDate = o.BookingDate,
                    ResidentId = o.ResidentId,
                    ResidentFirstname = o.ResidentFirstname,
                    ResidentLastname = o.ResidentLastname,
                    ResidentMobileNo = o.ResidentMobileNo,
                    ResidentEmail = o.ResidentEmail,
                    VisitorFullname = o.VisitorFullname,
                    VisitorMobileNo = o.VisitorMobileNo,
                    VisitorEmail = o.VisitorEmail,
                    IsAccompanied = o.IsAccompanied,
                    VisitorCompanions = o.VisitorCompanions,
                    AccessCode = o.AccessCode,
                    ValidityStartDate = o.ValidityStartDate,
                    ValidityEndDate = o.ValidityEndDate,
                    AccessStartTime = o.AccessStartTime,
                    AccessEndTime = o.AccessEndTime,
                    PassageMode = o.PassageMode,
                    IsRecurring = o.IsRecurring,
                    RecurringDays = RecuringDaysView(o),
                    IsExitClearanceRequired = o.IsExitClearanceRequired,
                    IsClearedToExit = o.IsClearedToExit,
                    isActiveBooking = o.isActiveBooking,
                    QrCodeText = o.QrCodeText,
                    QrCodeImage = o.QrCodeImage
                };
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<int> GetVisitorBookingsCount(string search, string estateId, string zoneId, string residentId, string isRecurring, string isClearedtoExit)
        {
            int data;
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim().ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"%{estateId?.Trim()}%";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"%{zoneId?.Trim()}%";
                residentId = string.IsNullOrEmpty(residentId) ? "%%" : $"%{residentId?.Trim()}%";
                isRecurring = string.IsNullOrEmpty(isRecurring) ? "%%" : $"%{isRecurring?.Trim().ToUpper()}%";
                isClearedtoExit = string.IsNullOrEmpty(isClearedtoExit) ? "%%" : $"%{isClearedtoExit?.Trim().ToUpper()}%";

                string result = await db.SQLSelectAsync(SQL.VisitorBookingsCountQry, new object[] { estateId, zoneId, residentId, search, isRecurring, isClearedtoExit });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> VisitorBooking(VisitorModelCreate model, string? visitorAccessCode = null, string? qrCodeImageUrl = null)
        {
            string data = string.Empty;
            try
            {
                //GenerateQRCode(model.QrCode);

                if (model.IsRecurring!.Equals("Y") && model.RecurringDays!.Count < 1) data = Constants.StatusResponses.ERROR;
                else
                {
                    string bookingId = Guid.NewGuid().ToString();
                    visitorAccessCode ??= Utility.GenerateRandomCharacters(7);

                    bool booking = await CreateBookingFXN(model: model, bookingId: bookingId, accessCode: visitorAccessCode, qrCodeImageUrl: qrCodeImageUrl);

                    if (booking)
                    {
                        if (model.IsRecurring!.Equals("Y"))
                        {
                            int recurringCount = 0;
                            model.RecurringDays?.ForEach(days =>
                            {
                                recurringCount = ReccurringVisitHandler(days, bookingId);
                            });
                            if (recurringCount > 0) data = Constants.StatusResponses.SUCCESS;
                            //else data = Constants.StatusResponses.PARTIAL_SUCCESS;
                        }
                        else data = Constants.StatusResponses.SUCCESS;
                    }
                    else data = Constants.StatusResponses.FAILED;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        //public static async Task<string> VisitorBulkBooking(List<VisitorModelCreate> model)
        //{
        //    string data;
        //    try
        //    {
        //        int a = 0;
        //        List<string> bookingId = new();
        //        model?.ForEach(o =>
        //        {
        //            bookingId.Add(Guid.NewGuid().ToString());
        //        });

        //        bool booking = await CreateBulkBookingFXN(model, bookingId);
        //        if (booking)
        //        {
        //            model?.ForEach(o =>
        //            {
        //                int recurringCount = 0;
        //                if (o.IsRecurring!.Equals("Y"))
        //                {
        //                    o.RecurringDays?.ForEach(days =>
        //                    {
        //                        recurringCount = ReccurringVisitHandler(days, bookingId[a]);
        //                    });
        //                }
        //                a++;
        //            });
        //            data = Constants.StatusResponses.SUCCESS;
        //        }
        //        else data = Constants.StatusResponses.FAILED;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return data;
        //}


        public static async Task<string> VisitorBulkBooking(VisitorModelCreateBulk model, string webRoot)
        {
            string data = string.Empty;
            try
            {
                int a = 0;
                List<string> bookingId = new();
                List<VisitorModelBulkCompanion> visitors = await ConvertBase64ToMultipleVisitor(model.Visitor!);

                visitors?.ForEach(o =>
                {
                    bookingId.Add(Guid.NewGuid().ToString());
                });

                string bookingGroupNo = Guid.NewGuid().ToString();

                bool booking = await CreateBulkBookingFXN(model, webRoot, bookingId, bookingGroupNo);
                if (booking)
                {
                    if (model.IsRecurring!.Equals("Y"))
                    {
                        int recurringCount = 0;
                        model.RecurringDays?.ForEach(days =>
                        {
                            recurringCount = ReccurringVisitHandler(days, bookingId[a]);
                        });
                        if (recurringCount > 0) data = Constants.StatusResponses.SUCCESS;
                        //else data = Constants.StatusResponses.PARTIAL_SUCCESS;
                    }
                    else data = Constants.StatusResponses.SUCCESS;
                }
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> ExitVisitorBooking(string bookingId)
        {
            string data;
            try
            {
                int action = await db.SQLExecuteAsync(SQL.ExitVisitorBookingQry, new object[] { "Y", bookingId });
                if (action > 0) data = Constants.StatusResponses.SUCCESS;
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }










        public static async Task<List<VisitorModelView>> GetVisitorBookingsFXN(string search, string estateId, string zoneId, string residentId, string isRecurring, string isClearedtoExit, int pageSize, int pageNumber)
        {
            List<VisitorModelView> data = new List<VisitorModelView>();
            try
            {
                search = string.IsNullOrEmpty(search) ? "%%" : $"%{search?.Trim().ToUpper()}%";
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"%{estateId?.Trim()}%";
                zoneId = string.IsNullOrEmpty(zoneId) ? "%%" : $"%{zoneId?.Trim()}%";
                residentId = string.IsNullOrEmpty(residentId) ? "%%" : $"%{residentId?.Trim()}%";
                isRecurring = string.IsNullOrEmpty(isRecurring) ? "%%" : $"%{isRecurring?.Trim().ToUpper()}%";
                isClearedtoExit = string.IsNullOrEmpty(isClearedtoExit) ? "%%" : $"%{isClearedtoExit?.Trim().ToUpper()}%";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.VisitorBookingsQry, new object[] { estateId, zoneId, residentId, search, isRecurring, isClearedtoExit, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new VisitorModelView
                        {
                            BookingId = i["booking_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            BookingTitle = i["booking_title"].ToString(),
                            BookingDate = Convert.ToDateTime(i["booking_date"]),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            VisitorFullname = i["visitors_name"].ToString(),
                            VisitorMobileNo = i["visitors_mobileno"].ToString(),
                            VisitorEmail = i["visitors_email"].ToString(),
                            IsAccompanied = i["is_accompanied"].ToString(),
                            VisitorCompanions = i["accompanied_by"].ToString(),
                            AccessCode = i["access_code"].ToString(),
                            ValidityStartDate = Convert.ToDateTime(i["validity_startdate"]),
                            ValidityEndDate = Convert.ToDateTime(i["validity_enddate"]),
                            AccessStartTime = Convert.ToDateTime(i["access_starttime"]),
                            AccessEndTime = Convert.ToDateTime(i["access_endtime"]),
                            PassageMode = i["passage_mode"].ToString(),
                            IsRecurring = i["is_recurring"].ToString(),
                            IsExitClearanceRequired = i["is_exit_clearance_required"].ToString(),
                            IsClearedToExit = i["is_cleared_to_exit"].ToString(),
                            RecurringMonday = i["recurring_monday"].ToString(),
                            RecurringTuesday = i["recurring_tuesday"].ToString(),
                            RecurringWednesday = i["recurring_wednesday"].ToString(),
                            RecurringThursday = i["recurring_thursday"].ToString(),
                            RecurringFriday = i["recurring_friday"].ToString(),
                            RecurringSaturday = i["recurring_saturday"].ToString(),
                            RecurringSunday = i["recurring_sunday"].ToString(),
                            isActiveBooking = Convert.ToDateTime(i["validity_enddate"]) < DateTime.Now ? "N" : "Y",
                            QrCodeText = i["qrcode_text"].ToString(),
                            QrCodeImage = i["qrcode_image"].ToString()
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


        public static async Task<List<VisitorModelView>> GetVisitorBookingByBookingGroupNoFXN(string bookingGroupNo)
        {
            List<VisitorModelView> data = new List<VisitorModelView>();
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.VisitorBookingsByBookingGroupNoQry, new object[] { bookingGroupNo });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new VisitorModelView
                        {
                            BookingId = i["booking_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            BookingTitle = i["booking_title"].ToString(),
                            BookingDate = Convert.ToDateTime(i["booking_date"]),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            VisitorFullname = i["visitors_name"].ToString(),
                            VisitorMobileNo = i["visitors_mobileno"].ToString(),
                            VisitorEmail = i["visitors_email"].ToString(),
                            IsAccompanied = i["is_accompanied"].ToString(),
                            VisitorCompanions = i["accompanied_by"].ToString(),
                            AccessCode = i["access_code"].ToString(),
                            ValidityStartDate = Convert.ToDateTime(i["validity_startdate"]),
                            ValidityEndDate = Convert.ToDateTime(i["validity_enddate"]),
                            AccessStartTime = Convert.ToDateTime(i["access_starttime"]),
                            AccessEndTime = Convert.ToDateTime(i["access_endtime"]),
                            PassageMode = i["passage_mode"].ToString(),
                            IsRecurring = i["is_recurring"].ToString(),
                            IsExitClearanceRequired = i["is_exit_clearance_required"].ToString(),
                            IsClearedToExit = i["is_cleared_to_exit"].ToString(),
                            RecurringMonday = i["recurring_monday"].ToString(),
                            RecurringTuesday = i["recurring_tuesday"].ToString(),
                            RecurringWednesday = i["recurring_wednesday"].ToString(),
                            RecurringThursday = i["recurring_thursday"].ToString(),
                            RecurringFriday = i["recurring_friday"].ToString(),
                            RecurringSaturday = i["recurring_saturday"].ToString(),
                            RecurringSunday = i["recurring_sunday"].ToString(),
                            isActiveBooking = Convert.ToDateTime(i["validity_enddate"]) < DateTime.Now ? "N" : "Y",
                            QrCodeText = i["qrcode_text"].ToString(),
                            QrCodeImage = i["qrcode_image"].ToString()
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


        public static async Task<VisitorModelView> GetVisitorBookingByAccessCodeFXN(string accessCode)
        {
            VisitorModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.VisitorBookingByAccessCodeQry, new object[] { accessCode });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new VisitorModelView
                        {
                            BookingId = i["booking_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            BookingTitle = i["booking_title"].ToString(),
                            BookingDate = Convert.ToDateTime(i["booking_date"]),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            VisitorFullname = i["visitors_name"].ToString(),
                            VisitorMobileNo = i["visitors_mobileno"].ToString(),
                            VisitorEmail = i["visitors_email"].ToString(),
                            IsAccompanied = i["is_accompanied"].ToString(),
                            VisitorCompanions = i["accompanied_by"].ToString(),
                            AccessCode = i["access_code"].ToString(),
                            ValidityStartDate = Convert.ToDateTime(i["validity_startdate"]),
                            ValidityEndDate = Convert.ToDateTime(i["validity_enddate"]),
                            AccessStartTime = Convert.ToDateTime(i["access_starttime"]),
                            AccessEndTime = Convert.ToDateTime(i["access_endtime"]),
                            PassageMode = i["passage_mode"].ToString(),
                            IsRecurring = i["is_recurring"].ToString(),
                            IsExitClearanceRequired = i["is_exit_clearance_required"].ToString(),
                            IsClearedToExit = i["is_cleared_to_exit"].ToString(),
                            RecurringMonday = i["recurring_monday"].ToString(),
                            RecurringTuesday = i["recurring_tuesday"].ToString(),
                            RecurringWednesday = i["recurring_wednesday"].ToString(),
                            RecurringThursday = i["recurring_thursday"].ToString(),
                            RecurringFriday = i["recurring_friday"].ToString(),
                            RecurringSaturday = i["recurring_saturday"].ToString(),
                            RecurringSunday = i["recurring_sunday"].ToString(),
                            isActiveBooking = Convert.ToDateTime(i["validity_enddate"]) < DateTime.Now ? "N" : "Y",
                            QrCodeText = i["qrcode_text"].ToString(),
                            QrCodeImage = i["qrcode_image"].ToString()
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


        public static async Task<VisitorModelView> GetVisitorBookingByIdFXN(string bookingId)
        {
            VisitorModelView? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.VisitorBookingByIdQry, new object[] { bookingId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new VisitorModelView
                        {
                            BookingId = i["booking_id"].ToString(),
                            EstateId = i["estate_id"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            ZoneName = i["zone_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            BookingTitle = i["booking_title"].ToString(),
                            BookingDate = Convert.ToDateTime(i["booking_date"]),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            VisitorFullname = i["visitors_name"].ToString(),
                            VisitorMobileNo = i["visitors_mobileno"].ToString(),
                            VisitorEmail = i["visitors_email"].ToString(),
                            IsAccompanied = i["is_accompanied"].ToString(),
                            VisitorCompanions = i["accompanied_by"].ToString(),
                            AccessCode = i["access_code"].ToString(),
                            ValidityStartDate = Convert.ToDateTime(i["validity_startdate"]),
                            ValidityEndDate = Convert.ToDateTime(i["validity_enddate"]),
                            AccessStartTime = Convert.ToDateTime(i["access_starttime"]),
                            AccessEndTime = Convert.ToDateTime(i["access_endtime"]),
                            PassageMode = i["passage_mode"].ToString(),
                            IsRecurring = i["is_recurring"].ToString(),
                            IsExitClearanceRequired = i["is_exit_clearance_required"].ToString(),
                            IsClearedToExit = i["is_cleared_to_exit"].ToString(),
                            RecurringMonday = i["recurring_monday"].ToString(),
                            RecurringTuesday = i["recurring_tuesday"].ToString(),
                            RecurringWednesday = i["recurring_wednesday"].ToString(),
                            RecurringThursday = i["recurring_thursday"].ToString(),
                            RecurringFriday = i["recurring_friday"].ToString(),
                            RecurringSaturday = i["recurring_saturday"].ToString(),
                            RecurringSunday = i["recurring_sunday"].ToString(),
                            isActiveBooking = Convert.ToDateTime(i["validity_enddate"]) < DateTime.Now ? "N" : "Y",
                            QrCodeText = i["qrcode_text"].ToString(),
                            QrCodeImage = i["qrcode_image"].ToString()
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


        public static async Task<bool> CreateBookingFXN(VisitorModelCreate model, string? bookingId = null, string? bookingGroupNo = null, string? accessCode = null, string? qrCodeImageUrl = null)
        {
            bool data;
            try
            {
                bookingId ??= Guid.NewGuid().ToString();
                accessCode ??= Utility.GenerateRandomCharacters(7);
                bookingGroupNo ??= Guid.NewGuid().ToString();

                DateTime validityStartDate = Convert.ToDateTime(model.ValidityStartDate);
                DateTime validityEndDate = Convert.ToDateTime(model.ValidityEndDate);

                DateTime accessStartTime = Convert.ToDateTime(model.AccessStartTime);
                DateTime accessEndTime = Convert.ToDateTime(model.AccessEndTime);

                object[] param = new object[] { model.EstateId!, model.ZoneId!, bookingId, DateTime.Now, string.IsNullOrEmpty(model.BookingTitle) ? "Visit" : model.BookingTitle!, bookingGroupNo, model.ResidentId!, model.VisitorFullname!, model.VisitorMobileNo!, model.VisitorEmail!, model.IsAccompanied!, model.VisitorCompanions!, accessCode, validityStartDate, validityEndDate, accessStartTime!, accessEndTime!, model.PassageMode!, model.IsRecurring!, model.IsExitClearanceRequired!, "N", model.QrCodeText ?? "", qrCodeImageUrl ?? "" };
                int action = await db.SQLExecuteAsync(SQL.VisitorBookingCreateQry, param);
                if (action > 0) data = true;
                else data = false;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        //public static async Task<bool> CreateBulkBookingFXN(List<VisitorModelCreate> model, List<string>? bookingId = null, string? bookingGroupNo = null, List<string>? accessCode = null)
        //{
        //    bool data;
        //    try
        //    {
        //        bookingGroupNo ??= Guid.NewGuid().ToString();

        //        List<string> sql = new();
        //        List<object> param = new();

        //        int a = 0;

        //        if (bookingId == null)
        //        {
        //            bookingId = new();
        //            model?.ForEach(o =>
        //            {
        //                bookingId!.Add(Guid.NewGuid().ToString());
        //            });

        //        }
        //        if (accessCode == null)
        //        {
        //            accessCode = new();
        //            model?.ForEach(o =>
        //            {
        //                accessCode!.Add(Utility.GenerateRandomCharacters(7));
        //            });
        //        }


        //        model?.ForEach(o =>
        //        {
        //            DateTime validityStartDate = Convert.ToDateTime(o.ValidityStartDate);
        //            DateTime validityEndDate = Convert.ToDateTime(o.ValidityEndDate);

        //            DateTime accessStartTime = Convert.ToDateTime(o.AccessStartTime);
        //            DateTime accessEndTime = Convert.ToDateTime(o.AccessEndTime);

        //            param.Add(new object[] { o.EstateId!, o.ZoneId!, bookingId[a], DateTime.Now, string.IsNullOrEmpty(o.BookingTitle) ? "Visit" : o.BookingTitle!, bookingGroupNo, o.ResidentId!, o.VisitorFullname!, o.VisitorMobileNo!, o.VisitorEmail!, o.IsAccompanied!, o.VisitorCompanions!, accessCode[a], validityStartDate, validityEndDate, accessStartTime!, accessEndTime!, o.PassageMode!, o.IsRecurring!, o.IsExitClearanceRequired!, "N" });
        //            sql.Add(SQL.VisitorBookingCreateQry);
        //            a++;
        //        });

        //        int action = await db.SQLExecuteTransactionsAsync(sql.ToArray(), param.ToArray());
        //        if (action > 0) data = true;
        //        else data = false;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return data;
        //}


        public static async Task<bool> CreateBulkBookingFXN(VisitorModelCreateBulk model, string webRoot, List<string>? bookingId = null, string? bookingGroupNo = null, List<string>? accessCode = null)
        {
            bool data = false;
            try
            {
                bookingGroupNo ??= Guid.NewGuid().ToString();
                List<VisitorModelBulkCompanion> visitors = await ConvertBase64ToMultipleVisitor(model.Visitor!);
                List<string> qrCodeImageUrl = new();

                List<string> sql = new();
                List<object> param = new();

                int a = 0;

                if (bookingId == null)
                {
                    bookingId = new();
                    visitors?.ForEach(o =>
                    {
                        bookingId!.Add(Guid.NewGuid().ToString());
                    });

                }
                if (accessCode == null)
                {
                    accessCode = new();
                    visitors?.ForEach(o =>
                    {
                        accessCode!.Add(Utility.GenerateRandomCharacters(7));
                    });
                }

                visitors?.ForEach(o =>
                {
                    FileModelResponse qrCodeImageRes = Utility.GenerateQRCode(o.QrCodeText!, webRoot, Constants.FileStorageLocations.QRCODE_PHOTO_PATH);
                    qrCodeImageUrl.Add(qrCodeImageRes.FileData.FilePath);
                });


                DateTime validityStartDate = Convert.ToDateTime(model.ValidityStartDate);
                DateTime validityEndDate = Convert.ToDateTime(model.ValidityEndDate);

                DateTime accessStartTime = Convert.ToDateTime(model.AccessStartTime);
                DateTime accessEndTime = Convert.ToDateTime(model.AccessEndTime);


                visitors?.ForEach(o =>
                {
                    param.Add(new object[] { model.EstateId!, model.ZoneId!, bookingId[a], DateTime.Now, string.IsNullOrEmpty(model.BookingTitle) ? "Visit" : model.BookingTitle!, bookingGroupNo, model.ResidentId!, o.VisitorFullname!, o.VisitorMobileNo!, o.VisitorEmail!, o.IsAccompanied!, o.VisitorCompanions!, accessCode[a], validityStartDate, validityEndDate, accessStartTime!, accessEndTime!, model.PassageMode!, model.IsRecurring!, model.IsExitClearanceRequired!, "N", o.QrCodeText, qrCodeImageUrl[a] });
                    sql.Add(SQL.VisitorBookingCreateQry);
                    a++;
                });

                int action = await db.SQLExecuteTransactionsAsync(sql.ToArray(), param.ToArray());
                if (action > 0) data = true;
                else data = false;

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<List<VisitorModelBulkCompanion>> ConvertBase64ToMultipleVisitor(string base64)
        {
            List<VisitorModelBulkCompanion> data = new();
            try
            {
                //byte[] bytes = Convert.FromBase64String(base64);
                //string jsonStr = Encoding.UTF8.GetString(bytes);
                string jsonStr = Utility.Base64Decoder(base64);
                data = JsonConvert.DeserializeObject<List<VisitorModelBulkCompanion>>(jsonStr!)!;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static bool CreateBookingRecurringFXN(string sql, string status, string bookingId)
        {
            bool data;
            try
            {
                int action = db.SQLExecute(sql, new object[] { status, bookingId });
                if (action > 0) data = true;
                else data = false;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static int ReccurringVisitHandler(string recurringDays, string bookingId)
        {
            int count = 0;
            try
            {
                switch (recurringDays)
                {
                    case Constants.VisitingDays.MONDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringMondayQry, "Y", bookingId)) count += 1;
                        break;
                    case Constants.VisitingDays.TUESDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringTuesdayQry, "Y", bookingId)) count += 1;
                        break;
                    case Constants.VisitingDays.WEDNESDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringWednesdayQry, "Y", bookingId)) count += 1;
                        break;
                    case Constants.VisitingDays.THURSDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringThursdayQry, "Y", bookingId)) count += 1;
                        break;
                    case Constants.VisitingDays.FRIDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringFridayQry, "Y", bookingId)) count += 1;
                        break;
                    case Constants.VisitingDays.SATURDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringSaturdayQry, "Y", bookingId)) count += 1;
                        break;
                    case Constants.VisitingDays.SUNDAY:
                        if (CreateBookingRecurringFXN(SQL.VisitorBookingRecurringSundayQry, "Y", bookingId)) count += 1;
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return count;
        }


        public static List<VisitorCompanionModel> VisitorCompanionSerializer(string jsonStr)
        {
            List<VisitorCompanionModel> data = new();
            try
            {
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    data = JsonConvert.DeserializeObject<List<VisitorCompanionModel>>(jsonStr)!;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static List<string> RecuringDaysView(VisitorModelView o)
        {
            List<string> data = new();
            try
            {
                if (o.RecurringMonday!.Equals("Y")) data.Add(Constants.VisitingDays.MONDAY);
                if (o.RecurringTuesday!.Equals("Y")) data.Add(Constants.VisitingDays.TUESDAY);
                if (o.RecurringWednesday!.Equals("Y")) data.Add(Constants.VisitingDays.WEDNESDAY);
                if (o.RecurringThursday!.Equals("Y")) data.Add(Constants.VisitingDays.THURSDAY);
                if (o.RecurringFriday!.Equals("Y")) data.Add(Constants.VisitingDays.FRIDAY);
                if (o.RecurringSaturday!.Equals("Y")) data.Add(Constants.VisitingDays.SATURDAY);
                if (o.RecurringSunday!.Equals("Y")) data.Add(Constants.VisitingDays.SUNDAY);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }




    }
}
