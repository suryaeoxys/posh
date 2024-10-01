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

namespace Posh_TRPT_Services.Report
{
    public class ReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportRepository _reportRepository;
        public readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IUnitOfWork unitOfWork
            , IReportRepository reportRepository
            , IMapper mapper,
            ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Get Filtered Data Of Orders
        public async Task<APIResponse<ReportData>> GetFilteredDataOfOrders(string startDate, string endDate, int statusType, Guid? driverId)
        {
            APIResponse<ReportData> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide GetOrderStatuses in DashBoardService Method ", DateTime.UtcNow);
                var data = await _reportRepository.GetFilteredDataOfOrders(startDate, endDate,statusType,driverId);
                if (data != null && data.ReportOrderData?.Count() > 0)
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

        #region GetAllStatusForReport
        public async Task<APIResponse<IEnumerable<BookingStatus>>> GetAllStatusForReport()
        {
            APIResponse<IEnumerable<BookingStatus>> _APIResponse = new();
            try
            {
                _logger.LogInformation("{0} InSide GetOrderStatuses in DashBoardService Method ", DateTime.UtcNow);
                var data = await _reportRepository.GetAllStatusForReport();
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
