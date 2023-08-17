using CommonFactory;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using TheEstate.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;
using TheEstate.Models.AuthModels;

namespace TheEstate.Repo
{
    public class AuthRepo : AppDbContext
    {

        public static async Task<string> CreateUser(AuthModelLogin model)
        {
            string data;
            try
            {
                bool exists = await CheckExistsUsername(model.Username!);

                if (!exists)
                {
                    string loginType = string.Empty;
                    bool isValidFormat = ValidateUsername(model.Username!);

                    if (isValidFormat)
                    {
                        bool isValidEmail = XGeneral.ValidateEmailAddress(model.Username);
                        bool isValidPhone = ValidatePhoneNumber(model.Username);

                        if (isValidEmail) loginType = Constants.LoginTypes.EMAIL.ToString();
                        if (isValidPhone) loginType = Constants.LoginTypes.MOBILENO.ToString();

                        if(isValidEmail || isValidPhone)
                        {
                            string id = Guid.NewGuid().ToString();
                            string otp = Utility.GenerateRandomCharacters(4);

                            object[] param = new object[] { id, model.Username!, model.Password, loginType, DateTime.Now, Constants.ActiviyStatus.PENDING.ToString(), otp, DateTime.Now, Constants.OnboardingState.ACOUNT_VERIFICATION.ToString() };
                            int action = await db.SQLExecuteAsync(SQL.CreateUserQry, param);

                            if (action > 0)
                            {
                                if (loginType == Constants.LoginTypes.EMAIL.ToString())
                                {
                                    TemplateModel tmpl = await TemplateManager.Email_AccountVerification(otp);
                                    bool sendMail = await NotificationManager.SendEmail(model.Username!, tmpl.Subject!, tmpl.EmailContent!);
                                }
                                if(loginType == Constants.LoginTypes.MOBILENO.ToString())
                                {
                                    TemplateModel tmpl = await TemplateManager.Sms_AccountVerification(otp);
                                    bool sendMessage = await NotificationManager.SendSms(model.Username, tmpl.SmsContent!);
                                }
                                data = id;
                            }
                            else data = Constants.StatusResponses.FAILED;
                        }
                        else data = Constants.StatusResponses.FAILED;
                    }
                    else data = Constants.StatusResponses.FAILED;
                }
                else data = Constants.StatusResponses.EXISTS;
                
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<AuthModel> Login(AuthModelLogin model)
        {
            AuthModel? data = null;

            try
            {
                AuthUserModel authUser = await AuthUser(model.Username!);
                if (authUser != null && authUser.Password!.Equals(model.Password))
                {
                    data = new AuthModel
                    {
                        Id = authUser.Id,
                        Username = authUser.Username,
                        LoginMethod = authUser.LoginMethod,
                        Email = authUser.Email,
                        MobileNo = authUser.MobileNo,
                        Firstname = authUser.Firstname,
                        Lastname = authUser.Lastname,
                        Status = authUser.Status,
                        OnboadingStage = authUser.OnboadingStage,
                        ImageUrl = authUser.ImageUrl,
                        Residency = authUser.Residency,
                    };
                }
            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }


        public static async Task<string> VerifyOtp(string profileId, string otp)
        {
            string data = string.Empty;

            try
            {
                OtpModel otpData = await GetOtpByProfileId(profileId);
                if (otpData != null)
                {
                    DateTime letency = otpData.OtpDate.AddMinutes(30);
                    if (DateTime.Now > letency) data = Constants.StatusResponses.EXPIRED;
                    else
                    {
                        if (otpData.Otp!.Equals(otp))
                        {
                            int action = await db.SQLExecuteAsync(SQL.VerifyOtp, new[] { Constants.ActiviyStatus.ACTIVE.ToString(), "", Constants.OnboardingState.PROFILE_SETUP.ToString(), profileId });
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


        public static async Task<string> ResendOtp(string profileId)
        {
            string data = string.Empty;
            try
            {
                AuthModel profile = await ProfileRepo.GetProfileById(profileId);
                if (profile != null)
                {
                    string otp = Utility.GenerateRandomCharacters(4);
                    bool reset = await ResetOtp(profileId, otp);
                    if (reset)
                    {
                        if (profile.LoginMethod!.Equals(Constants.LoginTypes.EMAIL.ToString()))
                        {
                            TemplateModel tmpl = await TemplateManager.Email_AccountVerification(otp);
                            bool sendMail = await NotificationManager.SendEmail(profile.Username!, tmpl.Subject!, tmpl.EmailContent!);
                            data = Constants.StatusResponses.SUCCESS;
                        }
                    }
                    else data = Constants.StatusResponses.FAILED;
                }
                else data = Constants.StatusResponses.NOTFOUND;

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }










        public static bool ValidateUsername(string username)
        {
            bool data = false;
            try
            {
                bool hasUnwantedChars = username.IndexOfAny(new char[] { '*', '&', '#', '!', '$', '%', '^' }) != -1;
                bool hasWhiteSpace = username.Contains(" ");

                if (hasUnwantedChars) data = false;
                else if (hasWhiteSpace) data = false;
                else data = true;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            bool data;

            try
            {
                Regex regex = new Regex("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");
                if (regex.IsMatch(phoneNumber)) data = true; 
                else data = false;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<AuthUserModel> AuthUser(string username)
        {
            AuthUserModel? data = null;

            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.AuthUserQry, new object[] { username });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new AuthUserModel
                        {
                            Id = i["profile_id"].ToString(),
                            Username = i["username"].ToString(),
                            Password = i["password"].ToString(),
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


        public static async Task<bool> CheckExistsUsername(string username)
        {
            bool data = false;
            try
            {
                string result = await db.SQLSelectAsync(SQL.CheckExistsUsernameQry, new[] { username });
                int count = Convert.ToInt32(result);
                if (count > 0) data = true;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<OtpModel> GetOtpByProfileId(string profileId)
        {
            OtpModel? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.OtpByProfileIdQry, new[] { profileId });
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


        public static async Task<bool> ActivateDeactivateUserProfile(string status, string profileId)
        {
            bool data = false;
            try
            {
                int action = await db.SQLExecuteAsync(SQL.VerifyOtp, new[] { status, profileId });
                if (action > 0) data = true;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static async Task<bool> ResetOtp(string profileId, string otp)
        {
            bool data = false;
            try
            {
                int action = await db.SQLExecuteAsync(SQL.ResetOtpQry, new object[] { otp, DateTime.Now, profileId });
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
