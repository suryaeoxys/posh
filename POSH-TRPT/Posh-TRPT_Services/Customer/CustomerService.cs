using AutoMapper;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.Customer;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.CustomerDTO;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Customer
{
    public  class CustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        public readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(IUnitOfWork unitOfWork
            , ICustomerRepository customerRepository
            , IMapper mapper
            , ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }


		#region GetCustomerDetails

		/// <summary>
		/// method to GetCustomerDetails
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<List<CustomerDTO>>> GetCustomerDetails()
        {
            APIResponse<List<CustomerDTO>> _APIResponse = new APIResponse<List<CustomerDTO>>();
            _logger.LogInformation("{0} InSide  GetCustomerDetails Method in CustomerService  Started", DateTime.UtcNow);
            try
            {
                var data = await _customerRepository.GetCustomerDetails();
                var customerRecord = _mapper.Map<List<CustomerDTO>>(data);
                if (await _unitOfWork.CommitAsync() > 0 || data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = customerRecord;
                    _APIResponse.Message = "Customer Details found";
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  GetCustomerDetails Method in CustomerService  Message:{1}", DateTime.UtcNow, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = customerRecord;
                    _APIResponse.Message = "Customer not found";
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide  GetCustomerDetails Method in CustomerService  Message:{1}", DateTime.UtcNow, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = GlobalResourceFile.Exception;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide  DeleteCustomer Method in CustomerService Message:{1} Error:{2} Exception:{3}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Error, ex.Message);
            }
            return _APIResponse;
        }
		#endregion


		#region GetCustomerDetailsById

		/// <summary>
		/// method to GetCustomerDetailsById
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<APIResponse<CustomerDTO>> GetCustomerDetailsById(string userId)
        {
            APIResponse<CustomerDTO> _APIResponse = new APIResponse<CustomerDTO>();
            _logger.LogInformation("{0} InSide  GetCustomerDetailsById Method in CustomerService Id:{1}  Started", DateTime.UtcNow, userId);
            try
            {
                var data = await _customerRepository.GetCustomerById(userId);
                var customerRecord = _mapper.Map<CustomerDTO>(data);
                if (await _unitOfWork.CommitAsync() > 0 || data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = customerRecord;
                    _APIResponse.Message = "Customer Details found";
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  GetCustomerDetailsById Method in CustomerService  Message:{1} userId:{2} PhoneNumber:{3}", DateTime.UtcNow, _APIResponse.Message, userId, data.MobileNumber);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = customerRecord;
                    _APIResponse.Message = "Customer not found";
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide  GetCustomerDetailsById Method in CustomerService  Message:{1} userId:{2}", DateTime.UtcNow, _APIResponse.Message, userId);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = GlobalResourceFile.Exception;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide  GetCustomerDetailsById Method in CustomerService userId:{1} Message:{2} Error:{3} Exception:{4}", DateTime.UtcNow, userId, _APIResponse.Message, _APIResponse.Error, ex.Message);
            }
            return _APIResponse;
        }
		#endregion

		#region DeleteCustomer
		/// <summary>
		/// method to DeleteCustomer
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> DeleteCustomer(string id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            _logger.LogInformation("{0} InSide  DeleteCustomer Method in CustomerService Id:{1} Started", DateTime.UtcNow, id);
            try
            {
                var driver = await _customerRepository.DeleteCustomerByAdmin(id);

                if (driver == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = driver;
                    _APIResponse.Message = EmployeeResource.DeleteSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  DeleteCustomer Method in CustomerService Id:{1} Message:{2}", DateTime.UtcNow, id, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = driver;
                    _APIResponse.Message = EmployeeResource.DeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                    _logger.LogInformation("{0} InSide  DeleteCustomer Method in CustomerService Id:{1} Message:{2}", DateTime.UtcNow, id, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide  DeleteCustomer Method in CustomerService Id:{1} Message:{2} Error:{3} Exception:{4}", DateTime.UtcNow, id, _APIResponse.Message, _APIResponse.Error, ex.Message);
            }
            return _APIResponse;
        }
		#endregion


		#region UpdateCutomerDetailsByAdmin
		/// <summary>
		/// method to UpdateCutomerDetailsByAdmin
		/// </summary>
		/// <param name="details"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> UpdateCutomerDetailsByAdmin(CustomerDTO details)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            _logger.LogInformation("{0} InSide  UpdateCutomerDetailsByAdmin Method in CustomerService  PhoneNumber:{1} Name:{2} Id:{3} Started", DateTime.UtcNow, details.MobileNumber, details.Name, details.Id);
            try
            {
                var customerRecord = _mapper.Map<UserMobileData>(details);
                var data = await _customerRepository.UpdateCustomerByAdmin(customerRecord);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  UpdateCutomerDetailsByAdmin Method in CustomerService  PhoneNumber:{1} Name:{2} Id:{3} Message:{4}", DateTime.UtcNow, details.MobileNumber, details.Name, details.Id, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.UpdateFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                    _logger.LogInformation("{0} InSide  UpdateCutomerDetailsByAdmin Method in CustomerService  PhoneNumber:{1} Name:{2} Id:{3} Message:{4}", DateTime.UtcNow, details.MobileNumber, details.Name, details.Id, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.UpdateFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide  UpdateCutomerDetailsByAdmin Method in CustomerService  PhoneNumber:{1} Name:{2} Id:{3} Error:{4} Exception:{5}", DateTime.UtcNow, details.MobileNumber, details.Name, details.Id, _APIResponse.Error, ex.Message);
            }
            return _APIResponse;
        }
		#endregion

		#region GetCustomerBasicDetails
		/// <summary>
		/// method to GetCustomerBasicDetails
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<UserMobileData>> GetCustomerBasicDetails()
		{
			APIResponse<UserMobileData> _APIResponse = new APIResponse<UserMobileData>();
			_logger.LogInformation("{0} InSide  GetCustomerBasicDetails Method in CustomerService  Started", DateTime.UtcNow);
			try
			{
				var data = await _customerRepository.GetCustomerBasicDetails();
				if (await _unitOfWork.CommitAsync() > 0 || data.Id != null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.CustomerDetailsFound;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  GetCustomerBasicDetails Method in CustomerService Message:{1} Id:{2} Name:{3} PhoneNumber:{4}", DateTime.UtcNow, _APIResponse.Message,data.Id,data.Name,data.MobileNumber);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.CustomerDetailsNotFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  GetCustomerBasicDetails Method in CustomerService Message:{1}", DateTime.UtcNow, _APIResponse.Message);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.CustomerDetailsNotFound;
				_APIResponse.Status = HttpStatusCode.InternalServerError;

				_logger.LogError("{0} InSide  GetCustomerBasicDetails Method in CustomerService  Error:{1}", DateTime.UtcNow, _APIResponse.Error);
			}
			return _APIResponse;
		}
		#endregion
		#region GetNearestDriversDetail
		/// <summary>
		/// method to GetNearestDriversDetail
		/// </summary>
		/// <param name="nearestDriver"></param>
		/// <returns></returns>
		public async Task<APIResponse<DriverMobileData>> GetNearestDriversDetail(NearestDriverRequest nearestDriver)
        {
            APIResponse<DriverMobileData> _APIResponse = new APIResponse<DriverMobileData>();
            _logger.LogInformation("{0} InSide  GetNearestDriversDetail Method in CustomerService  Started", DateTime.UtcNow);
            try
            {
                var data = await _customerRepository.GetNearestDriversDetail(nearestDriver)!;
                if (await _unitOfWork.CommitAsync() > 0 || data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = GlobalResourceFile.DriversAvailable;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  GetNearestDriversDetail Method in CustomerService Ended Message:{1} DeviceId:{2} MobileNumber:{3} Email:{4} UserId:{5}", DateTime.UtcNow, _APIResponse.Message, data.DeviceId, data.MobileNumber, data.Email, data.Id);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = GlobalResourceFile.DriversNotAvailable;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide  GetNearestDriversDetail Method in CustomerService Ended Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = GlobalResourceFile.DriversNotAvailable;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                _logger.LogError("{0} InSide  GetNearestDriversDetail Method in CustomerService  Error:{1}", DateTime.UtcNow, _APIResponse.Error);
            }
            return _APIResponse;
        } 
        #endregion
    }
}
