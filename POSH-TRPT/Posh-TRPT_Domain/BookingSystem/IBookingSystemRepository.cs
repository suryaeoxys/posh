using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;

namespace Posh_TRPT_Domain.BookingSystem
{
    public interface IBookingSystemRepository
    {
        Task<PriceResponse> GetAvailableCategoryPrice(DistanceCalculate model);
        Task<UserData> RideBookingNotifyDriver(DistanceCalculate model, CancellationToken cancellationToken);
        Task<BookingStatusResponse> RideBookingStatusUpdate(RideBookingStatus bookingStatus, BookingDetail record);
        Task<UserData> GetDriverInfoAfterBookingStatusUpdate(DistanceCalculate model,string userId,string newDriver, CancellationToken cancellationToken = default);
        Task<PagedResponse<List<BookingHistoryData>>> BookingHistory(PaginationFilter filter);
        Task<UserData> CurrentBooking();
        Task<RiderSourceLocation> GetRiderSourceLocation();
        Task<BonusHistoryData> FetchBonusHistoryRecord();
        Task<BookingHistoryUserData> GetBookingHistoryUserDetails(Guid Id, string emailidRideCompletion,bool rideCompletionMailSend =false);
        Task<string> TipsAndReview(TipsAndReviewModel model, CancellationToken cancellationToken = default);
        Task<UserLongLatInfo> GetCurrentLocation(Guid bookingId);
        Task<bool> CurrentBookingCancel(BookingCancellation record);




    }
}
