using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public interface IStripePaymentRepository
	{
		Task<string> CreateCustomer(string email, string name, string userId);
		Task<string> GetEmhemeralKey(string customerId);
		Task<StripeCustomerIntentCustom> CreatePaymentIntent(string Currency, decimal Amount,bool isWalletApplied, double CashBackPrice,CancellationToken cancellationToken);
		Task<string> GetAllCustomers(string customerEmail);
		Task<ConnectAccountReturnURL> CreateAccount(StripeCreateAccount createAccount);
		Task<string> ConnectAccountLink(ConnectAccoLink connectAcco);
		Task<ConnectAccountReturnURL> GetConnectAccountStatusUrl(string userId);
		Task<CustomerSetupIntentResponse> StripeSetupIntent();
		Task<CustomerPaymentMethodAvaliable> IsPaymentMethodAvailableForCustomer(CancellationToken cancellationToken);
		Task<StripeTransfer> StripeDriverPaymentSystem(StripeDriverPaymentInfo driverPaymentInfo);
		Task<CaptureAfterIntent> StripeCaptureIntent(string? paymentIntentId);
        Task<CaptureAfterIntent> ConfirmPaymentIntent(string? paymentMethod, string? paymentIntentId,CancellationToken cancellationToken);
		Task<int> StripeDriverBalance();
		Task<string> MakeDefaultPaymentMethod(string PaymentMethodId);
        Task<string> AddMoney(DigitalWalletData money);

    }
}
