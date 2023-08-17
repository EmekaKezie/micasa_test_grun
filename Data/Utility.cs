using Newtonsoft.Json;
using QRCoder;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using System.Text;
using TheEstate.Data.Database;
using TheEstate.Models.AppModels;
using TheEstate.Models.VisitorModels;

namespace TheEstate.Data
{
    public class Utility : AppDbContext
    {
        static readonly string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

        public static string ReadFromFile(string dir, string file)
        {
            string ret = string.Empty;
            try
            {
                string dirPath = Path.Combine(AppContext.BaseDirectory, dir);
                string filePath = Path.Combine(AppContext.BaseDirectory, dir, file);
                bool exists = Directory.Exists(dirPath) && File.Exists(filePath);

                if (exists)
                {
                    ret = File.ReadAllText(filePath);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }


        public static T ConvertFileContentToJson<T>(string path, string file)
        {
            T? data = default;

            try
            {
                string fileContent = Utility.ReadFromFile(path, file);

                if (fileContent != null)
                {
                    data = JsonConvert.DeserializeObject<T>(fileContent);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return data!;
        }


        public static string GenerateRandomCharacters(int length = 10)
        {
            try
            {
                Random random = new Random();

                char[] chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[random.Next(0, validChars.Length)];
                }
                return new string(chars);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public static async Task<List<SettingModel>> GetSettingByGroup(string settingGrp)
        {
            List<SettingModel> data = new List<SettingModel>();
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.SettingByGroupQry, new[] { settingGrp });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data.Add(new SettingModel
                        {
                            SettingId = i["setting_id"].ToString(),
                            SettingValue = i["setting_value"].ToString(),
                            SettingGroup = i["setting_group"].ToString(),
                            SettingDesc = i["setting_desc"].ToString(),
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


        public static async Task<SettingModel> GetSettingById(string settingId)
        {
            SettingModel? data = null;
            try
            {
                DataTable dt = await db.SQLFetchAsync(SQL.SettingByGroupQry, new object[] { settingId });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        data = new SettingModel
                        {
                            SettingId = i["setting_id"].ToString(),
                            SettingValue = i["setting_value"].ToString(),
                            SettingGroup = i["setting_group"].ToString(),
                            SettingDesc = i["setting_desc"].ToString(),
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


        public static async Task<SmtpModel> SmtpParameters()
        {
            SmtpModel? data = null;
            try
            {
                List<SettingModel> smtp = await GetSettingByGroup(Constants.SettingGroups.SMTP);

                if (smtp.Count > 0)
                {
                    string sender = smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.SENDER)).Select(x => x.SettingValue).FirstOrDefault()!;
                    string username = smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.SENDER)).Select(x => x.SettingValue).FirstOrDefault()!;
                    int port = Convert.ToInt32(smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.PORT)).Select(x => x.SettingValue).FirstOrDefault()!);
                    bool useSsl = Convert.ToBoolean(smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.ENABLE_SSL)).Select(x => x.SettingValue).FirstOrDefault()!);
                    string host = smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.HOST)).Select(x => x.SettingValue).FirstOrDefault()!;
                    string password = smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.PASSWORD)).Select(x => x.SettingValue).FirstOrDefault()!;
                    string displayName = smtp.Where(x => x.SettingId!.Equals(Constants.SettingIds.SMTP_DISPLAY_NAME)).Select(x => x.SettingValue).FirstOrDefault()!;

                    data = new SmtpModel
                    {
                        Host = host,
                        Sender = sender,
                        Password = password,
                        Port = port,
                        UseSsl = useSsl,
                        Username = username,
                        DisplayName = displayName
                    };
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }


