using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.BookingSystemDTO;
using Posh_TRPT_Services.BookingSystem;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;
using System.Security.Claims;
using RideBookingStatus = Posh_TRPT_Domain.BookingSystem.RideBookingStatus;

namespace Posh_TRPT.Controllers
{
	[Route(GlobalConstants.GlobalValues.ControllerRoute)]
	[ApiController]
	public class BookingSystemController : ControllerBase
	{

		private readonly BookingSystemService _bookingSystemService;
		private readonly ILogger<BookingSystemController> _logger;
		private readonly IHttpContextAccessor _context;

		public BookingSystemController(BookingSystemService bookingSystemService, ILogger<BookingSystemController> logger, IHttpContextAccessor context)
		{
			_bookingSystemService = bookingSystemService;
			_logger = logger;
			_context = context;
		}

		#region GetAvailableCategoryPrice
		/// <summary>
		/// Method to get Category Price
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpGet]
		public async Task<IActionResult> GetAvailableCategoryPrice(DistanceCalculateDTO model)
		{
			try
			{

				var data = await _bookingSystemService.GetAvailableCategoryPrice(model);
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion

		#region RideBookingNotifyDriver
		/// <summary>
		/// Method to Notify Driver after Booking Request
		/// </summary>
		/// <param name="model"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="OperationCanceledException"></exception>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpGet]
		public async Task<IActionResult> RideBookingNotifyDriver([FromBody] DistanceCalculateDTO model, CancellationToken cancellationToken = default)
		{
			var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
			try
			{

				if (ModelState.IsValid)
				{

					if (!cancellationToken.IsCancellationRequested)
					{

						_logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemController Method Started-- UserId:{1}  CancellationToken = {2}", DateTime.UtcNow, userId, cancellationToken.IsCancellationRequested);
						var data = await _bookingSystemService.RideBookingNotifyDriver(model, cancellationToken);
						return Ok(data);

					}
					else
					{
                        _logger.LogInformation("{0} Cancelled Request InSide  RideBookingNotifyDriver in BookingSystemController Method--  Error Message:{1} UserId:{2} CancellationToken = {3}", DateTime.UtcNow, GlobalResourceFile.OperationCancelled, userId, cancellationToken.IsCancellationRequested);

                        cancellationToken = HttpContext.RequestAborted;
                        var data = await _bookingSystemService.RideBookingNotifyDriver(model, cancellationToken);
                        throw new OperationCanceledException(cancellationToken);
						

					}
				}
			}
			catch (Exception ex)
			{

				_logger.LogError("{0} InSide  RideBookingNotifyDriver in BookingSystemController Method--  Error Message:{1} UserId:{2}", DateTime.UtcNow, $"{nameof(Exception)} thrown with message: {ex.Message}", userId);
				
			}

			return BadRequest(ModelState);

		}
		#endregion

		#region RideBookingStatusUpdate
		/// <summary>
		/// Method to RideBookingStatusUpdate
		/// </summary>
		/// <param name="bookingStatus"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Driver)]
		[HttpPost]
		public async Task<IActionResult> RideBookingStatusUpdate([FromBody] RideBookingStatus bookingStatus)
		{

			try
			{
				var data = await _bookingSystemService.RideBookingStatusUpdate(bookingStatus);
				return Ok(data);
			}
			catch (Exception ex)
			{

				_logger.LogError("{0} InSide  RideBookingStatusUpdate in BookingSystemController Method--  Error Message:{1} RiderId:{2}", DateTime.UtcNow, $"{nameof(Exception)} thrown with message: {ex.Message}", bookingStatus.RiderId);
				return BadRequest(ModelState);
			}
		 }
		#endregion

		#region GetDriverInfoAfterBookingStatusUpdate
		/// <summary>
		/// Method to Get Driver Info After Booking Status Update
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpGet]
		public async Task<IActionResult> GetDriverInfoAfterBookingStatusUpdate(string userId)
		{
			try
			{

				var data = await _bookingSystemService.GetDriverInfoAfterBookingStatusUpdate(userId);
				return Ok(data);
			}
			catch (Exception ex)
			{

				_logger.LogError("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemController Method--  Error Message:{1} UserId:{2}", DateTime.UtcNow, $"{nameof(Exception)} thrown with message: {ex.Message}", userId);
				return BadRequest(ModelState);
			}
		}
		#endregion

		#region BookingHistory
		/// <summary>
		/// Method to Get ride history
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		[Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
		[HttpGet]
		public async Task<IActionResult> BookingHistory([FromQuery] PaginationFilter filter)
		{
			try
			{

				var data = await _bookingSystemService.BookingHistory(filter);
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion

		#region CurrentBooking
		/// <summary>
		/// Method to Get Driver Info After Booking Status Update
		/// </summary>
		/// <returns></returns>
		[Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
		[HttpGet]
		public async Task<IActionResult> CurrentBooking()
		{
			try
			{

				var data = await _bookingSystemService.CurrentBooking();
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion

		#region GetRiderSourceLocation
		/// <summary>
		/// Get Customer (Rider) Source Location
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpGet]
		public async Task<IActionResult> GetRiderSourceLocation()
		{
			try
			{
				var data = await _bookingSystemService.GetRiderSourceLocation();
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion

		#region GetBookingHistoryUserDetails
		/// <summary>
		/// Get booking history user details
		/// </summary>
		/// <returns></returns>
		[Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
		[HttpGet]
		public async Task<IActionResult> GetBookingHistoryUserDetails(Guid Id)
		{
			try
			{
				_logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails in BookingSystemController Method  -- BookingId: {2}", DateTime.UtcNow, Id);
				var data = await _bookingSystemService.GetBookingHistoryUserDetails(Id);
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}
		}
        #endregion

        #region TipsAndReview
		/// <summary>
		/// API to add Tips and Review
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        [Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
		[HttpPost]
		public async Task<IActionResult> TipsAndReview(TipsAndReviewModel model, CancellationToken cancellationToken = default)
		{
			try
			{

				_logger.LogInformation("{0} InSide  TipsAndReview in BookingSystemController Method  -- Tip: {1} Review:{2}", DateTime.UtcNow, model.Tip, model.Review);
				var data = await _bookingSystemService.TipsAndReview(model, cancellationToken);
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}

		}
        #endregion
		/// <summary>
		/// Method to get current location of driver and rider
		/// </summary>
		/// <param name="bookingId"></param>
		/// <returns></returns>
        [Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
        [HttpPost]
        public async Task<IActionResult> GetCurrentLocation(Guid bookingId)
        {
			try
			{
				_logger.LogInformation("{0} InSide  GetCurrentLocation in BookingSystemController Method  -- bookingId: {1}", DateTime.UtcNow, bookingId);
				var data = await _bookingSystemService.GetCurrentLocation(bookingId);
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}

        }


		/// <summary>
		/// Method to get Rider Bonus record.
		/// </summary>
		/// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Customer)]
        [HttpGet]
        #region FetchBonusHistoryRecord
        public async Task<IActionResult> FetchBonusHistoryRecord()
		{
			try
			{
				_logger.LogInformation("{0} InSide  FetchBonusHistoryRecord in BookingSystemController Method :", DateTime.UtcNow);
				var data = await _bookingSystemService.FetchBonusHistoryRecord();
				return Ok(data);
			}
			catch (Exception)
			{

				throw;
			}

		}
        #endregion

        [Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
        [HttpGet]
        public async Task<IActionResult> CurrentBookingCancel(string bookingId,string LocalTime)
        
		{
            try
            {

                _logger.LogInformation("{0} InSide  CurrentBookingCancel in BookingSystemController Method :", DateTime.UtcNow);
                string userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
                string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
                BookingCancellationDTO bookingCancellationDTO = new BookingCancellationDTO()
                {
                    UserId = userId,
                    Role = role,
                    BookingId = Guid.Parse(bookingId),
					LocalTime = LocalTime
                };
                var data = await _bookingSystemService.CurrentBookingCancel(bookingCancellationDTO);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }

        }
	
    }
}
