using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.StripePaymentDTO;
using Stripe;
using System.Net.Http.Headers;
using System.Security.Claims;
using Posh_TRPT_Utility.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Domain.Entity.Posh_TRPT_Domain.StripePayment;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class StripePaymentRepository : Repository<StripeCustomers>, IStripePaymentRepository
    {
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _config;
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StripePaymentRepository> _logger;
        private IMapper _mapper;
        public StripePaymentRepository(IHttpContextAccessor context, IConfiguration config, IOptions<StripeSettings> stripeSettings, UserManager<ApplicationUser> userManager, IMapper mapper, DbFactory dbFactory, ILogger<StripePaymentRepository> logger) : base(dbFactory)
        {
            _context = context;
            _config = config;
            _stripeSettings = stripeSettings.Value;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        #region CreateCustomer
        /// <summary>
        /// create stripe customer method
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CreateCustomer(string email, string name, string userId)
        {
            var userStripe = this.DbContextObj().TblStripeCustomers.Where(x => x.Email!.Equals(email)).FirstOrDefault()!;
            ApplicationUser userRecord = this.DbContextObj().Users.Where(x => x.Id.Equals(userId)).FirstOrDefault()!;
            if (userStripe is null)
            {
                Posh_TRPT_Domain.Entity.StripeCustomers customer = new Posh_TRPT_Domain.Entity.StripeCustomers();
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
                {
                    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                    var options = new CustomerCreateOptions
                    {
                        Name = name,
                        Email = email
                    };
                    var service = new CustomerService();
                    var result = await service.CreateAsync(options);
                    customer.Email = result.Email;
                    customer.StripeCustomerId = result.Id;
                    customer.UserId = userRecord.Id;
                    customer.Name = result.Name;
                    customer.Livemode = result.Livemode;
                    customer.CreatedBy = userRecord.Id;
                    customer.IsDeleted = false;
                    customer.IsActive = true;
                    customer.CreatedDate = DateTime.UtcNow;
                    customer.UpdatedBy = string.Empty;
                    await this.DbContextObj().TblStripeCustomers.AddAsync(customer);
                    await this.DbContextObj().SaveChangesAsync();
                    userRecord.StripeCustomerId = customer.StripeCustomerId;
                    userRecord.UpdatedBy = userRecord.Id;
                    userRecord.UpdatedDate = DateTime.UtcNow;
                    await _userManager.UpdateAsync(userRecord);
                    return result.Id;
                }
            }

            return System.Threading.Tasks.Task.FromResult<string>(null!).Result;
        }
        #endregion

        [NonAction]
        public async Task<string> GetEmhemeralKey(string customerId)
        {
            var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("customer", customerId);
            using (var client = new System.Net.Http.HttpClient())
            {
                using (var content = new FormUrlEncodedContent(keyValuePairs))
                {

                    content.Headers.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _stripeSettings.SecretKey);
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    content.Headers.Add("Stripe-Version", "2023-10-16");
                    client.BaseAddress = new Uri(_stripeSettings.StripeBaseUrl + _stripeSettings.StripeEphemeralKey);
                    using (HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content))
                    {
                        response.EnsureSuccessStatusCode();
                        CustomerEphemeralKey res = JsonConvert.DeserializeObject<CustomerEphemeralKey>(await response.Content.ReadAsStringAsync().ConfigureAwait(false))!;
                        StripeCustomers stripeCustomers = this.DbContextObj().TblStripeCustomers.Where(x => x.UserId!.Equals(userId) && x.StripeCustomerId!.Equals(customerId)).FirstOrDefault()!;
                        if (stripeCustomers != null)
                        {
                            stripeCustomers.EphemeralKey = res.Id;
                            stripeCustomers.EphemeralSecret = res.Secret;
                            stripeCustomers.EphemeralCreatedDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(res.Created)).DateTime;
                            stripeCustomers.EphemeralExpiresDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(res.Expires)).DateTime;
                            stripeCustomers.UpdatedDate = DateTime.UtcNow;
                            stripeCustomers.UpdatedBy = userId;
                            this.DbContextObj().Entry(stripeCustomers).State = EntityState.Modified;
                            await this.DbContextObj().SaveChangesAsync();
                        }
                        return res.Secret!;
                    }
                }
            }
        }
        public async Task<StripeCustomerIntentCustom> CreatePaymentIntent(string Currency, decimal Amount, bool isWalletApplied, double CashBackPrice, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{0} InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1} Amount ={2} CancellationToken = {3}", DateTime.UtcNow, Currency, Amount, cancellationToken.IsCancellationRequested);

            try
            {
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var customerData = await _userManager.FindByIdAsync(userId);


                /// This method is used to check customer has any payment method or not
                #region IsPaymentMethodAvailableForCustomer

                _logger.LogInformation("{0} Before IsPaymentMethodAvailableForCustomer() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1} Amount ={2} CancellationToken = {3}", DateTime.UtcNow, Currency, Amount, cancellationToken.IsCancellationRequested);

                var paymentMethod = await IsPaymentMethodAvailableForCustomer(cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("{0} After IsPaymentMethodAvailableForCustomer() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1} Amount ={2} CancellationToken = {3}", DateTime.UtcNow, Currency, Amount, cancellationToken.IsCancellationRequested);


                string paymetMethodId = string.Empty;
                if (!paymentMethod.IsPaymentMethodAvailable)
                {
                    _logger.LogInformation("{0} Response IsPaymentMethodAvailableForCustomer() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3}  IsPaymentMethodAvailable ={4}  PayementMethodCount = {5} CancellationToken = {6}", DateTime.UtcNow, Currency, Amount, userId, paymentMethod.IsPaymentMethodAvailable, paymentMethod.Data!.Count(), cancellationToken.IsCancellationRequested);
                    return new StripeCustomerIntentCustom();
                }

                _logger.LogInformation("{0} Response IsPaymentMethodAvailableForCustomer() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3}  IsPaymentMethodAvailable ={4}  PayementMethodCount = {5} CancellationToken = {6}", DateTime.UtcNow, Currency, Amount, userId, paymentMethod.IsPaymentMethodAvailable, paymentMethod.Data!.Count(), cancellationToken.IsCancellationRequested);

                #endregion


                /// This method is used to fetch the customer details from Stripe
                #region CustomerService
                _logger.LogInformation("{0} Before CustomerService() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId, cancellationToken.IsCancellationRequested);

                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                var serviceSource = new CustomerService();

                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                object obj = JsonConvert.SerializeObject(await serviceSource.GetAsync(customerData.StripeCustomerId, null, null, cancellationToken).ConfigureAwait(false));
                StripeCustomerData dataResult = JsonConvert.DeserializeObject<StripeCustomerData>(obj.ToString()!)!;
                //StripeCustomerData data = null;
                _logger.LogInformation("{0} After DeserializeObject CustomerService() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId, cancellationToken.IsCancellationRequested);

                #endregion

                if (!string.IsNullOrEmpty(dataResult?.InvoiceSettings?.DefaultPaymentMethod))
                {
                    var stripeCustomer = this.DbContextObj().TblStripeCustomers?.Where(z => z.UserId!.Equals(customerData.Id) && z.StripeCustomerId!.Equals(customerData.StripeCustomerId)).FirstOrDefault();
                    stripeCustomer!.Shipping = dataResult?.Shipping;
                    stripeCustomer!.Address = dataResult?.Address;
                    if (isWalletApplied)
                    {
                        stripeCustomer!.Promotion = (decimal)CashBackPrice;
                    }
                    else
                    {
                        stripeCustomer!.Promotion = 0.0m;
                    }
                    stripeCustomer!.Balance = dataResult?.Balance;
                    stripeCustomer!.DefaultSource = dataResult?.DefaultSource;
                    stripeCustomer!.Description = dataResult?.Description;
                    stripeCustomer!.Discount = dataResult?.Discount;
                    stripeCustomer!.InvoicePrefix = dataResult?.InvoicePrefix;
                    stripeCustomer!.Phone = dataResult?.Phone;
                    stripeCustomer!.Shipping = dataResult?.Shipping;
                    stripeCustomer!.UpdatedBy = userId;
                    stripeCustomer!.UpdatedDate = DateTime.UtcNow;

                    if (paymentMethod.Data?.Count() > 0)
                    {
                        paymetMethodId = dataResult?.InvoiceSettings?.DefaultPaymentMethod!;
                        stripeCustomer!.DefaultPaymentMethod = dataResult?.InvoiceSettings?.DefaultPaymentMethod!;
                    }


                    _logger.LogInformation("{0} Response before updating Customer InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId,/* stripeCustomer.Id,*/ cancellationToken.IsCancellationRequested);

                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    this.DbContextObj().Entry(stripeCustomer).State = EntityState.Modified;
                    await this.DbContextObj().SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("{0} Response After updating Customer InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId, /*stripeCustomer.Id, */cancellationToken.IsCancellationRequested);


                    if (!string.IsNullOrEmpty(paymetMethodId))
                    {

                        PaymentIntentCreateOptions options = null!;
                        options = new PaymentIntentCreateOptions
                        {
                            Amount = Convert.ToInt32(Amount),
                            Currency = Currency,
                            Customer = customerData.StripeCustomerId,
                            CaptureMethod = "manual",
                            PaymentMethod = paymetMethodId,
                            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                            {
                                Enabled = true,
                                AllowRedirects = "never"
                            },
                        };

                        var service = new PaymentIntentService();
                        StripeCustomerIntentDTO stripeCustomerIntentDTO = null!;

                        _logger.LogInformation("{0} Before CreateAsync() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId, cancellationToken.IsCancellationRequested);

                        if (cancellationToken.IsCancellationRequested)
                            cancellationToken.ThrowIfCancellationRequested();

                        var custData = await service.CreateAsync(options, null, cancellationToken).ConfigureAwait(false);
                        StripeCustomerIntentCustom res = JsonConvert.DeserializeObject<StripeCustomerIntentCustom>(custData.ToJson())!;

                        _logger.LogInformation("{0} After DeserializeObject CreateAsync() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId, cancellationToken.IsCancellationRequested);

                        if (res != null && res.Id != null)
                        {
                            _logger.LogInformation("{0} Response CreateAsync() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} StripeCustomerIntentId ={4} CancellationToken = {5}", DateTime.UtcNow, Currency, Amount, userId, res.Id, cancellationToken.IsCancellationRequested);

                            _context.HttpContext.Session.SetString("paymentIntentId", res.Id!);
                            _context.HttpContext.Session.SetString("paymetMethodId", paymetMethodId);
                            var stripeCustomerIntent = new StripeCustomerIntent();

                            stripeCustomerIntentDTO = new StripeCustomerIntentDTO
                            {
                                Id = Guid.NewGuid(),
                                UserId = customerData?.Id,
                                StripeCustomerIntentId = res?.Id,
                                Object = res?.Object,
                                Amount = res!.Amount,
                                AmountCapturable = res.AmountCapturable,
                                AmountReceived = res.AmountReceived,
                                AutomaticPaymentMethods_Enable = res.AutomaticPaymentMethods!.Enabled,
                                CaptureMethod = res.CaptureMethod,
                                ClientSecret = res.ClientSecret,
                                ConfirmationMethod = res.ConfirmationMethod,
                                Currency = res.Currency,
                                CustomerId = res.Customer,
                                Livemode = res.Livemode,
                                Metadata = null!,
                                PaymentMethod = res.PaymentMethod,
                                PaymentMethodOptions_Card = res.PaymentMethodOptions!.Card?.RequestThreeDSecure,
                                Status = res.Status,
                                CreatedBy = userId,
                                CreatedDate = DateTime.UtcNow,
                                UpdatedBy = userId,
                                UpdatedDate = DateTime.UtcNow

                            };


                            _logger.LogInformation("{0} Before InSide CreatePaymentIntent in StripPaymentRepository Method before saving into database Currency ={1}  Amount = {2}  StripeCustomerIntent = {3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, stripeCustomerIntentDTO.Id, cancellationToken.IsCancellationRequested);

                            if (cancellationToken.IsCancellationRequested)
                                cancellationToken.ThrowIfCancellationRequested();

                            await this.DbContextObj().TblStripeCustomersIntent.AddAsync(_mapper.Map<StripeCustomerIntent>(stripeCustomerIntentDTO), cancellationToken);
                            await this.DbContextObj().SaveChangesAsync(cancellationToken);

                            _logger.LogInformation("{0} After InSide CreatePaymentIntent in StripPaymentRepository Method before saving into database Currency ={1}  Amount = {2}  StripeCustomerIntent = {3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, stripeCustomerIntentDTO.Id, cancellationToken.IsCancellationRequested);

                            return res;

                        }

                        _logger.LogInformation("{0} Response CreateAsync() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} StripeCustomerIntentId ={4} CancellationToken = {5}", DateTime.UtcNow, Currency, Amount, userId, null, cancellationToken.IsCancellationRequested);

                        return res!;
                    }

                }
                _logger.LogInformation("{0} Response CustomerService() InSide CreatePaymentIntent in StripPaymentRepository Method Currency ={1}  Amount = {2}  Rider ={3} CancellationToken = {4}", DateTime.UtcNow, Currency, Amount, userId, cancellationToken.IsCancellationRequested);


                return new StripeCustomerIntentCustom();
            }
            catch (Exception)
            {

                return null!;
            }

        }
        public async Task<string> GetAllCustomers(string customerEmail)
        {
            var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("email", customerEmail);
            using (var client = new System.Net.Http.HttpClient())
            {
                using (var content = new FormUrlEncodedContent(keyValuePairs))
                {

                    content.Headers.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _stripeSettings.SecretKey);
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.BaseAddress = new Uri(_stripeSettings.StripeBaseUrl + _stripeSettings.StripeCustomers);
                    using (HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content))
                    {
                        response.EnsureSuccessStatusCode();
                        AllStripeCustomer res = JsonConvert.DeserializeObject<AllStripeCustomer>(await response.Content.ReadAsStringAsync().ConfigureAwait(false))!;
                        return res.CustomerId!;
                    }
                }
            }
        }



        public async Task<ConnectAccountReturnURL> CreateAccount(StripeCreateAccount createAccount)
        {
            string userId = string.Empty;
            userId = _context.HttpContext!.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var role = _context.HttpContext!.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
            if (role is not null)
            {
                if (role!.Equals(AuthorizationLevel.Roles.SuperAdmin))
                {
                    var userData = await _userManager.FindByEmailAsync(createAccount.Email).ConfigureAwait(false);
                    userId = userData?.Id!;
                }
            }

            if (string.IsNullOrEmpty(userId))
            {
                userId = createAccount.UserId!;
            }
            ConnectAccountReturnURL accountReturnURL = new ConnectAccountReturnURL();
            var userRecord = await _userManager.FindByIdAsync(userId);
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            if (userRecord?.Payouts_Enabled == false)
            {
                if (string.IsNullOrEmpty(userRecord?.StripeConnectedAccountId))
                {

                    var options = new AccountCreateOptions
                    {
                        Type = createAccount.AccountType,
                        Country = createAccount.Country,
                        Email = createAccount.Email,
                        Capabilities = new AccountCapabilitiesOptions
                        {
                            CardPayments = new AccountCapabilitiesCardPaymentsOptions
                            {
                                Requested = true,
                            },
                            Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
                        },
                    };
                    var service = new Stripe.AccountService();
                    Stripe.Account account = await service.CreateAsync(options).ConfigureAwait(false);
                    StripeConnectAccountUsers stripeConnect = new StripeConnectAccountUsers
                    {
                        ConnectAccountId = account.Id,
                        UserId = userId,
                        AccountType = account.Type,
                        Country = account.Country,
                        Created = account.Created,
                        Default_Currency = account.DefaultCurrency,
                        Payouts_Enabled = account.PayoutsEnabled,
                        Email = account.Email,
                        External_Accounts_URL = account.ExternalAccounts!.Url,
                        Login_Links_URL = string.Empty,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false
                    };
                    await this.DbContextObj().TblStripeConnectAccountUsers.AddAsync(stripeConnect);
                    await this.DbContextObj().SaveChangesAsync();
                    userRecord!.StripeConnectedAccountId = account.Id;
                    userRecord!.UpdatedDate = DateTime.UtcNow;
                    userRecord!.Payouts_Enabled = account.PayoutsEnabled;
                    userRecord!.UpdatedBy = userId;
                    await _userManager.UpdateAsync(userRecord).ConfigureAwait(false);
                    if (account.PayoutsEnabled)
                    {
                        accountReturnURL.PayoutStatus = true;
                        accountReturnURL.URL = null!;
                    }
                    else
                    {
                        ConnectAccoLink accoLink = new ConnectAccoLink
                        {
                            ConnectAcctId = account.Id,
                            RefreshUrl = _config["Request:URL"] + _stripeSettings.ReAuth,
                            ReturnUrl = _config["Request:URL"] + _stripeSettings.Return,
                            AccountType = _stripeSettings.Account_Onboarding,
                            UserId = userId
                        };
                        accountReturnURL.PayoutStatus = false;
                        accountReturnURL.URL = await ConnectAccountLink(accoLink).ConfigureAwait(false);
                    }
                    accountReturnURL.PayoutStatus = userRecord.Payouts_Enabled;
                    accountReturnURL.URL = string.Empty;
                    return accountReturnURL;
                }
                else
                {
                    var service = new Stripe.AccountService();
                    Stripe.Account account = await service.GetAsync(userRecord?.StripeConnectedAccountId).ConfigureAwait(false);
                    if (account.PayoutsEnabled)
                    {
                        var data = this.DbContextObj().TblStripeConnectAccountUsers?.Where(x => x.UserId!.Equals(userId) && x.ConnectAccountId!.Equals(userRecord!.StripeConnectedAccountId)).FirstOrDefault()!;
                        data.UpdatedDate = DateTime.UtcNow;
                        data.Payouts_Enabled = account.PayoutsEnabled;
                        data.UpdatedBy = userId;
                        this.DbContextObj().Entry(data).State = EntityState.Modified;
                        await this.DbContextObj().SaveChangesAsync().ConfigureAwait(false);
                        userRecord!.UpdatedDate = DateTime.UtcNow;
                        userRecord!.Payouts_Enabled = account.PayoutsEnabled;
                        userRecord!.UpdatedBy = userId;
                        await _userManager.UpdateAsync(userRecord).ConfigureAwait(false);
                        accountReturnURL.PayoutStatus = true;
                        accountReturnURL.URL = null!;
                    }
                    else
                    {
                        ConnectAccoLink accoLink = new ConnectAccoLink
                        {
                            ConnectAcctId = userRecord?.StripeConnectedAccountId,
                            RefreshUrl = _config["Request:URL"] + _stripeSettings.ReAuth,
                            ReturnUrl = _config["Request:URL"] + _stripeSettings.Return,
                            AccountType = _stripeSettings.Account_Onboarding,
                            UserId = userId
                        };
                        accountReturnURL.PayoutStatus = false;
                        accountReturnURL.URL = await ConnectAccountLink(accoLink).ConfigureAwait(false);
                        return accountReturnURL;
                    }

                }

            }
            accountReturnURL.PayoutStatus = userRecord!.Payouts_Enabled;
            accountReturnURL.URL = string.Empty;
            return accountReturnURL;
        }

        public async Task<string> ConnectAccountLink(ConnectAccoLink connectAcco)
        {
            var userId = connectAcco.UserId;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var options = new AccountLinkCreateOptions
            {
                Account = connectAcco.ConnectAcctId,
                RefreshUrl = connectAcco.RefreshUrl,
                ReturnUrl = connectAcco.ReturnUrl,
                Type = connectAcco.AccountType
            };
            var service = new Stripe.AccountService();
            var linkService = new AccountLinkService();
            AccountLink accountLink = await linkService.CreateAsync(options);
            return accountLink.Url;
        }

        public async Task<ConnectAccountReturnURL> GetConnectAccountStatusUrl(string userId)
        {
            ConnectAccountReturnURL accountURL = new ConnectAccountReturnURL();
            try
            {
                _logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails in BookingSystemRepository Method  -- UserId = {1}", DateTime.UtcNow, userId);

                var userDriver = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
                if (userDriver.Payouts_Enabled == false && userDriver!.StatusId!.Value.ToString().ToLower().Equals(GlobalConstants.GlobalValues.Approved.ToLower()!))
                {
                    StripeCreateAccount createAccount = new StripeCreateAccount { UserId = userDriver.Id, AccountType = _stripeSettings.AccountType, Country = _stripeSettings.Country, Email = userDriver.Email, Capabilities_card_payments_requested = true, Capabilities_transfers_requested = true };
                    accountURL = await CreateAccount(createAccount).ConfigureAwait(false);
                    userDriver!.Payouts_Enabled = accountURL!.PayoutStatus;
                    userDriver!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                    userDriver.UpdatedDate = DateTime.UtcNow;
                    await _userManager.UpdateAsync(userDriver);
                    return accountURL;
                }

                return accountURL;
            }
            catch (Exception)
            {
                return accountURL;
            }
        }

        public async Task<CustomerSetupIntentResponse> StripeSetupIntent()
        {
            var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var customerData = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(customerData.StripeCustomerId!))
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("customer", customerData.StripeCustomerId!);
                using (var client = new System.Net.Http.HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(keyValuePairs))
                    {

                        content.Headers.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _stripeSettings.SecretKey);
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        client.BaseAddress = new Uri(_stripeSettings.StripeBaseUrl + _stripeSettings.Setup_Intent);
                        using (HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content))
                        {
                            response.EnsureSuccessStatusCode();
                            StripeSetupIntentDTO res = JsonConvert.DeserializeObject<StripeSetupIntentDTO>(await response.Content.ReadAsStringAsync().ConfigureAwait(false))!;
                            var customerSetupIntentData = this.DbContextObj().TblStripeCustomersSetupIntent?.Where(x => x.CustomerId!.Equals(customerData.StripeCustomerId!)).FirstOrDefault()!;
                            var customerCustomerData = this.DbContextObj().TblStripeCustomers?.Where(x => x.StripeCustomerId!.Equals(customerData.StripeCustomerId!)).FirstOrDefault()!;
                            StripeCustomersSetupIntent stripeSetpuIntent = new StripeCustomersSetupIntent();
                            if (res.CustomerPaymentIntentId != null && string.IsNullOrEmpty(customerSetupIntentData?.CustomerPaymentIntentId))
                            {
                                stripeSetpuIntent.StripeCustomerRecordId = customerCustomerData?.Id;
                                stripeSetpuIntent.CustomerId = res.Customer;
                                stripeSetpuIntent.UserId = userId;
                                stripeSetpuIntent.Created = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(res.Created)).DateTime;
                                stripeSetpuIntent.CustomerPaymentIntentId = res.CustomerPaymentIntentId;
                                stripeSetpuIntent.EphemeralKey_Secret = res.EphemeralKey_Secret;
                                stripeSetpuIntent.Latest_Charge = res.Latest_Charge;
                                stripeSetpuIntent.Payment_Method = res.Payment_Method;
                                stripeSetpuIntent.Usage = res.Usage;
                                stripeSetpuIntent.Client_Secret = res.Client_Secret;
                                stripeSetpuIntent.Description = res.Description;
                                stripeSetpuIntent.Latest_Charge = res.Latest_Charge;
                                stripeSetpuIntent.Livemode = res.Livemode;
                                stripeSetpuIntent.Status = res.Status;
                                stripeSetpuIntent.CreatedDate = DateTime.UtcNow;
                                stripeSetpuIntent.CreatedBy = userId;
                                stripeSetpuIntent.Status = res.Status;
                                await this.DbContextObj().TblStripeCustomersSetupIntent.AddAsync(stripeSetpuIntent);
                                await this.DbContextObj().SaveChangesAsync();
                            }
                            stripeSetpuIntent.EphemeralKey_Secret = await GetEmhemeralKey(customerData.StripeCustomerId!).ConfigureAwait(false);
                            return new CustomerSetupIntentResponse { Client_Secret = res.Client_Secret, EphemeralKey_Secret = stripeSetpuIntent.EphemeralKey_Secret, CustomerId = customerData.StripeCustomerId! };
                        }
                    }
                }
            }
            return new CustomerSetupIntentResponse();
        }

        #region IsPaymentMethodAvailableForCustomer
        /// <summary>
        /// This method is used to check rider has any default payment or not
        /// </summary>
        /// <returns></returns>

        public async Task<CustomerPaymentMethodAvaliable> IsPaymentMethodAvailableForCustomer(CancellationToken cancellationToken)
        {
            try
            {
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var customerData = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(customerData.StripeCustomerId!))
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("customer", customerData.StripeCustomerId!);
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        using (var content = new FormUrlEncodedContent(keyValuePairs))
                        {

                            content.Headers.Clear();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _stripeSettings.SecretKey);
                            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                            client.BaseAddress = new Uri(string.Concat(_stripeSettings.StripeBaseUrl, _stripeSettings.Payment_Methods, customerData.StripeCustomerId!, _stripeSettings.IsAvaliable, _stripeSettings.Limit));

                            if (cancellationToken.IsCancellationRequested)
                                cancellationToken.ThrowIfCancellationRequested();

                            using (HttpResponseMessage response = await client.GetAsync(client.BaseAddress, cancellationToken))
                            {
                                response.EnsureSuccessStatusCode();

                                if (cancellationToken.IsCancellationRequested)
                                    cancellationToken.ThrowIfCancellationRequested();

                                CustomerPaymentMethodAvaliable res = JsonConvert.DeserializeObject<CustomerPaymentMethodAvaliable>(await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false))!;
                                res.IsPaymentMethodAvailable = res.Data?.Count() > 0 ? true : false;
                                return res;
                            }
                        }
                    }
                }
                return new CustomerPaymentMethodAvaliable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        public async Task<StripeTransfer> StripeDriverPaymentSystem(StripeDriverPaymentInfo driverPaymentInfo)
        {
            _logger.LogInformation("{0}InSide StripeDriverPaymentSystem in StripPaymentRepository Method RiderId ={1}   RiderCustomerId  ={2}  Rate ={3}  DriverAccountNo = {4}", DateTime.UtcNow, driverPaymentInfo.RiderId, driverPaymentInfo.RiderCustomerId, driverPaymentInfo.Rate, driverPaymentInfo.DriverAccountNo);
            try
            {
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var customerData = await _userManager.FindByIdAsync(userId);
                var driverId = this.DbContextObj().Users?.Where(x => x.StripeConnectedAccountId!.Equals(driverPaymentInfo.DriverAccountNo)).FirstOrDefault()!.Id;
                StripeDriverPaymentTransferDetailsDTO transferDetailsDTO = null!;
                var options2 = new TransferCreateOptions
                {
                    Amount = Convert.ToInt64(driverPaymentInfo.Rate),
                    Currency = "usd",
                    Destination = driverPaymentInfo.DriverAccountNo,
                    TransferGroup = "Payment_Order", // Optional: A string that identifies this transaction as part of a group
                };
                _logger.LogInformation("{0}InSide StripeDriverPaymentSystem in StripPaymentRepository Method Before  CreateAsync()  RiderId ={1}   RiderCustomerId  ={2}  Rate ={3}  DriverAccountNo = {4} Amount_Transfer ={5} Currency ={6} DriverStripAccount = {7}",
                    DateTime.UtcNow, driverPaymentInfo.RiderId, driverPaymentInfo.RiderCustomerId, driverPaymentInfo.Rate, driverPaymentInfo.DriverAccountNo, options2.Amount, options2.Currency, options2.Description);

                var transferService = new TransferService();
                Transfer transfer = await transferService.CreateAsync(options2).ConfigureAwait(false);
                StripeTransfer stripeTransfer = JsonConvert.DeserializeObject<StripeTransfer>(transfer.ToJson())!;
                _logger.LogInformation("{0}InSide StripeDriverPaymentSystem in StripPaymentRepository Method After DeserializeObject CreateAsync()  RiderId ={1}   RiderCustomerId  ={2}  Rate ={3}  DriverAccountNo = {4} Amount_Transfer ={5} Currency ={6} DriverStripAccount = {7}",
                   DateTime.UtcNow, driverPaymentInfo.RiderId, driverPaymentInfo.RiderCustomerId, driverPaymentInfo.Rate, driverPaymentInfo.DriverAccountNo, options2.Amount, options2.Currency, options2.Description);

                if (stripeTransfer!.Id != null)
                {
                    transferDetailsDTO = new StripeDriverPaymentTransferDetailsDTO
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userId,
                        IsActive = true,
                        IsDeleted = false,
                        UserId = userId,
                        RiderId = driverPaymentInfo.RiderId,
                        RiderCustomerId = driverPaymentInfo.RiderCustomerId,
                        DriverId = driverId,
                        TransferId = stripeTransfer.Id,
                        Object = stripeTransfer!.Object,
                        Amount = driverPaymentInfo.Rate,
                        AmountReversed = stripeTransfer.AmountReversed,
                        BalanceTransactionId = stripeTransfer.BalanceTransaction,
                        Created = DateTimeOffset.FromUnixTimeSeconds(stripeTransfer.Created).DateTime,
                        Currency = stripeTransfer.Currency,
                        Description = (string)stripeTransfer.Description!,
                        DestinationAccountNo = stripeTransfer.Destination,
                        DestinationPaymentId = stripeTransfer.DestinationPayment,
                        Livemode = stripeTransfer.Livemode,
                        Reversed = stripeTransfer.Reversed,
                        SourceTransaction = (string)stripeTransfer.SourceTransaction!,
                        SourceType = stripeTransfer.SourceType,
                        TransferGroup = stripeTransfer.TransferGroup,
                        Reversals_Object = null,
                        Reversals_HasMore = false,
                        Reversals_TotalCount = 0,
                        Reversals_Url = null
                    };
                    _logger.LogInformation("{0}InSide StripeDriverPaymentSystem in StripPaymentRepository Method before saving into database  RiderId ={1}   RiderCustomerId  ={2}  Rate ={3}  DriverAccountNo = {4} Amount_Transfer ={5} Currency ={6} DriverStripAccount = {7}  StripeDriverPaymentTransferDetails = {8}",
                           DateTime.UtcNow, driverPaymentInfo.RiderId, driverPaymentInfo.RiderCustomerId, driverPaymentInfo.Rate, driverPaymentInfo.DriverAccountNo, options2.Amount, options2.Currency, options2.Description, transferDetailsDTO.Id);

                    await this.DbContextObj().TblStripeDriverPaymentTransferDetails.AddAsync(_mapper.Map<StripeDriverPaymentTransferDetails>(transferDetailsDTO));
                    await this.DbContextObj().SaveChangesAsync();
                    _logger.LogInformation("{0}InSide StripeDriverPaymentSystem in StripPaymentRepository Method After saving into database  RiderId ={1}   RiderCustomerId  ={2}  Rate ={3}  DriverAccountNo = {4} Amount_Transfer ={5} Currency ={6} DriverStripAccount = {7}  StripeDriverPaymentTransferDetails = {8}",
                           DateTime.UtcNow, driverPaymentInfo.RiderId, driverPaymentInfo.RiderCustomerId, driverPaymentInfo.Rate, driverPaymentInfo.DriverAccountNo, options2.Amount, options2.Currency, options2.Description, transferDetailsDTO.Id);

                    return stripeTransfer;
                }
                _logger.LogInformation("{0}InSide StripeDriverPaymentSystem in StripPaymentRepository Method RiderId ={1}   RiderCustomerId  ={2}  Rate ={3}  DriverAccountNo = {4} Amount_Transfer ={5} Currency ={6} DriverStripAccount = {7}  StripeDriverPaymentTransferDetails = {8} Response ={9}",
       DateTime.UtcNow, driverPaymentInfo.RiderId, driverPaymentInfo.RiderCustomerId, driverPaymentInfo.Rate, driverPaymentInfo.DriverAccountNo, options2.Amount, options2.Currency, options2.Description, null, null);

                return stripeTransfer;
            }
            catch (Exception)
            {

                throw;
            }
        }





        [NonAction]
        public async Task<CaptureAfterIntent> ConfirmPaymentIntent(string? paymentMethod, string? paymentIntentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{0} InSide ConfirmPaymentIntent in StripPaymentRepository started Method IntentId ={1}  PaymentMethod = {2} CancellationToken = {3}", DateTime.UtcNow, paymentIntentId, paymentMethod, cancellationToken.IsCancellationRequested);
            try
            {
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                PaymentIntentConfirmDTO intentConfirm = new PaymentIntentConfirmDTO();
                var options = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = paymentMethod,
                    ReturnUrl = "https://www.example.com",
                };
                var service = new PaymentIntentService();
                _logger.LogInformation("{0} InSide ConfirmPaymentIntent in StripPaymentRepository Method before Strip ConfirmAsync() IntentId ={1}  PaymentMethod = {2} CancellationToken = {3}", DateTime.UtcNow, paymentIntentId, paymentMethod, cancellationToken.IsCancellationRequested);


                var data = await service.ConfirmAsync(paymentIntentId, options, null, cancellationToken).ConfigureAwait(false);
                var response = JsonConvert.DeserializeObject<CaptureAfterIntent>(data.ToJson())!;



                _logger.LogInformation("{0} InSide ConfirmPaymentIntent in StripPaymentRepository Method After Strip ConfirmAsync() DeserializeObject IntentId ={1}  PaymentMethod = {2} CancellationToken = {3}", DateTime.UtcNow, paymentIntentId, paymentMethod, cancellationToken.IsCancellationRequested);

                if (response.Id != null)
                {
                    intentConfirm!.Id = Guid.NewGuid();
                    intentConfirm.UserId = userId;
                    intentConfirm.PaymentIntentConfirmId = response.Id;
                    intentConfirm.CreatedDate = DateTime.UtcNow;
                    intentConfirm.CreatedBy = userId;
                    intentConfirm.Amount = response.Amount;
                    intentConfirm.AmountCapturable = response.AmountCapturable;
                    intentConfirm.AmountReceived = response.AmountReceived;
                    intentConfirm.CaptureMethod = response.CaptureMethod;
                    intentConfirm.ClientSecret = response.ClientSecret;
                    intentConfirm.ConfirmationMethod = response.ConfirmationMethod;
                    intentConfirm.Created = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(response.Created)).DateTime;
                    intentConfirm.Currency = response.Currency;
                    intentConfirm.CustomerId = response.Customer?.ToString() ?? response.Customer?.ToString();
                    intentConfirm.LatestCharge = response.LatestCharge;
                    intentConfirm.Livemode = response.Livemode;
                    intentConfirm.Metadata = string.Empty;
                    intentConfirm.Status = response.Status;

                    _logger.LogInformation("{0} InSide ConfirmPaymentIntent in StripPaymentRepository Method IntentId ={1}  Before Saving TblPaymentIntentConfirm with PaymentIntentConfirmId ={2} PaymentIntentConfirm = {3} CancellationToken = {4}", DateTime.UtcNow, paymentIntentId, intentConfirm.PaymentIntentConfirmId, intentConfirm!.Id, cancellationToken.IsCancellationRequested);
                    await this.DbContextObj().TblPaymentIntentConfirm.AddAsync(_mapper.Map<PaymentIntentConfirm>(intentConfirm), cancellationToken).ConfigureAwait(false);
                    await this.DbContextObj().SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("{0} InSide ConfirmPaymentIntent in StripPaymentRepository Method IntentId ={1}  Before Saving TblPaymentIntentConfirm with PaymentIntentConfirmId ={2}  PaymentIntentConfirm = {3} CancellationToken = {4}", DateTime.UtcNow, paymentIntentId, intentConfirm.Status, intentConfirm!.Id, cancellationToken.IsCancellationRequested);

                }
                else
                {
                    _logger.LogInformation("{0}InSide ConfirmPaymentIntent in StripPaymentRepository Method IntentId ={1}   IntentConfirmStatus ={2}  PaymentIntentConfirm = {3} Response ={4} CancellationToken = {5}", DateTime.UtcNow, paymentIntentId, intentConfirm.Status, null, null, cancellationToken.IsCancellationRequested);
                }
                return response;
            }

            catch (Exception)
            {

                throw;
            }
        }
        [NonAction]
        public async Task<CaptureAfterIntent> StripeCaptureIntent(string? paymentIntentId)
        {
            try
            {
                _logger.LogInformation("{0} InSide StripeCaptureIntent in StripPaymentRepository Method IntentId ={1}", DateTime.UtcNow, paymentIntentId);
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                PaymentIntentCaptureDTO intentCapture = new PaymentIntentCaptureDTO();
                _logger.LogInformation("{0} InSide StripeCaptureIntent in StripPaymentRepository Method IntentId ={1}  Before Capture Amount", DateTime.UtcNow, paymentIntentId);
                var service = new PaymentIntentService();
                var data = await service.CaptureAsync(paymentIntentId).ConfigureAwait(false);
                var response = JsonConvert.DeserializeObject<CaptureAfterIntent>(data.ToJson())!;
                _logger.LogInformation("{0} InSide StripeCaptureIntent in StripPaymentRepository Method IntentId ={1}  After Capture Amount DeserializeObject", DateTime.UtcNow, paymentIntentId);
                if (response.Id != null)
                {
                    intentCapture!.Id = Guid.NewGuid();
                    intentCapture.Application = response.Application?.ToString() ?? response.Application?.ToString();
                    intentCapture.UserId = userId;
                    intentCapture.PaymentIntentCaptureId = response.Id;
                    intentCapture.CreatedDate = DateTime.UtcNow;
                    intentCapture.CreatedBy = userId;
                    intentCapture.Amount = response.Amount;
                    intentCapture.ClientSecret = response.ClientSecret;
                    intentCapture.Metadata = response.Metadata!.ToString();
                    intentCapture.TransferGroup = response.TransferGroup?.ToString() ?? response.TransferGroup?.ToString();
                    intentCapture.AmountCapturable = response.AmountCapturable;
                    intentCapture.ApplicationFeeAmount = response.ApplicationFeeAmount?.ToString() ?? response.ApplicationFeeAmount?.ToString();
                    intentCapture.AmountDetails_Tip = string.Empty;
                    intentCapture.AmountReceived = response.AmountReceived;
                    intentCapture.AutomaticPaymentMethods = string.Empty;
                    intentCapture.CaptureMethod = response.CaptureMethod;
                    intentCapture.CanceledAt = response.CanceledAt?.ToString() ?? response.CanceledAt?.ToString();
                    intentCapture.CancellationReason = response.CancellationReason?.ToString() ?? response.CancellationReason?.ToString();
                    intentCapture.ConfirmationMethod = response.ConfirmationMethod;
                    intentCapture.Created = DateTimeOffset.FromUnixTimeSeconds(response.Created).DateTime;
                    intentCapture.Currency = response.Currency;
                    intentCapture.CustomerId = response.Customer?.ToString() ?? response.Customer?.ToString();
                    intentCapture.Description = response.Description;
                    intentCapture.Invoice = response.Invoice?.ToString() ?? response.Invoice?.ToString();
                    intentCapture.LastPaymentError = response.LastPaymentError?.ToString() ?? response.LastPaymentError?.ToString();
                    intentCapture.LatestCharge = response.LatestCharge;
                    intentCapture.Livemode = response.Livemode;
                    intentCapture.Status = response.Status;
                    intentCapture.TransferData = response.TransferData?.ToString() ?? response.TransferData?.ToString();
                    _logger.LogInformation("{0} InSide StripeCaptureIntent in StripPaymentRepository Method IntentId ={1}  Before Saving TblPaymentIntentCapture with CaptureId ={2}  PaymentIntentCapture ={3}", DateTime.UtcNow, paymentIntentId, intentCapture.PaymentIntentCaptureId, intentCapture.Id);
                    await this.DbContextObj().TblPaymentIntentCapture.AddAsync(_mapper.Map<PaymentIntentCapture>(intentCapture)).ConfigureAwait(false);
                    await this.DbContextObj().SaveChangesAsync();
                    _logger.LogInformation("{0} InSide StripeCaptureIntent in StripPaymentRepository Method IntentId ={1}  After Saving TblPaymentIntentCapture with CaptureId ={2} Response = {3}  PaymentIntentCapture ={4}", DateTime.UtcNow, paymentIntentId, intentCapture.PaymentIntentCaptureId, response.Status, intentCapture.Id);
                    if (response.Status!.Equals("succeeded"))
                    {
                        return response;
                    }
                    else
                    {

                    }
                }
                else
                {
                    _logger.LogInformation("{0} InSide StripeCaptureIntent in StripPaymentRepository Method IntentId ={1}  with CaptureId ={2} Response = {3}  PaymentIntentCapture ={4}", DateTime.UtcNow, paymentIntentId, intentCapture.PaymentIntentCaptureId, null, null);
                }
                return response;

            }

            catch (Exception)
            {

                throw;
            }
        }


        #region StripeDriverBalance
        public async Task<int> StripeDriverBalance()
        {
            var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            string userAccountId = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(userId)).Select(x => x.StripeConnectedAccountId).FirstOrDefault()!;
            if (!string.IsNullOrEmpty(userAccountId))
            {
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                var options = new BalanceGetOptions();
                var requestOptions = new RequestOptions
                {
                    StripeAccount = userAccountId,
                };
                var service = new BalanceService();
                var accountBalance = await service.GetAsync(options, requestOptions);
                var response = JsonConvert.DeserializeObject<StripeAccountBalance>(accountBalance.ToJson())!;
                if (response.Available!.Count() > 0)
                {
                    if (response.Available![0]?.Currency == "usd")
                    {
                        return (response.Available![0].Amount / 100);
                    }
                    return response.Available![0].Amount;
                }
                return 0;
            }
            return 0;
        }
        #endregion

        #region MakeDefaultPaymentMethod
        public async Task<string> MakeDefaultPaymentMethod(string PaymentMethodId)
        {
            var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var customerId = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(userId)).Select(x => x.StripeCustomerId).FirstOrDefault()!;
            if (!string.IsNullOrEmpty(PaymentMethodId) && !string.IsNullOrEmpty(customerId))
            {
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                var serviceSource = new CustomerService();
                object obj = JsonConvert.SerializeObject(await serviceSource.GetAsync(customerId).ConfigureAwait(false));
                StripeCustomerData dataResult = JsonConvert.DeserializeObject<StripeCustomerData>(obj.ToString()!)!;
                var options = new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions { DefaultPaymentMethod = PaymentMethodId }
                };
                var result = serviceSource.Update(dataResult!.Id, options);
                var data = JsonConvert.DeserializeObject<StripeCustomerData>(obj.ToString()!)!;
                if (data?.InvoiceSettings?.DefaultPaymentMethod != null)
                {
                    return GlobalResourceFile.Success;
                }

            }
            return GlobalResourceFile.Failed;
        }
        #endregion

        public async Task<string> AddMoney(DigitalWalletData money)
        {

            if (money != null)
            {
                await this.DbContextObj().TblDigitalWallet.AddAsync(_mapper.Map<DigitalWallet>(money));
                var data = await this.DbContextObj().SaveChangesAsync();
                if (data == 1)
                {
                    return GlobalResourceFile.Success;
                }
            }
            return GlobalResourceFile.Failed;
        }
    }
}