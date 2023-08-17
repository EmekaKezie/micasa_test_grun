using CommonFactory;
using System.Data;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;
using TheEstate.Models.AuthModels;
using TheEstate.Models.ProfileModels;
using TheEstate.Models.ResidentModels;
using TheEstate.RepoUtil;
using static System.Net.WebRequestMethods;

namespace TheEstate.Repo
{
    public class ProfileRepo : AppDbContext
    {

        public static async Task<AuthModel> GetProfileById(string profileId)
        {
            AuthModel? data = null;

            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ProfileByIdQry, new object[] { profileId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new AuthModel
                        {
                            Id = i["profile_id"].ToString(),
                            Username = i["username"].ToString(),
                            LoginMethod = i["login_method"].ToString(),
                            Email = i["emailaddress"].ToString(),
                            MobileNo = i["mobileno"].ToString(),
                            Firstname = i["firstname"].ToString(),
                            Lastname = i["lastname"].ToString(),
                            Status = i["status"].ToString(),
                            ImageUrl = i["image_url"].ToString(),
                            OnboadingStage = i["onboarding_stage"].ToString(),
                            Residency = await ResidentRepo.GetResidentByProfileId(i["profile_id"].ToString()!)

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


        public static async Task<AuthModel> GetProfileByUsername(string username)
        {
            AuthModel? data = null;

            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ProfileByUsernameQry, new object[] { username });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new AuthModel
                        {
                            Id = i["profile_id"].ToString(),
                            Username = i["username"].ToString(),
                            LoginMethod = i["login_method"].ToString(),
                            Email = i["emailaddress"].ToString(),
                            MobileNo = i["mobileno"].ToString(),
                            Firstname = i["firstname"].ToString(),
                            Lastname = i["lastname"].ToString(),
                            Status = i["status"].ToString(),
                            ImageUrl = i["image_url"].ToString(),
                            OnboadingStage = i["onboarding_stage"].ToString(),
                            Residency = await ResidentRepo.GetResidentByProfileId(i["profile_id"].ToString()!)
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


        public static async Task<string> ProfileSetup(ProfileModelSetup model)
        {
            string data = string.Empty;
            try
            {
                int action = await db.SQLExecuteAsync(SQL.ProfileSetupQry, new object[] { model.Email, model.MobileNo, model.Firstname, model.Lastname, model.ImageUrl, "", "", 0, Constants.OnboardingState.COMPLETE.ToString(), model.ProfileId });
                if (action > 0) data = Constants.StatusResponses.SUCCESS;
                else data = Constants.StatusResponses.FAILED;
            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }


        public static async Task<string> AddEstate(AddEstateModel model)
        {
            string data = string.Empty;
            try
            {
                ResidentModelView resident = await ResidentRepo.GetResidentByResidenCode(model.ResidentCode);
                if (resident != null)
                {
                    //if (resident.ResidentEmail!.Equals(resident.ProfileEmail) || resident.ResidentMobileNo!.Equals(resident.ProfileMobileNo))
                    //{
                    //    //update resident table with the profile id, no need of otp
                    //    int action = await db.SQLExecuteAsync(SQL.AddProfileToResidentQry, new object[] { Constants.ActiviyStatus.ACTIVE.ToString(), model.ProfileId, DateTime.Now, model.IsDefault ? "Y" : "N", model.ResidentCode });
                    //    if (action > 0) data = Constants.StatusResponses.SUCCESS;
                    //    else data = Constants.StatusResponses.FAILED;
                    //}
                    //else
                    //{
                    string otp = Utility.GenerateRandomCharacters(4);
                    int action = await db.SQLExecuteAsync(SQL.ResidentOnboardingOptQry, new object[] { model.ProfileId, otp, DateTime.Now, model.ResidentCode });
                    if (action > 0)
                    {
                        TemplateModel tmpl = await TemplateManager.Email_ResidentVerification(otp);
                        bool sendMail = await NotificationManager.SendEmail(resident.ResidentEmail, tmpl.Subject!, tmpl.EmailContent!);
                    }

                    data = Constants.StatusResponses.SUCCESS;
                    //}
                }
                else data = Constants.StatusResponses.NOTFOUND;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<string> VerifyResidentOtp(string residentCode, string otp)
        {
            string data = string.Empty;

            try
            {
                OtpModel otpData = await GetResidentOtpByResidentCode(residentCode);
                if (otpData != null)
                {
                    DateTime letency = otpData.OtpDate.AddMinutes(30);
                    if (DateTime.Now > letency || otpData.Otp == "") data = Constants.StatusResponses.EXPIRED;
                    else
                    {
                        if (otpData.Otp!.Equals(otp))
                        {
                            int action = await db.SQLExecuteAsync(SQL.ResidentOnboardVerifyOtpQry, new object[] { Constants.ActiviyStatus.ACTIVE.ToString(), DateTime.Now, "", residentCode });
                            if (action > 0) data = Constants.StatusResponses.SUCCESS;
                            else data = Constants.StatusResponses.FAILED;
                        }
                        else data = Constants.StatusResponses.INVALID;
                    }
                }
                else data = Constants.StatusResponses.INVALID;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }







        public static async Task<OtpModel> GetResidentOtpByResidentCode(string residentCode)
        {
            OtpModel? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.ResidnetOtpByResidenCodeQry, new[] { residentCode });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new OtpModel
                        {
                            Otp = i["otp"].ToString(),
                            OtpDate = Convert.ToDateTime(i["otp_timestamp"])
                        };
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }
    }
}
