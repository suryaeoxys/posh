using AutoMapper;
using Google.Apis.Auth.OAuth2;
using iText.Html2pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Posh_TRPT_Domain.API;
using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.BookingSystemDTO;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.EmailUtils;
using Posh_TRPT_Utility.JwtUtils;
using Posh_TRPT_Utility.Resources;
using Stripe;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;
using BookingDetail = Posh_TRPT_Domain.Entity.BookingDetail;
using BookingStatusResponse = Posh_TRPT_Domain.BookingSystem.BookingStatusResponse;
using DriverDetail = Posh_TRPT_Domain.BookingSystem.DriverDetail;
using RideBookingStatus = Posh_TRPT_Domain.BookingSystem.RideBookingStatus;
using RiderDetail = Posh_TRPT_Domain.BookingSystem.RiderDetail;
using RiderSourceLocation = Posh_TRPT_Domain.BookingSystem.RiderSourceLocation;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class BookingSystemRepository : Repository<Object>, IBookingSystemRepository
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingSystemRepository> _logger;
        private readonly IStripePaymentRepository _paymentRepository;
        private readonly StripeSettings _stripeSettings;
        private IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookingSystemRepository(IWebHostEnvironment webHostEnvironment, IOptions<StripeSettings> stripeSettings, IStripePaymentRepository paymentRepository, IMapper mapper, ILogger<BookingSystemRepository> logger, IConfiguration config, DbFactory dbFactory, IHttpContextAccessor context, UserManager<ApplicationUser> userManager) : base(dbFactory)
        {
            _config = config;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _stripeSettings = stripeSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        #region GetAvailableCategoryPrice
        /// <summary>
        ///  API to get Available Category Price Details
        ///  This method is used to get the nearest category avaiable for booking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PriceResponse> GetAvailableCategoryPrice(DistanceCalculate model)
        {
            #region Object Declaration
            PriceResponse response = new PriceResponse();
            List<AvailableCategoryFare> listOfCategoryPrice = new List<AvailableCategoryFare>();
            #endregion

            try
            {

                _logger.LogInformation("{0} InSide  GetAvailableCategoryPrice in BookingSystemRepository Method Started-- CityId = {1}, CategoryId = {2},  RiderId = {3}, RiderDeviceId = {4}, DriverId = {5}, DriverDeviceId = {6} " +
                                "SourceLat = {7}, SourceLong = {8}, DestinationLat ={9}, DestinationLong = {10}",
                                DateTime.UtcNow, model.StateId, model.CategoryId, model.riderDetail!.Id, model.riderDetail.DeviceId, model.driverDetail!.Id, model.driverDetail.DeviceId, model.Source!.Latitude, model.Source.Longitude, model.Destination!.Latitude, model.Destination.Longitude);


                // use to fetch the Distance And Time between sourc and destination
                Tuple<int, int, decimal> distanceTime = DistanceTimeBetweenSourceDestination(model).Result;



                if (distanceTime != null)
                {
                    SqlParameter[] sqlParameters = new SqlParameter[] {
                                                                                                new SqlParameter { ParameterName ="@sourcelat", Value = model.Source!.Latitude} ,
                                                                                                new SqlParameter { ParameterName ="@sourcelng", Value = model.Source!.Longitude},
                                                                                                new SqlParameter { ParameterName ="@destinationlat", Value = model.Destination!.Latitude} ,
                                                                                                new SqlParameter { ParameterName ="@destinationlng", Value = model.Destination!.Longitude},
                                                                                                new SqlParameter { ParameterName ="@stateid", Value = model.StateId},
                                                                                                new SqlParameter { ParameterName ="@Distance", Value = ((int)distanceTime.Item1)},
                                                                                                new SqlParameter { ParameterName ="@Time", Value = ((int)distanceTime.Item2)},
                                                                                                new SqlParameter { ParameterName ="@minimumDistance", Value = model.MinimumDistance},

                   };


                    // Get list of Ride Category nearest to source lat and long and group the driver with same category
                    listOfCategoryPrice = this.DbContextObj().GetListOfRecordExecuteProcedure<AvailableCategoryFare>("sp_GetAvailableCategoryPrice1 @sourcelat,@sourcelng,@destinationlat,@destinationlng,@stateid ,@Distance,@Time,@minimumDistance", sqlParameters);


                    // Calculate the distance and time of Nearest Driver 
                    var task = listOfCategoryPrice.Select(async result =>
                    {
                        model.Destination.Latitude = result.DriverLat;
                        model.Destination.Longitude = result.DriverLong;
                        var finalDistanceTimeEstimate = await DistanceTimeBetweenSourceDestination(model);
                        result.Miles = finalDistanceTimeEstimate.Item1;
                        result.Miles *= (decimal)0.00062137;
                        result.DriverEstimationTime = finalDistanceTimeEstimate.Item2;
                        result.DriverEstimationTime /= 60;
                        result.Price += distanceTime.Item3;
                    });
                    await Task.WhenAll(task);
                    listOfCategoryPrice = listOfCategoryPrice.OrderBy(s => s.DriverEstimationTime).GroupBy(s => s.Name).Select(s => s.FirstOrDefault()).ToList()!;




                    _logger.LogInformation("{0} InSide  GetAvailableCategoryPrice in BookingSystemRepository Method Ended-- CityId = {1}, CategoryId = {2},  RiderId = {3}, RiderDeviceId = {4}, DriverId = {5}, DriverDeviceId = {6} " +
       "SourceLat = {7}, SourceLong = {8}, DestinationLat ={9}, DestinationLong = {10},  CategoryList = {11}",
       DateTime.UtcNow, model.StateId, model.CategoryId, model.riderDetail!.Id, model.riderDetail.DeviceId, model.driverDetail!.Id, model.driverDetail.DeviceId, model.Source!.Latitude, model.Source.Longitude, model.Destination!.Latitude, model.Destination.Longitude, listOfCategoryPrice.Count());

                    response.Record = listOfCategoryPrice;
                    response.Promotion = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(model.riderDetail!.Id)).Select(x => x.Balance).FirstOrDefaultAsync()!;
                    return await Task.FromResult<PriceResponse>(response!);
                }

                return Task.FromResult<PriceResponse>(null!).Result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region RideBookingNotifyDriver
        /// <summary>
        /// Thi method is used to get the nearest driver and send them rider request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>      
        public async Task<UserData> RideBookingNotifyDriver(DistanceCalculate model, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemRepository Method Started-- Source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken = {15}", DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination!.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId, cancellationToken.IsCancellationRequested);


            #region Object/Variable
            string paymentIntentId = string.Empty;
            string paymetMethodId = string.Empty;
            UserData currentBookingDetail = new UserData();
            #endregion

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                #region Update status of wallet if exists
                var digitalWallet = await this.DbContextObj().TblDigitalWallet.Where(s => s.UserId == model.riderDetail.Id).FirstOrDefaultAsync();
                if (digitalWallet != null)
                {
                    digitalWallet.IsApplied = model.IsWalletApplied;
                    this.DbContextObj().TblDigitalWallet.Update(digitalWallet);
                    await this.DbContextObj().SaveChangesAsync();
                }
                #endregion

                // This method is used to notify the nearest driver
                var driverNotify = await NotifyDriver(model, cancellationToken, paymentIntentId, paymetMethodId, 1);


                switch (driverNotify)
                {
                    case "requires_capture": currentBookingDetail = await GetDriverInfoAfterBookingStatusUpdate(model, model.riderDetail!.Id!, "NewDriver", cancellationToken); break;
                    case "requires_action": currentBookingDetail.IntentPaymentStatus = "Insufficient funds"; currentBookingDetail.type = "100"; break;
                    case "card_error": currentBookingDetail.IntentPaymentStatus = "card_error"; currentBookingDetail.type = "101"; ; break;
                    case "Not Found": currentBookingDetail.IntentPaymentStatus = "Default card not set"; currentBookingDetail.type = "102"; break;
                    case "Failed": break;
                    case "CancelException": break;
                    case "No_Driver_Available": currentBookingDetail.IntentPaymentStatus = "No_Driver_Available"; currentBookingDetail.type = "103"; break;
                }


                _logger.LogInformation("{0} InSide  RideBookingNotifyDriver in BookingSystemRepository Method Ended-- Source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken = {15} IntentPaymentStatus : {16}", DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination!.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId, cancellationToken.IsCancellationRequested, currentBookingDetail.type);

                return (currentBookingDetail!.type == "103") ? Task.FromResult<UserData>(null!).Result : currentBookingDetail!;

            }

            // This  is used after cancelled the ride by user using Cancellation token
            catch (OperationCanceledException)
            {
                _logger.LogInformation("{0} InSide When Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- Source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken = {15}", DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination!.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId, cancellationToken.IsCancellationRequested);

                await RideCancelByCancellationToken(model);
                return new UserData();

                //var detailData = await this.DbContextObj().TblBookingDetail.Where(s => s.RiderId == model.riderDetail.Id && s.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.NOTIFIED_DRIVER)).OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();
                //StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                //DigitalWalletData walletData = new DigitalWalletData();

                //if (detailData != null)
                //{
                //    #region Cancel payment intent created while booking ride
                //    PaymentIntentService service = new PaymentIntentService();
                //    var data = await service.CancelAsync(detailData.PaymentIntentId, new PaymentIntentCancelOptions { CancellationReason = "requested_by_customer" }).ConfigureAwait(false);

                //    _logger.LogInformation("{0}InSide Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- PaymentIntentId : {1} CancellationReason:{2}", DateTime.UtcNow, detailData.PaymentIntentId, "requested_by_customer");

                //    StripeCustomerIntentCustom res = JsonConvert.DeserializeObject<StripeCustomerIntentCustom>(data.ToJson())!;
                //    #endregion

                //    detailData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED);
                //    detailData.UpdatedDate = DateTime.UtcNow;
                //    detailData.UpdatedBy = detailData.RiderId;
                //    var seconds = DateTime.UtcNow.Subtract((DateTime)detailData.UpdatedDate).TotalSeconds;
                //    detailData.LocalUpdatedDateTime = Convert.ToDateTime(model.LocalTime).AddSeconds(seconds);
                //    detailData.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;

                //    _logger.LogInformation("{0}InSide Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- BookingStatusId : {1} RiderId:{2} StatusType: {3}", DateTime.UtcNow, detailData.BookingStatusId, detailData.RiderId, detailData.StatusType);

                //    var result = await this.DbContextObj().Users.Where(s => s.Id == detailData.DriverId).FirstOrDefaultAsync().ConfigureAwait(false);

                //    if (result != null)
                //    {
                //        result.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                //        result.UpdatedDate = DateTime.UtcNow;
                //        result.UpdatedBy = detailData.RiderId;
                //        this.DbContextObj().Entry(result).State = EntityState.Modified;
                //        await this.DbContextObj().SaveChangesAsync();


                //        var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(detailData.RiderId)).FirstOrDefaultAsync()!;
                //        var rideCategoryPrice = await this.DbContextObj().TblCategoryPrices?.Where(x => x.Id!.Equals(detailData.CategoryPriceId)).FirstOrDefaultAsync()!;
                //        if (wallateRecord != null && wallateRecord.IsApplied)
                //        {
                //            wallateRecord!.Balance += Math.Truncate((decimal)(detailData.ServiceFee * rideCategoryPrice!.Bonus_Amount)! * 100) / 100;
                //            wallateRecord.UpdatedDate = DateTime.UtcNow;
                //            wallateRecord.UpdatedBy = detailData.RiderId;
                //            this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                //            await this.DbContextObj().SaveChangesAsync();
                //        }

                //        var bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.BookingId!.Equals(detailData.Id)).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;

                //        if (bonusRecord != null)
                //        {
                //            bonusRecord.IsApplied = false;
                //            bonusRecord.CashBack = 0.0m;
                //            this.DbContextObj().TblRideBonusHistory.Update(bonusRecord);
                //            await this.DbContextObj().SaveChangesAsync();
                //        }


                //    }
                //    detailData.PaymentStatus = "canceled";
                //    this.DbContextObj().Entry(detailData).State = EntityState.Modified;
                //    await this.DbContextObj().SaveChangesAsync();


                //}
                //return new UserData();
            }

            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region NotifyDriver
        /// <summary>
        /// Method to send notification to the Driver
        /// Used to capture the required amount of ride from Rider's account
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> NotifyDriver(DistanceCalculate model, CancellationToken cancellationToken, string paymentIntentId, string paymetMethodId, int isCurrentRequest)
        {
            #region Object Declaration
            StringBuilder stringBuilder = new StringBuilder();
            UserDataResponse userDataResponse = new UserDataResponse();
            UserData userData = new UserData();
            string userId = string.Empty;
            //NotificationModel notificationModel = new NotificationModel();
            #endregion

            try
            {

                string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;

                _logger.LogInformation("{0} InSide  NotifyDriver in BookingSystemRepository Method-- source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken = {15}", DateTime.UtcNow, model.Source!.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination!.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId, cancellationToken.IsCancellationRequested);


                if (!string.IsNullOrEmpty(model!.driverDetail!.Id))
                {
                    _logger.LogInformation("{0} InSide  NotifyDriver in BookingSystemRepository Method -- source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken = {15}", DateTime.UtcNow, model.Source.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId, cancellationToken.IsCancellationRequested);

                    userId = model.riderDetail!.Id!;
                }
                else
                {
                    userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
                }


                #region Find nearest driver detail
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_config["DistanceMatrixAPI:URL"] + String.Concat(model.Destination!.Latitude, ",", model.Destination.Longitude, "&origins=", model.Source!.Latitude, ",", model.Source.Longitude, "&units=imperial&key=", _config["DistanceMatrixAPI:KEY"]), cancellationToken))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var getTheDistanceTime = JsonConvert.DeserializeObject<RouteDistanceResponseAPI>(apiResponse);



                        SqlParameter[] sqlParameters = new SqlParameter[]
                        {
                                                                                                new SqlParameter { ParameterName ="@sourcelat", Value = model.Source!.Latitude} ,
                                                                                                new SqlParameter { ParameterName ="@sourcelng", Value = model.Source!.Longitude},
                                                                                                new SqlParameter { ParameterName ="@sourcedestinationlat", Value = model.Destination!.Latitude} ,
                                                                                                new SqlParameter { ParameterName ="@sourcedestinationlng", Value = model.Destination!.Longitude},
                                                                                                new SqlParameter { ParameterName ="@riderId", Value = model.riderDetail?.Id},
                                                                                                new SqlParameter { ParameterName ="@stateid", Value = model.StateId},
                                                                                                new SqlParameter { ParameterName ="@Distance", Value = getTheDistanceTime!.rows![0].elements![0]!.distance!.value},
                                                                                                new SqlParameter { ParameterName ="@Time", Value = getTheDistanceTime.rows[0].elements![0]!.duration!.value},
                                                                                                new SqlParameter { ParameterName ="@minimumDistance", Value = model.MinimumDistance},
                                                                                                new SqlParameter { ParameterName ="@categoryId", Value = model.CategoryId},
                                                                                                new SqlParameter { ParameterName ="@isCurrentRequest", Value = isCurrentRequest},
                        };
                        _logger.LogInformation("{0} InSide  NotifyDriver in BookingSystemRepository Method -- source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14} CancellationToken = {15}", DateTime.UtcNow, model.Source.Latitude, model.Source.Longitude, model.Source.SourceName, model.Destination.Latitude, model.Destination.Longitude, model.Destination.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail!.Id, model.riderDetail!.DeviceId, model.driverDetail!.Id, model.driverDetail!.DeviceId, cancellationToken.IsCancellationRequested);
                        stringBuilder.Append("Calling Sp_NotifyNearestDriverBooking");

                        BookingNotificationRequesr result = await this.DbContextObj().GetRecordExecuteProcedureAsync<BookingNotificationRequesr>("Sp_NotifyNearestDriverBooking @sourcelat,@sourcelng,@sourcedestinationlat,@sourcedestinationlng,@riderId, @stateid,@Distance,@Time,@minimumDistance,@categoryId,@isCurrentRequest", sqlParameters, cancellationToken).ConfigureAwait(false);

                        if (result == null || result.Id! == default)
                        {
                            return "No_Driver_Available";
                        }
                        stringBuilder.AppendLine(" Name " + result.Name);
                        stringBuilder.AppendLine(" PhoneNumber " + result.PhoneNumber);
                        stringBuilder.AppendLine(" DriverName " + result.DriverName);
                        stringBuilder.AppendLine(" DriverDeviceId " + result.DeviceId);
                        stringBuilder.AppendLine(" UserId " + result.UserId);
                        stringBuilder.AppendLine(" Price " + result.Price);
                        stringBuilder.AppendLine(" CategoryId " + result.CategoryId);
                        stringBuilder.AppendLine(" Latitude " + result.Latitude);
                        stringBuilder.AppendLine(" Longitude " + result.Longitude);
                        stringBuilder.AppendLine(" Email " + result.Email);
                        stringBuilder.AppendLine(" DriverLat " + result.DriverLat);
                        stringBuilder.AppendLine(" DriverLong " + result.DriverLong);
                        stringBuilder.AppendLine(" Angle " + result.Angle);
                        stringBuilder.AppendLine(" DriverEstimationTime " + result.DriverEstimationTime);
                        stringBuilder.AppendLine(" RideCategoryName " + result.RideCategoryName);
                        _logger.LogInformation("{0} InSide  NotifyDriver in BookingSystemRepository Method after Sp_NotifyNearestDriverBooking  {1} CancellationToken = {2}", DateTime.UtcNow, stringBuilder.ToString(), cancellationToken.IsCancellationRequested);

                        #region Notify booking request to nearest driver
                        var distanceTimeEstimation = DistanceTimeBetweenSourceDestination(model).Result;
                        result.Price += distanceTimeEstimation.Item3;

                        decimal processFee = await CalculateStripeFees(result.Price); // stripe processing fees
                        decimal driverPrice = await GetActualPrice(result.Price, result.ServiceFee, true, 0.0, model!.IsWalletApplied); // get actual price to be  displayed driver
                        decimal distance = getTheDistanceTime!.rows![0].elements![0]!.distance!.value;
                        int time = getTheDistanceTime.rows[0].elements![0]!.duration!.value;


                        NotificationModel notificationModel = new NotificationModel
                        {
                            DeviceId = result.DeviceId,
                            IsAndroidDevice = true,
                            Title = "Booking Request",
                            Body = string.Concat("Booking Request of price $", driverPrice, " from ", model.Source.SourceName, " ")

                        };



                        userDataResponse.SourceToDestinationDistance = Convert.ToString(distance * 0.00062137m);
                        userDataResponse.SourceToDestinationTime = Convert.ToString(time / 60);
                        userDataResponse.price = Convert.ToString(driverPrice);
                        userDataResponse.Angle = result.Angle;
                        userDataResponse.ServiceFee = result.ServiceFee;

                        // Rider Source Record
                        userDataResponse.sourceLat = model.Source.Latitude;
                        userDataResponse.sourceLong = model.Source.Longitude;
                        userDataResponse.sourcePlaceName = model.Source.SourceName;

                        // Rider Destination Record
                        userDataResponse.destinationLat = model.Destination.Latitude;
                        userDataResponse.destinationLong = model.Destination.Longitude;
                        userDataResponse.destinationPlaceName = model.Destination.DestinationName;


                        // Rider Details
                        userDataResponse.riderDetail = new Posh_TRPT_Domain.PushNotification.RiderDetail
                        {
                            Id = model.riderDetail!.Id,
                            DeviceId = model.riderDetail.DeviceId,

                            source = new Posh_TRPT_Domain.BookingSystem.Source()
                            {
                                Latitude = model.Source.Latitude,
                                Longitude = model.Source.Longitude,
                                SourceName = model.Source.SourceName,
                            },

                            destination = new Destination()
                            {
                                Latitude = model.Destination.Latitude,
                                Longitude = model.Destination.Longitude,
                                DestinationName = model.Destination.DestinationName,
                            }
                        };


                        // Driver Details
                        userDataResponse.driverDetail = new Posh_TRPT_Domain.PushNotification.DriverDetail
                        {
                            Id = result.UserId,
                            DeviceId = result.DeviceId,
                            source = new Posh_TRPT_Domain.BookingSystem.Source()
                            {
                                Latitude = result.DriverLat,
                                Longitude = result.DriverLong,
                            },

                            destination = new Destination()
                            {
                                Latitude = model.Source.Latitude,
                                Longitude = model.Source.Longitude,
                                DestinationName = model.Source.SourceName
                            }

                        };




                        // Calculate Driver to Rider Distance/Time
                        double tempLat = model.Destination.Latitude;
                        double tempLng = model.Destination.Longitude;
                        model.Destination.Latitude = result.DriverLat;
                        model.Destination.Longitude = result.DriverLong;
                        var distanceTimeEstimate = DistanceTimeBetweenSourceDestination(model).Result;
                        model.Destination.Latitude = tempLat;
                        model.Destination.Longitude = tempLng;




                        ApplicationUser user = await _userManager.FindByIdAsync(userId);
                        userDataResponse.TollFees = distanceTimeEstimation.Item3;
                        userDataResponse.name = user.Name;
                        userDataResponse.RiderProfilePic = string.IsNullOrEmpty(user.ProfilePhoto!) ? _config["Request:Url"] + "Images" + "/person.png" : _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + user.ProfilePhoto!;
                        userDataResponse.distance = Convert.ToString((distanceTimeEstimate.Item1 * (decimal)0.00062137));
                        userDataResponse.time = Convert.ToString(distanceTimeEstimate.Item2 / 60m);
                        userDataResponse.id = Guid.Empty;
                        userDataResponse.RideCategoryName = result.RideCategoryName;
                        userData.type = GlobalConstants.GlobalValues.BookingStatus.ST_NOTIFIED_DRIVER;
                        userData.userData = userDataResponse;
                        model.StatusType = userData.type;
                        _logger.LogInformation("{0}InSide  NotifyDriver in BookingSystemRepository Method Details: --DriverId:{1} DeviceId : {2}  IsAndroidDevice: {3} Title:{4} CancellationToken = {5}", DateTime.UtcNow, result.UserId, notificationModel.DeviceId, notificationModel.IsAndroidDevice, notificationModel.Title, cancellationToken.IsCancellationRequested);


                        #region Stripe Part integration

                        // notificationModel = await NotificationBody(result.DeviceId!, "Booking Request", string.Concat("Booking Request of price ", driverPrice, " from ", model.Source.SourceName, " "));

                        switch (role)
                        {
                            case AuthorizationLevel.Roles.Customer:
                                {
                                    if (cancellationToken.IsCancellationRequested)
                                        cancellationToken.ThrowIfCancellationRequested();


                                    // Check if intent already created or not
                                    var intentData = await this.DbContextObj().TblBookingDetail.Where(s => s.RiderId == userDataResponse.riderDetail.Id).OrderByDescending(s => s.UpdatedDate).Select(s => new
                                    {
                                        s.PaymentIntentId,
                                        s.DefaultPaymentMethodId,
                                        s.PaymentStatus
                                    }).FirstOrDefaultAsync();


                                    if (intentData != null && intentData.PaymentStatus!.Equals("requires_capture") && isCurrentRequest == 0)
                                    {

                                        var saveBookingDetails = await BookingDetails(result, model, userDataResponse, intentData?.PaymentIntentId!, intentData?.DefaultPaymentMethodId!, intentData?.PaymentStatus!).ConfigureAwait(false);
                                      
                                        
                                        Parallel.Invoke(() => SendNotification(notificationModel, userData));

                                        // added newly
                                        if (model.IsWalletApplied)
                                        {
                                            if (saveBookingDetails != null)
                                            {
                                                var bonusHistoryData = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.UserId!.Equals(model.riderDetail!.Id) && x.IsApplied == true).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                                                if (bonusHistoryData != null)
                                                {
                                                    bonusHistoryData.BookingId = saveBookingDetails.Id;
                                                    bonusHistoryData.UpdatedDate = DateTime.UtcNow;
                                                    bonusHistoryData.UpdatedBy = bonusHistoryData.UserId;
                                                    this.DbContextObj().Entry(bonusHistoryData).State = EntityState.Modified;
                                                    await this.DbContextObj().SaveChangesAsync();
                                                }
                                            }

                                        }
                                        return Task<UserData>.FromResult(intentData!.PaymentStatus).Result;

                                    }
                                    else
                                    {
                                        StripeCustomerIntentCustom dataIntent = new StripeCustomerIntentCustom();
                                        CaptureAfterIntent resultIntent = new CaptureAfterIntent();

                                        try
                                        {
                                            dataIntent = await _paymentRepository.CreatePaymentIntent("usd", (decimal.Subtract(result.Price, (decimal)model.CashBackPrice)) * 100, model.IsWalletApplied, model.CashBackPrice, cancellationToken).ConfigureAwait(false);

                                            if (!string.IsNullOrEmpty(dataIntent?.Status) && !string.IsNullOrEmpty(dataIntent?.Id))
                                            {
                                                if (dataIntent.Status!.Equals("requires_confirmation"))
                                                {
                                                    paymentIntentId = _context.HttpContext!.Session.GetString("paymentIntentId")!;
                                                    paymetMethodId = _context.HttpContext!.Session.GetString("paymetMethodId")!;

                                                    if (cancellationToken.IsCancellationRequested)
                                                        cancellationToken.ThrowIfCancellationRequested();

                                                    resultIntent = await _paymentRepository.ConfirmPaymentIntent(dataIntent.PaymentMethod, dataIntent.Id, cancellationToken).ConfigureAwait(false);


                                                    if (resultIntent.Status!.Equals("requires_capture"))
                                                    {
                                                        var saveBookingDetails = await BookingDetails(result, model, userDataResponse, dataIntent.Id, dataIntent.PaymentMethod!, resultIntent.Status).ConfigureAwait(false);

                                                        var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(saveBookingDetails.RiderId)).FirstOrDefaultAsync()!;
                                                        if (wallateRecord != null)
                                                        {
                                                            if (model.IsWalletApplied && ((decimal)wallateRecord!.Balance! > (decimal)model.CashBackPrice))
                                                            {

                                                                wallateRecord!.Balance = decimal.Subtract((decimal)wallateRecord.Balance!, (decimal)model.CashBackPrice);// BonusCalculate(wallateRecord, saveBookingDetails);
                                                                wallateRecord.UpdatedDate = DateTime.UtcNow;
                                                                wallateRecord.IsApplied = true;
                                                                wallateRecord.UpdatedBy = userId;
                                                                this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                                                                await this.DbContextObj().SaveChangesAsync();



                                                                RideBonusHistory rideBonusHistory = new RideBonusHistory();
                                                                rideBonusHistory.Id = Guid.NewGuid();
                                                                rideBonusHistory.DigitalWalletId = wallateRecord.Id;
                                                                rideBonusHistory.UserId = saveBookingDetails.RiderId;
                                                                rideBonusHistory.IsApplied = true;
                                                                rideBonusHistory.Bonus = 0.0m;
                                                                rideBonusHistory.CashBack = (decimal)model.CashBackPrice;
                                                                rideBonusHistory.BookingId = saveBookingDetails.Id;
                                                                rideBonusHistory.CreatedDate = DateTime.UtcNow;
                                                                await this.DbContextObj().TblRideBonusHistory.AddAsync(rideBonusHistory);
                                                                await this.DbContextObj().SaveChangesAsync();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var walletRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(saveBookingDetails.RiderId)).FirstOrDefaultAsync()!;
                                                            if (walletRecord != null)
                                                            {
                                                                walletRecord!.IsApplied = false;
                                                                walletRecord.UpdatedBy = userId;
                                                                this.DbContextObj().TblDigitalWallet.Update(walletRecord);
                                                                await this.DbContextObj().SaveChangesAsync();
                                                            }
                                                        }
                                                        if (cancellationToken.IsCancellationRequested)
                                                            cancellationToken.ThrowIfCancellationRequested();
                                                        Parallel.Invoke(() => SendNotification(notificationModel, userData));
                                                        if (cancellationToken.IsCancellationRequested)
                                                            cancellationToken.ThrowIfCancellationRequested();
                                                        return Task<UserData>.FromResult(resultIntent.Status).Result;

                                                    }

                                                    else if (resultIntent.Status!.Equals("requires_action"))
                                                    {
                                                        var record = await this.DbContextObj().Users.Where(s => s.Id == userDataResponse.driverDetail.Id).FirstAsync();
                                                        record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);

                                                        await _userManager.UpdateAsync(record).ConfigureAwait(false);
                                                        _logger.LogInformation("{0}InSide  NotifyDriver in BookingSystemRepository Method Details: --DriverId:{1}  DeviceId : {2}  IsAndroidDevice: {3} Title:{4} CancellationToken = {5}  ResultIntentStatus : {6}", DateTime.UtcNow, result.UserId, notificationModel.DeviceId, notificationModel.IsAndroidDevice, notificationModel.Title, cancellationToken.IsCancellationRequested, resultIntent.Status);

                                                        return Task<UserData>.FromResult(resultIntent.Status).Result;
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                var data = await _userManager.FindByEmailAsync(result.Email).ConfigureAwait(false);
                                                data.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                                                data.UpdatedDate = DateTime.UtcNow;
                                                data.UpdatedBy = userId;
                                                await _userManager.UpdateAsync(data).ConfigureAwait(false);
                                                // added newly
                                                return Task<UserData>.FromResult("Not Found").Result;
                                            }



                                        }
                                        catch (Stripe.StripeException ex)
                                        {
                                            switch (ex.StripeError.Type)// .HttpStatusCode)
                                            {
                                                case "card_error":
                                                case "invalid_request_error":

                                                    var record = await this.DbContextObj().Users.Where(s => s.Id == userDataResponse.driverDetail.Id).FirstAsync();
                                                    record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);

                                                    await _userManager.UpdateAsync(record).ConfigureAwait(false);
                                                    return Task<UserData>.FromResult(ex.StripeError.Type.ToString()).Result;


                                            }


                                        }




                                    }



                                    break;
                                }
                            case AuthorizationLevel.Roles.Driver:
                                {
                                    var driverId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                                    var intentData = this.DbContextObj().TblBookingDetail.Where(s => s.RiderId == userDataResponse.riderDetail.Id && s.DriverId == driverId && s.PaymentStatus!.Equals("requires_capture")).OrderByDescending(s => s.UpdatedDate).Select(s => new
                                    {
                                        s.PaymentIntentId,
                                        s.DefaultPaymentMethodId,
                                        s.PaymentStatus
                                    }).FirstOrDefault();
                                    var saveBookingDetails = await BookingDetails(result, model, userDataResponse, intentData?.PaymentIntentId!, intentData?.DefaultPaymentMethodId!, intentData?.PaymentStatus!).ConfigureAwait(false);
                                    Parallel.Invoke(() => SendNotification(notificationModel, userData));
                                    return Task<UserData>.FromResult("Success").Result;

                                }
                        }
                        #endregion

                        return Task<UserData>.FromResult("Failed").Result;

                    }


                    #endregion
                }
                #endregion
            }


            catch (SqlException)
            {
                _logger.LogInformation("{0} InSide Catch to handle cancellation  NotifyDriver in BookingSystemRepository Method after Sp_NotifyNearestDriverBooking -- Type={1} DriverId = {2} , RiderName = {3}, RiderId = {4}, RiderDeviceId = {5}, DriverId ={6}, DriverDeviceId = {7}  CancellationToken = {8}", DateTime.UtcNow, "", model.riderDetail!.Id, "", "", "", "", "", cancellationToken.IsCancellationRequested);

                try
                {
                    var detailData = await this.DbContextObj().TblBookingDetail.Where(s => s.RiderId == model.riderDetail!.Id && s.StatusType == "1").OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();
                    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                    if (detailData != null)
                    {
                        var service = new PaymentIntentService();
                        var data = await service.CancelAsync(detailData.PaymentIntentId, new PaymentIntentCancelOptions { CancellationReason = "requested_by_customer" }).ConfigureAwait(false);
                        _logger.LogInformation("{0}InSide Cancelled Request NotifyDriver in BookingSystemRepository Method -- PaymentIntentId : {1} CancellationReason:{2}", DateTime.UtcNow, detailData.PaymentIntentId, "requested_by_customer");
                        StripeCustomerIntentCustom res = JsonConvert.DeserializeObject<StripeCustomerIntentCustom>(data.ToJson())!;
                        detailData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED);
                        detailData.UpdatedDate = DateTime.UtcNow;
                        detailData.UpdatedBy = detailData.RiderId;
                        detailData.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                        detailData.PaymentStatus = res.Status;
                        _logger.LogInformation("{0}InSide Cancelled Request NotifyDriver in BookingSystemRepository Method -- BookingStatusId : {1} RiderId:{2} StatusType: {3}", DateTime.UtcNow, detailData.BookingStatusId, detailData.RiderId, detailData.StatusType);
                        var result = await this.DbContextObj().Users.Where(s => s.Id == detailData.DriverId).AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);

                        if (result != null && result.BookingStatusId != Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED))
                        {
                            result.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                            result.UpdatedDate = DateTime.UtcNow;
                            result.UpdatedBy = detailData.RiderId;

                            this.DbContextObj().Users.Update(result);
                            await this.DbContextObj().SaveChangesAsync();
                        }
                        this.DbContextObj().Entry(detailData).State = EntityState.Modified;
                        await this.DbContextObj().SaveChangesAsync();
                        var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(detailData.RiderId)).FirstOrDefaultAsync()!;
                        var bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.UserId == (detailData.RiderId!) && x.BookingId == detailData.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                        if (wallateRecord != null && wallateRecord.IsApplied)
                        {
                            wallateRecord!.Balance += bonusRecord!.CashBack;
                            wallateRecord.UpdatedDate = DateTime.UtcNow;
                            wallateRecord.UpdatedBy = detailData.RiderId;
                            this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                            await this.DbContextObj().SaveChangesAsync();
                        }



                        if (bonusRecord != null)
                        {
                            bonusRecord.IsApplied = false;
                            bonusRecord.CashBack = 0.0m;
                            this.DbContextObj().TblRideBonusHistory.Update(bonusRecord);
                            await this.DbContextObj().SaveChangesAsync();
                        }

                        //var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED, IntentPaymentStatus = "cancelled" };
                        //notificationModel = await NotificationBody(result!.DeviceId!, "Ride cancelled", "Booking Request cancelled by user");
                        //SendNotification(notificationModel, dataUser);

                        //NotificationModel notificationModel = new NotificationModel
                        //{
                        //    DeviceId = result!.DeviceId,
                        //    IsAndroidDevice = result.Platform!.Where(s => s.Equals("Android") || s.Equals("Mobile")) != null ? true : false,
                        //    Title = "Ride cancelled",
                        //    Body = "Booking Request cancelled by user"

                        //};
                        // SendNotification(notificationModel, dataUser);

                        return Task<UserData>.FromResult("CancellException").Result;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                throw;
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region RideBookingStatusUpdate
        /// <summary>
        /// API to update status of booking ride
        /// </summary>
        /// <param name="bookingStatus"></param>
        /// <returns></returns>
        public async Task<BookingStatusResponse> RideBookingStatusUpdate(RideBookingStatus bookingStatus, BookingDetail record)
        {
            try
            {
                UserData uData = new UserData();
                _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:= {1} IsAccepted:={2}", DateTime.UtcNow, bookingStatus.RiderId, bookingStatus.IsAccepted);
                BookingStatusResponse statusResponse = new BookingStatusResponse();
                string userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
                _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:= {1} IsAccepted:={2} DriverId(UserId):={3}", DateTime.UtcNow, bookingStatus.RiderId, bookingStatus.IsAccepted, userId);
                ApplicationUser userData = this.DbContextObj().Users.Where(x => x.Id.Equals(userId) && x.IsDeleted == false && x.IsActive == true).FirstOrDefault()!;
                IdentityResult result = null!;
                if (userData is not null)
                {
                    switch (bookingStatus.IsAccepted)
                    {
                        case GlobalConstants.GlobalValues.BookingStatus.ST_ACCEPTED:
                            {
                                record = this.DbContextObj().TblBookingDetail.AsNoTracking().Where(s => s.DriverId == userId && (s.StatusType == "1") && !string.IsNullOrEmpty(s.PaymentStatus)).FirstOrDefault()!;
                                var isCancelled = this.DbContextObj().TblBookingDetail.AsNoTracking().Where(s => s.DriverId == userId && !string.IsNullOrEmpty(s.PaymentStatus)).OrderByDescending(s => s.UpdatedDate).FirstOrDefault()!;
                                if (isCancelled != null && (isCancelled.StatusType == "6" || isCancelled!.StatusType == "9") && (isCancelled.BookingStatusId.ToString()!.ToUpper().Equals(GlobalConstants.GlobalValues.BookingStatus.CANCELLED) || isCancelled.BookingStatusId.ToString()!.ToUpper().Equals(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE)))
                                {
                                    statusResponse.BookingStatus = this.DbContextObj().TblBookingStatus.AsNoTracking().Where(x => x.Id.Equals(Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED))).FirstOrDefault()!.Name;
                                    statusResponse.DriverId = userId!;
                                    statusResponse.BookingStatusId = GlobalConstants.GlobalValues.BookingStatus.CANCELLED;
                                    statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                                    statusResponse.PaymentToDriverStatus = isCancelled?.PaymentStatus;
                                    _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);
                                    return statusResponse;
                                }
                                userData.UpdatedDate = DateTime.UtcNow;
                                userData.UpdatedBy = userId!;



                                userData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.ACCEPT);
                                statusResponse.BookingStatus = this.DbContextObj().TblBookingStatus.AsNoTracking().Where(x => x.Id.Equals(userData.BookingStatusId)).FirstOrDefault()!.Name;
                                statusResponse.DriverId = userId!;
                                statusResponse.BookingStatusId = userData.BookingStatusId.ToString().ToUpper();
                                statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_ACCEPTED;
                                _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);
                                result = await _userManager.UpdateAsync(userData);
                                _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   DriverId:={1} BookingStatusId:={2} UserDataUpdateResult:={3}", DateTime.UtcNow, userData.Id, userData.BookingStatusId, result.Succeeded);

                                switch (record)
                                {
                                    case null:
                                        {
                                            break;
                                        }
                                    default:
                                        {
                                            record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.ACCEPT);
                                            record!.RiderId = bookingStatus.RiderId;
                                            record.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_ACCEPTED;
                                            record.CreatedDate = DateTime.UtcNow;
                                            record.UpdatedBy = userId!;
                                            record.UpdatedDate = DateTime.UtcNow;

                                            record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);

                                            var distanceCalculate = new DistanceCalculate();

                                            distanceCalculate.Source = new Posh_TRPT_Domain.BookingSystem.Source()
                                            {
                                                Latitude = (double)record.RiderLat!,

                                                Longitude = (double)record.RiderLong!
                                            };


                                            distanceCalculate.Destination = new Posh_TRPT_Domain.BookingSystem.Destination()
                                            {
                                                Latitude = (double)record.DriverLat!,

                                                Longitude = (double)record.DriverLong!
                                            };

                                            var minimumRiderDriverDistance = await DistanceTimeBetweenSourceDestination(distanceCalculate);

                                            record.RiderDriverDistance = minimumRiderDriverDistance.Item1 * 0.00062137m;


                                            this.DbContextObj().TblBookingDetail.Update(record);
                                            int resultBook = await this.DbContextObj().SaveChangesAsync();
                                            _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:={1} BookingStatusId:={2} BookingDetailUpdateResult:={3}", DateTime.UtcNow, record!.RiderId, record!.BookingStatusId, resultBook);
                                            _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   Going to call GetDriverInfoAfterBookingStatusUpdate(driverId):={1} ", DateTime.UtcNow, statusResponse.DriverId);
                                            Task dataNotification = GetDriverInfoAfterBookingStatusUpdate(new DistanceCalculate(), statusResponse.DriverId, "NewDriver");
                                            _ = Task.WhenAll(dataNotification);

                                            break;
                                        }
                                }
                                break;
                            }
                        case GlobalConstants.GlobalValues.BookingStatus.ST_DECLINED:
                        case GlobalConstants.GlobalValues.BookingStatus.ST_AUTO_DECLINED:
                            {
                                record = this.DbContextObj().TblBookingDetail.AsNoTracking().Where(s => s.DriverId == userId && s.StatusType == "1" && !string.IsNullOrEmpty(s.PaymentStatus)).FirstOrDefault()!;
                                DriverBookingData data = _mapper.Map<DriverBookingData>(record);
                                var isCancelled = this.DbContextObj().TblBookingDetail.AsNoTracking().Where(s => s.DriverId == userId && !string.IsNullOrEmpty(s.PaymentStatus)).OrderByDescending(s => s.UpdatedDate).FirstOrDefault()!;
                                if (isCancelled != null && (isCancelled.StatusType == "6" || isCancelled!.StatusType == "9") && (isCancelled.BookingStatusId.ToString()!.ToUpper().Equals(GlobalConstants.GlobalValues.BookingStatus.CANCELLED) || isCancelled.BookingStatusId.ToString()!.ToUpper().Equals(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE)))
                                {
                                    statusResponse.BookingStatus = this.DbContextObj().TblBookingStatus.AsNoTracking().Where(x => x.Id.Equals(Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED))).FirstOrDefault()!.Name;
                                    statusResponse.DriverId = userId!;
                                    statusResponse.BookingStatusId = GlobalConstants.GlobalValues.BookingStatus.CANCELLED;
                                    statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                                    statusResponse.PaymentToDriverStatus = isCancelled?.PaymentStatus;
                                    _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);
                                    return statusResponse;
                                }

                                switch (record?.BookingStatusId.ToString()!.ToUpper())
                                {
                                    case GlobalConstants.GlobalValues.BookingStatus.NOTIFIED_DRIVER:
                                    case GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE:
                                        {
                                            switch (record)
                                            {
                                                case null:
                                                    {
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        if (bookingStatus.IsAccepted.Equals(GlobalConstants.GlobalValues.BookingStatus.ST_DECLINED))
                                                        {
                                                            record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.DECLINED);
                                                            record.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_DECLINED;
                                                        }
                                                        else
                                                        {
                                                            record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE);
                                                            record.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_AUTO_DECLINED;
                                                        }

                                                        record!.RiderId = bookingStatus.RiderId;
                                                        record!.RiderDeviceId = this.DbContextObj().Users.AsNoTracking().Where(x => x.Id.Equals(bookingStatus.RiderId)).FirstOrDefault()!.DeviceId;
                                                        record.UpdatedBy = userId!;
                                                        record.UpdatedDate = DateTime.UtcNow;
                                                        record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);
                                                        this.DbContextObj().TblBookingDetail.Update(record);
                                                        await this.DbContextObj().SaveChangesAsync();

                                                        DistanceCalculate model = new DistanceCalculate();
                                                        model.Source = new Posh_TRPT_Domain.BookingSystem.Source { SourceName = record.RiderSourceName, Latitude = (double)record.RiderDestinationLat!, Longitude = (double)record.RiderDestinationLong! };
                                                        model.Destination = new Destination { DestinationName = record.DestinationPlaceName, Latitude = (double)record.RiderDestinationLat!, Longitude = (double)record.RiderDestinationLong! };
                                                        model.riderDetail = new RiderDetail
                                                        {
                                                            Id = record.RiderId,
                                                            DeviceId = null!,
                                                        };
                                                        model.StateId = (Guid)record!.CityId!;
                                                        model.CategoryId = (Guid)record.CategoryId!;
                                                        model.driverDetail = new DriverDetail
                                                        {
                                                            Id = record.DriverId!,
                                                            DeviceId = null!,
                                                        };
                                                        model.MinimumDistance = (int)record.MinimumDistance!;
                                                        model.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_NOTIFIED_DRIVER;


                                                        if (bookingStatus.IsAccepted.Equals(GlobalConstants.GlobalValues.BookingStatus.ST_DECLINED))
                                                        {
                                                            _logger.LogInformation("{0} InSide Decliend  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:={1} BookingStatusId:={2}  RiderDeviceId=:{3}", DateTime.UtcNow, record!.RiderId, record!.BookingStatusId, record!.RiderDeviceId);
                                                            _logger.LogInformation("{0} InSide Declined Case  RideBookingStatusUpdate in BookingSystemRepository Method   Going to call RideBookingNotifyDriver(model):= source.SourceName{1}, source.Latitude{2},source.Longitude{3},Destination.SourceName{4}, Destination.Latitude{5},Destination.Longitude{6},RiderDetails.Id={7},RiderDetails.DeviceId={8},DriverDetails.Id={9},DriverDetails.DeviceId={10},MinimumDistance:={11},CityId={12},CategoryId:={13}", DateTime.UtcNow, record.RiderSourceName, (double)record.RiderDestinationLat!, (double)record.RiderDestinationLong!,
                                                            record.DestinationPlaceName, (double)record.RiderDestinationLat!, (double)record.RiderDestinationLong!, record.RiderId, model.riderDetail!.DeviceId!, record.DriverId!, model.driverDetail!.DeviceId, model.MinimumDistance, model.StateId, model.CategoryId);
                                                            statusResponse.BookingStatus = "DECLINED";
                                                            statusResponse.DriverId = record.DriverId!;
                                                            statusResponse.BookingStatusId = GlobalConstants.GlobalValues.BookingStatus.DECLINED;
                                                            statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_DECLINED;
                                                        }
                                                        else
                                                        {
                                                            _logger.LogInformation("{0} InSide Decliend  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:={1} BookingStatusId:={2}  RiderDeviceId=:{3}", DateTime.UtcNow, record!.RiderId, record!.BookingStatusId, record!.RiderDeviceId);
                                                            _logger.LogInformation("{0} InSide Declined Case  RideBookingStatusUpdate in BookingSystemRepository Method   Going to call RideBookingNotifyDriver(model):= source.SourceName{1}, source.Latitude{2},source.Longitude{3},Destination.SourceName{4}, Destination.Latitude{5},Destination.Longitude{6},RiderDetails.Id={7},RiderDetails.DeviceId={8},DriverDetails.Id={9},DriverDetails.DeviceId={10},MinimumDistance:={11},CityId={12},CategoryId:={13}", DateTime.UtcNow, record.RiderSourceName, (double)record.RiderDestinationLat!, (double)record.RiderDestinationLong!,
                                                            record.DestinationPlaceName, (double)record.RiderDestinationLat!, (double)record.RiderDestinationLong!, record.RiderId, model.riderDetail!.DeviceId!, record.DriverId!, model.driverDetail!.DeviceId, model.MinimumDistance, model.StateId, model.CategoryId);
                                                            statusResponse.BookingStatus = "AutomaticDecline";
                                                            statusResponse.DriverId = record.DriverId!;
                                                            statusResponse.BookingStatusId = string.Empty;
                                                            statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_AUTO_DECLINED;
                                                        }

                                                        //var notifyDriver = await NotifyDriver(model, cancellationToken, string.Empty, string.Empty).ConfigureAwait(false);   string paymentIntentId = string.Empty;


                                                        _context.HttpContext.Session.SetString("paymentIntentId", record?.PaymentIntentId!);
                                                        _context.HttpContext.Session.SetString("paymetMethodId", record?.DefaultPaymentMethodId!);


                                                        userData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                                                        userData.UpdatedDate = DateTime.UtcNow;
                                                        result = await _userManager.UpdateAsync(userData).ConfigureAwait(false);

                                                        _logger.LogInformation("{0} InSide Declined RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);

                                                        break;
                                                    }

                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED:
                            {
                                DriverBookingData data = _mapper.Map<DriverBookingData>(record);


                                switch (record?.BookingStatusId.ToString()!.ToUpper())
                                {
                                    case GlobalConstants.GlobalValues.BookingStatus.ACCEPT:
                                        {
                                            switch (record)
                                            {
                                                case null:
                                                    {
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED);
                                                        record.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                                                        record!.RiderId = bookingStatus.RiderId;
                                                        record!.RiderDeviceId = this.DbContextObj().Users.AsNoTracking().Where(x => x.Id.Equals(bookingStatus.RiderId)).FirstOrDefault()!.DeviceId;
                                                        record.UpdatedBy = userId!;
                                                        record.UpdatedDate = DateTime.UtcNow;
                                                        record.iscancelled = true;
                                                        record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);


                                                        DistanceCalculate model = new DistanceCalculate();
                                                        model.Source = new Posh_TRPT_Domain.BookingSystem.Source { SourceName = record.RiderSourceName, Latitude = (double)record.RiderDestinationLat!, Longitude = (double)record.RiderDestinationLong! };
                                                        model.Destination = new Destination { DestinationName = record.DestinationPlaceName, Latitude = (double)record.RiderDestinationLat!, Longitude = (double)record.RiderDestinationLong! };
                                                        model.riderDetail = new RiderDetail
                                                        {
                                                            Id = record.RiderId,
                                                            DeviceId = null!,
                                                        };
                                                        model.StateId = (Guid)record!.CityId!;
                                                        model.CategoryId = (Guid)record.CategoryId!;
                                                        model.driverDetail = new DriverDetail
                                                        {
                                                            Id = record.DriverId!,
                                                            DeviceId = null!,
                                                        };
                                                        model.MinimumDistance = (int)record.MinimumDistance!;
                                                        model.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_NOTIFIED_DRIVER;


                                                        {
                                                            _logger.LogInformation("{0} InSide Decliend  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:={1} BookingStatusId:={2}  RiderDeviceId=:{3}", DateTime.UtcNow, record!.RiderId, record!.BookingStatusId, record!.RiderDeviceId);
                                                            _logger.LogInformation("{0} InSide Declined Case  RideBookingStatusUpdate in BookingSystemRepository Method   Going to call RideBookingNotifyDriver(model):= source.SourceName{1}, source.Latitude{2},source.Longitude{3},Destination.SourceName{4}, Destination.Latitude{5},Destination.Longitude{6},RiderDetails.Id={7},RiderDetails.DeviceId={8},DriverDetails.Id={9},DriverDetails.DeviceId={10},MinimumDistance:={11},CityId={12},CategoryId:={13}", DateTime.UtcNow, record.RiderSourceName, (double)record.RiderDestinationLat!, (double)record.RiderDestinationLong!,
                                                            record.DestinationPlaceName, (double)record.RiderDestinationLat!, (double)record.RiderDestinationLong!, record.RiderId, model.riderDetail!.DeviceId!, record.DriverId!, model.driverDetail!.DeviceId, model.MinimumDistance, model.StateId, model.CategoryId);
                                                            statusResponse.BookingStatus = "Your ride is cancelled";
                                                            statusResponse.DriverId = record.DriverId!;
                                                            statusResponse.BookingStatusId = string.Empty;
                                                            statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                                                        }


                                                        _context.HttpContext.Session.SetString("paymentIntentId", record?.PaymentIntentId!);
                                                        _context.HttpContext.Session.SetString("paymetMethodId", record?.DefaultPaymentMethodId!);


                                                        userData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                                                        userData.UpdatedDate = DateTime.UtcNow;
                                                        result = await _userManager.UpdateAsync(userData).ConfigureAwait(false);



                                                        if (record != null)
                                                        {
                                                            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                                                            var service = new PaymentIntentService();
                                                            var data1 = await service.CancelAsync(record.PaymentIntentId, new PaymentIntentCancelOptions { CancellationReason = "requested_by_customer" }).ConfigureAwait(false);
                                                            _logger.LogInformation("{0}InSide Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- PaymentIntentId : {1} CancellationReason:{2}", DateTime.UtcNow, record.PaymentIntentId, "requested_by_customer");
                                                            StripeCustomerIntentCustom res = JsonConvert.DeserializeObject<StripeCustomerIntentCustom>(data1.ToJson())!;
                                                            record.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED);
                                                            record.UpdatedDate = DateTime.UtcNow;
                                                            record.UpdatedBy = record.RiderId;
                                                            record.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                                                            _logger.LogInformation("{0}InSide Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- BookingStatusId : {1} RiderId:{2} StatusType: {3}", DateTime.UtcNow, record.BookingStatusId, record.RiderId, record.StatusType);
                                                            var finalResult = await this.DbContextObj().Users.Where(s => s.Id == record.DriverId).FirstOrDefaultAsync().ConfigureAwait(false);

                                                            if (finalResult != null)
                                                            {
                                                                finalResult.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                                                                finalResult.UpdatedDate = DateTime.UtcNow;
                                                                finalResult.UpdatedBy = record.RiderId;

                                                                this.DbContextObj().Entry(finalResult).State = EntityState.Modified;
                                                                await this.DbContextObj().SaveChangesAsync();
                                                                var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(record.RiderId)).FirstOrDefaultAsync()!;
                                                                var bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.BookingId!.Equals(record.Id)).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                                                                if (wallateRecord != null && wallateRecord.IsApplied && bonusRecord != null)
                                                                {
                                                                    wallateRecord!.Balance += bonusRecord!.CashBack;
                                                                    wallateRecord.UpdatedDate = DateTime.UtcNow;
                                                                    wallateRecord.UpdatedBy = record.RiderId;
                                                                    this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                                                                    await this.DbContextObj().SaveChangesAsync();
                                                                }
                                                                if (bonusRecord != null)
                                                                {
                                                                    bonusRecord.IsApplied = false;
                                                                    bonusRecord.CashBack = 0.0m;
                                                                    this.DbContextObj().TblRideBonusHistory.Update(bonusRecord);
                                                                    await this.DbContextObj().SaveChangesAsync();
                                                                }

                                                            }
                                                            this.DbContextObj().Entry(record).State = EntityState.Modified;
                                                            await this.DbContextObj().SaveChangesAsync();


                                                        }

                                                        this.DbContextObj().TblBookingDetail.Update(record!);
                                                        await this.DbContextObj().SaveChangesAsync();
                                                        _logger.LogInformation("{0} InSide Declined RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);

                                                        break;
                                                    }

                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case GlobalConstants.GlobalValues.BookingStatus.ST_STARTED:
                            {
                                record = this.DbContextObj().TblBookingDetail.AsNoTracking().Where(s => s.DriverId == userId && (s.StatusType == "2") && !string.IsNullOrEmpty(s.PaymentStatus)).FirstOrDefault()!;

                                DistanceCalculate model = new DistanceCalculate();
                                Posh_TRPT_Domain.BookingSystem.Source source = new Posh_TRPT_Domain.BookingSystem.Source();
                                Posh_TRPT_Domain.BookingSystem.Destination destination = new Posh_TRPT_Domain.BookingSystem.Destination();
                                source.Longitude = (double)record.RiderLong!;
                                source.Latitude = (double)record.RiderLat!;
                                model.Source = source;
                                destination.Longitude = (double)record.RiderDestinationLong!;
                                destination.Latitude = (double)record.RiderDestinationLat!;
                                model.Destination = destination;
                                var distanceTime = await DistanceTimeBetweenSourceDestination(model);





                                userData.UpdatedDate = DateTime.UtcNow;
                                userData.UpdatedBy = userId!;
                                userData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.STARTED);
                                statusResponse.BookingStatus = this.DbContextObj().TblBookingStatus.AsNoTracking().Where(x => x.Id.Equals(userData.BookingStatusId)).FirstOrDefault()!.Name;
                                statusResponse.DriverId = userId!;
                                statusResponse.BookingStatusId = userData.BookingStatusId.ToString().ToUpper();
                                statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_STARTED;
                                _logger.LogInformation("{0} InSide Started  RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);
                                result = await _userManager.UpdateAsync(userData);
                                _logger.LogInformation("{0} InSide Started  RideBookingStatusUpdate in BookingSystemRepository Method   DriverId:={1} BookingStatusId:={2} UserDataUpdateResult:={3}", DateTime.UtcNow, userData.Id, userData.BookingStatusId, result.Succeeded);
                                switch (record)
                                {
                                    case null:
                                        {
                                            break;
                                        }
                                    default:
                                        {
                                            decimal distance = distanceTime.Item1;
                                            record.Distance = Convert.ToString(distance * 0.00062137m);

                                            int time = distanceTime.Item2;
                                            record.Time = Convert.ToString(time / 60);
                                            record.TollFees = distanceTime.Item3;

                                            record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.STARTED);
                                            record!.RiderId = bookingStatus.RiderId;
                                            record.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_STARTED;
                                            record.UpdatedBy = userId!;
                                            record.UpdatedDate = DateTime.UtcNow;
                                            record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);

                                            record.PickUpTime = Convert.ToDateTime(bookingStatus.LocalTime);

                                            this.DbContextObj().TblBookingDetail.Update(record);
                                            int resultBook = await this.DbContextObj().SaveChangesAsync();
                                            _logger.LogInformation("{0} InSide Started  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:={1} BookingStatusId:={2} BookingDetailUpdateResult:={3}", DateTime.UtcNow, record!.RiderId, record!.BookingStatusId, resultBook);
                                            break;
                                        }
                                }
                                break;
                            }
                        case GlobalConstants.GlobalValues.BookingStatus.ST_COMPLETED:
                            {
                                record = this.DbContextObj().TblBookingDetail.AsNoTracking().Where(s => s.DriverId == userId && (s.StatusType == "3") && !string.IsNullOrEmpty(s.PaymentStatus)).FirstOrDefault()!;
                                userData.UpdatedDate = DateTime.UtcNow;
                                //userData.LocalTimeZone = "Eastern Standard Time";
                                //DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(userData.UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById(LocalTimeZone));
                                userData.UpdatedBy = userId!;
                                userData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                                statusResponse.BookingStatus = this.DbContextObj().TblBookingStatus.AsNoTracking().Where(x => x.Id.Equals(Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.COMPLETED))).FirstOrDefault()!.Name;
                                statusResponse.DriverId = userId!;
                                statusResponse.BookingStatusId = GlobalConstants.GlobalValues.BookingStatus.COMPLETED;
                                statusResponse.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_COMPLETED;
                                result = await _userManager.UpdateAsync(userData).ConfigureAwait(false);
                                _logger.LogInformation("{0} InSide Completed  RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatus:= {1} DriverId:={2} BookingStatusId:{3} StatusType:{4} PaymentToDriverStatus:{5}", DateTime.UtcNow, statusResponse.BookingStatus, statusResponse.DriverId, statusResponse.BookingStatusId, statusResponse.StatusType, statusResponse.PaymentToDriverStatus);
                                _logger.LogInformation("{0} InSide Completed  RideBookingStatusUpdate in BookingSystemRepository Method   DriverId:={1} BookingStatusId:={2} UserDataUpdateResult:={3}", DateTime.UtcNow, userData.Id, userData.BookingStatusId, result.Succeeded);
                                switch (record)
                                {
                                    case null:
                                        {
                                            break;
                                        }
                                    default:
                                        {

                                            var riderRetail = await this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(record.RiderId)).Select(c => new { c.DeviceId, c.Email }).FirstOrDefaultAsync()!;

                                            record!.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.COMPLETED);
                                            record!.RiderId = bookingStatus.RiderId;
                                            record!.RiderDeviceId = riderRetail!.DeviceId;
                                            record!.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_COMPLETED;
                                            record.UpdatedBy = userId!;
                                            record.UpdatedDate = DateTime.UtcNow;
                                            record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);
                                            record.DropTime = Convert.ToDateTime(bookingStatus.LocalTime);


                                            _logger.LogInformation("{0} InSide Completed  RideBookingStatusUpdate in BookingSystemRepository Method   BookingStatusId:= {1} RiderId:={2} StatusType:{3} UpdatedBy:{4}", DateTime.UtcNow, record!.BookingStatusId, record!.RiderId, record!.StatusType, record.UpdatedBy);
                                            CaptureAfterIntent resultIntent = null!;
                                            int resultBook = 0;
                                            if (record.PaymentStatus!.Equals("requires_capture"))
                                            {
                                                _logger.LogInformation("{0} InSide Completed before resultIntent  RideBookingStatusUpdate in BookingSystemRepository Method   PaymentStatus:={1}", DateTime.UtcNow, record.PaymentStatus);

                                                var intentStatusId = await this.DbContextObj().TblPaymentIntentCapture.Where(s => s.PaymentIntentCaptureId == record.PaymentIntentId).Select(s => s.Status).FirstOrDefaultAsync();
                                                if (intentStatusId != null && intentStatusId.Equals("succeeded"))
                                                {
                                                    #region running code with percentage calculation


                                                    resultBook = await this.DbContextObj().SaveChangesAsync();
                                                    var driverAccoun = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
                                                    var riderAccount = await _userManager.FindByIdAsync(bookingStatus.RiderId).ConfigureAwait(false);
                                                    var processFee = Math.Round((0.30 + Convert.ToDouble(record.Price) * 0.029), 2);

                                                    var rateToAccount = (Convert.ToDecimal(record.Price) - record.ServiceFee) - Convert.ToDecimal(processFee);

                                                    var promoPrice = await this.DbContextObj().TblStripeCustomers.Where(x => x.UserId!.Equals(record.RiderId)).Select(x => x.Promotion).FirstOrDefaultAsync();
                                                    #endregion
                                                    //rateToAccount += promoPrice;
                                                    var resultPay = await _paymentRepository.StripeDriverPaymentSystem(new StripeDriverPaymentInfo { Rate = ((int)(rateToAccount * 100)!), DriverAccountNo = driverAccoun.StripeConnectedAccountId, RiderId = bookingStatus.RiderId, RiderCustomerId = riderAccount.StripeCustomerId }).ConfigureAwait(false);
                                                    if (resultPay != null && !string.IsNullOrEmpty(resultPay.Id))
                                                    {
                                                        DigitalWalletData walletData = null!;
                                                        var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(record.RiderId)).FirstOrDefaultAsync()!;
                                                        var BonusRecord = await this.DbContextObj().TblCategoryPrices?.Where(x => x.Id!.Equals(record.CategoryPriceId)).FirstOrDefaultAsync()!;
                                                        if (wallateRecord is null)
                                                        {
                                                            walletData = new DigitalWalletData
                                                            {
                                                                Id = Guid.NewGuid(),
                                                                UserId = record.RiderId,
                                                                BookingId = record.Id,
                                                                //Balance = Math.Truncate((decimal)(record.ServiceFee * 0.05m)! * 100) / 100,
                                                                Balance = Math.Truncate((decimal)(record.ServiceFee * BonusRecord!.Bonus_Amount)! * 100) / 100,
                                                                IsApplied = false,
                                                                IsDeleted = false,
                                                                CreatedDate = DateTime.UtcNow,
                                                                CreatedBy = record.RiderId,
                                                                UpdatedDate = null!,
                                                                UpdatedBy = null!,
                                                                IsActive = true
                                                            };
                                                            await _paymentRepository.AddMoney(walletData);
                                                            await this.DbContextObj().SaveChangesAsync();
                                                            RideBonusHistory rideBonusHistory = new RideBonusHistory();
                                                            rideBonusHistory.Id = Guid.NewGuid();
                                                            rideBonusHistory.DigitalWalletId = walletData.Id;
                                                            rideBonusHistory.UserId = record.RiderId;
                                                            //rideBonusHistory.Bonus = Math.Truncate((decimal)(record.ServiceFee * 0.05m)! * 100) / 100;
                                                            rideBonusHistory.Bonus = Math.Truncate((decimal)(record.ServiceFee * BonusRecord!.Bonus_Amount)! * 100) / 100;
                                                            rideBonusHistory.BookingId = record.Id;
                                                            rideBonusHistory.CreatedDate = DateTime.UtcNow;
                                                            await this.DbContextObj().TblRideBonusHistory.AddAsync(rideBonusHistory);
                                                            await this.DbContextObj().SaveChangesAsync();
                                                            record.Promotion = walletData.Balance;
                                                            record.UpdatedDate = DateTime.UtcNow;
                                                            record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);
                                                            this.DbContextObj().TblBookingDetail.Update(record);
                                                            await this.DbContextObj().SaveChangesAsync();
                                                        }
                                                        else
                                                        {

                                                            //wallateRecord.Balance += Math.Truncate((decimal)(record.ServiceFee * 0.05m)! * 100) / 100;
                                                            wallateRecord.Balance += Math.Truncate((decimal)(record.ServiceFee * BonusRecord!.Bonus_Amount)! * 100) / 100;
                                                            wallateRecord.UpdatedDate = DateTime.UtcNow;
                                                            wallateRecord.UpdatedBy = userId;
                                                            //wallateRecord.IsApplied=
                                                            this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                                                            await this.DbContextObj().SaveChangesAsync();
                                                            RideBonusHistory rideBonusHistory = new RideBonusHistory();
                                                            rideBonusHistory.Id = Guid.NewGuid();
                                                            rideBonusHistory.DigitalWalletId = wallateRecord.Id;
                                                            rideBonusHistory.UserId = record.RiderId;
                                                            //rideBonusHistory.Bonus = Math.Truncate((decimal)(record.ServiceFee * 0.05m)! * 100) / 100;
                                                            rideBonusHistory.Bonus = Math.Truncate((decimal)(record.ServiceFee * BonusRecord!.Bonus_Amount)! * 100) / 100;

                                                            rideBonusHistory.BookingId = record.Id;
                                                            rideBonusHistory.CreatedDate = DateTime.UtcNow;
                                                            await this.DbContextObj().TblRideBonusHistory.AddAsync(rideBonusHistory);
                                                            await this.DbContextObj().SaveChangesAsync();
                                                            record.Promotion = promoPrice;
                                                            record.UpdatedDate = DateTime.UtcNow;
                                                            record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);
                                                            this.DbContextObj().TblBookingDetail.Update(record);
                                                            await this.DbContextObj().SaveChangesAsync();
                                                        }

                                                    }
                                                    record.PaymentStatus = intentStatusId;

                                                    _logger.LogInformation("{0} InSide Completed inside resultIntent  RideBookingStatusUpdate in BookingSystemRepository Method   PaymentStatus:={1} ServiceFee:{2}", DateTime.UtcNow, intentStatusId, record.ServiceFee);
                                                    record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);
                                                    this.DbContextObj().TblBookingDetail.Update(record);
                                                    statusResponse.PaymentToDriverStatus = intentStatusId;

                                                    statusResponse.PaymentToDriverStatus = intentStatusId;
                                                    var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_COMPLETED, IntentPaymentStatus = intentStatusId };
                                                    NotificationModel notificationModel = new NotificationModel
                                                    {
                                                        DeviceId = record!.RiderDeviceId,
                                                        IsAndroidDevice = userData.Platform!.Where(s => s.Equals("Android") || s.Equals("Mobile")) != null ? true : false,
                                                        Title = "Ride completed",
                                                        Body = "Ride has been completed!"

                                                    };

                                                    Parallel.Invoke(() => SendNotification(notificationModel, dataUser), async () => await GetBookingHistoryUserDetails(record.Id!, riderRetail.Email, true));

                                                    //SendNotification(notificationModel, dataUser);
                                                    //var findResult = GetBookingHistoryUserDetails(record.Id!, true).Result;


                                                }

                                                else
                                                {
                                                    resultIntent = await _paymentRepository.StripeCaptureIntent(record.PaymentIntentId).ConfigureAwait(false);
                                                    _logger.LogInformation("{0} InSide Completed after resultIntent  RideBookingStatusUpdate in BookingSystemRepository Method   PaymentStatus:={1}", DateTime.UtcNow, resultIntent.Status);
                                                    if (resultIntent.Status!.Equals("succeeded"))
                                                    {
                                                        #region running code with percentage calculation                                                  


                                                        resultBook = await this.DbContextObj().SaveChangesAsync();
                                                        var driverAccoun = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
                                                        var riderAccount = await _userManager.FindByIdAsync(bookingStatus.RiderId).ConfigureAwait(false);
                                                        var processFee = Math.Round((0.30 + Convert.ToDouble(record.Price) * 0.029), 2);
                                                        var rateToAccount = (Convert.ToDecimal(record.Price) - record.ServiceFee) - Convert.ToDecimal(processFee);
                                                        var promoPrice = await this.DbContextObj().TblStripeCustomers.Where(x => x.UserId!.Equals(record.RiderId)).Select(x => x.Promotion).FirstOrDefaultAsync();
                                                        #endregion
                                                        //rateToAccount += promoPrice;
                                                        var resultPay = await _paymentRepository.StripeDriverPaymentSystem(new StripeDriverPaymentInfo { Rate = ((int)(rateToAccount * 100)!), DriverAccountNo = driverAccoun.StripeConnectedAccountId, RiderId = bookingStatus.RiderId, RiderCustomerId = riderAccount.StripeCustomerId }).ConfigureAwait(false);
                                                        if (resultPay != null && !string.IsNullOrEmpty(resultPay.Id))
                                                        {

                                                            DigitalWalletData walletData = null!;
                                                            var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(record.RiderId)).FirstOrDefaultAsync()!;
                                                            var BonusRecord = await this.DbContextObj().TblCategoryPrices?.Where(x => x.Id!.Equals(record.CategoryPriceId)).FirstOrDefaultAsync()!;
                                                            if (wallateRecord is null)
                                                            {
                                                                // When wallet is not created
                                                                walletData = new DigitalWalletData
                                                                {
                                                                    Id = Guid.NewGuid(),
                                                                    UserId = record.RiderId,
                                                                    BookingId = record.Id,
                                                                    //Balance = BonusCalculate(new DigitalWallet { Balance = 0.0m }, record),
                                                                    Balance = BonusCalculate(new DigitalWallet { Balance = 0.0m }, record, BonusRecord!.Bonus_Amount),
                                                                    IsApplied = false,
                                                                    IsDeleted = false,
                                                                    CreatedDate = DateTime.UtcNow,
                                                                    CreatedBy = record.RiderId,
                                                                    UpdatedDate = null!,
                                                                    UpdatedBy = null!,
                                                                    IsActive = true
                                                                };
                                                                await _paymentRepository.AddMoney(walletData);
                                                                await this.DbContextObj().SaveChangesAsync();


                                                                // When rider bonus history is not created
                                                                RideBonusHistory rideBonusHistory = new RideBonusHistory()
                                                                {
                                                                    Id = Guid.NewGuid(),
                                                                    DigitalWalletId = walletData.Id,
                                                                    IsApplied = false,
                                                                    UserId = record.RiderId,
                                                                    Bonus = walletData.Balance,
                                                                    BookingId = record.Id,
                                                                    CreatedDate = DateTime.UtcNow
                                                                };
                                                                await this.DbContextObj().TblRideBonusHistory.AddAsync(rideBonusHistory);
                                                                await this.DbContextObj().SaveChangesAsync();

                                                            }
                                                            else
                                                            {


                                                                //wallateRecord.Balance = BonusCalculate(wallateRecord, record);
                                                                wallateRecord.Balance = BonusCalculate(wallateRecord, record, BonusRecord!.Bonus_Amount);
                                                                wallateRecord.UpdatedDate = DateTime.UtcNow;
                                                                wallateRecord.UpdatedBy = userId;
                                                                this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                                                                await this.DbContextObj().SaveChangesAsync();

                                                                var bookingBonusHistory = await this.DbContextObj().TblRideBonusHistory.Where(s => s.BookingId.Equals(record.Id)).FirstOrDefaultAsync();

                                                                if (bookingBonusHistory != null && bookingBonusHistory!.IsApplied)
                                                                {
                                                                    //bookingBonusHistory.Bonus = BonusCalculate(new DigitalWallet { Balance = 0.0m }, record);
                                                                    bookingBonusHistory.Bonus = BonusCalculate(new DigitalWallet { Balance = 0.0m }, record, BonusRecord!.Bonus_Amount);
                                                                    bookingBonusHistory.UpdatedDate = DateTime.UtcNow;
                                                                    this.DbContextObj().TblRideBonusHistory.Update(bookingBonusHistory);
                                                                    await this.DbContextObj().SaveChangesAsync();
                                                                }
                                                                else
                                                                {
                                                                    RideBonusHistory rideBonusHistory = new RideBonusHistory();
                                                                    rideBonusHistory.Id = Guid.NewGuid();
                                                                    rideBonusHistory.DigitalWalletId = wallateRecord.Id;
                                                                    rideBonusHistory.UserId = record.RiderId;
                                                                    rideBonusHistory.IsApplied = false;
                                                                    //rideBonusHistory.Bonus = BonusCalculate(new DigitalWallet { Balance = 0.0m }, record);
                                                                    rideBonusHistory.Bonus = BonusCalculate(new DigitalWallet { Balance = 0.0m }, record, BonusRecord!.Bonus_Amount);
                                                                    rideBonusHistory.BookingId = record.Id;
                                                                    rideBonusHistory.CreatedDate = DateTime.UtcNow;
                                                                    await this.DbContextObj().TblRideBonusHistory.AddAsync(rideBonusHistory);
                                                                    await this.DbContextObj().SaveChangesAsync();
                                                                }

                                                            }


                                                        }
                                                        record.PaymentStatus = resultIntent.Status;
                                                        record.UpdatedDate = DateTime.UtcNow;
                                                        record.LocalUpdatedDateTime = Convert.ToDateTime(bookingStatus.LocalTime);
                                                        this.DbContextObj().TblBookingDetail.Update(record);
                                                        await this.DbContextObj().SaveChangesAsync();

                                                        _logger.LogInformation("{0} InSide Completed inside resultIntent  RideBookingStatusUpdate in BookingSystemRepository Method   PaymentStatus:={1} ServiceFee:{2}", DateTime.UtcNow, resultIntent.Status, record.ServiceFee);
                                                        //this.DbContextObj().TblBookingDetail.Update(record);
                                                        statusResponse.PaymentToDriverStatus = resultIntent.Status;
                                                    }

                                                    statusResponse.PaymentToDriverStatus = resultIntent.Status;
                                                    var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_COMPLETED, IntentPaymentStatus = resultIntent.Status };
                                                    NotificationModel notificationModel = new NotificationModel
                                                    {
                                                        DeviceId = record!.RiderDeviceId,
                                                        IsAndroidDevice = userData.Platform!.Where(s => s.Equals("Android") || s.Equals("Mobile")) != null ? true : false,
                                                        Title = "Ride completed",
                                                        Body = "Ride has been completed!"

                                                    };
                                                    Parallel.Invoke(() => SendNotification(notificationModel, dataUser), async () => await GetBookingHistoryUserDetails(record.Id!, riderRetail.Email, true));
                                                    //SendNotification(notificationModel, dataUser);
                                                    //var findResult = GetBookingHistoryUserDetails(record.Id!, true).Result;
                                                }



                                            }
                                            _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   RiderId:={1} BookingStatusId:={2} BookingDetailUpdateResult:={3}", DateTime.UtcNow, record!.RiderId, record!.BookingStatusId, resultBook);
                                            _logger.LogInformation("{0} InSide  RideBookingStatusUpdate in BookingSystemRepository Method   Going to call GetDriverInfoAfterBookingStatusUpdate(driverId):={1} ", DateTime.UtcNow, statusResponse.DriverId);


                                            break;
                                        }

                                }
                                break;
                            }
                    }
                }
                return result.Succeeded == true ? statusResponse : Task.FromResult<BookingStatusResponse>(null!).Result;
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetDriverInfoAfterBookingStatusUpdate
        /// <summary>
        ///  This method is used to get the booking status with details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newDriver"></param>
        /// <returns></returns>
        public async Task<UserData> GetDriverInfoAfterBookingStatusUpdate(DistanceCalculate model, string userId, string newDriver, CancellationToken cancellationToken = default)
        {
            UserDataResponse userDataResponse = new UserDataResponse();
            UserData userData = new UserData();
            try
            {
                _logger.LogInformation("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemRepository Method before Sp_GetDriverInfoAfterBookingStatusUpdate -- UserId = {1} Method Calling Source ={2} CancellationToken ={3}", DateTime.UtcNow, userId, newDriver, cancellationToken.IsCancellationRequested);

                SqlParameter[] sqlParameter = new SqlParameter[]
                                                                {
                                                                                                                new SqlParameter { ParameterName ="@userId", Value = userId},
                                                                                                                new SqlParameter { ParameterName = "@Value", Value = newDriver}
                                                                };

                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();


                DriverBookingData result = await this.DbContextObj().GetRecordExecuteProcedureAsync<DriverBookingData>("Sp_GetDriverInfoAfterBookingStatusUpdate @userId,@Value", sqlParameter, cancellationToken);

                if (result.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.DECLINED) || result.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE))
                {
                    if (result.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE))
                    {
                        var detailData = await this.DbContextObj().TblBookingDetail.Where(s => s.DriverId == result.DriverId && s.RiderId == result.RiderId && s.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE)).OrderByDescending(s => s.UpdatedDate).FirstOrDefaultAsync();
                        var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_AUTO_DECLINED, IntentPaymentStatus = "requires_capture" };
                        string deviceId = this.DbContextObj().Users?.AsNoTracking().Where(s => s.Id == detailData!.DriverId).Select(s => s.DeviceId).FirstOrDefault()!;
                        NotificationModel notificationModel = new NotificationModel
                        {
                            DeviceId = deviceId,
                            IsAndroidDevice = true,
                            Title = "Ride declined",
                            Body = "Ride request auto declined by System"

                        };
                        SendNotification(notificationModel, dataUser);
                    }

                    // This method is used to notify the nearest driver
                    var driverNotify = await NotifyDriver(model, cancellationToken, null!, null!, 0);


                    UserData currentBookingDetail = new UserData();
                    switch (driverNotify)
                    {
                        case "requires_capture": currentBookingDetail = await GetDriverInfoAfterBookingStatusUpdate(model, model.riderDetail!.Id!, "NewDriver", cancellationToken); break;
                        case "requires_action": currentBookingDetail.IntentPaymentStatus = "Insufficient funds"; currentBookingDetail.type = "100"; break;
                        case "card_error": currentBookingDetail.IntentPaymentStatus = "card_error"; currentBookingDetail.type = "101"; ; break;
                        case "Not Found": currentBookingDetail.IntentPaymentStatus = "Default card not set"; currentBookingDetail.type = "102"; break;
                        case "Failed": break;
                        case "CancelException": break;
                        case "No_Driver_Available":
                            currentBookingDetail.IntentPaymentStatus = "No_Driver_Available"; currentBookingDetail.type = "103";



                            var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(result.RiderId)).FirstOrDefaultAsync()!;
                            var bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.BookingId!.Equals(result.Id)).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                            if (wallateRecord != null && wallateRecord.IsApplied && bonusRecord != null)
                            {
                                wallateRecord!.Balance += bonusRecord!.CashBack;
                                wallateRecord.UpdatedDate = DateTime.UtcNow;
                                wallateRecord.UpdatedBy = result.RiderId;
                                this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                                await this.DbContextObj().SaveChangesAsync();
                            }
                            if (bonusRecord != null)
                            {
                                bonusRecord.IsApplied = false;
                                bonusRecord.CashBack = 0.0m;
                                this.DbContextObj().TblRideBonusHistory.Update(bonusRecord);
                                await this.DbContextObj().SaveChangesAsync();
                            }



                            break;
                    }

                    // currentBookingDetail = await GetDriverInfoAfterBookingStatusUpdate(model, userId, newDriver, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();





                    return currentBookingDetail!;
                    //return currentBookingDetail;
                }
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                if (result != null && result.DriverId != null)
                {
                    var DriverPhoto = string.IsNullOrEmpty(result.ProfilePhoto) ? _config["Request:Url"] + "Images" + "/person.png" : _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + result.ProfilePhoto;
                    result.ProfilePhoto = DriverPhoto;

                    userDataResponse.price = Convert.ToString(Math.Round(Convert.ToDecimal(result.Price), 2));
                    userDataResponse.sourceLat = result.RiderLat;
                    userDataResponse.sourceLong = result.RiderLong;
                    userDataResponse.sourcePlaceName = result.RiderSourceName;
                    userDataResponse.destinationLat = result.RiderDestinationLat;
                    userDataResponse.destinationLong = result.RiderDestinationLong;
                    userDataResponse.destinationPlaceName = result.DestinationPlaceName;
                    userDataResponse.ServiceFee = result.ServiceFee;
                    var driverRatings = this.DbContextObj().TblTipsReviews
                .Where(r => r.DriverId == result!.DriverId)
                .GroupBy(g => g.DriverId, r => r.RatingByRider)
                .Select(g => new
                {
                    RatingByRider = g.Average(r => r)
                });
                    var avrgRating = (driverRatings!.ToList().Count == 0) ? Convert.ToDecimal("0.0") : Convert.ToDecimal(driverRatings!.ToList()[0].RatingByRider);

                    userDataResponse.riderDetail = new Posh_TRPT_Domain.PushNotification.RiderDetail
                    {
                        Id = result.RiderId,
                        DeviceId = await this.DbContextObj().Users.AsNoTracking().Where(s => s.Id == result.RiderId).Select(s => s.DeviceId).FirstOrDefaultAsync(),
                        RiderMobile = result.RiderMobile,
                        DriverMobile = result.DriverMobile,




                        source = new Posh_TRPT_Domain.BookingSystem.Source()
                        {
                            Latitude = (double)result.RiderLat!,
                            Longitude = (double)result.RiderLong!,
                            SourceName = result.RiderSourceName
                        },

                        destination = new Destination()
                        {
                            Latitude = result.RiderDestinationLat,
                            Longitude = result.RiderDestinationLong,
                            DestinationName = result.DestinationPlaceName,
                        }
                    };

                    userDataResponse.driverDetail = new Posh_TRPT_Domain.PushNotification.DriverDetail
                    {
                        Id = result.DriverId,
                        DeviceId = result.DriverDeviceId,
                        RiderMobile = result.RiderMobile,
                        DriverMobile = result.DriverMobile,
                        Rating = Math.Round(Convert.ToDecimal(avrgRating), 1),
                        source = new Posh_TRPT_Domain.BookingSystem.Source()
                        {
                            Latitude = (double)result.DriverLat!,
                            Longitude = (double)result.DriverLong!
                        },
                        destination = new Destination()
                        {
                            Latitude = (double)result.RiderLat,
                            Longitude = (double)result.RiderLong,
                            DestinationName = result.RiderSourceName,
                        }

                    };

                    userDataResponse.name = result.DriverName;
                    userDataResponse.DriverProfilePic = result.ProfilePhoto;
                    userDataResponse.distance = Convert.ToString(result.Distance);
                    userDataResponse.time = Convert.ToString(result.Time);
                    userDataResponse.id = Guid.Empty;
                    userDataResponse.RideCategoryName = result.RideCategoryName;
                    userDataResponse.VehiclePlate = result.VehiclePlate;
                    userDataResponse.VehicleModel = result.Model;
                    userDataResponse.VehicleColor = result.VehicleColor;
                    userDataResponse.Angle = result.Angle;
                    userDataResponse.MinimumDistance = result.MinimumDistance;
                    userData.type = result.StatusType;
                    userData.userData = userDataResponse;

                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();
                    _logger.LogInformation("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemRepository Method after Sp_GetDriverInfoAfterBookingStatusUpdate -- Type={1} DriverId = {2} , RiderName = {3}, RiderId = {4}, RiderDeviceId = {5}, DriverId ={6}, DriverDeviceId = {7} CancellationToken = {8}", DateTime.UtcNow, userData.type, userId, result.RiderName, result.RiderId, userDataResponse.riderDetail.DeviceId, result.DriverId, userDataResponse.driverDetail.DeviceId, cancellationToken.IsCancellationRequested);

                    return Task<UserData>.FromResult(userData).Result;
                }

                _logger.LogInformation("{0} InSide  GetDriverInfoAfterBookingStatusUpdate in BookingSystemRepository Method after Sp_GetDriverInfoAfterBookingStatusUpdate -- Type={1} DriverId = {2} , RiderName = {3}, RiderId = {4}, RiderDeviceId = {5}, DriverId ={6}, DriverDeviceId = {7}  CancellationToken = {8}", DateTime.UtcNow, "", userId, "", "", "", "", "", cancellationToken.IsCancellationRequested);
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();
                return null!;
            }
            catch (SqlException)
            {
                _logger.LogInformation("{0} InSide Catch to handle cancellation  GetDriverInfoAfterBookingStatusUpdate in BookingSystemRepository Method after Sp_GetDriverInfoAfterBookingStatusUpdate -- Type={1} DriverId = {2} , RiderName = {3}, RiderId = {4}, RiderDeviceId = {5}, DriverId ={6}, DriverDeviceId = {7}  CancellationToken = {8}", DateTime.UtcNow, "", userId, "", "", "", "", "", cancellationToken.IsCancellationRequested);

                var detailData = await this.DbContextObj().TblBookingDetail.Where(s => s.RiderId == userId && (s.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.NOTIFIED_DRIVER)
                || s.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.DECLINED)
                || s.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE))).OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                if (detailData != null)
                {
                    var service = new PaymentIntentService();
                    var data = await service.CancelAsync(detailData.PaymentIntentId, new PaymentIntentCancelOptions { CancellationReason = "requested_by_customer" }).ConfigureAwait(false);
                    _logger.LogInformation("{0}InSide Cancelled Request GetDriverInfoAfterBookingStatusUpdate in BookingSystemRepository Method -- PaymentIntentId : {1} CancellationReason:{2}", DateTime.UtcNow, detailData.PaymentIntentId, "requested_by_customer");
                    StripeCustomerIntentCustom res = JsonConvert.DeserializeObject<StripeCustomerIntentCustom>(data.ToJson())!;
                    detailData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED);
                    detailData.UpdatedDate = DateTime.UtcNow;
                    detailData.UpdatedBy = detailData.RiderId;
                    detailData.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                    detailData.PaymentStatus = res.Status;
                    _logger.LogInformation("{0}InSide Cancelled Request GetDriverInfoAfterBookingStatusUpdate in BookingSystemRepository Method -- BookingStatusId : {1} RiderId:{2} StatusType: {3}", DateTime.UtcNow, detailData.BookingStatusId, detailData.RiderId, detailData.StatusType);
                    var result = await this.DbContextObj().Users.Where(s => s.Id == detailData.DriverId).FirstOrDefaultAsync().ConfigureAwait(false);

                    if (result != null)
                    {
                        result.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                        result.UpdatedDate = DateTime.UtcNow;
                        result.UpdatedBy = detailData.RiderId;

                        this.DbContextObj().Users.Update(result);

                        //this.DbContextObj().Entry(result).State = EntityState.Modified;
                        await this.DbContextObj().SaveChangesAsync();
                    }
                    this.DbContextObj().Entry(detailData).State = EntityState.Modified;
                    await this.DbContextObj().SaveChangesAsync();
                    var wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(detailData.RiderId)).FirstOrDefaultAsync()!;
                    var bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.UserId == (detailData.RiderId!) && x.BookingId == detailData.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                    if (wallateRecord != null && wallateRecord.IsApplied)
                    {
                        wallateRecord!.Balance += bonusRecord!.CashBack;
                        wallateRecord.UpdatedDate = DateTime.UtcNow;
                        wallateRecord.UpdatedBy = detailData.RiderId;
                        this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                        await this.DbContextObj().SaveChangesAsync();
                    }


                    if (bonusRecord != null)
                    {
                        bonusRecord.IsApplied = false;
                        bonusRecord.CashBack = 0.0m;
                        this.DbContextObj().TblRideBonusHistory.Update(bonusRecord);
                        await this.DbContextObj().SaveChangesAsync();
                    }

                    var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED, IntentPaymentStatus = "cancelled" };
                    NotificationModel notificationModel = new NotificationModel
                    {
                        DeviceId = result!.DeviceId,
                        IsAndroidDevice = result.Platform!.Where(s => s.Equals("Android") || s.Equals("Mobile")) != null ? true : false,
                        Title = "Ride cancelled",
                        Body = "Booking Request cancelled by user"

                    };
                    SendNotification(notificationModel, dataUser);

                    return dataUser;
                }
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region BookingHistory
        /// <summary>
        /// API to get booking history
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResponse<List<BookingHistoryData>>> BookingHistory(PaginationFilter filter)

        {

            try
            {
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
                _logger.LogInformation("{0}InSide  BookingHistory in BookingSystemRepository Method --  userId:{1} role: {2}", DateTime.UtcNow, userId, role);
                List<BookingHistoryData> result = null!;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                _logger.LogInformation("{0}InSide  BookingHistory in BookingSystemRepository Method --  PageNumber:{1} PageSize: {2}", DateTime.UtcNow, filter.PageNumber, filter.PageSize);
                SqlParameter[] sqlParameter = new SqlParameter[]
                                                                {
                                                                                                                new SqlParameter { ParameterName ="@UserId", Value = userId}
                                                                };
                result = this.DbContextObj().GetListOfRecordExecuteProcedure<BookingHistoryData>("Sp_GetBookingHistory @UserId", sqlParameter);
                if (result.Count() > 0)
                {
                    switch (role)
                    {
                        case AuthorizationLevel.Roles.Customer:
                            {
                                result = (from data in result
                                          where data.BookingStatusName != "DECLINED"
                                          select data).ToList();
                                break;
                            }
                    }
                    int totalRecords = result.Count();
                    int totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);
                    //var filterData = result?.Skip((filter.PageNumber - 1) * (filter.PageSize)).TakeLast(filter.PageSize).Reverse().OrderByDescending(x => x.CreatedDateTime).ToList();
                    var filterData = result?.Skip((filter.PageNumber - 1) * (filter.PageSize)).Take(filter.PageSize).ToList();
                    var response = new PagedResponse<List<BookingHistoryData>>(filterData!, validFilter.PageNumber, validFilter.PageSize)
                    {
                        TotalPages = totalPages,
                        TotalRecords = totalRecords,
                        Message = totalRecords > 0 ? "Record Found" : "No record found",
                        Successed = true,
                        Errors = null!,
                        Data = filterData!
                    };
                    _logger.LogInformation("{0}InSide  BookingHistory in BookingSystemRepository Method --  TotalPages:{1} TotalRecords: {2} Message:{3}  Successed:{4} Errors:{5}", DateTime.UtcNow, response.TotalPages, response.TotalRecords, response.Message, response.Successed, response.Errors);
                    return response;
                }
                var responseRecord = new PagedResponse<List<BookingHistoryData>>(null!, validFilter.PageNumber, validFilter.PageSize)
                {
                    TotalPages = 0,
                    TotalRecords = 0,
                    Message = "No record found",
                    Successed = false,
                    Errors = null!,
                    Data = null!
                };
                _logger.LogInformation("{0}InSide  BookingHistory in BookingSystemRepository Method --  TotalPages:{1} TotalRecords: {2} Message:{3}  Successed:{4} Errors:{5}", DateTime.UtcNow, responseRecord.TotalPages, responseRecord.TotalRecords, responseRecord.Message, responseRecord.Successed, responseRecord.Errors);

                return responseRecord;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region CurrentBooking
        /// <summary>
        /// API to get current booking details
        /// </summary>
        /// <returns></returns>
        public async Task<UserData> CurrentBooking()
        {
            UserDataResponse userDataResponse = new UserDataResponse();
            UserData userData = new UserData();
            var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
            try
            {
                _logger.LogInformation("{0} InSide  CurrentBooking in BookingSystemRepository Method before Sp_GetDriverInfoAfterBookingStatusUpdate -- DriverId = {1} ", DateTime.UtcNow, userId);

                SqlParameter[] sqlParameter = new SqlParameter[]
                                                                {
                                                                                                                new SqlParameter { ParameterName ="@userId", Value = userId},
                                                                                                                                new SqlParameter { ParameterName = "@Value", Value = "CurrentDriver"}
                                                };
                var result = this.DbContextObj().GetRecordExecuteProcedure<DriverBookingData>("Sp_GetDriverInfoAfterBookingStatusUpdate @userId,@Value", sqlParameter);
                if (result != null && result.Id != null)
                {

                    var driverRatings = this.DbContextObj().TblTipsReviews
                            .Where(r => r.DriverId == result!.DriverId)
                            .GroupBy(g => g.DriverId, r => r.RatingByRider)
                            .Select(g => new
                            {
                                RatingByRider = g.Average(r => r)
                            });
                    var avrgRating = (driverRatings!.ToList().Count == 0) ? Convert.ToDecimal("0.0") : Convert.ToDecimal(driverRatings!.ToList()[0].RatingByRider);


                    if (result.RiderId == null)
                    {
                        return userData;
                    }
                    var DriverPhoto = string.IsNullOrEmpty(result.ProfilePhoto) ? _config["Request:Url"] + "Images" + "/person.png" : _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + result.ProfilePhoto;
                    result.ProfilePhoto = DriverPhoto;
                    userDataResponse.id = (Guid)result.Id!;

                    switch (role)
                    {
                        case AuthorizationLevel.Roles.Customer:
                            {
                                var isAppliedPromo = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.UserId!.Equals(result!.RiderId) && x.IsApplied == true && x.BookingId == result.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                                if (isAppliedPromo != null && isAppliedPromo!.IsApplied)
                                {
                                    result!.Price = Convert.ToString(Convert.ToDecimal(result.Price));

                                }
                                break;
                            }

                    }

                    userDataResponse.price = Convert.ToString(result.Price);
                    userDataResponse.sourceLat = result.RiderLat;
                    userDataResponse.sourceLong = result.RiderLong;
                    userDataResponse.sourcePlaceName = result.RiderSourceName;
                    userDataResponse.destinationLat = result.RiderDestinationLat;
                    userDataResponse.destinationLong = result.RiderDestinationLong;
                    userDataResponse.destinationPlaceName = result.DestinationPlaceName;
                    userDataResponse.Angle = result.Angle;
                    userDataResponse.MinimumDistance = result.MinimumDistance;
                    userDataResponse.RiderName = result.RiderName;
                    userDataResponse.ServiceFee = result.ServiceFee;
                    var driver = await _userManager.FindByIdAsync(result!.RiderId);
                    userDataResponse.riderDetail = new Posh_TRPT_Domain.PushNotification.RiderDetail
                    {
                        Id = result.RiderId,
                        DeviceId = driver.DeviceId,
                        RiderMobile = result.RiderMobile,
                        DriverMobile = result.DriverMobile,
                        source = new Posh_TRPT_Domain.BookingSystem.Source()
                        {
                            Latitude = (double)result.RiderLat!,
                            Longitude = (double)result.RiderLong!,
                            SourceName = result.RiderSourceName
                        },

                        destination = new Destination()
                        {
                            Latitude = result.RiderDestinationLat,
                            Longitude = result.RiderDestinationLong,
                            DestinationName = result.DestinationPlaceName,
                        }
                    };
                    userDataResponse.driverDetail = new Posh_TRPT_Domain.PushNotification.DriverDetail
                    {
                        Id = result.DriverId,
                        DeviceId = result.DriverDeviceId,
                        DriverMobile = result.DriverMobile,
                        RiderMobile = result.RiderMobile,
                        Rating = Math.Round(Convert.ToDecimal(avrgRating), 1),
                        source = new Posh_TRPT_Domain.BookingSystem.Source()
                        {
                            Latitude = (double)result.DriverLat!,
                            Longitude = (double)result.DriverLong!
                        },
                        destination = new Destination()
                        {
                            Latitude = (double)result.RiderLat,
                            Longitude = (double)result.RiderLong,
                            DestinationName = result.RiderSourceName,
                        }

                    };
                    userDataResponse.name = result.DriverName;
                    userDataResponse.DriverProfilePic = result.ProfilePhoto;
                    userDataResponse.distance = Convert.ToString(result.Distance);
                    userDataResponse.time = Convert.ToString(result.Time);
                    userDataResponse.RideCategoryName = result.RideCategoryName;
                    userDataResponse.VehiclePlate = result.VehiclePlate;
                    userDataResponse.VehicleModel = result.Model;
                    userDataResponse.VehicleColor = result.VehicleColor;
                    userData.type = result.StatusType;
                    userData.userData = userDataResponse;
                    _logger.LogInformation("{0} InSide  CurrentBooking in BookingSystemRepository Method after Sp_GetDriverInfoAfterBookingStatusUpdate -- Type={1} UserId = {2} , RiderName = {3}, RiderId = {4}, RiderDeviceId = {5}, DriverId ={6}, DriverDeviceId = {7}", DateTime.UtcNow, userData.type, userId, result.RiderName, result.RiderId, userDataResponse.riderDetail.DeviceId, result.DriverId, userDataResponse.driverDetail.DeviceId);

                    _logger.LogInformation("{0} InSide  CurrentBooking in BookingSystemRepository Method after Sp_GetDriverInfoAfterBookingStatusUpdate -- DriverId = {1} , RiderName = {2}, RiderId = {3}, RiderDeviceId = {4}, DriverId ={5}, DriverDeviceId = {6}", DateTime.UtcNow, userId, result.RiderName, result.RiderId, result.RiderDeviceId, result.DriverId, result.DriverDeviceId);
                    return userData;
                }
                userData.type = "0";
                return userData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetRiderSourceLocation
        /// <summary>
        /// API to get rider source location
        /// </summary>
        /// <returns></returns>
        public async Task<RiderSourceLocation> GetRiderSourceLocation()
        {
            try
            {
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("{0} InSide  GetRiderSourceLocation in BookingSystemRepository -- UserId = {1} ", DateTime.UtcNow, userId);
                var userData = await _userManager.FindByIdAsync(userId);
                if (userData is not null)
                {
                    _logger.LogInformation("{0} InSide  GetRiderSourceLocation in UserData in BookingSystemRepository -- UserId = {1} RiderLatitude:{2} RiderLongitude:{3} ", DateTime.UtcNow, userId, userData?.Latitude, userData?.Longitude);
                    return new RiderSourceLocation { RiderLatitude = userData?.Latitude, RiderLongitude = userData?.Longitude };
                }
                return Task.FromResult<RiderSourceLocation>(null!).Result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region BookingDetails
        /// <summary>
        /// API to add booking details
        /// </summary>
        /// <param name="result"></param>
        /// <param name="model"></param>
        /// <param name="userDataResponse"></param>
        /// <returns></returns>
        public async Task<BookingDetail> BookingDetails(BookingNotificationRequesr result, DistanceCalculate model, UserDataResponse userDataResponse, string paymentIntentId, string paymetMethodId, string status)
        {
            try
            {
                BookingDetail bookingDetail = new BookingDetail();
                var booking = this.DbContextObj().TblBookingDetail.AsNoTracking().
                                Where(
                                y => y.DriverId != result.UserId
                                && y.RiderId == model.riderDetail!.Id
                                && (y.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.DECLINED) || y.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.STARTED) || y.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.COMPLETED) || y.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.ACCEPT))).FirstOrDefault();
                var userData = await _userManager.FindByIdAsync(result.UserId);
                _logger.LogInformation("{0}InSide  BookingDetails in BookingSystemRepository Method Details Started: --DriverId:{1} DeviceId : {2}", DateTime.UtcNow, userData.Id, userData.DeviceId);
                if (userData.BookingStatusId.Equals(Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED)) && booking is not null)
                {

                    booking!.StatusType = model.StatusType;
                    booking.DriverId = result.UserId;
                    booking.RiderId = model.riderDetail!.Id;
                    bookingDetail.PaymentIntentId = paymentIntentId;
                    bookingDetail.DefaultPaymentMethodId = paymetMethodId;
                    bookingDetail.PaymentStatus = status;
                    booking.CategoryPriceId = result.Id;
                    booking.RiderSourceName = model.Source!.SourceName;
                    booking.RiderLat = model.Source.Latitude;
                    booking.RiderLong = model.Source.Longitude;
                    booking.VehicleId = this.DbContextObj().TblVehicleDetails.AsNoTracking().Where(s => s.Vehicle_Identification_Number == result.VehicleIdentificationNumber).Select(s => s.Id).First();
                    booking.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.NOTIFIED_DRIVER);
                    booking.Price = Convert.ToString(result.Price);
                    booking.Distance = userDataResponse.distance;
                    booking.Time = userDataResponse.time;
                    booking.CreatedDate = booking.CreatedDate;
                    booking.UpdatedDate = DateTime.UtcNow;
                    booking.ServiceFee = result.ServiceFee;// result.ServiceFee;
                    booking.Angle = userDataResponse.Angle;
                    booking.DestinationPlaceName = model.Destination!.DestinationName;
                    booking.RiderDestinationLat = model.Destination.Latitude;
                    booking.RiderDestinationLong = model.Destination.Longitude;
                    booking.DriverLat = userDataResponse.driverDetail!.source!.Latitude;
                    booking.DriverLong = userDataResponse.driverDetail!.source!.Longitude;
                    booking.MinimumDistance = model.MinimumDistance;
                    booking.CityId = model.StateId;
                    booking.CategoryId = model.CategoryId;
                    booking.StripeFees = Math.Round((0.30m + (result.Price * 0.029m)), 2);
                    booking.RiderDeviceId = model.riderDetail.DeviceId;
                    booking.LocalUpdatedDateTime = Convert.ToDateTime(model.LocalTime);
                    booking.TollFees = userDataResponse.TollFees;

                    _logger.LogInformation("{0}InSide  BookingDetails before updating BookingSystemRepository Method Details Started: --CategoryId:{1} CityId : {2} MinimumDistance:{3} DriverLong:{4} DriverLat:{5} RiderDestinationLong:{6} RiderDestinationLat:{7} DestinationPlaceName:{8} Angle:{9} ServiceFee:{10} UpdatedDate:{11} CreatedDate:{12} Time:{13} Distance:{14} Price:{15} BookingStatusId:{16} VehicleId:{17} RiderLong:{18} RiderLat:{19} RiderSourceName:{21} CategoryPriceId:{22} PaymentStatus:{23} DefaultPaymentMethodId:{24} PaymentIntentId:{25} RiderId:{26} DriverId:{27} StatusType{28}", DateTime.UtcNow, booking.CategoryId, booking.CityId, booking.MinimumDistance, booking.DriverLong, booking.DriverLat, booking.RiderDestinationLong, booking.RiderDestinationLat, booking.DestinationPlaceName, booking.Angle, booking.ServiceFee, booking.UpdatedDate, booking.CreatedDate, booking.Time, booking.Distance, booking.Price, booking.BookingStatusId, booking.VehicleId, booking.RiderLong, booking.RiderLat, booking.RiderSourceName, booking.CategoryPriceId, bookingDetail.PaymentStatus, bookingDetail.DefaultPaymentMethodId, bookingDetail.PaymentIntentId, booking.RiderId, booking.DriverId, booking!.StatusType);
                    this.DbContextObj().TblBookingDetail.Update(booking);
                    _logger.LogInformation("{0}InSide  BookingDetails after updating BookingSystemRepository Method Details Started: --CategoryId:{1} CityId : {2} MinimumDistance:{3} DriverLong:{4} DriverLat:{5} RiderDestinationLong:{6} RiderDestinationLat:{7} DestinationPlaceName:{8} Angle:{9} ServiceFee:{10} UpdatedDate:{11} CreatedDate:{12} Time:{13} Distance:{14} Price:{15} BookingStatusId:{16} VehicleId:{17} RiderLong:{18} RiderLat:{19} RiderSourceName:{21} CategoryPriceId:{22} PaymentStatus:{23} DefaultPaymentMethodId:{24} PaymentIntentId:{25} RiderId:{26} DriverId:{27} StatusType{28}", DateTime.UtcNow, booking.CategoryId, booking.CityId, booking.MinimumDistance, booking.DriverLong, booking.DriverLat, booking.RiderDestinationLong, booking.RiderDestinationLat, booking.DestinationPlaceName, booking.Angle, booking.ServiceFee, booking.UpdatedDate, booking.CreatedDate, booking.Time, booking.Distance, booking.Price, booking.BookingStatusId, booking.VehicleId, booking.RiderLong, booking.RiderLat, booking.RiderSourceName, booking.CategoryPriceId, bookingDetail.PaymentStatus, bookingDetail.DefaultPaymentMethodId, bookingDetail.PaymentIntentId, booking.RiderId, booking.DriverId, booking!.StatusType);
                    await this.DbContextObj().SaveChangesAsync();
                    return booking;
                }
                else
                {
                    bookingDetail.PaymentIntentId = paymentIntentId;
                    bookingDetail.DefaultPaymentMethodId = paymetMethodId;
                    bookingDetail.PaymentStatus = status;
                    bookingDetail.StatusType = model.StatusType;
                    bookingDetail.ServiceFee = result.ServiceFee;//result.ServiceFee;
                    bookingDetail.DriverId = result.UserId;
                    bookingDetail.RiderId = model.riderDetail!.Id;
                    bookingDetail.RiderDeviceId = model.riderDetail.DeviceId;
                    bookingDetail.CategoryPriceId = result.Id;
                    bookingDetail.RiderSourceName = model.Source!.SourceName;
                    bookingDetail.RiderLat = model.Source.Latitude;
                    bookingDetail.RiderLong = model.Source.Longitude;
                    bookingDetail.CreatedDate = DateTime.UtcNow;
                    bookingDetail.VehicleId = this.DbContextObj().TblVehicleDetails.AsNoTracking().Where(s => s.Vehicle_Identification_Number == result.VehicleIdentificationNumber).Select(s => s.Id).First();
                    bookingDetail.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.NOTIFIED_DRIVER);
                    bookingDetail.Price = Convert.ToString(result.Price);
                    bookingDetail.Distance = userDataResponse.distance;
                    bookingDetail.Time = userDataResponse.time;
                    bookingDetail.Angle = userDataResponse.Angle;
                    bookingDetail.UpdatedDate = DateTime.UtcNow;
                    bookingDetail.DestinationPlaceName = model.Destination!.DestinationName;
                    bookingDetail.RiderDestinationLat = model.Destination.Latitude;
                    bookingDetail.RiderDestinationLong = model.Destination.Longitude;
                    bookingDetail.DriverLat = userDataResponse.driverDetail!.source!.Latitude;
                    bookingDetail.DriverLong = userDataResponse.driverDetail!.source!.Longitude;
                    bookingDetail.MinimumDistance = model.MinimumDistance;
                    bookingDetail.CityId = model.StateId;
                    bookingDetail.CategoryId = model.CategoryId;
                    bookingDetail.StripeFees = Math.Round((0.30m + (result.Price * 0.029m)), 2);
                    bookingDetail.LocalCreatedDateTime = Convert.ToDateTime(model.LocalTime);
                    bookingDetail.LocalUpdatedDateTime = Convert.ToDateTime(model.LocalTime);
                    bookingDetail.TollFees = userDataResponse.TollFees;
                    _logger.LogInformation("{0}InSide  BookingDetails before adding BookingSystemRepository Method Details Started: --CategoryId:{1} CityId : {2} MinimumDistance:{3} DriverLong:{4} DriverLat:{5} RiderDestinationLong:{6} RiderDestinationLat:{7} DestinationPlaceName:{8} Angle:{9} ServiceFee:{10} UpdatedDate:{11} CreatedDate:{12} Time:{13} Distance:{14} Price:{15} BookingStatusId:{16} VehicleId:{17} RiderLong:{18} RiderLat:{19} RiderSourceName:{21} CategoryPriceId:{22} PaymentStatus:{23} DefaultPaymentMethodId:{24} PaymentIntentId:{25} RiderId:{26} DriverId:{27} StatusType{28}", DateTime.UtcNow, bookingDetail.CategoryId, bookingDetail.CityId, bookingDetail.MinimumDistance, bookingDetail.DriverLong, bookingDetail.DriverLat, bookingDetail.RiderDestinationLong, bookingDetail.RiderDestinationLat, bookingDetail.DestinationPlaceName, bookingDetail.Angle, bookingDetail.ServiceFee, bookingDetail.UpdatedDate, bookingDetail.CreatedDate, bookingDetail.Time, bookingDetail.Distance, bookingDetail.Price, bookingDetail.BookingStatusId, bookingDetail.VehicleId, bookingDetail.RiderLong, bookingDetail.RiderLat, bookingDetail.RiderSourceName, bookingDetail.CategoryPriceId, bookingDetail.PaymentStatus, bookingDetail.DefaultPaymentMethodId, bookingDetail.PaymentIntentId, bookingDetail.RiderId, bookingDetail.DriverId, bookingDetail!.StatusType);
                    await this.DbContextObj().TblBookingDetail.AddAsync(bookingDetail);
                    _logger.LogInformation("{0}InSide  BookingDetails after adding BookingSystemRepository Method Details Started: --CategoryId:{1} CityId : {2} MinimumDistance:{3} DriverLong:{4} DriverLat:{5} RiderDestinationLong:{6} RiderDestinationLat:{7} DestinationPlaceName:{8} Angle:{9} ServiceFee:{10} UpdatedDate:{11} CreatedDate:{12} Time:{13} Distance:{14} Price:{15} BookingStatusId:{16} VehicleId:{17} RiderLong:{18} RiderLat:{19} RiderSourceName:{21} CategoryPriceId:{22} PaymentStatus:{23} DefaultPaymentMethodId:{24} PaymentIntentId:{25} RiderId:{26} DriverId:{27} StatusType{28}", DateTime.UtcNow, bookingDetail.CategoryId, bookingDetail.CityId, bookingDetail.MinimumDistance, bookingDetail.DriverLong, bookingDetail.DriverLat, bookingDetail.RiderDestinationLong, bookingDetail.RiderDestinationLat, bookingDetail.DestinationPlaceName, bookingDetail.Angle, bookingDetail.ServiceFee, bookingDetail.UpdatedDate, bookingDetail.CreatedDate, bookingDetail.Time, bookingDetail.Distance, bookingDetail.Price, bookingDetail.BookingStatusId, bookingDetail.VehicleId, bookingDetail.RiderLong, bookingDetail.RiderLat, bookingDetail.RiderSourceName, bookingDetail.CategoryPriceId, bookingDetail.PaymentStatus, bookingDetail.DefaultPaymentMethodId, bookingDetail.PaymentIntentId, bookingDetail.RiderId, bookingDetail.DriverId, bookingDetail!.StatusType);
                    await this.DbContextObj().SaveChangesAsync();
                    return bookingDetail;
                }
                //   return true;


            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SendNotification
        /// <summary>
        /// API to send push notification
        /// </summary>
        /// <param name="notificationModel"></param>
        /// <param name="userData"></param>
        public async void SendNotification(NotificationModel notificationModel, UserData userData)
        {
            try
            {
                _logger.LogInformation("{0} InSide  SendNotification in BookingSystemRepository Method :  DriverDeviceId = {1} ", DateTime.UtcNow, notificationModel?.DeviceId);
                string fileName = Path.Combine(_webHostEnvironment.WebRootPath, "helpful-cosine-410722-firebase-adminsdk-yftwt-fbcdb2d817.json"); //Download from Firebase Console ServiceAccount
                string scopes = _config["GoogleNotification:Scopes"]!;
                var bearertoken = ""; // Bearer Token in this variable
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    bearertoken = GoogleCredential
                      .FromStream(stream) // Loads key file
                      .CreateScoped(scopes) // Gathers scopes requested
                      .UnderlyingCredential // Gets the credentials
                      .GetAccessTokenForRequestAsync().Result; // Gets the Access Token
                }
                ///--------Calling FCM-----------------------------

                var clientHandler = new HttpClientHandler();
                var client = new HttpClient(clientHandler);

                client.BaseAddress = new Uri(_config["GoogleNotification:BaseAddress"]!); // FCM HttpV1 API

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Configuration.Content));

                //client.DefaultRequestHeaders.Accept.Add("Authorization", "Bearer " + bearertoken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtAuthenticationDefaults.BearerPrefix, bearertoken); // Authorization Token in this variable

                //---------------Assigning Of data To Model --------------

                Root rootObj = new Root();
                rootObj.Message = new Posh_TRPT_Domain.PushNotification.Message();
                rootObj.Message.Token = notificationModel!.DeviceId; //FCM Token id
                rootObj.Message.Data = new Posh_TRPT_Domain.PushNotification.Data();
                rootObj.Message.Data.Title = notificationModel.Title;
                rootObj.Message.Data.Body = notificationModel.Body;
                //rootObj.Message.Data.UserData = userData;
                rootObj.Message.Notification = new Notification();
                rootObj.Message.Notification.Title = notificationModel.Title;
                rootObj.Message.Notification.Body = notificationModel.Body;

                //-------------Convert Model To JSON ----------------------

                var jsonObj = System.Text.Json.JsonSerializer.Serialize<Object>(rootObj);

                //------------------------Calling Of FCM Notify API-------------------

                var data = new StringContent(jsonObj, Encoding.UTF8, Configuration.Content);
                data.Headers.ContentType = new MediaTypeHeaderValue(Configuration.Content);

                var response = client.PostAsync(_config["GoogleNotification:BaseAddress"]!, data).Result; // Calling The FCM httpv1 API

                //---------- Deserialize Json Response from API ----------------------------------

                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                var responseObj = System.Text.Json.JsonSerializer.Serialize<object>(jsonResponse);
                _logger.LogInformation("{0} Inside After send notification getting response : {1}", DateTime.UtcNow, jsonResponse);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("{0} InSide  SendNotification in BookingSystemRepository Method -- Success : ==> {1}", DateTime.UtcNow, response.IsSuccessStatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide  SendNotification in BookingSystemRepository Method -- Notification_Error : {1}", DateTime.UtcNow, ex.Message);
            }
}
        #endregion

        #region DistanceTimeBetweenSourceDestination
        /// <summary>
        /// Method to calculate Distance and Time between Source and Destination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Tuple<int, int, decimal>> DistanceTimeBetweenSourceDestination(DistanceCalculate model)
        {
            try
            {


                Posh_TRPT_Domain.TollApiRequestBody.TollApiRequestBody root = new();
                root.origin = new Posh_TRPT_Domain.TollApiRequestBody.Origin()
                {
                    location = new Posh_TRPT_Domain.TollApiRequestBody.Location()
                    {
                        latLng = new Posh_TRPT_Domain.TollApiRequestBody.LatLng
                        {
                            latitude = model.Source!.Latitude,



                            longitude = model.Source!.Longitude
                        }
                    }
                };


                root.destination = new Posh_TRPT_Domain.TollApiRequestBody.Destination()
                {
                    location = new Posh_TRPT_Domain.TollApiRequestBody.Location()
                    {
                        latLng = new Posh_TRPT_Domain.TollApiRequestBody.LatLng
                        {
                            latitude = model.Destination!.Latitude,



                            longitude = model.Destination!.Longitude
                        }
                    }
                };

                root.travelMode = "DRIVE";
                root.extraComputations = new List<string>()
                {
                   "TOLLS"
                };
                using (var httpClient = new System.Net.Http.HttpClient())
                {

                    _logger.LogInformation("{0} InSide before result  DistanceTimeBetweenSourceDestination in BookingSystemRepository Method -- source  Latitude : {1}  source Longitude: {2} SourceName:{3} destination Latitude:{4} destination Longitude:{5} DestinationName:{6} StateId:{7} StatusType:{8} MinimumDistance:{9} CategoryId:{10} RiderId:{11} RiderDeviceId:{12} DriverId:{13} DriverDeviceId:{14}", DateTime.UtcNow, model.Source?.Latitude, model.Source?.Longitude, model.Source?.SourceName, model.Destination?.Latitude, model.Destination?.Longitude, model.Destination?.DestinationName, model.StateId, model.StatusType, model.MinimumDistance, model.CategoryId, model.riderDetail?.Id, model.riderDetail?.DeviceId, model.driverDetail?.Id, model.driverDetail?.DeviceId);
                    var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(root);
                    httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.duration,routes.distanceMeters,routes.travelAdvisory.tollInfo,routes.legs.travelAdvisory.tollInfo");
                    httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", _config["DistanceMatrixAPI:KEY"]);

                    // Create HttpContent for the request body
                    var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                    // Send POST request
                    var response = await httpClient.PostAsync(_config["DistanceMatrixAPI:URL2"], content);
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Posh_TRPT_Domain.TollInformation.TollAPIResponseBody>(apiResponse);
                        string durationString = result!.routes[0].duration;
                        int time = 0;
                        if (durationString.EndsWith("s"))
                        {
                            durationString = durationString.Substring(0, durationString.Length - 1);
                            time = int.TryParse(durationString, out int x) ? x : 0;
                        }
                        if (result.routes[0].travelAdvisory.tollInfo != null)
                        {
                            return (result.routes[0].travelAdvisory.tollInfo.estimatedPrice != null) ? new Tuple<int, int, decimal>((result.routes[0].distanceMeters), time, Convert.ToDecimal((result.routes[0].travelAdvisory.tollInfo.estimatedPrice[0].units)))
                      : new Tuple<int, int, decimal>(result.routes[0].distanceMeters, time, 0.0m);
                        }
                        // (result.routes[0].travelAdvisory.tollInfo.estimatedPrice != null) ? new Tuple<int, int, decimal>((result.routes[0].distanceMeters), time, Convert.ToDecimal((result.routes[0].travelAdvisory.tollInfo.estimatedPrice[0].units)))
                        return new Tuple<int, int, decimal>(result.routes[0].distanceMeters, time, 0.0m);
                    }
                    return null!;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetBookingHistoryUserDetails
        /// <summary>
        /// Method to get booking history details on history id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BookingHistoryUserData> GetBookingHistoryUserDetails(Guid Id, string emailIdRideCompletion, bool rideCompletionMailSend = false)

        {
            BookingHistoryUserData bookingHistoryUserData = new BookingHistoryUserData();
            string userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
            try
            {
                _logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails in BookingSystemRepository Method  -- UserId = {1} BookingId: {2}", DateTime.UtcNow, userId, Id);

                SqlParameter[] sqlParameters = new SqlParameter[]
                                                                {
                                                                        new SqlParameter { ParameterName ="@userId", Value = userId},
                                                                        new SqlParameter { ParameterName ="@Id", Value = Id},
                                                                };
                var result = this.DbContextObj().GetRecordExecuteProcedure<BookingHistoryUserData>("Sp_GetBookingHistoryUserData @userId, @Id", sqlParameters);
                _logger.LogInformation("{0} InSide  GetBookingHistoryUserDetails", result.BookingStatusName);
                string RiderProfilePic = string.IsNullOrEmpty(result.RiderProfilePic!) ? _config["Request:Url"] + "Images" + "/person.png" : _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + result.RiderProfilePic!;
                string DriverProfilePic = string.IsNullOrEmpty(result.DriverProfilePic!) ? _config["Request:Url"] + "Images" + "/person.png" : _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + result.DriverProfilePic!;
                result.RiderProfilePic = RiderProfilePic;
                result.DriverProfilePic = DriverProfilePic;
                if (result != null && (result.BookingStatusName == "COMPLETED" || result.BookingStatusName == "CANCELLED") || result!.BookingStatusName == "NotifyPaymentSuccess")
                {
                    var bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.BookingId == result.BookingId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                    _logger.LogInformation("InSide GetBookingHistoryUserDetails GeneratePdf method called in BookingSystemRepository");
                    decimal cashBackValue = bonusRecord != null ? (decimal)bonusRecord!.CashBack! : 0.0m;
                    result.Email = emailIdRideCompletion;
                    string getPdf = await GeneratePdf(result, rideCompletionMailSend, cashBackValue).ConfigureAwait(false);
                    result.URL = getPdf;
                }
                switch (role)
                {
                    case AuthorizationLevel.Roles.Customer:
                        {
                            var isAppliedPromo = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.UserId!.Equals(result!.RiderId) && x.IsApplied == true && x.BookingId == result.BookingId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;
                            if (isAppliedPromo != null && isAppliedPromo!.IsApplied)
                            {
                                result!.Price = Convert.ToString(Convert.ToDecimal(result.Price) - isAppliedPromo!.CashBack);
                                result!.Promotion = isAppliedPromo!.CashBack;
                            }
                            break;
                        }
                    case AuthorizationLevel.Roles.Driver:
                        {
                            result!.Price = Convert.ToString(Convert.ToDecimal(result.Price) - Convert.ToDecimal(result!.ServiceFee) - result!.StripeFee);
                            break;
                        }
                }
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_config["DistanceMatrixAPI:URL"] + String.Concat(result?.DestinationLat, ",", result?.DestinationLong, "&origins=", result?.SourceLat, ",", result?.SourceLong, "&units=imperial&key=", _config["DistanceMatrixAPI:KEY"])))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var d = JsonConvert.DeserializeObject<RouteDistanceResponseAPI>(apiResponse);
                        decimal distance = d!.rows![0].elements![0]!.distance!.value;
                        result!.SourceToDestinationDistance = Convert.ToString(distance * 0.00062137m);
                        int time = d.rows[0].elements![0]!.duration!.value;
                        result.SourceToDestinationTime = Convert.ToString(time / 60);
                    }
                }
                _logger.LogInformation("{0} InSide after calling SP Sp_GetBookingHistoryUserData  GetBookingHistoryUserDetails in BookingSystemRepository Method  -- DriverLong:={1}, DriverLat:={2}, DriverDeviceId:={3},DriverId:={4}, RiderDeviceId:={5}, RiderId:={6}, DriverName:={7}, RiderName:={8}, MinimumDistance:={9} Angle:={10} VehicleModel:={11}, VehicleColor:={12}, VehiclePlate:={13}, DestinationName:={14}, DestinationLong:={15}, DestinationLat:={16}, SourceName:={17}, SourceLong:={18}, SourceLat:={19}, Time:={20}, Distance:={21}, DriverProfilePic:={22}, RiderProfilePic:={23}, Name:={24}, Price:={25},UserId:={26}, BookingId:={27}, Email:={28}, CategoryName:={29}, CreatedDateTime:={30}, BookingStatusName:={31}, Type:={32}", DateTime.UtcNow, result.DriverLong, result.DriverLat, result.DriverDeviceId, result.DriverId, result.RiderDeviceId, result.RiderId, result.DriverName, result.RiderName, result.MinimumDistance, result.Angle, result.VehicleModel, result.VehicleColor, result.VehiclePlate, result.DestinationName, result.DestinationLong, result.DestinationLat, result.SourceName, result.SourceLong, result.SourceLat, result.Time, result.Distance, result.DriverProfilePic, result.RiderProfilePic, result.Name, result.Price, result.UserId, result.BookingId, result.Email, result.CategoryName, result.CreatedDateTime, result.BookingStatusName, result.Type);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GenerateReceiptPdf
        public async Task<string> GeneratePdf(BookingHistoryUserData bookingData, bool rideCompletionMailSend = false, decimal cashback = 0.0m)
        {
            try
            {
                _logger.LogInformation("InSide GeneratePdf in BookingSystemRepository");

                string baseUrl = _config["Request:Url"];
                string webRootPath = _webHostEnvironment.WebRootPath;
                string htmlContent = string.Empty;

                string logoPath = $"{baseUrl}{GlobalConstants.GlobalValues.poshlogopath}";
                string coinPath = $"{baseUrl}{GlobalConstants.GlobalValues.coinlogopath}";

                var formattedDateTime = DateTime.UtcNow.ToString("M/dd/yyyy");
                if (bookingData!.Type! == "4")
                {
                    string receiptPath = System.IO.Path.Combine(webRootPath, "Receipts", "receipt.html");
                    htmlContent = System.IO.File.ReadAllText(receiptPath);
                    decimal totalprice = (decimal)(Convert.ToDecimal(bookingData.Price) - cashback)!;
                    double distance = Convert.ToDouble(bookingData.Distance!);
                    double totaldistance = Math.Round(distance, 2);
                    htmlContent = htmlContent.Replace("{BookingId}", bookingData.BookingId.ToString());
                    htmlContent = htmlContent.Replace("{RiderName}", bookingData.RiderName ?? "");
                    htmlContent = htmlContent.Replace("{Total}", totalprice.ToString());
                    htmlContent = htmlContent.Replace("{category}", bookingData.CategoryName ?? "");
                    htmlContent = htmlContent.Replace("{totaldistance}", totaldistance.ToString());
                    htmlContent = htmlContent.Replace("{CurrentTime}", formattedDateTime);
                    htmlContent = htmlContent.Replace("{source}", bookingData.Time ?? "");
                    htmlContent = htmlContent.Replace("{SourceName}", bookingData.SourceName ?? "");
                    htmlContent = htmlContent.Replace("{DestinationName}", bookingData.DestinationName ?? "");
                    htmlContent = htmlContent.Replace("{DriverName}", bookingData.DriverName ?? "");
                    htmlContent = htmlContent.Replace("{subtotal}", bookingData.Price ?? "");
                    htmlContent = htmlContent.Replace("{Promotion}", cashback.ToString() ?? "");
                    htmlContent = htmlContent.Replace("{VehiclePlate}", bookingData.VehiclePlate ?? "");
                    htmlContent = htmlContent.Replace("{paymentmode}", "Card");
                    htmlContent = htmlContent.Replace("{paymenttime}", bookingData.CreatedDateTime ?? "");
                    htmlContent = htmlContent.Replace("{logoPath}", logoPath);
                    htmlContent = htmlContent.Replace("{coinPath}", coinPath);
                    htmlContent = htmlContent.Replace("{min}", Math.Round(Convert.ToDouble(bookingData.Time), 2).ToString() ?? "");
                    //htmlContent = htmlContent.Replace("{sourceToDestinationTime}", bookingData.SourceToDestinationTime ?? "");
                    //htmlContent = htmlContent.Replace("{sourceToDestinationDistance}", bookingData.SourceToDestinationDistance ?? "");

                }
                else if (bookingData!.Type! == "6")
                {
                    string receiptPath = System.IO.Path.Combine(webRootPath, "Receipts", "cancelreceipt.html");
                    htmlContent = System.IO.File.ReadAllText(receiptPath);
                    decimal totalprice = (decimal)(Convert.ToDecimal(bookingData.CancellationCharge))!;
                    double distance = Convert.ToDouble(bookingData.Distance!);
                    double totaldistance = Math.Round(distance, 2);



                    htmlContent = htmlContent.Replace("{BookingId}", bookingData.BookingId.ToString());
                    htmlContent = htmlContent.Replace("{RiderName}", bookingData.RiderName ?? "");
                    htmlContent = htmlContent.Replace("{Total}", totalprice.ToString());
                    htmlContent = htmlContent.Replace("{category}", bookingData.CategoryName ?? "");
                    htmlContent = htmlContent.Replace("{totaldistance}", totaldistance.ToString());
                    htmlContent = htmlContent.Replace("{CurrentTime}", formattedDateTime);
                    htmlContent = htmlContent.Replace("{source}", bookingData.Time ?? "");
                    htmlContent = htmlContent.Replace("{SourceName}", bookingData.SourceName ?? "");
                    htmlContent = htmlContent.Replace("{DestinationName}", bookingData.DestinationName ?? "");
                    htmlContent = htmlContent.Replace("{DriverName}", bookingData.DriverName ?? "");
                    htmlContent = htmlContent.Replace("{subtotal}", "0.0");
                    htmlContent = htmlContent.Replace("{Promotion}", "0.0");
                    htmlContent = htmlContent.Replace("{VehiclePlate}", bookingData.VehiclePlate ?? "");
                    htmlContent = htmlContent.Replace("{paymentmode}", "Card");
                    htmlContent = htmlContent.Replace("{paymenttime}", bookingData.CreatedDateTime ?? "");
                    htmlContent = htmlContent.Replace("{logoPath}", logoPath);
                    htmlContent = htmlContent.Replace("{coinPath}", coinPath);
                    htmlContent = htmlContent.Replace("{min}", Math.Round(Convert.ToDouble(bookingData.Time), 2).ToString() ?? "");
                    htmlContent = htmlContent.Replace("{cancellationfee}", bookingData.CancellationCharge.ToString() ?? "");

                }
                string mainFolderPath = System.IO.Path.Combine(webRootPath, "Receipt");
                string riderFolderName = Convert.ToString(bookingData.BookingId) ?? "";
                string riderFolderPath = System.IO.Path.Combine(mainFolderPath, riderFolderName);
                //if (System.IO.Directory.Exists(riderFolderPath))
                //{
                //    foreach (var item in System.IO.Directory.GetFiles(riderFolderPath))
                //    {
                //        System.IO.File.Delete(item);
                //    }
                //    System.IO.Directory.Delete(riderFolderPath);
                //    Directory.CreateDirectory(riderFolderPath);
                //}
                //else
                {
                    Directory.CreateDirectory(riderFolderPath);
                }
                string pdfFileName = $"Receipt_{bookingData.BookingId}_{DateTime.UtcNow:ddMMMyyyy_HHmmss}.pdf";
                string pdfFilePath = System.IO.Path.Combine(riderFolderPath, pdfFileName);
                string pdfUrl = $"{baseUrl}/Receipt/{riderFolderName}/{pdfFileName}";

                ConvertHtmlToPdf(htmlContent, pdfFilePath);
                _logger.LogInformation("End GeneratePdf in BookingSystemRepository {0}: PDF URL:", pdfUrl);

                if (rideCompletionMailSend)
                {

                    var sendMailTask = Task.Run(() => SendEmailRideCompletion(bookingData.Email!, bookingData.RiderName!, pdfFilePath));
                    await sendMailTask;
                }




                return pdfUrl;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InSide GeneratePdf in BookingSystemRepository Exception {0}", ex.Message);
                return "";
            }
        }

        #endregion

        #region SendEmailRideCompletion
        private void SendEmailRideCompletion(String emailId, string name, string attachment)
        {
            var sendOtp = new RideCompletion
            {
                Status = GlobalConstants.GlobalValues.RideCompletion,
                Port = Convert.ToInt32(_config["EmailConfiguration:Port"]!),
                MailFrom = _config["EmailConfiguration:AdminEmail"]!,
                MailFromAlias = _config["EmailConfiguration:Alias"]!,
                Password = _config["EmailConfiguration:Password"]!,
                Subject = "Ride Has been completed successfully",
                Name = name,
                Attachment = attachment,
                Host = _config["EmailConfiguration:Host"]!,
                MailTo = emailId
            };

            var Message = EmailUtility.RideCompletionMail(sendOtp);


            _logger.LogInformation("{0} InSide SendEmailOTP in TokenRepository Method Ended--    Email-Id : {1}  Message: {2}", DateTime.UtcNow, emailId, Message);
        }
        #endregion

        #region GeneratePdfAfterRideComplete
        public string GeneratePdfAfterRideComplete(BookingHistoryUserData bookingData)
        {
            try
            {
                _logger.LogInformation("InSide GeneratePdfAfterRideComplete in BookingSystemRepository");

                string baseUrl = _config["Request:Url"];
                string webRootPath = _webHostEnvironment.WebRootPath;
                string receiptPath = System.IO.Path.Combine(webRootPath, "Receipts", "receipt.html");
                string logoPath = $"{baseUrl}{GlobalConstants.GlobalValues.poshlogopath}";
                string coinPath = $"{baseUrl}{GlobalConstants.GlobalValues.coinlogopath}";

                var formattedDateTime = DateTime.UtcNow.ToString("M/dd/yyyy h:mm tt");
                decimal totalprice = (decimal)(Convert.ToDecimal(bookingData.Price) - bookingData.Promotion)!;
                double distance = Convert.ToDouble(bookingData.Distance!);
                double totaldistance = Math.Round(distance, 2);
                string htmlContent = System.IO.File.ReadAllText(receiptPath);
                htmlContent = htmlContent.Replace("{BookingId}", bookingData.BookingId.ToString());
                htmlContent = htmlContent.Replace("{RiderName}", bookingData.RiderName ?? "");
                htmlContent = htmlContent.Replace("{Total}", totalprice.ToString());
                htmlContent = htmlContent.Replace("{category}", bookingData.CategoryName ?? "");
                htmlContent = htmlContent.Replace("{totaldistance}", totaldistance.ToString());
                htmlContent = htmlContent.Replace("{CurrentTime}", formattedDateTime);
                htmlContent = htmlContent.Replace("{source}", bookingData.Time ?? "");
                htmlContent = htmlContent.Replace("{SourceName}", bookingData.SourceName ?? "");
                htmlContent = htmlContent.Replace("{DestinationName}", bookingData.DestinationName ?? "");
                htmlContent = htmlContent.Replace("{DriverName}", bookingData.DriverName ?? "");
                htmlContent = htmlContent.Replace("{subtotal}", bookingData.Price ?? "");
                htmlContent = htmlContent.Replace("{Promotion}", bookingData.Promotion.ToString() ?? "");
                htmlContent = htmlContent.Replace("{VehiclePlate}", bookingData.VehiclePlate ?? "");
                htmlContent = htmlContent.Replace("{paymentmode}", "Card");
                htmlContent = htmlContent.Replace("{paymenttime}", bookingData.CreatedDateTime ?? "");
                htmlContent = htmlContent.Replace("{logoPath}", logoPath);
                htmlContent = htmlContent.Replace("{coinPath}", coinPath);
                htmlContent = htmlContent.Replace("{min}", bookingData.Time ?? "");

                string mainFolderPath = System.IO.Path.Combine(webRootPath, "Receipt");
                string riderFolderName = Convert.ToString(bookingData.BookingId) ?? "";
                string riderFolderPath = System.IO.Path.Combine(mainFolderPath, riderFolderName);

                if (!Directory.Exists(riderFolderPath))
                {
                    Directory.CreateDirectory(riderFolderPath);
                }
                //else
                //{
                //    string[] existingFiles = Directory.GetFiles(riderFolderPath, "*.pdf");
                //    foreach (string file in existingFiles)
                //    {
                //        try
                //        {
                //            System.IO.File.Delete(file);
                //        }
                //        catch (Exception ex)
                //        {
                //            throw new Exception($"Error deleting existing PDF file: {ex.Message}", ex);
                //        }
                //    }
                //}

                string pdfFileName = $"Receipt_{bookingData.BookingId}_{DateTime.UtcNow:ddMMMyyyy_HHmmss}.pdf";
                string pdfFilePath = System.IO.Path.Combine(riderFolderPath, pdfFileName);
                string pdfUrl = $"{baseUrl}/Receipt/{riderFolderName}/{pdfFileName}";

                ConvertHtmlToPdf(htmlContent, pdfFilePath);
                _logger.LogInformation("End GeneratePdf in BookingSystemRepository {0}: PDF URL:", pdfUrl);



                //var fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                //var fileInfo = new FileInfo(pdfFilePath);

                // SendEmailRideCompletion( bookingData.RiderEmailId!,bookingData.RiderName!,pdfFilePath);

                return pdfUrl;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InSide GeneratePdf in BookingSystemRepository Exception {0}", ex.Message);
                throw new Exception($"Error GeneratePdf method: {ex.Message}", ex);
            }
        }

        #endregion

        #region ConvertHtmlToPdf
        public static void ConvertHtmlToPdf(string html, string pdfFilePath)
        {
            try
            {

                using (var stream = new FileStream(pdfFilePath, FileMode.Create))
                {
                    HtmlConverter.ConvertToPdf(html, stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error ConvertHtmlToPdf: {ex.Message}", ex);
            }

        }
        #endregion

        #region TipsAndReview
        /// <summary>
        /// Method fro TipsAndReviews
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> TipsAndReview(TipsAndReviewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                string DriverAccountNo = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id!.Equals(userId))!.Select(x => x!.StripeConnectedAccountId!)!.FirstOrDefault()!;
                string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
                TipsAndReviewDTO tipsReviews = new TipsAndReviewDTO();
                string result = string.Empty;
                var bookingRecordExists = await this.DbContextObj().TblTipsReviews?.Where(x => x.BookingDetailsId!.Value.ToString().Equals(model.BookingId!.ToString())).FirstOrDefaultAsync()!;
                var bookinDetail = this.DbContextObj().TblBookingDetail?.AsNoTracking().Where(x => x.Id.ToString()!.Equals(model.BookingId!.ToString()!)).FirstOrDefault();
                string driverAccountNumber = string.Empty;
                StripeTransfer stripeTransfer = null!;
                string customerAccountNumber = string.Empty;
                if (bookinDetail != null)
                {
                    driverAccountNumber = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(bookinDetail.DriverId)).Select(x => x.StripeConnectedAccountId).FirstOrDefault()!;
                    customerAccountNumber = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(bookinDetail.RiderId)).Select(x => x.StripeCustomerId).FirstOrDefault()!;

                    _logger.LogInformation("{0} InSide  TipsAndReview in BookingSystemController Method  -- Tip: {1} Review:{2} UserId:{3} Role:{4} Rating:{5} driverAccountNumber:{6} customerAccountNumber:{7}", DateTime.UtcNow, model.Tip, model.Review, userId, role, model.Rating, driverAccountNumber, customerAccountNumber);
                    if (bookingRecordExists != null)
                    {
                        switch (role)
                        {
                            case AuthorizationLevel.Roles.Customer:
                                {
                                    if (!string.IsNullOrEmpty(customerAccountNumber) && !string.IsNullOrEmpty(driverAccountNumber))
                                    {
                                        string paymentIntentId = string.Empty;
                                        string paymetMethodId = string.Empty;
                                        if (model.Tip > 0.0 || !string.IsNullOrEmpty(model.Review))
                                        {
                                            if (model.Tip > 0.0)
                                            {
                                                double backTip = model.Tip;
                                                var processFee = Math.Round((0.30 + model.Tip * 0.029), 2);
                                                CaptureAfterIntent resultIntentData = null!;
                                                StripeDriverPaymentInfo driverPaymentInfoTip = new StripeDriverPaymentInfo();
                                                driverPaymentInfoTip.Rate = Convert.ToInt32(model.Tip * 100);
                                                driverPaymentInfoTip.RiderCustomerId = customerAccountNumber;
                                                driverPaymentInfoTip.DriverAccountNo = driverAccountNumber;
                                                driverPaymentInfoTip.RiderId = bookinDetail?.RiderId;
                                                _logger.LogInformation("{0} InSide  TipsAndReview When Tip is greater than 0 in BookingSystemController Method  -- Tip: {1} Review:{2} UserId:{3} Role:{4} Rating:{5} driverAccountNumber:{6} customerAccountNumber:{7} Rate:{8} RiderCustomerId:{9} RiderId:{10}", DateTime.UtcNow, model.Tip, model.Review, userId, role, model.Rating, driverAccountNumber, customerAccountNumber, driverPaymentInfoTip!.Rate, driverPaymentInfoTip!.RiderCustomerId, driverPaymentInfoTip!.RiderId);
                                                var dataIntent = await _paymentRepository.CreatePaymentIntent("usd", driverPaymentInfoTip.Rate, false, 0.0, cancellationToken).ConfigureAwait(false);
                                                _logger.LogInformation("{0} InSide  TipsAndReview outside If dataIntent in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id);
                                                if (!string.IsNullOrEmpty(dataIntent?.Status) && !string.IsNullOrEmpty(dataIntent?.Id))
                                                {
                                                    _logger.LogInformation("{0} InSide  TipsAndReview inside If in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status);
                                                    if (dataIntent!.Status!.Equals("requires_confirmation"))
                                                    {
                                                        paymentIntentId = _context.HttpContext!.Session.GetString("paymentIntentId")!;
                                                        paymetMethodId = _context.HttpContext!.Session.GetString("paymetMethodId")!;

                                                        if (cancellationToken.IsCancellationRequested)
                                                            cancellationToken.ThrowIfCancellationRequested();

                                                        var resultIntent = await _paymentRepository.ConfirmPaymentIntent(paymetMethodId, paymentIntentId, cancellationToken).ConfigureAwait(false);
                                                        _logger.LogInformation("{0} InSide  TipsAndReview outside If resultIntent in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId);
                                                        if (resultIntent.Status!.Equals("requires_capture"))
                                                        {
                                                            resultIntentData = await _paymentRepository.StripeCaptureIntent(paymentIntentId).ConfigureAwait(false);
                                                            _logger.LogInformation("{0} InSide  TipsAndReview outside If resultIntentData in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6} resultIntentData Status:{7}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId, resultIntentData.Status);

                                                            if (resultIntentData.Status!.Equals("succeeded"))
                                                            {
                                                                driverPaymentInfoTip.Rate = (int)((decimal.Subtract(Convert.ToDecimal(backTip), Convert.ToDecimal(processFee))) * 100);
                                                                stripeTransfer = await _paymentRepository.StripeDriverPaymentSystem(driverPaymentInfoTip).ConfigureAwait(false);
                                                                if (stripeTransfer.Id != null)
                                                                {

                                                                    bookingRecordExists!.TipMoney = model.Tip;
                                                                    bookingRecordExists!.StripeProcessFees = processFee;
                                                                    bookingRecordExists!.TipPaid = Convert.ToDouble(decimal.Subtract(Convert.ToDecimal(backTip), Convert.ToDecimal(processFee)));
                                                                    bookingRecordExists!.TipPaymentStatus = resultIntentData?.Status;
                                                                    bookingRecordExists!.ReviewByRider = bookingRecordExists!.ReviewByRider ?? model.Review;
                                                                    bookingRecordExists!.RiderId = bookingRecordExists?.RiderId ?? userId;
                                                                    bookingRecordExists!.DriverId = bookinDetail?.DriverId ?? null!;
                                                                    bookingRecordExists!.RatingByRider = model.Rating;
                                                                    bookingRecordExists!.TransferId = bookingRecordExists?.TransferId ?? stripeTransfer.Id;
                                                                    bookingRecordExists!.BalanceTransactionId = bookingRecordExists?.BalanceTransactionId ?? stripeTransfer.BalanceTransaction;
                                                                    bookingRecordExists!.DestinationAccountNo = bookingRecordExists?.DestinationAccountNo ?? stripeTransfer.Destination;
                                                                    bookingRecordExists!.DestinationPaymentId = bookingRecordExists?.DestinationPaymentId ?? stripeTransfer.DestinationPayment;
                                                                    bookingRecordExists!.RiderCustomerId = bookingRecordExists?.RiderCustomerId ?? customerAccountNumber;
                                                                    bookingRecordExists!.UpdatedDate = DateTime.UtcNow;
                                                                    bookingRecordExists!.UpdatedBy = userId;
                                                                    this.DbContextObj().TblTipsReviews?.Update(bookingRecordExists);
                                                                    this.DbContextObj().SaveChanges();
                                                                    result = GlobalResourceFile.Success;
                                                                    return result;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            bookingRecordExists!.ReviewByRider = bookingRecordExists!.ReviewByRider ?? model.Review;
                                            bookingRecordExists!.RatingByRider = model.Rating;
                                            bookingRecordExists!.RiderId = bookingRecordExists?.RiderId ?? userId;
                                            bookingRecordExists!.DriverId = bookinDetail?.DriverId ?? null!;
                                            bookingRecordExists!.UpdatedDate = DateTime.UtcNow;
                                            bookingRecordExists!.UpdatedBy = userId;
                                            this.DbContextObj().TblTipsReviews?.Update(bookingRecordExists);
                                            this.DbContextObj().SaveChanges();
                                            result = GlobalResourceFile.Success;
                                            return result;
                                        }

                                    }
                                    break;
                                }
                            case AuthorizationLevel.Roles.Driver:
                                {
                                    bookingRecordExists!.ReviewByDriver = model.Review ?? model.Review;
                                    bookingRecordExists!.RatingByDriver = model.Rating;
                                    bookingRecordExists!.UpdatedBy = userId;
                                    bookingRecordExists!.DriverId = bookinDetail?.DriverId ?? null!;
                                    bookingRecordExists!.BookingDetailsId = bookinDetail!.Id;
                                    bookingRecordExists!.UpdatedDate = DateTime.UtcNow;
                                    this.DbContextObj().TblTipsReviews?.Update(bookingRecordExists);
                                    this.DbContextObj().SaveChanges();
                                    result = GlobalResourceFile.Success;
                                    return result;
                                }

                        }
                    }
                    else
                    {
                        switch (role)
                        {
                            case AuthorizationLevel.Roles.Customer:
                                {
                                    if (!string.IsNullOrEmpty(customerAccountNumber) && !string.IsNullOrEmpty(driverAccountNumber))
                                    {
                                        string paymentIntentId = string.Empty;
                                        string paymetMethodId = string.Empty;
                                        if (model.Tip > 0.0 || !string.IsNullOrEmpty(model.Review))
                                        {
                                            if (model.Tip > 0.0)
                                            {
                                                double backTip = model.Tip;
                                                var processFee = Math.Round((0.30 + model.Tip * 0.029), 2);
                                                CaptureAfterIntent resultIntentData = null!;
                                                StripeDriverPaymentInfo driverPaymentInfoTip = new StripeDriverPaymentInfo();
                                                driverPaymentInfoTip.Rate = Convert.ToInt32(model.Tip * 100);
                                                driverPaymentInfoTip.RiderCustomerId = customerAccountNumber;
                                                driverPaymentInfoTip.DriverAccountNo = driverAccountNumber;
                                                driverPaymentInfoTip.RiderId = bookinDetail?.RiderId;
                                                _logger.LogInformation("{0} InSide  TipsAndReview When Tip is greater than 0 in BookingSystemController Method  -- Tip: {1} Review:{2} UserId:{3} Role:{4} Rating:{5} driverAccountNumber:{6} customerAccountNumber:{7} Rate:{8} RiderCustomerId:{9} RiderId:{10}", DateTime.UtcNow, model.Tip, model.Review, userId, role, model.Rating, driverAccountNumber, customerAccountNumber, driverPaymentInfoTip!.Rate, driverPaymentInfoTip!.RiderCustomerId, driverPaymentInfoTip!.RiderId);

                                                var dataIntent = await _paymentRepository.CreatePaymentIntent("usd", driverPaymentInfoTip.Rate, false, 0.0, cancellationToken).ConfigureAwait(false);
                                                _logger.LogInformation("{0} InSide  TipsAndReview outside If dataIntent in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id);

                                                if (!string.IsNullOrEmpty(dataIntent?.Status) && !string.IsNullOrEmpty(dataIntent?.Id))
                                                {
                                                    _logger.LogInformation("{0} InSide  TipsAndReview inside If in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status);

                                                    if (dataIntent!.Status!.Equals("requires_confirmation"))
                                                    {
                                                        paymentIntentId = _context.HttpContext!.Session.GetString("paymentIntentId")!;
                                                        paymetMethodId = _context.HttpContext!.Session.GetString("paymetMethodId")!;

                                                        if (cancellationToken.IsCancellationRequested)
                                                            cancellationToken.ThrowIfCancellationRequested();

                                                        var resultIntent = await _paymentRepository.ConfirmPaymentIntent(paymetMethodId, paymentIntentId, cancellationToken).ConfigureAwait(false);
                                                        _logger.LogInformation("{0} InSide  TipsAndReview outside If resultIntent in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId);

                                                        if (resultIntent.Status!.Equals("requires_capture"))
                                                        {
                                                            resultIntentData = await _paymentRepository.StripeCaptureIntent(paymentIntentId).ConfigureAwait(false);
                                                            _logger.LogInformation("{0} InSide  TipsAndReview outside If resultIntentData in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6} resultIntentData Status:{7}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId, resultIntentData.Status);

                                                            if (resultIntentData.Status!.Equals("succeeded"))
                                                            {
                                                                driverPaymentInfoTip.Rate = (int)((decimal.Subtract(Convert.ToDecimal(backTip), Convert.ToDecimal(processFee))) * 100);
                                                                stripeTransfer = await _paymentRepository.StripeDriverPaymentSystem(driverPaymentInfoTip).ConfigureAwait(false);
                                                                if (stripeTransfer.Id != null)
                                                                {
                                                                    tipsReviews = new TipsAndReviewDTO
                                                                    {
                                                                        Id = Guid.NewGuid(),
                                                                        BookingDetailsId = model.BookingId,
                                                                        TipMoney = model.Tip,
                                                                        StripeProcessFees = processFee,
                                                                        TipPaid = Convert.ToDouble(decimal.Subtract(Convert.ToDecimal(backTip), Convert.ToDecimal(processFee))),
                                                                        TipPaymentStatus = resultIntentData.Status,
                                                                        ReviewByRider = model.Review,
                                                                        ReviewByDriver = null!,
                                                                        DriverId = bookinDetail?.DriverId ?? null!,
                                                                        RatingByRider = model.Rating,
                                                                        RiderId = userId,
                                                                        TransferId = stripeTransfer.Id,
                                                                        BalanceTransactionId = stripeTransfer.BalanceTransaction,
                                                                        DestinationAccountNo = stripeTransfer.Destination,
                                                                        DestinationPaymentId = stripeTransfer.DestinationPayment,
                                                                        RiderCustomerId = customerAccountNumber,
                                                                        UpdatedDate = null!,
                                                                        CreatedDate = DateTime.UtcNow,
                                                                        IsActive = true,
                                                                        IsDeleted = false,
                                                                        UpdatedBy = null,
                                                                        CreatedBy = userId

                                                                    };
                                                                    this.DbContextObj().TblTipsReviews?.Add(_mapper.Map<TipsReviews>(tipsReviews));
                                                                    this.DbContextObj().SaveChanges();
                                                                    result = GlobalResourceFile.Success;
                                                                    return result;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            tipsReviews = new TipsAndReviewDTO
                                            {
                                                Id = Guid.NewGuid(),
                                                BookingDetailsId = model.BookingId,
                                                ReviewByRider = model.Review,
                                                RatingByRider = model.Rating,
                                                RiderId = userId,
                                                DriverId = bookinDetail?.DriverId ?? null!,
                                                UpdatedDate = null!,
                                                CreatedDate = DateTime.UtcNow,
                                                IsActive = true,
                                                IsDeleted = false,
                                                UpdatedBy = null,
                                                CreatedBy = userId

                                            };
                                            this.DbContextObj().TblTipsReviews?.Add(_mapper.Map<TipsReviews>(tipsReviews));
                                            this.DbContextObj().SaveChanges();
                                            result = GlobalResourceFile.Success;
                                            return result;
                                        }

                                    }


                                    break;
                                }
                            case AuthorizationLevel.Roles.Driver:
                                {
                                    tipsReviews = new TipsAndReviewDTO
                                    {
                                        Id = Guid.NewGuid(),
                                        BookingDetailsId = model.BookingId,
                                        TipMoney = model.Tip,
                                        ReviewByRider = null!,
                                        ReviewByDriver = model.Review ?? model.Review,
                                        DriverId = userId,
                                        RiderId = bookinDetail?.RiderId ?? null!,
                                        RatingByDriver = model.Rating,
                                        TransferId = null!,
                                        BalanceTransactionId = null!,
                                        DestinationAccountNo = DriverAccountNo ?? DriverAccountNo,
                                        DestinationPaymentId = null!,
                                        RiderCustomerId = null!,
                                        UpdatedDate = null!,
                                        CreatedDate = DateTime.UtcNow,
                                        IsActive = true,
                                        IsDeleted = false,
                                        UpdatedBy = null,
                                        CreatedBy = userId

                                    };
                                    this.DbContextObj().TblTipsReviews?.Add(_mapper.Map<TipsReviews>(tipsReviews));
                                    this.DbContextObj().SaveChanges();
                                    result = GlobalResourceFile.Success;
                                    return result;
                                }
                        }


                    }
                }
                return result;


            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetCurrentLocation
        /// <summary>
        /// Method to get current location of Driver and Rider
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<UserLongLatInfo> GetCurrentLocation(Guid bookingId)
        {
            try
            {

                var record = await (from booking in this.DbContextObj().TblBookingDetail.Where(s => s.Id!.ToString().Equals(bookingId.ToString()))
                                    select new UserLongLatInfo { DriverLatitude = booking.DriverLat, DriverLongitude = booking.DriverLong, RiderLatitude = booking.RiderLat, RiderLongitude = booking.RiderLong }).FirstOrDefaultAsync();
                _logger.LogInformation("{0} InSide  GetCurrentLocation in BookingSystemController Method  -- DriverLat: {1} DriverLong:{2} RiderLat:{3} RiderLong:{4}", DateTime.UtcNow, record!.DriverLatitude, record!.DriverLongitude, record!.RiderLatitude, record!.RiderLongitude);
                if (record != null)
                {
                    return record;
                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FetchBonusHistoryRecord
        /// <summary>
        /// Method to get ride promotion history
        /// </summary>
        /// <returns></returns>

        public async Task<BonusHistoryData> FetchBonusHistoryRecord()
        {
            try
            {
                BonusHistoryData historyData = new BonusHistoryData();
                string userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;

                var bonusRecord = await this.DbContextObj().TblRideBonusHistory.Where(s => s.UserId!.Equals(userId)).ToListAsync();

                var totalCashBack = bonusRecord.Sum(s => s.Bonus);
                var redeemableAmount = decimal.Subtract((decimal)totalCashBack!, (decimal)bonusRecord.Sum(s => s.CashBack)!);
                var history = bonusRecord.Where(s => s.Bonus != 0.0m).Select(s => s.Bonus).ToList();
                historyData.TotalCashback = (totalCashBack == 0) ? 0.0m : totalCashBack;
                historyData.ReedemableCashback = (redeemableAmount == 0) ? 0.0m : redeemableAmount;
                historyData.CashbackHistory = (history.Count() == 0) ? null : history;
                return Task.FromResult<BonusHistoryData>(historyData!).Result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InSide FetchBonusHistoryRecord in BookingSystemRepository Exception {0}", ex.Message);
                throw;
            }

        }
        #endregion

        #region GetActualPrice
        /// <summary>
        /// Method to Calculete actual price
        /// </summary>
        /// <param name="price"></param>
        /// <param name="serviceCharge"></param>
        /// <param name="isDriver"></param>
        /// <returns></returns>
        private async Task<decimal> GetActualPrice(decimal price, decimal serviceCharge, bool isDriver, double cashback, bool isApplied)
        {
            try
            {
                double processFee = 0.0;
                decimal serviceFees = 0.0m;
                decimal rateToAccount = 0.0m;
                double cashBack = 0.0;
                if (isDriver)
                {
                    processFee = Math.Round((0.30 + Convert.ToDouble(price) * 0.029), 2);
                    serviceFees = serviceCharge;
                    cashBack = cashback;

                }
                rateToAccount = (Convert.ToDecimal(price) - serviceFees) - Convert.ToDecimal(processFee) - Convert.ToDecimal(cashBack);


                return await Task.FromResult<decimal>(rateToAccount!);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region BonusCalculate
        private decimal BonusCalculate(DigitalWallet wallateRecord, BookingDetail record, decimal bonus)
        {
            try
            {
                return decimal.Add((decimal)wallateRecord.Balance!, Math.Truncate((decimal)(record.ServiceFee * bonus)!) / 100);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region CurrentBookingCancel
        public async Task<bool> CurrentBookingCancel(BookingCancellation record)
        {
            try
            {
                BonusHistoryData historyData = new BonusHistoryData();

                var userCurrentBooking = await this.DbContextObj().TblBookingDetail.Where(s => (s.RiderId!.Equals(record.UserId!.ToString()) || s.DriverId!.Equals(record.UserId!.ToString())) && s.StatusType == GlobalConstants.GlobalValues.BookingStatus.ST_ACCEPTED).FirstOrDefaultAsync();



                switch (record.Role)
                {
                    case AuthorizationLevel.Roles.Customer:
                        {
                            var statusUpdate = await RideBookingStatusUpdate(new RideBookingStatus() { IsAccepted = "6", RiderId = userCurrentBooking!.RiderId, LocalTime = record.LocalTime }, userCurrentBooking);

                            var driverDetail = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(userCurrentBooking!.DriverId)).Select(x => new { x.StripeConnectedAccountId, x.Latitude, x.Longitude, x.DeviceId }).FirstOrDefault()!;

                            var distanceCalculate = new DistanceCalculate();

                            distanceCalculate.Source = new Posh_TRPT_Domain.BookingSystem.Source()
                            {
                                Latitude = (double)userCurrentBooking.RiderLat!,

                                Longitude = (double)userCurrentBooking.RiderLong!
                            };


                            distanceCalculate.Destination = new Posh_TRPT_Domain.BookingSystem.Destination()
                            {
                                Latitude = (double)userCurrentBooking.DriverLat!,

                                Longitude = (double)userCurrentBooking.DriverLong!
                            };

                            var minimumRiderDriverDistance = await DistanceTimeBetweenSourceDestination(distanceCalculate);
                            var mileDistance = minimumRiderDriverDistance.Item1 * 0.00062137;

                            userCurrentBooking.PaymentStatus = "cancelled";

                            decimal charableDistance = userCurrentBooking.RiderDriverDistance- Convert.ToDecimal(mileDistance);

                            if ((Convert.ToDecimal(mileDistance) < userCurrentBooking.RiderDriverDistance) && charableDistance > 0.3M)
                            {


                                var cancellation_Fee = this.DbContextObj().TblCategoryPrices.Where(x => x.Id == userCurrentBooking.CategoryPriceId).FirstOrDefaultAsync().Result;

                                if (cancellation_Fee != null)
                                {
                                    StripeDriverPaymentInfo driverPaymentInfoTip = new StripeDriverPaymentInfo();

                                    var dd = Convert.ToDecimal(userCurrentBooking.Price);

                                    var cancellationCharge = Convert.ToDecimal(((cancellation_Fee.Cancel_Penalty * Convert.ToDecimal(userCurrentBooking.Price)) / 100));

                                    var dataIntent = await _paymentRepository.CreatePaymentIntent("usd", cancellationCharge * 100, false, 0.0, new CancellationToken()).ConfigureAwait(false);
                                    _logger.LogInformation("{0} InSide  CurrentBookingCancel outside If dataIntent in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id);
                                    if (!string.IsNullOrEmpty(dataIntent?.Status) && !string.IsNullOrEmpty(dataIntent?.Id))
                                    {
                                        _logger.LogInformation("{0} InSide  TipsAndReview inside If in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status);
                                        if (dataIntent!.Status!.Equals("requires_confirmation"))
                                        {
                                            string paymentIntentId = _context.HttpContext!.Session.GetString("paymentIntentId")!;
                                            string paymetMethodId = _context.HttpContext!.Session.GetString("paymetMethodId")!;



                                            var resultIntent = await _paymentRepository.ConfirmPaymentIntent(dataIntent.PaymentMethod, dataIntent.Id, new CancellationToken()).ConfigureAwait(false);
                                            _logger.LogInformation("{0} InSide  CurrentBookingCancel outside If resultIntent in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId);
                                            if (resultIntent.Status!.Equals("requires_capture"))
                                            {
                                                var resultIntentData = await _paymentRepository.StripeCaptureIntent(paymentIntentId).ConfigureAwait(false);
                                                _logger.LogInformation("{0} InSide  CurrentBookingCancel outside If resultIntentData in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6} resultIntentData Status:{7}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId, resultIntentData.Status);

                                                if (resultIntentData.Status!.Equals("succeeded"))
                                                {
                                                    driverPaymentInfoTip.Rate = (int)((cancellationCharge - (decimal)Math.Round((0.30 + Convert.ToDouble(cancellationCharge) * 0.029), 2)) * 100);
                                                    //var rateToAccount = (Convert.ToDecimal(record.Price) - record.ServiceFee) - (decimal)Math.Round((0.30 + Convert.ToDouble(userCurrentBooking.CancelledCharge) * 0.029), 2);
                                                    driverPaymentInfoTip.RiderCustomerId = userCurrentBooking.DriverId;

                                                    var processingFees = (decimal)Math.Round((0.30 + Convert.ToDouble(userCurrentBooking.CancelledCharge) * 0.029), 2);

                                                    driverPaymentInfoTip.DriverAccountNo = driverDetail.StripeConnectedAccountId;
                                                    driverPaymentInfoTip.RiderId = userCurrentBooking.RiderId;

                                                    _logger.LogInformation("{0} InSide  CurrentBookingCancel start StripeDriverPaymentSystem in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6} resultIntentData Status:{7}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId, resultIntentData.Status);


                                                    var stripeTransfer = await _paymentRepository.StripeDriverPaymentSystem(driverPaymentInfoTip).ConfigureAwait(false);
                                                    if (stripeTransfer.Id != null)
                                                    {
                                                        userCurrentBooking.iscancelled = true;
                                                        userCurrentBooking.CancelledCharge = cancellationCharge;
                                                        userCurrentBooking.CancelledStripeFee = (decimal)Math.Round((0.30 + Convert.ToDouble(userCurrentBooking.CancelledCharge) * 0.029), 2);
                                                        userCurrentBooking.PaymentStatus = "succeeded";
                                                        userCurrentBooking.PaymentIntentId = dataIntent!.Id;
                                                    }

                                                    _logger.LogInformation("{0} InSide  CurrentBookingCancel End StripeDriverPaymentSystem in BookingSystemRepository Method  -- dataIntent = {1} dataIntent Id: {2} dataIntent Status:{3} resultIntent Status:{4} paymentIntentId:{5} paymetMethodId:{6} resultIntentData Status:{7}", DateTime.UtcNow, dataIntent?.Status, dataIntent?.Id, dataIntent?.Status, resultIntent!.Status, paymentIntentId, paymetMethodId, resultIntentData.Status);


                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            userCurrentBooking!.UpdatedDate = DateTime.UtcNow;
                            userCurrentBooking!.UpdatedBy = record.UserId;

                            userCurrentBooking.LocalUpdatedDateTime = Convert.ToDateTime(record.LocalTime);
                            this.DbContextObj().TblBookingDetail?.Update(userCurrentBooking);
                            await this.DbContextObj().SaveChangesAsync();

                            _logger.LogInformation("{0} InSide  CurrentBookingCancel Before Notification send in BookingSystemRepository Method ", DateTime.UtcNow);

                            var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED, IntentPaymentStatus = string.Empty };
                            NotificationModel notificationModel = new NotificationModel
                            {
                                DeviceId = driverDetail!.DeviceId,
                                IsAndroidDevice = true,
                                Title = "Ride cancelled",
                                Body = "Ride has been cancelled by rider!"

                            };

                            Parallel.Invoke(() => SendNotification(notificationModel, dataUser));

                            _logger.LogInformation("{0} InSide  CurrentBookingCancel After Notification send in BookingSystemRepository Method  ", DateTime.UtcNow);

                            break;
                        }
                    case AuthorizationLevel.Roles.Driver:
                        {

                            _logger.LogInformation("{0} InSide  CurrentBookingCancel For Driver Before RideBookingStatusUpdate  in BookingSystemRepository Method ", DateTime.UtcNow);


                            await RideBookingStatusUpdate(new RideBookingStatus() { IsAccepted = "6", RiderId = userCurrentBooking!.RiderId, LocalTime = record.LocalTime }, userCurrentBooking);
                            var dataUser = new UserData { type = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED, IntentPaymentStatus = string.Empty };

                            _logger.LogInformation("{0} InSide  CurrentBookingCancel For Driver After RideBookingStatusUpdate  in BookingSystemRepository Method ", DateTime.UtcNow);

                            NotificationModel notificationModel = new NotificationModel
                            {
                                DeviceId = userCurrentBooking!.RiderDeviceId,
                                IsAndroidDevice = true,
                                Title = "Ride cancelled",
                                Body = "Ride has been cancelled by driver!"

                            };

                            Parallel.Invoke(() => SendNotification(notificationModel, dataUser));
                            _logger.LogInformation("{0} InSide  CurrentBookingCancel After Notification send in BookingSystemRepository Method ", DateTime.UtcNow);

                            break;
                        }
                }

                return Task.FromResult<bool>(true).Result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InSide FetchBonusHistoryRecord in BookingSystemRepository Exception {0}", ex.Message);
                throw;
            }

        }
        #endregion

        #region RideCancelByCancellationToken
        private async Task<bool> RideCancelByCancellationToken(DistanceCalculate model)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            DigitalWalletData walletData = new DigitalWalletData();
            PaymentIntentService service = new PaymentIntentService();

            BookingDetail? detailData = await this.DbContextObj().TblBookingDetail.Where(s => s.RiderId == model.riderDetail!.Id && s.BookingStatusId == Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.NOTIFIED_DRIVER)).OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();


            if (detailData != null)
            {
                #region Cancel payment intent created while booking ride

                PaymentIntent data = await service.CancelAsync(detailData.PaymentIntentId, new PaymentIntentCancelOptions { CancellationReason = "requested_by_customer" }).ConfigureAwait(false);

                _logger.LogInformation("{0}InSide Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- PaymentIntentId : {1} CancellationReason:{2}", DateTime.UtcNow, detailData.PaymentIntentId, "requested_by_customer");

                StripeCustomerIntentCustom res = JsonConvert.DeserializeObject<StripeCustomerIntentCustom>(data.ToJson())!;
                #endregion



                _logger.LogInformation("{0}InSide Cancelled Request RideBookingNotifyDriver in BookingSystemRepository Method -- BookingStatusId : {1} RiderId:{2} StatusType: {3}", DateTime.UtcNow, detailData.BookingStatusId, detailData.RiderId, detailData.StatusType);

                // Update the status of Driver
                ApplicationUser? result = await this.DbContextObj().Users.Where(s => s.Id == detailData.DriverId).FirstOrDefaultAsync().ConfigureAwait(false);
                if (result != null)
                {
                    result.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED);
                    result.UpdatedDate = DateTime.UtcNow;
                    result.UpdatedBy = detailData.RiderId;
                    this.DbContextObj().Entry(result).State = EntityState.Modified;
                    await this.DbContextObj().SaveChangesAsync();
                }


                #region Use to update the record of wallet and bonus provided
                DigitalWallet? wallateRecord = await this.DbContextObj().TblDigitalWallet?.Where(x => x.UserId!.Equals(detailData.RiderId)).FirstOrDefaultAsync()!;
                CategoryPrice? rideCategoryPrice = await this.DbContextObj().TblCategoryPrices?.Where(x => x.Id!.Equals(detailData.CategoryPriceId)).FirstOrDefaultAsync()!;
                RideBonusHistory? bonusRecord = await this.DbContextObj().TblRideBonusHistory?.Where(x => x.BookingId!.Equals(detailData.Id)).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync()!;

                if (wallateRecord != null && wallateRecord.IsApplied)
                {
                    wallateRecord!.Balance += Math.Truncate((decimal)(detailData.ServiceFee * rideCategoryPrice!.Bonus_Amount)! * 100) / 100;
                    wallateRecord.UpdatedDate = DateTime.UtcNow;
                    wallateRecord.UpdatedBy = detailData.RiderId;
                    this.DbContextObj().TblDigitalWallet.Update(wallateRecord);
                    await this.DbContextObj().SaveChangesAsync();
                }

                if (bonusRecord != null)
                {
                    bonusRecord.IsApplied = false;
                    bonusRecord.CashBack = 0.0m;
                    this.DbContextObj().TblRideBonusHistory.Update(bonusRecord);
                    await this.DbContextObj().SaveChangesAsync();
                }
                #endregion



                /// Update the record of TblBookingHistory table

                detailData.BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.CANCELLED);
                detailData.UpdatedDate = DateTime.UtcNow;
                detailData.UpdatedBy = detailData.RiderId;
                double seconds = DateTime.UtcNow.Subtract((DateTime)detailData.UpdatedDate).TotalSeconds;
                detailData.LocalUpdatedDateTime = Convert.ToDateTime(model.LocalTime).AddSeconds(seconds);
                detailData.StatusType = GlobalConstants.GlobalValues.BookingStatus.ST_CANCELLED;
                detailData.PaymentStatus = "cancelled";
                this.DbContextObj().Entry(detailData).State = EntityState.Modified;
                await this.DbContextObj().SaveChangesAsync();

                return true;
            }
            return false;
        }
        #endregion

        #region CalculateStripeFees
        private async Task<decimal> CalculateStripeFees(decimal price)
        {
            return await Task.FromResult<decimal>(Math.Round((0.30m + (price * 0.029m)), 2));
        }
        #endregion

        #region NotificationBody

    }
    #endregion
}