        public static async Task<SmsModel> SmsParameters()
        {
            SmsModel? data = null;
            try
            {
                List<SettingModel> sms = await GetSettingByGroup(Constants.SettingGroups.SMS);
                if(sms.Count > 0)
                {
                    string url = sms.Where(x => x.SettingId!.Equals(Constants.SettingIds.SMS_GATEWAY_URL)).Select(x => x.SettingValue).FirstOrDefault()!;
                    string username = sms.Where(x => x.SettingId!.Equals(Constants.SettingIds.SMS_GATEWAY_USERNAME)).Select(x => x.SettingValue).FirstOrDefault()!;
                    string password = sms.Where(x => x.SettingId!.Equals(Constants.SettingIds.SMS_GATEWAY_PASSWORD)).Select(x => x.SettingValue).FirstOrDefault()!;
                    string senderId = sms.Where(x => x.SettingId!.Equals(Constants.SettingIds.SMS_GATEWAY_SENDERID)).Select(x => x.SettingValue).FirstOrDefault()!;

                    data = new()
                    {
                        Url = url,
                        Username = username,
                        Password = password,
                        SenderId = senderId,
                    };
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static bool CheckFileContentType(string fileMime)
        {
            bool data;
            try
            {
                bool accept = false;

                string[] mimes = Constants.FileMime.Mimes;

                foreach (string a in mimes)
                {
                    if (fileMime == a)
                    {
                        accept = true;
                        break;
                    }
                }
                data = accept;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static bool CheckFileExists(IFormFile file)
        {
            bool data;
            try
            {
                data = file != null && file?.Length > 0;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static FileModelResponse FileUploader(string webRoot, string path, string base64Str)
        {
            FileModelResponse? data = null;
            try
            {
                if (!string.IsNullOrEmpty(base64Str))
                {
                    string[] mimesWordDoc = Constants.FileMime.MimeWordDocReplacer;
                    string fileMime = base64Str.Split(';')[0].Split(':')[1].Replace('@', ' ').Trim();
                    if (fileMime == mimesWordDoc[0]) fileMime = mimesWordDoc[1];


                    string extension = $".{fileMime.Split('/')[1]}";
                    string name = $"{DateTime.Now.Ticks}";
                    string fileName = $"{name}{extension}";
                    base64Str = base64Str.Split(',')[1];
                    byte[] bytes = Convert.FromBase64String(base64Str);
                    int fileSize = Convert.ToInt32(bytes.Length);


                    bool acceptUpload = CheckFileContentType(fileMime);
                    if (acceptUpload)
                    {
                        bool hasUploaded = false;

                        string folderPath = Path.Combine(AppContext.BaseDirectory, webRoot, path);
                        bool dirExists = Directory.Exists(folderPath);
                        if (!dirExists) Directory.CreateDirectory(folderPath);

                        string fullPath = Path.Combine(folderPath, fileName);

                        using (FileStream stream = new FileStream(path: fullPath, mode: FileMode.Create))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Flush();
                            hasUploaded = true;
                        }


                        string baseUrl = db.SQLSelect(SQL.SettingValueByIdQry, new[] { "API_BASEURL" });
                        string relativePath = Path.Combine(baseUrl, path, fileName!);

                        if (hasUploaded)
                        {
                            data = new FileModelResponse
                            {
                                FileData = new FileModel
                                {
                                    FilePath = relativePath.ToString().Replace("\\", "/"),
                                    FileName = fileName,
                                    FileSize = fileSize,
                                    FileMime = fileMime
                                },
                                Status = Constants.StatusResponses.SUCCESS
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static FileModelResponse FileUploader(string webRoot, string path, IFormFile file)
        {
            FileModelResponse? data = null;
            try
            {
                bool fileExists = CheckFileExists(file);
                if (fileExists)
                {
                    string[] mimesWordDoc = Constants.FileMime.MimeWordDocReplacer;
                    string fileMime = file.ContentType;
                    if (fileMime == mimesWordDoc[0]) fileMime = mimesWordDoc[1];

                    string extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    string fileName = $"{DateTime.Now.Ticks}{extension}";
                    int fileSize = Convert.ToInt32(file?.Length);

                    bool acceptUpload = CheckFileContentType(fileMime);
                    if (acceptUpload)
                    {
                        bool hasUploaded = false;

                        string folderPath = Path.Combine(AppContext.BaseDirectory, webRoot, path);
                        bool dirExists = Directory.Exists(folderPath);
                        if (!dirExists) Directory.CreateDirectory(folderPath);

                        string fullPath = Path.Combine(folderPath, fileName);

                        byte[] bytes = null;
                        using (var ms = new MemoryStream())
                        {
                            file!.CopyTo(ms);
                            bytes = ms.ToArray();
                        }

                        using (FileStream stream = new FileStream(path: fullPath, mode: FileMode.Create))
                        {
                            stream.Write(bytes, 0, fileSize);
                            stream.Flush();
                            hasUploaded = true;
                        }

                        string baseUrl = db.SQLSelect(SQL.SettingValueByIdQry, new[] { "API_BASEURL" });
                        string relativePath = Path.Combine(baseUrl!, path, fileName!);
                        if (hasUploaded)
                        {
                            data = new FileModelResponse
                            {
                                FileData = new FileModel
                                {
                                    FilePath = relativePath.ToString().Replace("\\", "/"),
                                    FileName = fileName,
                                    FileSize = fileSize,
                                    FileMime = fileMime
                                },
                                Status = Constants.StatusResponses.SUCCESS
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data!;
        }


        public static DateTime NullDate()
        {
            string nullDateString = "1/1/0001 12:00:00 AM";
            return DateTime.Parse(nullDateString, CultureInfo.InvariantCulture);
        }


        public static string Base64Decoder(string base64)
        {
            string data;
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                data = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static T DecodeAnyTypeFromBase64<T>(string base64)
        {
            T? data;
            try
            {
                string jsonStr = Utility.Base64Decoder(base64);
                data = JsonConvert.DeserializeObject<T>(jsonStr!)!;
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }


        public static FileModelResponse GenerateQRCode(string text, string webroot, string path)
        {
            FileModelResponse? data;
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

                    QRCode qrCode = new(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    string folderPath = Path.Combine(AppContext.BaseDirectory, webroot, path);
                    bool dirExists = Directory.Exists(folderPath);
                    if (!dirExists) Directory.CreateDirectory(folderPath);

                    string fileName = $"{DateTime.Now.Ticks}.png";
                    string fullPath = Path.Combine(folderPath, fileName);


                    // Save QR code image to disk in PNG format
                    qrCodeImage.Save(fullPath, ImageFormat.Png);

                    string baseUrl = db.SQLSelect(SQL.SettingValueByIdQry, new[] { Constants.SettingIds.API_BASEURL });
                    string relativePath = Path.Combine(baseUrl!, path, fileName!);

                    data = new()
                    {
                        FileData = new FileModel()
                        {
                            FilePath = relativePath.ToString().Replace("\\", "/"),
                        },
                        Status = Constants.StatusResponses.SUCCESS
                    };
                }
                else data = null;

            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }

    }
}
