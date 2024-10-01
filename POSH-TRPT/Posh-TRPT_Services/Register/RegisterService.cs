using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.Token;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.DriverDTO;
using Posh_TRPT_Models.DTO.RegisterDTO;
using Posh_TRPT_Services.Token;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Register
{
    public class RegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegisterRepository _registerRepository;
        public readonly IMapper _mapper;
        private readonly ILogger<RegisterService> _logger;
        public RegisterService(IUnitOfWork unitOfWork
            , IRegisterRepository registerRepository
            , IMapper mapper,
            ILogger<RegisterService> logger)
        {
            _unitOfWork = unitOfWork;
            _registerRepository = registerRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #region GetCities
        public async Task<APIResponse<List<SelectListItem>>> GetCities()
        {
            APIResponse<List<SelectListItem>> _APIResponse = new APIResponse<List<SelectListItem>>();
            try
            {
                var data = _registerRepository.GetCities();
                if (await _unitOfWork.CommitAsync() > 0 || data.Result.Count! >= 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data.Result;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    if (data.Result.Count == 0)
                    {
                        _APIResponse.Status = HttpStatusCode.NoContent;
                    }
                    else
                    {
                        _APIResponse.Status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetCityById
        public async Task<APIResponse<List<CityData>>> GetCityById(string stateId)
        {
            APIResponse<List<CityData>> _APIResponse = new APIResponse<List<CityData>>();
            try
            {
                var data = _registerRepository.GetCityById(stateId);
                if (await _unitOfWork.CommitAsync() > 0 || data.Result.Count! >= 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data.Result;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    if (data.Result.Count == 0)
                    {
                        _APIResponse.Status = HttpStatusCode.NoContent;
                    }
                    else
                    {
                        _APIResponse.Status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

         #region GetCityById
        public async Task<APIResponse<Guid?>> AppExists(string countryName, string stateName, string cityName)
        {
            APIResponse<Guid?> _APIResponse = new APIResponse<Guid?>();
            try
            {
				_logger.LogInformation("{0} InSide AppExists Method of RegisterService Started--   countryName : {1},  stateName : {2},  cityName :{3}  started.", DateTime.UtcNow, countryName, stateName, cityName);

				var data = _registerRepository.AppExists(countryName, stateName,  cityName);
                if (await _unitOfWork.CommitAsync() > 0 || data.Result != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data.Result;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;

					_logger.LogInformation("{0} InSide AppExists Method of RegisterService Ended --   countryName : {1},  stateName : {2},  cityName :{3} Message : {4} CityId : {5}  end.", DateTime.UtcNow, countryName, stateName, cityName,_APIResponse.Message, data.Result);
				}
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;

					_logger.LogInformation("{0} InSide AppExists Method of RegisterService Ended --   countryName : {1},  stateName : {2},  cityName :{3}  Message : {4} CityId : {5}  ended.", DateTime.UtcNow, countryName, stateName, cityName, _APIResponse.Message,"");
				}
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide AppExists Method of RegisterService Ended --   countryName : {1},  stateName : {2},  cityName :{3}  Message : {4}  ended.", DateTime.UtcNow, countryName, stateName, cityName, _APIResponse.Message);

				
            }
            return _APIResponse;
        }
        #endregion

        #region GetStateById
        public async Task<APIResponse<List<StateData>>> GetStateById(string stateId)
        {
            APIResponse<List<StateData>> _APIResponse = new APIResponse<List<StateData>>();
            try
            {
                var data = _registerRepository.GetStateById(stateId);
                if (await _unitOfWork.CommitAsync() > 0 || data.Result.Count! >= 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data.Result;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    if (data.Result.Count == 0)
                    {
                        _APIResponse.Status = HttpStatusCode.NoContent;
                    }
                    else
                    {
                        _APIResponse.Status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetStates
        public async Task<APIResponse<List<SelectListItem>>> GetStates()
        {
            APIResponse<List<SelectListItem>> _APIResponse = new APIResponse<List<SelectListItem>>();
            try
            {
                var data = _registerRepository.GetStates();
                if (await _unitOfWork.CommitAsync() > 0 || data.Result.Count! >= 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data.Result;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    if (data.Result.Count == 0)
                    {
                        _APIResponse.Status = HttpStatusCode.NoContent;
                    }
                    else
                    {
                        _APIResponse.Status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetCountries
        public async Task<APIResponse<List<CountryData>>> GetCountries()
        {
            APIResponse<List<CountryData>> _APIResponse = new APIResponse<List<CountryData>>();
            try
            {
                var data = _registerRepository.GetCountries();
                if (await _unitOfWork.CommitAsync() > 0 || data.Result.Count! >= 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data.Result;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    if (data.Result.Count == 0)
                    {
                        _APIResponse.Status = HttpStatusCode.NoContent;
                    }
                    else
                    {
                        _APIResponse.Status = HttpStatusCode.OK;
                    }

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = CommonResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetDocumentsById
        public async Task<APIResponse<DriverDocumentDTO>> GetDocumentsById(Guid id)
        {
            APIResponse<DriverDocumentDTO> _APIResponse = new APIResponse<DriverDocumentDTO>();
            try
            {
                var data = await _registerRepository.GetDriverDocuments(id);
                if (await _unitOfWork.CommitAsync() > 0 || data is not null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<DriverDocumentDTO>(data);
                    _APIResponse.Message = DriverRegisterResource.DocumentFound;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.DocumentNotFound;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DocumentNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region AddDocuments
        public async Task<APIResponse<int>> AddDocuments(DriverDocuments documents)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.AddDriverDocuments(documents);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DocumentAdded;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.DocumentsAddFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DocumentsAddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region AddDriverMasterDetails
        public async Task<APIResponse<MasterRegisterResponse>> AddDriverMasterDetails(MasterRegister masterRegister)
        {
            APIResponse<MasterRegisterResponse> _APIResponse = new APIResponse<MasterRegisterResponse>();
            try
            {
                _logger.LogInformation("{0} InSide AddDriverMasterDetails Method Ended--   userId:{1} started.", DateTime.UtcNow, masterRegister.UserId);
                var data = await _registerRepository.AddDriverMasterDetails(masterRegister);
                _logger.LogInformation("{0} InSide AddDriverMasterDetails Method Ended--   userId:{1} Message:{2}", DateTime.UtcNow, masterRegister.UserId, data.GeneralMessage);
                if (await _unitOfWork.CommitAsync() > 0 || data is not null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = data.GeneralMessage!;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide AddDriverMasterDetails Method Ended--   userId:{1} Message:{2}", DateTime.UtcNow, masterRegister.UserId, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = data!.GeneralMessage!;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide AddDriverMasterDetails Method Ended--   userId:{1} Message:{2}", DateTime.UtcNow, masterRegister.UserId, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DocumentsAddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide AddDriverMasterDetails Method Ended--   userId:{1} Message:{2} Error:{3}", DateTime.UtcNow, masterRegister.UserId, _APIResponse.Message, _APIResponse.Error);
            }
            return _APIResponse;
        }
        #endregion

        #region UpdateDocument
        public async Task<APIResponse<int>> UpdateDocument(DriverDocuments documents)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.UpdateDriverDocuments(documents);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DocumentUpdated;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.DocumentsUpdateFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DocumentsUpdateFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region DeleteDocument
        public async Task<APIResponse<int>> DeleteDocument(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.DeleteDriverDocuments(id);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DocumentDeleted;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.DocumentDeleteFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DocumentDeleteFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetVehicleDetailsById
        public async Task<APIResponse<VehicleDetailDTO>> GetVehicleDetailsById(Guid id)
        {
            APIResponse<VehicleDetailDTO> _APIResponse = new APIResponse<VehicleDetailDTO>();
            try
            {
                var data = _mapper.Map<VehicleDetailDTO>(await _registerRepository.GetVehicleDetailsById(id));
                if (await _unitOfWork.CommitAsync() > 0 || data is not null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailFound;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailNotFound;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.VehicleDetailNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region RegisterVehicleDetails
        public async Task<APIResponse<int>> RegisterVehicleDetails(VehicleDetail vehicleDetail)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.RegisterVehicleDetails(vehicleDetail);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailsAdded;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailsAddFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.VehicleDetailsAddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region UpdateVehicleDetails
        public async Task<APIResponse<int>> UpdateVehicleDetails(VehicleDetail vehicleDetail)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.UpdateVehicleDetails(vehicleDetail);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailsUpdated;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailsUpdateFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.VehicleDetailsUpdateFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region DeleteVehicleDetails
        public async Task<APIResponse<int>> DeleteVehicleDetails(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.DeleteVehicleDetails(id);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.VehicleDetailsDeteted;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.VehicleDeleteFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.VehicleDeleteFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetAddressById
        public async Task<APIResponse<GeneralAddressDTO>> GetAddressById(Guid id)
        {
            APIResponse<GeneralAddressDTO> _APIResponse = new APIResponse<GeneralAddressDTO>();
            try
            {
                var data = _mapper.Map<GeneralAddressDTO>(await _registerRepository.GetAddressById(id));
                if (await _unitOfWork.CommitAsync() > 0 || data is not null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.AddressDetailFound;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.AddressNotFound;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.AddressNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region RegisterAddress
        public async Task<APIResponse<int>> RegisterAddress(GeneralAddress generalAddress)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.RegisterAddress(generalAddress);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.AddressDetailAdded;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.AddressDetailsAddFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.AddressDetailsAddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region UpdateAddress
        public async Task<APIResponse<int>> UpdateAddress(GeneralAddress generalAddress)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.UpdateAddress(generalAddress);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.AddressDetailsUpdated;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.AddressDetailUpdateFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.AddressDetailUpdateFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region DeleteAddress
        public async Task<APIResponse<int>> DeleteAddress(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.DeleteAddress(id);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.AddressDetailsDeleted;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.AddressDeleteFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.AddressDeleteFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region DeleteUser
        public async Task<APIResponse<int>> DeleteUser(string id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                var data = await _registerRepository.DeleteUser(id);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDelete;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = DriverRegisterResource.DriverDeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DriverDeleteFailed;
                _APIResponse.Status = HttpStatusCode.NotFound;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetDriverMasterDetails
        public async Task<APIResponse<DriverDataResponse>> GetDriverMasterDetails()
        {
            APIResponse<DriverDataResponse> _APIResponse = new APIResponse<DriverDataResponse>();
            try
            {
                var data = await _registerRepository.GetDriverMasterDetails();
                if (await _unitOfWork.CommitAsync() > 0 || data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDetailsFound;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region GetAllDriversBasicDetails
        public async Task<APIResponse<List<DriverUserData>>> GetAllDriversBasicDetails()
        {
            APIResponse<List<DriverUserData>> _APIResponse = new APIResponse<List<DriverUserData>>();
            try
            {
                var data = await _registerRepository.GetAllDriversBasicDetails();
                if (await _unitOfWork.CommitAsync() > 0 || data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDetailsFound;
                    _APIResponse.Status = HttpStatusCode.OK;

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }
            return _APIResponse;
        }


        #endregion

        #region GetDriverMasterDetailsById
        public async Task<APIResponse<DriverDataResponse>> GetDriverMasterDetailsById(string userId)
        {
            APIResponse<DriverDataResponse> _APIResponse = new APIResponse<DriverDataResponse>();
            try
            {
                _logger.LogInformation("{0} InSide GetDriverMasterDetailsById Method of RegisterService Started--   UserId:{1} Started.", DateTime.UtcNow, userId);
                var data = await _registerRepository.GetDriverMasterDetailsById(userId);
                if (await _unitOfWork.CommitAsync() > 0 || data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDetailsFound;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide GetDriverMasterDetailsById Method of RegisterService Started--   UserId:{1} DriverDocumentId:{2} DriverEmail:{3} Message:{4}.", DateTime.UtcNow, userId, data!.DocumentsData!.Id, data!.User!.Email, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide GetDriverMasterDetailsById Method of RegisterService Started--   UserId:{1} Message:{2}.", DateTime.UtcNow, userId, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                _logger.LogError("{0} InSide GetDriverMasterDetailsById Method --   UserId:{1} Error:{2}", DateTime.UtcNow, userId, _APIResponse.Error);
            }
            return _APIResponse;
        }
        #endregion

        #region SetUserApprovalStatus
        public async Task<APIResponse<bool>> SetUserApprovalStatus(UserApprovalStatus approvalStatus)
        {
            APIResponse<bool> _APIResponse = new APIResponse<bool>();
            try
            {
                _logger.LogInformation("{0} InSide SetUserApprovalStatus Method of RegisterService Started--   StatusId:{1} RideCategoryId:{2} Message:{3} UserId:{4}  started.", DateTime.UtcNow, approvalStatus.StatusId, approvalStatus.RideCategoryId, approvalStatus.Message, approvalStatus.UserId);
                var data = await _registerRepository.SetUserApprovalStatus(approvalStatus);
                if (await _unitOfWork.CommitAsync() > 0 || data)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverStatusUpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide SetUserApprovalStatus Method of RegisterService Started--   StatusId:{1} RideCategoryId:{2} Message:{3} UserId:{4} Message:{5}.", DateTime.UtcNow, approvalStatus.StatusId, approvalStatus.RideCategoryId, approvalStatus.Message, approvalStatus.UserId, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DriverStatusUpdateFailed;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide SetUserApprovalStatus Method of RegisterService Started--   StatusId:{1} RideCategoryId:{2} Message:{3} UserId:{4} Message:{5}.", DateTime.UtcNow, approvalStatus.StatusId, approvalStatus.RideCategoryId, approvalStatus.Message, approvalStatus.UserId, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide SetUserApprovalStatus Method of RegisterService Started--   StatusId:{1} RideCategoryId:{2} Message:{3} UserId:{4} Error:{5}.", DateTime.UtcNow, approvalStatus.StatusId, approvalStatus.RideCategoryId, approvalStatus.Message, approvalStatus.UserId, _APIResponse.Error);
            }
            return _APIResponse;
        }
        #endregion

        #region AddOrUpdateDriverLocationInfo
        public async Task<APIResponse<int>> AddOrUpdateDriverLocationInfo(DriverLocationUpdateModel locationData)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            try
            {
                _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterService Started--   DeviceId:{1} Latitude:{2} Longitude:{3}  started.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude);
                var data = await _registerRepository.AddOrUpdateDriverLocationInfo(locationData);
                if (await _unitOfWork.CommitAsync() > 0 || data == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DetailsUpdated;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterService Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Message:{4}.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = DriverRegisterResource.DetailUpdateFails;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterService Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Message:{4}.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterService Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Error:{4}..", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, _APIResponse.Error);
            }
            return _APIResponse;
        }
		#endregion

		#region AddOrUpdateDriverLocationInfo
		public async Task<APIResponse<string>> UpdateUserProfile(UserProfileModel userProfile)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				_logger.LogInformation("{0} InSide UpdateUserProfile Method of RegisterService Started--   UserId:{1} Name:{2} Dob:{3} ProfilePic:{4} Email:{5}  started.", DateTime.UtcNow, userProfile.UserId, userProfile.Name, userProfile.Dob,userProfile.ProfilePic, userProfile.Email);
				var data = await _registerRepository.UpdateUserProfile(userProfile);
				if (await _unitOfWork.CommitAsync() > 0 || data.Equals(CommonResource.UpdateSuccess))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.UpdateSuccess;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide UpdateUserProfile Method of RegisterService --   UserId:{1} Name:{2} Dob:{3} ProfilePic:{4} Email:{5} Message:{6}.", DateTime.UtcNow, userProfile.UserId, userProfile.Name, userProfile.Dob, userProfile.ProfilePic, userProfile.Email, _APIResponse.Message);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.UpdateFailed;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide UpdateUserProfile Method of RegisterService --   UserId:{1} Name:{2} Dob:{3} ProfilePic:{4} Email:{5} Message:{6}.", DateTime.UtcNow, userProfile.UserId, userProfile.Name, userProfile.Dob, userProfile.ProfilePic, userProfile.Email, _APIResponse.Message);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = CommonResource.UpdateFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide UpdateUserProfile Method of RegisterService Started--  UserId:{1} Name:{2} Dob:{3} ProfilePic:{4} Email:{5} Error:{4}..", DateTime.UtcNow, userProfile.UserId, userProfile.Name, userProfile.Dob, userProfile.ProfilePic, userProfile.Email, _APIResponse.Error);
			}
			return _APIResponse;
		}
		#endregion

		#region DeleteUserAccount
		public async Task<APIResponse<string>> DeleteUserAccount(string id)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				_logger.LogInformation("{0} InSide DeleteUserAccount Method of RegisterService Started--   UserId:{1}  started.", DateTime.UtcNow,id);
				var data = await _registerRepository.DeleteUserAccount(id);
				if (await _unitOfWork.CommitAsync() > 0 || data!.Equals("success"))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.DeleteSuccess;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide DeleteUserAccount Method of RegisterService --   UserId:{1} Message:{2}. ended", DateTime.UtcNow,id,_APIResponse.Message);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.DeleteFailed;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide DeleteUserAccount Method of RegisterService --   UserId:{1}  Message:{2}. ended", DateTime.UtcNow,id, _APIResponse.Message);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide DeleteUserAccount Method of RegisterService Started--  UserId:{1}  Error:{2}", DateTime.UtcNow, id, _APIResponse.Error);
			}
			return _APIResponse;
		}
		#endregion

	}
}

