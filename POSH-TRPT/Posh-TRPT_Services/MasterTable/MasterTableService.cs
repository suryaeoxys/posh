using AutoMapper;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.MasterTable;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.MasterTableDTO;
using Posh_TRPT_Services.Register;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.MasterTable
{
    public class MasterTableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMasterTableRepository _masterRepository;
        public readonly IMapper _mapper;
        private readonly ILogger<MasterTableService> _logger;
        public MasterTableService(IUnitOfWork unitOfWork
            , IMasterTableRepository masterRepository
            , IMapper mapper,
            ILogger<MasterTableService> logger)
        {
            _unitOfWork = unitOfWork;
            _masterRepository = masterRepository;
            _mapper = mapper;
            _logger = logger;
        }


		
		#region GetCountryList
		/// <summary>
		///  all method used for Country Crud operation
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<IEnumerable<CountryDTO>>> GetCountryList()
        {
            APIResponse<IEnumerable<CountryDTO>> _APIResponse = new APIResponse<IEnumerable<CountryDTO>>();

            try
            {
                _logger.LogInformation("{0} InSide GetCountryList Method of MasterTableService Started--   started.", DateTime.UtcNow);
                var countryList = await _masterRepository.GetCountryList();

                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<CountryDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide GetCountryList Method of MasterTableService-- Message:{1} Status:{2}.", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                    _logger.LogInformation("{0} InSide GetCountryList Method of MasterTableService-- Message:{1} Status:{2}.", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide GetCountryList Method of MasterTableService--   Message:{1} Status:{2} Error:{3}.", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error);

            }


            return _APIResponse;
        } 
        #endregion

        #region UpdateCountry
        /// <summary>
        /// method to UpdateCountry
        /// </summary>
        /// <param name="countryDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse<int>> UpdateCountry(CountryDTO countryDTO)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var newCountry = _mapper.Map<Country>(countryDTO);
                var country = await _masterRepository.UpdateCountry(newCountry);

                if (await _unitOfWork.CommitAsync() > 0 || country == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = country;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.UpdateFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.UpdateFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region DeleteCountry
		/// <summary>
		/// method to DeleteCountry
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> DeleteCountry(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var countryList = await _masterRepository.DeleteCountry(id);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                }

            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region AddCountry
		/// <summary>
		/// method to AddCountry
		/// </summary>
		/// <param name="countryDTO"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> AddCountry(CountryDTO countryDTO)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var newCountry = _mapper.Map<Country>(countryDTO);
                var countryList = await _masterRepository.AddCountry(newCountry);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 2)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 3)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        } 
        #endregion
        


        #region State
        /// <summary>
        /// Have all method for State 
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<IEnumerable<StateDTO>>> GetStateList()
        {
            APIResponse<IEnumerable<StateDTO>> _APIResponse = new APIResponse<IEnumerable<StateDTO>>();

            try
            {
                var countryList = await _masterRepository.GetStateList();






                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<StateDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }

		#endregion

		#region GetStateList
		/// <summary>
		/// method to GetStateList
		/// </summary>
		/// <param name="countryId"></param>
		/// <returns></returns>
		public async Task<APIResponse<IEnumerable<StateDTO>>> GetStateList(Guid countryId)
        {
            APIResponse<IEnumerable<StateDTO>> _APIResponse = new APIResponse<IEnumerable<StateDTO>>();

            try
            {

                var countryList = await _masterRepository.GetStateList(countryId);
                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<StateDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region GetStateDetailById
		/// <summary>
		/// method to GetStateDetailById
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public async Task<APIResponse<StateDTO>> GetStateDetailById(Guid stateId)
        {
            APIResponse<StateDTO> _APIResponse = new APIResponse<StateDTO>();

            try
            {

                var countryList = await _masterRepository.GetStateDetailById(stateId);
                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<StateDTO>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region DeleteState
		/// <summary>
		/// method to DeleteState
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> DeleteState(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var countryList = await _masterRepository.DeleteState(id);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                }

            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region AddState
		/// <summary>
		/// method to AddState
		/// </summary>
		/// <param name="stateDTO"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> AddState(StateDTO stateDTO)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            stateDTO.Id = (stateDTO.Id == Guid.Empty || stateDTO.Id == null) ? Guid.NewGuid() : stateDTO.Id;

            try
            {

                var newState = _mapper.Map<State>(stateDTO);
                var stateList = await _masterRepository.AddState(newState);

                if (stateList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = stateList;
                    _APIResponse.Message = EmployeeResource.AddSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (stateList == 2)
                {

                    _APIResponse.Success = true;
                    _APIResponse.Data = stateList;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (stateList == 3)
                {

                    _APIResponse.Success = true;
                    _APIResponse.Data = stateList;
                    _APIResponse.Message = EmployeeResource.AddFailed;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = GlobalResourceFile.Exception;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region City
		#region GetCityDetailById
		/// <summary>
		/// method to GetCityDetailById
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public async Task<APIResponse<CityDTO>> GetCityDetailById(Guid stateId)
        {
            APIResponse<CityDTO> _APIResponse = new APIResponse<CityDTO>();

            try
            {

                var countryList = await _masterRepository.GetCityDetailById(stateId);
                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<CityDTO>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region DeleteCity
		/// <summary>
		/// method to DeleteCity
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> DeleteCity(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var countryList = await _masterRepository.DeleteCity(id);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                }

            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region AddCity
		/// <summary>
		/// method to AddCity
		/// </summary>
		/// <param name="cityDTO"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> AddCity(CityDTO cityDTO)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            cityDTO.Id = (cityDTO.Id == Guid.Empty || cityDTO.Id == null) ? Guid.NewGuid() : cityDTO.Id;

            try
            {
                var newCountry = _mapper.Map<City>(cityDTO);
                var countryList = await _masterRepository.AddCity(newCountry);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.AddSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 2)
                {

                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 3)
                {

                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.AddFailed;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = GlobalResourceFile.Exception;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region GetCityList
		/// <summary>
		/// method to GetCityList
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<IEnumerable<CityDTO>>> GetCityList()
        {
            APIResponse<IEnumerable<CityDTO>> _APIResponse = new APIResponse<IEnumerable<CityDTO>>();
            try
            {
                var countryList = await _masterRepository.GetCityList();


                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<CityDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        } 
        #endregion
        #endregion


        #region All method used for RideCategory
        /// <summary>
        ///  all method used for RideCategory Crud operation
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<IEnumerable<RideCategoryDTO>>> GetRideCategoryList()
        {
            APIResponse<IEnumerable<RideCategoryDTO>> _APIResponse = new APIResponse<IEnumerable<RideCategoryDTO>>();

            try
            {
                var countryList = await _masterRepository.GetRideCategoryList();

                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<RideCategoryDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }


		#region UpdateRideCategory
		/// <summary>
		/// method to UpdateRideCategory
		/// </summary>
		/// <param name="countryDTO"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> UpdateRideCategory(RideCategoryDTO countryDTO)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var newCountry = _mapper.Map<RideCategory>(countryDTO);
                var country = await _masterRepository.UpdateRideCategory(newCountry);

                if (await _unitOfWork.CommitAsync() > 0 || country == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = country;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.UpdateFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.UpdateFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion

		#region DeleteRideCategory

		/// <summary>
		/// method to DeleteRideCategory
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> DeleteRideCategory(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var countryList = await _masterRepository.DeleteRideCategory(id);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                }

            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region AddRideCategory
		/// <summary>
		/// method to AddRideCategory
		/// </summary>
		/// <param name="countryDTO"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> AddRideCategory(RideCategoryDTO countryDTO)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var newCountry = _mapper.Map<RideCategory>(countryDTO);
                var countryList = await _masterRepository.AddRideCategory(newCountry);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 2)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 3)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.AddFailed;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion
		#endregion

		#region GetCategoryPriceList
		/// <summary>
		/// method to GetCategoryPriceList
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<IEnumerable<CategoryPriceDTO>>> GetCategoryPriceList()
        {
            APIResponse<IEnumerable<CategoryPriceDTO>> _APIResponse = new APIResponse<IEnumerable<CategoryPriceDTO>>();
            try
            {
                var countryList = await _masterRepository.GetCategoryPriceList();


                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<CategoryPriceDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion

		#region AddCategoryPrice

		/// <summary>
		/// method to AddCategoryPrice
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> AddCategoryPrice(CategoryPriceDTO model)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();
            model.Id = (model.Id == Guid.Empty) ? Guid.NewGuid() : model.Id;

            try
            {
                var newCountry = _mapper.Map<CategoryPrice>(model);
                var countryList = await _masterRepository.AddCategoryPrice(newCountry);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.AddSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 2)
                {

                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.UpdateSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else if (countryList == 3)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.AddFailed;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = GlobalResourceFile.Exception;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion


		#region GetCategoryPriceDetailById
		/// <summary>
		/// method to GetCategoryPriceDetailById
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public async Task<APIResponse<CategoryPriceDTO>> GetCategoryPriceDetailById(Guid categoryId)
        {
            APIResponse<CategoryPriceDTO> _APIResponse = new APIResponse<CategoryPriceDTO>();

            try
            {

                var countryList = await _masterRepository.GetCategoryPriceDetailById(categoryId);
                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<CategoryPriceDTO>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        } 
        #endregion



        /// <summary>
        ///  all method used for Country Crud operation
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse<IEnumerable<StatusDTO>>> GetStatusList()
        {
            APIResponse<IEnumerable<StatusDTO>> _APIResponse = new APIResponse<IEnumerable<StatusDTO>>();

            try
            {
                var countryList = await _masterRepository.GetStatusList();

                if (countryList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<StatusDTO>>(countryList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }

		#region DeleteCategoryPrice
		/// <summary>
		/// method to DeleteCategoryPrice
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<APIResponse<int>> DeleteCategoryPrice(Guid id)
        {
            APIResponse<int> _APIResponse = new APIResponse<int>();

            try
            {
                var countryList = await _masterRepository.DeleteCategoryPrice(id);

                if (countryList == 1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = countryList;
                    _APIResponse.Message = EmployeeResource.DeleteFailed;
                    _APIResponse.Status = HttpStatusCode.NoContent;
                }

            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
		#endregion
		#region GetCityDetailByStateId

		/// <summary>
		/// method to GetCityDetailByStateId
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public async Task<APIResponse<IEnumerable<CityDTO>>> GetCityDetailByStateId(Guid stateId)
        {
            APIResponse<IEnumerable<CityDTO>> _APIResponse = new APIResponse<IEnumerable<CityDTO>>();

            try
            {

                var cityList = await _masterRepository.GetCityByStateId(stateId);
                if (cityList != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<CityDTO>>(cityList);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        } 
        #endregion

    }
}
