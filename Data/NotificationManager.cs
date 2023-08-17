using Serilog;
using System.Net.Mail;
using System.Net;
using TheEstate.Models.AppModels;
using CommonFactory;
using Newtonsoft.Json;
using System.Text;

namespace TheEstate.Data
{
    public class NotificationManager
    {
        public static async Task<bool> SendEmail(string email, string subject, string message)
        {
            bool ret = false;

            try
            {
                SmtpModel smtp = await Utility.SmtpParameters();

                bool isValidEmail = XGeneral.ValidateEmailAddress(email);

                if (isValidEmail)
                {
                    if (smtp != null)
                    {
                        using (SmtpClient client = new(smtp.Host, smtp.Port))
                        {
                            MailMessage newMail = new MailMessage();
                            newMail.To.Add(new MailAddress(email));

                            newMail.From = new MailAddress(smtp.Sender!, smtp.DisplayName);
                            newMail.Subject = subject;
                            newMail.Body = message;
                            newMail.IsBodyHtml = true;
                            newMail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html")));

                            client.UseDefaultCredentials = false;
                            newMail.SubjectEncoding = System.Text.Encoding.UTF8;
                            client.EnableSsl = smtp.UseSsl;
                            client.Credentials = new NetworkCredential(smtp.Username, smtp.Password);
                            client.Port = smtp.Port;


                            try
                            {
                                client.Send(newMail);
                                ret = true;
                            }
                            catch (SmtpException ex)
                            {
                                Log.Error($"Sending Welcome Email to {email} -> [{ex.Message}] [{ex.InnerException?.Message}]");
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Sending Welcome Email to {email} -> [{ex.Message}] [{ex.InnerException?.Message}]");
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        public static async Task<bool> SendSms(string phoneNumber, string message, bool isFlash = false)
        {
            bool data = false;
            try
            {
                SmsModel smsModel = await Utility.SmsParameters();
                if (smsModel != null)
                {
                    HttpClient client = new();
                    var param = new
                    {
                        User = smsModel.Username,
                        Pass = smsModel.Password,
                        Sender = smsModel.SenderId,
                        Message = message,
                        Mobile = phoneNumber,
                        Flash = Convert.ToInt32(isFlash)
                    };
                    string stringifyParam = JsonConvert.SerializeObject(param);

                    HttpContent content = new StringContent(stringifyParam, Encoding.UTF8, "application/json"); ;
                    HttpResponseMessage response = await client.PostAsync(smsModel.Url, content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        data = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}
