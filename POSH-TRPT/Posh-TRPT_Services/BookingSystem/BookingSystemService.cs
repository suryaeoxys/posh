using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.BookingSystemDTO;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;
using BookingStatusResponse = Posh_TRPT_Domain.BookingSystem.BookingStatusResponse;
using RideBookingStatus = Posh_TRPT_Domain.BookingSystem.RideBookingStatus;
using RiderSourceLocation = Posh_TRPT_Domain.BookingSystem.RiderSourceLocation;

namespace Posh_TRPT_Services.BookingSystem
{
	public class BookingSystemService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBookingSystemRepository _bookingSystemRepository;
		public readonly IMapper _mapper;
		private readonly ILogger<BookingSystemService> _logger;
		public BookingSystemService(IUnitOfWork unitOfWork
			, IBookingSystemRepository bookingSystemRepository
			, IMapper mapper,
			ILogger<BookingSystemService> logger)
		{
			_unitOfWork = unitOfWork;
			_bookingSystemRepository = bookingSystemRepository;
			_mapper = mapper;
			_logger = logger;
		}




		#region GetAvailableCategoryPrice
		/// <summary>
		/// Service to get available category price
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		public async Task<APIResponse<PriceResponse>> GetAvailableCategoryPrice(DistanceCalculateDTO model)
		{
			APIResponse<PriceResponse> _APIResponse = new APIResponse<PriceResponse>();


			try
			{


				_logger.LogInformation("{0} InSide  GetAvailableCategoryPrice in BookingSystemService Method Started--  Source_Latitude : {1} Source_Longitude: {2}  Destination_Latitude : {3} Destination_Longitude: {4}  CityId : {5}  MinimumDistance : {6} RiderDetailsId: {7} StateId: {8}",
					DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Destination!.Latitude, model.Destination.Longitude, model.StateId, model.MinimumDistance,model.riderDetail?.Id, model.StateId);
				DistanceCalculate distanceModel = _mapper.Map<DistanceCalculate>(model);
                PriceResponse  data = await _bookingSystemRepository.GetAvailableCategoryPrice(distanceModel);
				List<AvailableCategoryFareDTO> result = _mapper.Map<List<AvailableCategoryFareDTO>>(data.Record);
                if (await _unitOfWork.CommitAsync() > 0 || data.Record?.Count() > 0)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = "List of Available Price base on Category";
					_APIResponse.Status = HttpStatusCode.OK;

					_logger.LogInformation("{0} InSide  GetAvailableCategoryPrice in BookingSystemService Method End--  Source_Latitude : {1} Source_Longitude: {2}  Destination_Latitude : {3} Destination_Longitude: {4}  CityId : {5}  MinimumDistance : {6}   Message:{7}",
				   DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Destination.Latitude, model.Destination.Longitude, model.StateId, model.MinimumDistance, _APIResponse.Message);

				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = null;
					_APIResponse.Message = "No Category is avaiable";
					_APIResponse.Status = HttpStatusCode.NotFound;

					_logger.LogInformation("{0} InSide  GetAvailableCategoryPrice in BookingSystemService Method End--  Source_Latitude : {1} Source_Longitude: {2}  Destination_Latitude : {3} Destination_Longitude: {4}  CityId : {5}  MinimumDistance : {6}   Message:{7}",
				 DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Destination.Latitude, model.Destination.Longitude, model.StateId, model.MinimumDistance, _APIResponse.Message);
				}


			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  GetAvailableCategoryPrice in BookingSystemService Method End--  Source_Latitude : {1} Source_Longitude: {2}  Destination_Latitude : {3} Destination_Longitude: {4}  CityId : {5}  MinimumDistance : {6}   Error:{7} Exception:{8}",
			   DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Destination!.Latitude, model.Destination.Longitude, model.StateId, model.MinimumDistance, _APIResponse.Error, ex.Message);


			}
			return _APIResponse;
		}
		#endregion



		#region RideBookingNotifyDriver
		/// <summary>
		/// Service to nofify the driver 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<APIResponse<UserData>> RideBookingNotifyDriver(DistanceCalculateDTO model, CancellationToken cancellationToken)
		{
			APIResponse<UserData> _APIResponse = new APIResponse<UserData>();
			try
			{
				_logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemService Method Started-- source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} CityId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken ={15}", DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination!.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId,cancellationToken.IsCancellationRequested);
				
				DistanceCalculate distanceModel = _mapper.Map<DistanceCalculate>(model);
				UserData data = await _bookingSystemRepository.RideBookingNotifyDriver(distanceModel, cancellationToken);

				if (await _unitOfWork.CommitAsync() > 0 || data != null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = "Driver avaiable";
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemService Method  End --   Latitude : {1}  Longitude: {2} Message:{3} CancellationToken ={4}", DateTime.UtcNow, model.Source.Latitude, model.Source.Longitude, _APIResponse.Message,cancellationToken.IsCancellationRequested);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = null;
					_APIResponse.Message = "No Driver is avaiable";
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemService Method  End--   Latitude : {1}  Longitude: {2} Message:{3} CancellationToken = {4}", DateTime.UtcNow, model.Source.Latitude, model.Source.Longitude, _APIResponse.Message, cancellationToken.IsCancellationRequested);
				}
			}



            catch (OperationCanceledException)
			{
                _APIResponse.Success = true;
                _APIResponse.Data = null;
                _APIResponse.Message = "Request is canceled by user";
				_APIResponse.Status = (HttpStatusCode)499;
                _logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemService Method  End--   Latitude : {1}  Longitude: {2} Message:{3} CancellationToken = {4}", DateTime.UtcNow, model.Source?.Latitude, model.Source?.Longitude, _APIResponse.Message, cancellationToken.IsCancellationRequested);

            }

            catch (Exception ex)
			{

                    _APIResponse.Success = false;
                    _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                    _APIResponse.Message = GlobalResourceFile.Exception;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                    _logger.LogError("{0} InSide  RideBookingNotifyDriver in BookingSystemService Method --   Latitude : {1}  Longitude: {2} Error:{3} Exception:{4} CancellationToken = {5}", DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, _APIResponse.Error, ex.Message, cancellationToken.IsCancellationRequested);
                
            }
			return _APIResponse;
		}
        #endregion

        #region RideBookingStatusUpdate
        /// <summary>
        /// Service to update 
        /// </summary>
        /// <param name="bookingStatus"></param>
        /// <returns></returns>
        public async Task<APIResponse<BookingStatusResponse>> RideBookingStatusUpdate(RideBookingStatus bookingStatus)
		{
			APIResponse<BookingStatusResponse> _APIResponse = new APIResponse<BookingStatusResponse>();
			try
			{
				_logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method Started-- IsAccepted : {1} RiderId: {2}", DateTime.UtcNow, bookingStatus.IsAccepted, bookingStatus.RiderId);
				BookingStatusResponse data = await _bookingSystemRepository.RideBookingStatusUpdate(bookingStatus, new BookingDetail());
				if (await _unitOfWork.CommitAsync() > 0 || data.BookingStatusId!.ToString().Equals(GlobalConstants.GlobalValues.BookingStatus.ACCEPT))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.BookingStatusAccepted;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method End-- IsAccepted : {1} data:{2} Message:{3} StatusCode:{4} RiderId:{5}", DateTime.UtcNow, bookingStatus.IsAccepted, data, _APIResponse.Message, _APIResponse.Status, bookingStatus.RiderId);
				}
				else if (await _unitOfWork.CommitAsync() > 0 || string.IsNullOrEmpty(data.BookingStatusId) && !string.IsNullOrEmpty(data.URL) && data.PayoutStatus == false)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.DriverPayoutStatusFalse;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method End-- IsAccepted : {1} data:{2} Message:{3} StatusCode:{4} RiderId:{5}", DateTime.UtcNow, bookingStatus.IsAccepted, data, _APIResponse.Message, _APIResponse.Status, bookingStatus.RiderId);
				}
				else if (await _unitOfWork.CommitAsync() > 0 || data.BookingStatusId!.ToString().Equals(GlobalConstants.GlobalValues.BookingStatus.COMPLETED))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.BookingStatusCompleted;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method End-- IsAccepted : {1} data:{2} Message:{3} StatusCode:{4} RiderId:{5}", DateTime.UtcNow, bookingStatus.IsAccepted, data, _APIResponse.Message, _APIResponse.Status, bookingStatus.RiderId);

				}
                else if (await _unitOfWork.CommitAsync() > 0 || data.BookingStatusId!.ToString().Equals(GlobalConstants.GlobalValues.BookingStatus.CANCELLED))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = GlobalResourceFile.BookingStatusCancelled;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method End-- IsAccepted : {1} data:{2} Message:{3} StatusCode:{4} RiderId:{5}", DateTime.UtcNow, bookingStatus.IsAccepted, data, _APIResponse.Message, _APIResponse.Status, bookingStatus.RiderId);

                }

                else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.BookingStatusDeclined;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method End-- IsAccepted : {1} data:{2} Message:{3} StatusCode:{4} RiderId:{5}", DateTime.UtcNow, bookingStatus.IsAccepted, data, _APIResponse.Message, _APIResponse.Status, bookingStatus.RiderId);
				}
			}

			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  RideBookingStatusUpdate in BookingSystemService Method : Message:{1} StatusCode:{2} RiderId:{3} Error:{4} Exception:{5}", DateTime.UtcNow, _APIResponse.Message, bookingStatus.RiderId, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
		#endregion


		#region GetDriverInfoAfterBookingStatusUpdate
		/// <summary>
		/// Service to get driver info after booking status update
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<APIResponse<UserData>> GetDriverInfoAfterBookingStatusUpdate(string userId)
		{
			APIResponse<UserData> _APIResponse = new APIResponse<UserData>();
			try
			{
				_logger.LogInformation("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemService Method Started-- UserId:{1}", DateTime.UtcNow, userId);
				UserData data = await _bookingSystemRepository.GetDriverInfoAfterBookingStatusUpdate(new DistanceCalculate(), userId, "NewDriver");
				if (await _unitOfWork.CommitAsync() > 0 || data is not null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = DriverRegisterResource.DriverDetailsFound;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemService Method End  Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = DriverRegisterResource.DriverDetailNotFound;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemService Method End Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemService Method : Message:{1} StatusCode:{2} Error:{3} Exception:{4} UserId:{5}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message, userId);
			}
			return _APIResponse;
		}
		#endregion

		#region BookingHistory
		/// <summary>
		/// Service to get booking history
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<PagedResponse<List<BookingHistoryData>>>> BookingHistory(PaginationFilter filter)
		{
			APIResponse<PagedResponse<List<BookingHistoryData>>> _APIResponse = new APIResponse<PagedResponse<List<BookingHistoryData>>>();
			try
			{
				_logger.LogInformation("{0} InSide  BookingHistory in BookingSystemService Method Started--", DateTime.UtcNow);
				PagedResponse<List<BookingHistoryData>> data = await _bookingSystemRepository.BookingHistory(filter);
				if (await _unitOfWork.CommitAsync() > 0 || data.TotalRecords > 0)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = data.Message;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  BookingHistory in BookingSystemService Method End  Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = data.Message;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  BookingHistory in BookingSystemService Method End Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  BookingHistory in BookingSystemService Method : Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
		#endregion

		#region CurrentBooking
		/// <summary>
		/// Service to get driver info after booking status update
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<UserData>> CurrentBooking()
		{
			APIResponse<UserData> _APIResponse = new APIResponse<UserData>();
			try
			{
				_logger.LogInformation("{0} InSide  CurrentBooking in BookingSystemService Method Started--", DateTime.UtcNow);
				UserData data = await _bookingSystemRepository.CurrentBooking();
				if (await _unitOfWork.CommitAsync() > 0 || data.userData is not null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = DriverRegisterResource.DriverDetailsFound;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  CurrentBooking in BookingSystemService Method  End Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = "No current booking found";
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  CurrentBooking in BookingSystemService Method End Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  CurrentBooking in BookingSystemService Method : Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
		#endregion

		#region GetRiderSourceLocation
		/// <summary>
		/// API to get rider source location                                                            
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<RiderSourceLocation>> GetRiderSourceLocation()
		{
			APIResponse<RiderSourceLocation> _APIResponse = new APIResponse<RiderSourceLocation>();
			try
			{
				_logger.LogInformation("{0} InSide  GetRiderSourceLocation in BookingSystemService Method Started-- ", DateTime.UtcNow);
				RiderSourceLocation data = await _bookingSystemRepository.GetRiderSourceLocation();
				if (await _unitOfWork.CommitAsync() > 0 || data is not null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.CustomerDetailsFound;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  GetRiderSourceLocation in BookingSystemService Method End  Message:{1} StatusCode:{2} RiderLong:{3} RiderLat:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, data.RiderLongitude, data.RiderLatitude);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.CustomerDetailsNotFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  GetRiderSourceLocation in BookingSystemService Method End Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  GetRiderSourceLocation in BookingSystemService Method Error: Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
		#endregion


		#region GetBookingHistoryUserDetails
		/// <summary>
		/// API to get booking history user details                                                            
		/// </summary>
		/// <returns></returns>
		public async Task<APIResponse<BookingHistoryUserData>> GetBookingHistoryUserDetails(Guid Id)
		{
			APIResponse<BookingHistoryUserData> _APIResponse = new APIResponse<BookingHistoryUserData>();
			try
			{
				_logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails in BookingSystemService Method Started-- BookingId: {1} ", DateTime.UtcNow, Id);
				BookingHistoryUserData data = await _bookingSystemRepository.GetBookingHistoryUserDetails(Id,string.Empty);
				if (await _unitOfWork.CommitAsync() > 0 || data.BookingId is not null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.UserBookingDetaillsFound;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails in BookingSystemService Method End  Message:{1} StatusCode:{2} BookingId: {3} UserId: {4} UserEmail: {5}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, data.BookingId, data.UserId, data.Email);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.UserBookingDetaillsNotFound;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails in BookingSystemService Method End Message:{1} StatusCode:{2}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  GetBookingHistoryUserDetails in BookingSystemService Method Error: Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
        #endregion

        #region TipsAndReview
		/// <summary>
		/// Service for Tips and Review
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        public async Task<APIResponse<string>> TipsAndReview(TipsAndReviewModel model, CancellationToken cancellationToken)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				_logger.LogInformation("{0} InSide  TipsAndReview in BookingSystemService Method  -- Tip: {1} Review:{2}", DateTime.UtcNow, model.Tip, model.Review);
				var data = await _bookingSystemRepository.TipsAndReview(model, cancellationToken);
				if (await _unitOfWork.CommitAsync() > 0 || data.Equals("success"))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.TipsReviewSuccess;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  TipsAndReview in BookingSystemService Method End  Tip: {1} Review:{2} data:{3} Message: {4} Status:{5}", DateTime.UtcNow, model.Tip, model.Review, data, _APIResponse.Message, _APIResponse.Status);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = GlobalResourceFile.TipsReviewNotProvided;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  TipsAndReview in BookingSystemService Method End  Tip: {1} Review:{2} data:{3} Message: {4} Status:{5}", DateTime.UtcNow, model.Tip, model.Review, data, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  TipsAndReview in BookingSystemService Method Error: Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
        #endregion

        #region GetCurrentLocation
        /// <summary>
        /// Method to get GetCurrentLocation
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<APIResponse<UserLongLatInfo>> GetCurrentLocation(Guid bookingId)
		{
			APIResponse<UserLongLatInfo> _APIResponse = new APIResponse<UserLongLatInfo>();
			try
			{
				_logger.LogInformation("{0} InSide  GetCurrentLocation in BookingSystemService Method  -- bookingId: {1}", DateTime.UtcNow, bookingId);
				var data = await _bookingSystemRepository.GetCurrentLocation(bookingId);
				if (await _unitOfWork.CommitAsync() > 0 || data != null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.FetchSuccess;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  GetCurrentLocation in BookingSystemService Method End DriverLatitude:{1} DriverLongitude:{2} RiderLatitude:{3} RiderLongitude:{4} Message: {5} Status:{6}", DateTime.UtcNow, data?.DriverLatitude, data?.DriverLongitude, data?.RiderLatitude, data?.RiderLongitude, _APIResponse.Message, _APIResponse.Status);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.FetchFailed;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  GetCurrentLocation in BookingSystemService Method End DriverLatitude:{1} DriverLongitude:{2} RiderLatitude:{3} RiderLongitude:{4} Message: {5} Status:{6}", DateTime.UtcNow, data?.DriverLatitude, data?.DriverLongitude, data?.RiderLatitude, data?.RiderLongitude, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  GetCurrentLocation in BookingSystemService Method Error: Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
        #endregion


        #region FetchBonusHistoryRecord
        public async Task<APIResponse<BonusHistoryData>> FetchBonusHistoryRecord()
		{
			APIResponse<BonusHistoryData> _APIResponse = new APIResponse<BonusHistoryData>();
			try
			{
				_logger.LogInformation("{0} InSide  FetchBonusHistoryRecord in BookingSystemService Method :", DateTime.UtcNow);
				var data = await _bookingSystemRepository.FetchBonusHistoryRecord();
				if (await _unitOfWork.CommitAsync() > 0 || data != null)
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.FetchSuccess;
					_APIResponse.Status = HttpStatusCode.OK;
					_logger.LogInformation("{0} InSide  FetchBonusHistoryRecord in BookingSystemService Method End TotalCashback:{1} ReedemableCashback:{2}: Message{3} Status{4}", DateTime.UtcNow, data?.TotalCashback, data?.ReedemableCashback, _APIResponse.Message, _APIResponse.Status);
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Data = data;
					_APIResponse.Message = CommonResource.FetchFailed;
					_APIResponse.Status = HttpStatusCode.NotFound;
					_logger.LogInformation("{0} InSide  FetchBonusHistoryRecord in BookingSystemService Method End TotalCashback:{1} ReedemableCashback:{2}:  Message{3} Status{4}", DateTime.UtcNow, data?.TotalCashback, data?.ReedemableCashback, _APIResponse.Message, _APIResponse.Status);
				}
			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = GlobalResourceFile.Exception;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide  FetchBonusHistoryRecord in BookingSystemService Method Error: Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
			}
			return _APIResponse;
		}
        #endregion

        public async Task<APIResponse<bool>> CurrentBookingCancel(BookingCancellationDTO record)
        {
            APIResponse<bool> _APIResponse = new APIResponse<bool>();
            try
            {
                _logger.LogInformation("{0} InSide  CurrentBookingCancel in BookingSystemService Method :", DateTime.UtcNow);
                BookingCancellation distanceModel = _mapper.Map<BookingCancellation>(record);
                var data = await _bookingSystemRepository.CurrentBookingCancel(distanceModel);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  CurrentBookingCancel in BookingSystemService Method End Cancelled:{1} : Message{2} Status{3}", DateTime.UtcNow, data, _APIResponse.Message, _APIResponse.Status);
                }
                else if (await _unitOfWork.CommitAsync() > 0 || data)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = GlobalResourceFile.BookingStatusCancelled;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide  CurrentBookingCancel in BookingSystemService Method End-- Cancelled :{1} :Message{2} Status{3}", DateTime.UtcNow, data, _APIResponse.Message, _APIResponse.Status);

                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Data = data;
                    _APIResponse.Message = CommonResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.NotFound;
                    _logger.LogInformation("{0} InSide  CurrentBookingCancel in BookingSystemService Method End Cancelled:{1} :  Message{2} Status{3}", DateTime.UtcNow, data,  _APIResponse.Message, _APIResponse.Status);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = GlobalResourceFile.Exception;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide  CurrentBookingCancel in BookingSystemService Method Error: Message:{1} StatusCode:{2} Error:{3} Exception:{4}", DateTime.UtcNow, _APIResponse.Message, _APIResponse.Status, _APIResponse.Error, ex.Message);
            }
            return _APIResponse;
        }
        
    }
}
