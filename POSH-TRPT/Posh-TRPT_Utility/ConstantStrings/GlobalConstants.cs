using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Utility.ConstantStrings
{
    public static class GlobalConstants
    {
        public static class GlobalValues
        {
            public const string ControllerRoute = "api/[controller]/[action]";
            public const string Pending = "16069375-A542-4EEA-B0AF-6AFA6272EF8B";
            public const string Approved = "57DEEADB-B1C5-4273-A830-ED8D3B001F70";
            public const string Declined = "95BBDB00-F026-4663-8676-1081E30BDFBE";
            public const string Approval_In_Progress = "8044B005-7AC7-40E0-A249-79DAB3E81C06";
            public const string PendingBody = "Views/Email/PendingEmailBody.cshtml";
            public const string ApprovedBody = "Views/Email/ApprovedEmailBody.cshtml";
            public const string DeclinedBody = "Views/Email/DeclinedEmailBody.cshtml";
            public const string Pending_In_ProgressBody = "Views/Email/PendingInProgressEmailBody.cshtml";
            public const string SuperAdminViewBody = "Views/Email/AdminEmailBody.cshtml";
            public const string SendOTP = "SendOTP";
            public const string RideCompletion = "RideCompletion";
            public const string SendOTPEmail = "Views/Email/VerificationCodeEmailBody.cshtml";
            public const string SendOTPEmailForgotPassword = "Views/Email/VerificationCodeForgotPassword.cshtml";
            public const string SendRegisterCustomerEmail = "Views/Email/CustomerRegisterEmailBody.cshtml";
            public const string VerificationRiderCodeEmailBody = "Views/Email/VerificationRiderCodeEmailBody.cshtml";
            public const string RideCompletionMailBody = "Views/RideCompletionMailBody.cshtml";
            public const string RideReceipt = "Views/Receipts/Receipt.html";
            public const string RideReceiptFolder = "Views/Receipts";
            public const string poshlogopath = "/Images/Logo.PNG";
            public const string coinlogopath = "/Images/cashIcon.PNG";
            public const string personImage = "/Images/person.png";
            public const string InspectionExpiryEmailBodyForDriver = "Views/Email/ExpireInspectionEmailToDriver.cshtml";

            public static class BookingStatus
            {
                public const string NOTIFIED_DRIVER = "3765FA3B-9B13-4429-91F0-04A01B922B05";
                public const string ACCEPT = "C6D95928-415C-4E44-9518-336A8CA02AAE";
                public const string DECLINED = "3588D36B-E0E6-41A3-AC7C-8104CD3412C7";
                public const string UNASSIGNED = "FB09BEF3-B1F2-4E21-A25D-A2D769315778";
                public const string CANCELLED = "341B9566-EDFF-4818-BB5A-A3CA83CDE9CA";
                public const string STARTED = "63550291-87E8-46E8-A5D7-337416C60ED4";
                public const string COMPLETED = "1D486984-54AB-43A2-B71C-99EA9F44BD2E";
                public const string AUTOMATICDECLINE = "56A4A248-5DA1-4FD3-B17C-9E8A7D6A2BFD";
                public const string NotifyPaymentSuccess = "8689AF8B-624D-46CC-B268-0CAA97AA227C";

                public const string ST_UNASSIGNED = "0";
                public const string ST_NOTIFIED_DRIVER = "1";
                public const string ST_ACCEPTED = "2";
                public const string ST_STARTED = "3";
                public const string ST_COMPLETED = "4";
                public const string ST_DECLINED = "5";
                public const string ST_CANCELLED = "6";
                public const string ST_AUTO_DECLINED = "9";
            }
            public static class SuperPermissionID
            {
                public const string SuperAdmin = "eadf0545-5cef-47f4-96f4-c552aab46504";
            }
        }
    }
}
