using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Posh_TRPT_Domain.InspectionData;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Utility.ConstantStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Utility.EmailUtils
{
	public static class EmailUtility
	{
		public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;
		public static string SendVerificationEmail(OTPtoEmail ptoEmail)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(ptoEmail.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(ptoEmail.MailTo!));
				mailMessage.Subject = ptoEmail.Subject;
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				switch (ptoEmail.Status)
				{
					case GlobalConstants.GlobalValues.SendOTP:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.SendOTPEmail);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{verificationCode}", ptoEmail.OTP);
							mailMessage.Body = body;
							break;
						}
				}
				var client = new SmtpClient(ptoEmail.Host, ptoEmail.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(ptoEmail.MailFrom!, ptoEmail.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;


				client.Send(mailMessage);
				return "sent success!";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public static string SendVerificationEmailForgotPassword(OTPtoEmail ptoEmail)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(ptoEmail.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(ptoEmail.MailTo!));
				mailMessage.Subject = ptoEmail.Subject;
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				switch (ptoEmail.Status)
				{
					case GlobalConstants.GlobalValues.SendOTP:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.SendOTPEmailForgotPassword);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{verificationCode}", ptoEmail.OTP);
							mailMessage.Body = body;
							break;
						}
				}
				var client = new SmtpClient(ptoEmail.Host, ptoEmail.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(ptoEmail.MailFrom!, ptoEmail.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;

				client.Send(mailMessage);
				return "sent success!";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public static string SendRegisterCustomerEmail(RegisterCustomerEmail customerEmail)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(customerEmail.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(customerEmail.MailTo!));
				mailMessage.Subject = customerEmail.Subject;
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				switch (customerEmail.Status)
				{
					case GlobalConstants.GlobalValues.SendRegisterCustomerEmail:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.SendRegisterCustomerEmail);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{UserName}", customerEmail.Username);
							mailMessage.Body = body;
							break;
						}
				}
				var client = new SmtpClient(customerEmail.Host, customerEmail.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(customerEmail.MailFrom!, customerEmail.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;



				client.Send(mailMessage);
				return "sent success!";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public static string SendRegisterCustomerOTPEmail(OTPtoEmail ptoEmail)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(ptoEmail.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(ptoEmail.MailTo!));
				mailMessage.Subject = ptoEmail.Subject;
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				switch (ptoEmail.Status)
				{
					case GlobalConstants.GlobalValues.SendOTP:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.VerificationRiderCodeEmailBody);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{verificationCode}", ptoEmail.OTP);
							mailMessage.Body = body;
							break;
						}
				}

				var client = new SmtpClient(ptoEmail.Host, ptoEmail.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(ptoEmail.MailFrom!, ptoEmail.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;

				client.Send(mailMessage);
				return "sent success!";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public static string RideCompletionMail(RideCompletion ptoEmail)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(ptoEmail.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(ptoEmail.MailTo!));
				mailMessage.Subject = ptoEmail.Subject;
				mailMessage.IsBodyHtml = true;
				mailMessage.Attachments.Add(new Attachment(ptoEmail.Attachment!));
				string body = string.Empty;
				switch (ptoEmail.Status)
				{
					case GlobalConstants.GlobalValues.SendOTP:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.RideCompletionMailBody);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{Name}", ptoEmail.Name);
							mailMessage.Body = body;
							break;
						}
					case GlobalConstants.GlobalValues.RideCompletion:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.RideCompletionMailBody);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{Name}", ptoEmail.Name);
							mailMessage.Body = body;
							break;
						}
				}
				var client = new SmtpClient(ptoEmail.Host, ptoEmail.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(ptoEmail.MailFrom!, ptoEmail.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;


				client.Send(mailMessage);
				return "sent success!";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public static bool SendEmail(EmailDriver emailDriver)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(emailDriver.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(emailDriver.MailTo!));
				mailMessage.Subject = emailDriver.Subject;
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				switch (emailDriver.DocStatus!.ToString()!.ToUpper())
				{
					case GlobalConstants.GlobalValues.Approved:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.ApprovedBody);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{UserName}", emailDriver.DriverName);
							body = body.Replace("{URL}", emailDriver.RequestUri);
							mailMessage.Body = body;
							break;
						}
					case GlobalConstants.GlobalValues.Declined:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.DeclinedBody);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{UserName}", emailDriver.DriverName);
							body = body.Replace("{URL}", emailDriver.RequestUri);
							body = body.Replace("{Reason}", emailDriver.Reason);
							mailMessage.Body = body;
							break;
						}
					case GlobalConstants.GlobalValues.Pending:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.PendingBody);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{UserName}", emailDriver.DriverName);
							body = body.Replace("{URL}", emailDriver.RequestUri);
							body = body.Replace("{Reason}", emailDriver.Reason);
							mailMessage.Body = body;
							break;
						}
					case GlobalConstants.GlobalValues.Approval_In_Progress:
						{
							string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.Approval_In_Progress);
							using (StreamReader reader = new StreamReader(filePath))
							{
								body = reader.ReadToEnd();
							}
							body = body.Replace("{UserName}", emailDriver.DriverName);
							body = body.Replace("{URL}", emailDriver.RequestUri);
							body = body.Replace("{Reason}", emailDriver.Reason);
							mailMessage.Body = body;
							break;
						}

				}
				var client = new SmtpClient(emailDriver.Host, emailDriver.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(emailDriver.MailFrom!, emailDriver.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;

				client.Send(mailMessage);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public static bool SendEmailToAdmin(EmailAdmin emailAdmin)
		{
			try
			{

				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(emailAdmin.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(emailAdmin.EmailTo!));

				mailMessage.Subject = emailAdmin.Subject;
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.SuperAdminViewBody);
				using (StreamReader reader = new StreamReader(filePath))
				{
					body = reader.ReadToEnd();
				}

				body = body.Replace("{SuperAdmin}", emailAdmin.SuperAdminName);
				body = body.Replace("{URL}", emailAdmin.RequestUri);
				body = body.Replace("{UserName}", emailAdmin.UserName);
				body = body.Replace("{UserEmail}", emailAdmin.UserEmail);
				body = body.Replace("{UserContact}", emailAdmin.UserContact);

				mailMessage.Body = body;
				var client = new SmtpClient(emailAdmin.Host, emailAdmin.Port);
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(emailAdmin.EmailFrom!, emailAdmin.Password);
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.Send(mailMessage);
				return true;

			}
			catch (Exception)
			{
				return false;
			}

		}

		public static bool SendEmailToDriverExpireInspection(EmailToDriverDueInspection email)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(email.MailFromAlias!);
				mailMessage.To.Add(new MailAddress(email.MailTo!));
				mailMessage.Subject = email.Subject;
				mailMessage.CC.Add(new MailAddress(email.EmailCC!));
				mailMessage.IsBodyHtml = true;
				string body = string.Empty;
				string filePath = System.IO.Path.GetFullPath(GlobalConstants.GlobalValues.InspectionExpiryEmailBodyForDriver);
				using (StreamReader reader = new StreamReader(filePath))
				{
					body = reader.ReadToEnd();
				}
				body = body.Replace("{UserName}", email.DriverName);
				body = body.Replace("{date}", email.Inspection_Expiry_Date);
				body = body.Replace("{note}", email.InspectionNote);
				body = body.Replace("{phone}", email.PhoneNumber);
				mailMessage.Body = body;
                var client = new SmtpClient(email.Host, email.Port);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(email.MailFrom!, email.Password);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(mailMessage);
                return true;
			}
			catch (Exception ex)
			{
				throw;
			}
        }

	}
}
