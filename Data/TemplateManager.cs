using System.Data;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;

namespace TheEstate.Data
{
    public class TemplateManager : AppDbContext
    {
        public static async Task<TemplateModel> GetTemplateById(string id)
        {
            TemplateModel? data = null;

            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.TemplateByIdQry, new[] { id });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new TemplateModel
                        {
                            TemplateId = i["template_id"].ToString(),
                            TemplateDesc = i["template_desc"].ToString(),
                            SmsContent = i["sms_content"].ToString(),
                            EmailContent = i["email_content"].ToString(),
                            Url = i["template_url"].ToString(),
                            Subject = i["email_subject"].ToString(),
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

        public static async Task<TemplateModel> Email_AccountVerification(string otp)
        {
            TemplateModel? data = null;
            try
            {
                TemplateModel tmpl = await GetTemplateById("ACCOUNT_VERIFICATION");
                string tmplBody = tmpl.EmailContent!.Replace("#OTP#", otp);
                data = new TemplateModel
                {
                    EmailContent = tmplBody,
                    Subject = tmpl.Subject,
                };
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }

        public static async Task<TemplateModel> Sms_AccountVerification(string otp)
        {
            TemplateModel? data = null;
            try
            {
                TemplateModel tmpl = await GetTemplateById("ACCOUNT_VERIFICATION");
                string tmplBody = tmpl.SmsContent!.Replace("#OTP#", otp);
                data = new TemplateModel
                {
                    SmsContent = tmplBody,
                    Subject = tmpl.Subject,
                };
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }


        public static async Task<TemplateModel> Email_ResidentVerification(string otp)
        {
            TemplateModel? data = null;
            try
            {
                TemplateModel tmpl = await GetTemplateById("RESIDENT_VERIFICATION");
                string tmplBody = tmpl.EmailContent!.Replace("#OTP#", otp);
                data = new TemplateModel
                {
                    EmailContent = tmplBody,
                    Subject = tmpl.Subject,
                };
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }

        public static async Task<TemplateModel> Sms_ResidentVerification(string otp)
        {
            TemplateModel? data = null;
            try
            {
                TemplateModel tmpl = await GetTemplateById("RESIDENT_VERIFICATION");
                string tmplBody = tmpl.SmsContent!.Replace("#OTP#", otp);
                data = new TemplateModel
                {
                    SmsContent = tmplBody,
                    Subject = tmpl.Subject,
                };
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }


        public static async Task<TemplateModel> Email_ResidentInvite(string otp, string residentCode)
        {
            TemplateModel? data = null;
            try
            {
                TemplateModel tmpl = await GetTemplateById("RESIDENT_INVITE");
                string tmplBody = tmpl.EmailContent!.Replace("#OTP#", otp)
                                                    .Replace("#RESIDENTCODE#", residentCode);
                data = new TemplateModel
                {
                    EmailContent = tmplBody,
                    Subject = tmpl.Subject,
                };
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }

    }
}
