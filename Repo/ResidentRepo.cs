using CommonFactory;
using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;
using TheEstate.Models.AuthModels;
using TheEstate.Models.ResidentModels;

namespace TheEstate.Repo
{
    public class ResidentRepo : AppDbContext
    {
        public static async Task<bool> CheckExistsResidentCode(string residenCode)
        {
            bool data = false;
            try
            {
                string result = await db.SQLSelectAsync(SQL.CheckExistsResidentCode, new[] { residenCode });
                int count = Convert.ToInt32(result);
                if (count > 0) data = true;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<List<ResidentModelView>> GetResidents(string estateId, string householdId, string residentId, string residentCode, int pageSize, int pageNumber)
        {
            List<ResidentModelView> data = new List<ResidentModelView>();
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                householdId = string.IsNullOrEmpty(householdId) ? "%%" : $"{householdId?.Trim()}";
                residentId = string.IsNullOrEmpty(residentId) ? "%%" : $"{residentId?.Trim()}";
                residentCode = string.IsNullOrEmpty(residentCode) ? "%%" : $"{residentCode?.Trim()}";
                pageSize = pageSize < 1 ? 1 : pageSize;
                pageNumber = (pageNumber - 1) * pageSize;

                DataTable dt = await db.SQLFetchAsync(SQL.ResidencyQry, new object[] { estateId, householdId, residentId, residentCode, pageSize, pageNumber });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new ResidentModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateImage = i["image_url"].ToString(),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentCategory = i["resident_category"].ToString(),
                            ResidentCode = i["resident_code"].ToString(),
                            ResidentPhoto = i["profilephoto"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            ResidentStatus = i["status"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            HouseHoldId = i["household_id"].ToString(),
                            HouseHoldLabel = i["household_label"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
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


        public static async Task<int> GetResidentsCount(string estateId, string householdId, string residentId, string residentCode)
        {
            int data;
            try
            {
                estateId = string.IsNullOrEmpty(estateId) ? "%%" : $"{estateId?.Trim()}";
                householdId = string.IsNullOrEmpty(householdId) ? "%%" : $"{householdId?.Trim()}";
                residentId = string.IsNullOrEmpty(residentId) ? "%%" : $"{residentId?.Trim()}";
                residentCode = string.IsNullOrEmpty(residentCode) ? "%%" : $"{residentCode?.Trim()}";

                string result = await db.SQLSelectAsync(SQL.ResidencyQryQryCount, new object[] { estateId, householdId, residentId, residentCode });
                data = Convert.ToInt32(result);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<List<ResidentModelView>> GetResidentByProfileId(string profileId)
        {
            List<ResidentModelView> data = new List<ResidentModelView>();
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ResidentByProfileId, new[] { profileId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new ResidentModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateImage = i["image_url"].ToString(),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentCategory = i["resident_category"].ToString(),
                            ResidentCode = i["resident_code"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            ResidentStatus = i["status"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            HouseHoldId = i["household_id"].ToString(),
                            HouseHoldLabel = i["household_label"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
                            ZoneId = i["zone_id"].ToString(),
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





        public static async Task<List<ResidentModelView>> GetResidentByHouseholdId(string householdId)
        {
            List<ResidentModelView> data = new List<ResidentModelView>();
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ResidentByHouseholdId, new[] { householdId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new ResidentModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateImage = i["image_url"].ToString(),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentCategory = i["resident_category"].ToString(),
                            ResidentCode = i["resident_code"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            ResidentStatus = i["status"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            HouseHoldId = i["household_id"].ToString(),
                            HouseHoldLabel = i["household_label"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
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


        public static async Task<ResidentModelView> GetResidentByResidenCode(string residentCode)
        {
            ResidentModelView data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ResidentByResidentCodeQry, new[] { residentCode });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new ResidentModelView
                        {
                            EstateId = i["estate_id"].ToString(),
                            EstateName = i["estate_name"].ToString(),
                            EstateDesc = i["estate_desc"].ToString(),
                            EstateImage = i["image_url"].ToString(),
                            ResidentId = i["resident_id"].ToString(),
                            ResidentCategory = i["resident_category"].ToString(),
                            ResidentCode = i["resident_code"].ToString(),
                            StreetId = i["street_id"].ToString(),
                            StreetName = i["street_name"].ToString(),
                            ResidentStatus = i["status"].ToString(),
                            ResidentEmail = i["resident_emailaddress"].ToString(),
                            ResidentMobileNo = i["resident_mobileno"].ToString(),
                            ResidentFirstname = i["resident_firstname"].ToString(),
                            ResidentLastname = i["resident_lastname"].ToString(),
                            HouseHoldId = i["household_id"].ToString(),
                            HouseHoldLabel = i["household_label"].ToString(),
                            PropertyId = i["property_id"].ToString(),
                            PropertyNo = i["property_no"].ToString(),
                            PropertyType = i["property_type"].ToString(),
                            PropertyCategory = i["property_category"].ToString(),
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


        public static async Task<string> ResidentRegistration(ResidentModelCreate model)
        {
            string data;
            try
            {
                bool action = await CreateResidentFXN(model: model, residentCategory: Constants.ResidentCategories.PRIMARY.ToString(), residentStatus: Constants.ActiviyStatus.PENDING.ToString(), residentProfileId: model.ProfileId!, otp: "", otpTimestamp: Utility.NullDate());
                if (action) data = Constants.StatusResponses.SUCCESS;
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> InviteResident(ResidentModelCreate model)
        {
            string data;
            try
            {
                bool isvalidEmail = false;
                if (!string.IsNullOrEmpty(model.ResidentEmail)) isvalidEmail = XGeneral.ValidateEmailAddress(model.ResidentEmail);
                else isvalidEmail = true;
                
                if (isvalidEmail)
                {
                    bool action = false;
                    string residentCode = Utility.GenerateRandomCharacters(8);
                    if (!string.IsNullOrEmpty(model.ResidentUsername))
                    {
                        AuthModel profile = await ProfileRepo.GetProfileByUsername(model.ResidentUsername);
                        if (profile != null)
                        {
                            ResidentModelCreate createResident = new()
                            {
                                EstateId = model.EstateId,
                                HouseHoldId = model.HouseHoldId,
                                ProfileId = model.ProfileId,
                                ResidentEmail = profile.Email,
                                ResidentFirstname = profile.Firstname,
                                ResidentLastname = profile.Lastname,
                                ResidentMobileNo = profile.MobileNo,
                                ResidentUsername = profile.Username,
                            };

                            action = await CreateResidentFXN(model: createResident, residentCategory: Constants.ResidentCategories.SECONDARY.ToString(), residentStatus: Constants.ActiviyStatus.ACTIVE.ToString(), residentProfileId: profile.Id!, residentCode: residentCode, otp: "", otpTimestamp: Utility.NullDate());
                            if (action) data = Constants.StatusResponses.SUCCESS;
                            else data = Constants.StatusResponses.FAILED;
                        }
                        else data = Constants.StatusResponses.INVALID; //invalid username
                    }
                    else
                    {
                        string profileId = "";
                        string otp = Utility.GenerateRandomCharacters(4);
                        action = await CreateResidentFXN(model: model, residentCategory: Constants.ResidentCategories.SECONDARY.ToString(), residentStatus: Constants.ActiviyStatus.PENDING.ToString(), residentProfileId: profileId, residentCode: residentCode, otp: otp, otpTimestamp: DateTime.Now);
                        if (action)
                        {
                            TemplateModel tmpl = await TemplateManager.Email_ResidentInvite(otp, residentCode);
                            bool sendMail = await NotificationManager.SendEmail(model.ResidentEmail!, tmpl.Subject!, tmpl.EmailContent!);
                            data = Constants.StatusResponses.SUCCESS;
                        }
                        else data = Constants.StatusResponses.FAILED;
                    }
                }
                else data = Constants.StatusResponses.INVALID_FORMAT;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }










        public static async Task<bool> CreateResidentFXN(ResidentModelCreate model, string residentCategory, string residentStatus, string residentProfileId, string otp, DateTime otpTimestamp, string? residentId = null, string? residentCode = null)
        {
            bool data = false;
            try
            {
                residentId ??= Guid.NewGuid().ToString();
                residentCode ??= Utility.GenerateRandomCharacters(8);

                object[] param = new object[] { model.EstateId!, model.HouseHoldId!, residentId, residentCategory, residentCode, model.ResidentFirstname!, model.ResidentLastname!, model.ResidentMobileNo!, model.ResidentEmail!, residentStatus, residentProfileId, DateTime.Now, model.ProfileId!, otp, otpTimestamp };
                int action = await db.SQLExecuteAsync(SQL.ResidentCreateQry, param);
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
