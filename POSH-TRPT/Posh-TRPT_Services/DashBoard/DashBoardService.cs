using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.DashBoard;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.Report;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.Resources;
using System.Net;

namespace Posh_TRPT_Services.DashBoard
{
    public class DashBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashBoardRepository _dashBoardRepository;
        public readonly IMapper _mapper;
        private readonly ILogger<DashBoardService> _logger;
        public DashBoardService(IUnitOfWork unitOfWork
            , IDashBoardRepository dashBoardRepository
            , IMapper mapper,
            ILogger<DashBoardService> logger)
        {
            _unitOfWork = unitOfWork;
            _dashBoardRepository = dashBoardRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #region GetDriverRiderCounts
        public async Task<APIResponse<DriverRiderRideCounts>> GetDriverRiderRideCounts()
        {
            APIResponse<DriverRiderRideCounts> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide  GetDriverRiderRideCounts in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.GetDriverRiderRideCounts();
                if (data != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
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

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region Get MonthlyCompletedCanceledRidesData
        public async Task<APIResponse<IEnumerable<MonthlyCompletedCanceledRides>>> MonthlyCompletedCanceledRidesData()
        {
            APIResponse<IEnumerable<MonthlyCompletedCanceledRides>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide MonthlyCompletedCanceledRidesData in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.MonthlyRidesDetails();
                if (data != null && data.Count() > 0)
                { 
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
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

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region Get Currently Running Rides
        public async Task<APIResponse<IEnumerable<CurrentlyRunningRides>>> CurrentlyRunningRides()
        {
            APIResponse<IEnumerable<CurrentlyRunningRides>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide CurrentlyRunningRides in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.CurrentlyRunningRides();
                if (  data != null && data.Count() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
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

                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region Get Drivers Data With Earnings
        public async Task<APIResponse<IEnumerable<DriversDataResponse>>> GetDriversDataWithEarnings()
        {
            APIResponse<IEnumerable<DriversDataResponse>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide GetDriversDataWithEarnings in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.GetDriversDataWithEarnings();
                if (data != null && data.Count() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide GetDriversDataWithEarnings status {1} in DashBoardService Method ", DateTime.UtcNow, _APIResponse.Status);
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

        #region Total Earnings Details
        public async Task<APIResponse<IEnumerable<TotalEarningByDate>>> TotalEarningsDetails()
        {
            APIResponse<IEnumerable<TotalEarningByDate>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide TotalEarningsDetails in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.TotalEarningsDetails();
                if (data != null && data.Count() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
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
                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region Get Orders Counts With Status
        public async Task<APIResponse<IEnumerable<OrdersCountWithStatus>>> GetOrdersPercentageWithStatus()
        {
            APIResponse<IEnumerable<OrdersCountWithStatus>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide GetOrdersPercentageWithStatus in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.GetOrdersCountsWithStatus();
                if (data != null && data.Count() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
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
                throw;
            }
            return _APIResponse;
        }
        #endregion

        #region Top 10 Rated Drivers
        public async Task<APIResponse<IEnumerable<DriverRating>>> Top10RatedDrivers()
        {
            APIResponse<IEnumerable<DriverRating>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide GetOrderStatuses in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.Top10RatedDrivers();
                if (data != null && data.Count() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
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


        #region Top 5 Earned Drivers
        public async Task<APIResponse<IEnumerable<DriverEarning>>> Top5EarnedDrivers()
        {
            APIResponse<IEnumerable<DriverEarning>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide Top5EarnedDrivers in DashBoardService Method ", DateTime.UtcNow);
                var data = await _dashBoardRepository.Top5EarnedDrivers();
                if (data != null && data.Count() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
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

    }
}
