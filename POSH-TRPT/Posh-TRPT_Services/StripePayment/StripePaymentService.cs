using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.StripePaymentDTO;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.StripePayment
{
	public class StripePaymentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IStripePaymentRepository _paymentRepository;
		private IMapper _mapper;
		public StripePaymentService(IUnitOfWork unitOfWork, IStripePaymentRepository paymentRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_paymentRepository = paymentRepository;
			_mapper = mapper;
        }

		public async Task<APIResponse<string>> CreateCustomer(string email, string name, string userId)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				var resultData = await _paymentRepository.CreateCustomer(email, name, userId);
				if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(resultData))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.CustomerCreated;
					_APIResponse.Status = HttpStatusCode.Created;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = GlobalResourceFile.CustomerRegistrationFailed;
					_APIResponse.Status = HttpStatusCode.BadRequest;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;						
				_APIResponse.Status = HttpStatusCode.InternalServerError;

				
			}
			return _APIResponse;
		}

		public async Task<APIResponse<string>> GetEmhemeralKey(string customerId)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				var resultData = await _paymentRepository.GetEmhemeralKey(customerId);
				if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(resultData))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.KeyFound;
					_APIResponse.Status = HttpStatusCode.Found;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = GlobalResourceFile.KeyNotFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

				
			}
			return _APIResponse;
		}
		public async Task<APIResponse<StripeCustomerIntentCustom>> CreatePaymentIntent(string Currency, decimal Amount, bool isWalletApplied,double CashBackPrice)
		{
			APIResponse<StripeCustomerIntentCustom> _APIResponse = new APIResponse<StripeCustomerIntentCustom>();
			try
			{
				var resultData = await _paymentRepository.CreatePaymentIntent(Currency, Amount,isWalletApplied, CashBackPrice, new CancellationToken());
				if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(resultData.Status))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.CreatePaymentIntentResponseDataFound;
					_APIResponse.Status = HttpStatusCode.OK;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = GlobalResourceFile.CreatePaymentIntentResponseDataNotFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

			}
			return _APIResponse;
		}

		public async Task<APIResponse<string>> GetAllCustomers(string customerEmail)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				var resultData = await _paymentRepository.GetAllCustomers(customerEmail);
				if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(resultData))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.CustomerDetailsFound;
					_APIResponse.Status = HttpStatusCode.Found;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = GlobalResourceFile.CustomerDetailsFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

				
			}
			return _APIResponse;
		}

		public async Task<APIResponse<ConnectAccountReturnURL>> CreateAccount(StripeCreateAccount createAccount)
		{
			APIResponse<ConnectAccountReturnURL> _APIResponse = new APIResponse<ConnectAccountReturnURL>();
			try
			{
				var resultData = await _paymentRepository.CreateAccount(createAccount);
				if (await _unitOfWork.CommitAsync() > 0 || !resultData.PayoutStatus)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.PayoutStatusConnectAccountFalse;
					_APIResponse.Status = HttpStatusCode.OK;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.PayoutStatusConnectAccountTrue;
					_APIResponse.Status = HttpStatusCode.Found;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

			
			}
			return _APIResponse;
		}

		public async Task<APIResponse<string>> ConnectAccountLink(ConnectAccoLink connectAcco)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				var resultData = await _paymentRepository.ConnectAccountLink(connectAcco);
				if (await _unitOfWork.CommitAsync() > 0 || resultData != null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.AccountLinkCreated;
					_APIResponse.Status = HttpStatusCode.Created;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = GlobalResourceFile.AccountLinkCreationFailed;
					_APIResponse.Status = HttpStatusCode.BadRequest;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

				
			}
			return _APIResponse;
		}

		public async Task<APIResponse<ConnectAccountReturnURL>> GetConnectAccountStatusUrl(string userId)
		{
			APIResponse<ConnectAccountReturnURL> _APIResponse = new APIResponse<ConnectAccountReturnURL>();
			try
			{
				ConnectAccountReturnURL data = await _paymentRepository.GetConnectAccountStatusUrl(userId);
				if (await _unitOfWork.CommitAsync() > 0 || data.PayoutStatus)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.PayoutStatusConnectAccountTrue;
					_APIResponse.Status = HttpStatusCode.OK;
					
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.PayoutStatusConnectAccountFalse;
					_APIResponse.Status = HttpStatusCode.Found;
					
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				
			}
			return _APIResponse;
		}

		public async Task<APIResponse<CustomerSetupIntentResponse>> StripeSetupIntent()
		{
			APIResponse<CustomerSetupIntentResponse> _APIResponse = new APIResponse<CustomerSetupIntentResponse>();
			try
			{
				var resultData = await _paymentRepository.StripeSetupIntent();
				if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(resultData.Client_Secret))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.SetupIntentResponseDataFound;
					_APIResponse.Status = HttpStatusCode.OK;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = GlobalResourceFile.SetupIntentResponseDataNotFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

			
			}
			return _APIResponse;
		}

		public async Task<APIResponse<CustomerPaymentMethodAvaliable>> IsPaymentMethodAvailableForCustomer()
		{
			APIResponse<CustomerPaymentMethodAvaliable> _APIResponse = new APIResponse<CustomerPaymentMethodAvaliable>();
			try
			{
				var resultData = await _paymentRepository.IsPaymentMethodAvailableForCustomer(new CancellationToken());
				if (await _unitOfWork.CommitAsync() > 0 || resultData!.Data!.Count>0)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.PaymetMethodFound;
					_APIResponse.Status = HttpStatusCode.Found;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.NoPaymetMethodFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

			}
			return _APIResponse;
		}

		public async Task<APIResponse<StripeTransfer>> StripeDriverPaymentSystem(StripeDriverPaymentInfo driverPaymentInfo)
		{
			APIResponse<StripeTransfer> _APIResponse = new APIResponse<StripeTransfer>();
			try
			{
				var resultData = await _paymentRepository.StripeDriverPaymentSystem(driverPaymentInfo);
				if (await _unitOfWork.CommitAsync() > 0 || resultData!=null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.PaymetMethodFound;
					_APIResponse.Status = HttpStatusCode.Found;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = resultData;
					_APIResponse.Message = GlobalResourceFile.NoPaymetMethodFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.FetchFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

			}
			return _APIResponse;
		}
        public async Task<APIResponse<int>> StripeDriverBalance()
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var resultData = await _paymentRepository.StripeDriverBalance();
                if (await _unitOfWork.CommitAsync() > 0 || resultData>0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = resultData;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = resultData;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

            }
            return _APIResponse;
        }

        public async Task<APIResponse<string>> MakeDefaultPaymentMethod(string PaymentMethodId)
        {
            APIResponse<string> _APIResponse = new APIResponse<string>();
            try
            {
                var resultData = await _paymentRepository.MakeDefaultPaymentMethod(PaymentMethodId);
                if (await _unitOfWork.CommitAsync() > 0 || resultData.Equals(GlobalResourceFile.Success))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = resultData;
                    _APIResponse.Message = CommonResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = resultData;
                    _APIResponse.Message = CommonResource.UpdateFailed;
                    _APIResponse.Status = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

            }
            return _APIResponse;
        }
        public async Task<APIResponse<string>> AddMoney(DigitalWalletData money)
        {
            APIResponse<string> _APIResponse = new APIResponse<string>();
            try
            {
                var resultData = await _paymentRepository.AddMoney(money);
                if (await _unitOfWork.CommitAsync() > 0 || resultData.Equals(GlobalResourceFile.Success))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = resultData;
                    _APIResponse.Message = CommonResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = resultData;
                    _APIResponse.Message = CommonResource.UpdateFailed;
                    _APIResponse.Status = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

            }
            return _APIResponse;
        }
    }
}
