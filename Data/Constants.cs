namespace TheEstate.Data
{
    public class Constants
    {
        public class JWTClaimTypes
        {
            public const string Id = "Id";
            public const string Username = "Username";
            public const string Email = "Email";
            public const string MobilePhone = "MobilePhone";
            public const string ResidentCategory = "ResidentCategory";
        }


        public class StatusResponses
        {
            public const string SUCCESS = "SUCCESS";
            public const string FAILED = "FAILED";
            public const string EXISTS = "EXISTS";
            public const string EXPIRED = "EXPIRED";
            public const string ERROR = "ERROR";
            public const string INVALID = "INVALID";
            public const string INVALID_FORMAT = "INVALID_FORMAT";
            public const string NOTFOUND = "NOTFOUND";
            public const string SENTOTP = "SENTOTP";
            public const string SENTOTP_EMAIL = "SENTOTP_EMAIL";
            public const string SENTOTP_MOBILE = "SENTOTP_MOBILE";
            public const string PARTIAL_SUCCESS = "PARTIAL_SUCCESS";
        }


        public class FileMime
        {
            public static readonly string[] Mimes = new string[] {
                "image/png",
                "image/jpeg",
                "image/bmp",
                "image/gif",
                "image/x-png",
                "file/txt",
                "file/pdf",
                "file/doc",
                "file/docx",
                "file/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
            public static readonly string[] MimeWordDocReplacer = new string[] { "file/vnd.openxmlformats-officedocument.wordprocessingml.document", "file/doc" };
        }


        public class FileStorageLocations
        {
            public const string PROFILE_PHOTO_PATH = "uploads\\profile";
            public const string ESTATE_PHOTO_PATH = "uploads\\estate";
            public const string PROPERTY_PHOTO_PATH = "uploads\\property";
            public const string QRCODE_PHOTO_PATH = "uploads\\qrcode";
            public const string PAYMENT_RECEIPT_PATH = "uploads\\receipt";
        }


        public class VisitingDays
        {
            public const string MONDAY = "MONDAY";
            public const string TUESDAY = "TUESDAY";
            public const string WEDNESDAY = "WEDNESDAY";
            public const string THURSDAY = "THURSDAY";
            public const string FRIDAY = "FRIDAY";
            public const string SATURDAY = "SATURDAY";
            public const string SUNDAY = "SUNDAY";
        }


        public class SettingIds
        {
            public const string SENDER = "SENDER";
            public const string PORT = "PORT";
            public const string ENABLE_SSL = "ENABLE_SSL";
            public const string HOST = "HOST";
            public const string PASSWORD = "PASSWORD";
            public const string SMTP_DISPLAY_NAME = "SMTP_DISPLAY_NAME";

            public const string PAYSTACK_SECRET_KEY = "PAYSTACK_SECRET_KEY";
            public const string PAYSTACK_PUBLIC_KEY = "PAYSTACK_PUBLIC_KEY";
            public const string PAYSTACK_CALLBACK_URL = "PAYSTACK_CALLBACK_URL";

            public const string API_BASEURL = "API_BASEURL";
            
            public const string SMS_GATEWAY_URL = "SMS_GATEWAY_URL";
            public const string SMS_GATEWAY_USERNAME = "SMS_GATEWAY_USERNAME";
            public const string SMS_GATEWAY_PASSWORD = "SMS_GATEWAY_PASSWORD";
            public const string SMS_GATEWAY_SENDERID = "SMS_GATEWAY_SENDERID";
        }


        public class SettingGroups
        {
            public const string SMTP = "SMTP";
            public const string PAYSTACK = "PAYSTACK";
            public const string SMS = "SMS";
        }


        public class Defaults
        {
            public const string DefaultZoneId = "000";
            public const string DefaultZoneName = "DEFAULT";
        }





        public enum ResidentCategories
        {
            PRIMARY,
            SECONDARY
        }

        public enum LoginTypes
        {
            EMAIL,
            MOBILENO
        }


        public enum ActiviyStatus
        {
            ACTIVE,
            PENDING,
            OPEN,
            CLOSED,
            ACCEPTED,
            ALL,
            SELECT
        }


        public enum OnboardingState
        {
            ACOUNT_VERIFICATION,
            PROFILE_SETUP,
            COMPLETE
        }


        public enum PaymentGateways
        {
            PAYSTACK,
            GTPAY
        }


        

        //public enum VisitingDays
        //{
        //    MONDAY,
        //    TUESDAY,
        //    WEDNESDAY,
        //    THURSDAY,
        //    FRIDAY,
        //    SATURDAY,
        //    SUNDAY
        //}

    }
}
