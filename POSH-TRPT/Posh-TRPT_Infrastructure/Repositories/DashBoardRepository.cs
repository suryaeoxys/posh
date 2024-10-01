using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Posh_TRPT_Domain.DashBoard;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;
using Stripe;
using System.IO;
using System.Text;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class DashBoardRepository : Repository<object>, IDashBoardRepository
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<DashBoardRepository> _logger;
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;
        private static IWebHostEnvironment _environment => (IWebHostEnvironment)_httpContext.RequestServices.GetService(typeof(IWebHostEnvironment))!;
        public DashBoardRepository(DbFactory dbFactory, IHttpContextAccessor context, ILogger<DashBoardRepository> logger,
            UserManager<ApplicationUser> userManager, IOptions<StripeSettings> stripeSettings, IConfiguration config) : base(dbFactory)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _stripeSettings = stripeSettings.Value;
            _config = config;
        }

        #region GetDriverRiderCounts
        public async Task<DriverRiderRideCounts> GetDriverRiderRideCounts()
        {
            try
            {
                DriverRiderRideCounts result = new();
                _logger.LogInformation("{0} InSide  GetDriverRiderRideCounts in DashBoardRepository Method ", DateTime.UtcNow);
                result = await this.DbContextObj().GetRecordExecuteProcedureAsync<DriverRiderRideCounts>("Sp_GetDriverRiderRideCounts", new SqlParameter[] { });
                if (result != null)
                {
                    _logger.LogInformation("{0} InSide after calling SP Sp_GetDriverRiderRideCounts  GetDriverRiderRideCounts in DashBoardRepository Method  -- TotalDriver:={1}, TotalRiders:={2}, TotalCurrentRider:={3}, TotalCompleteRides:={4}", DateTime.UtcNow, result.TotalDrivers, result.TotalRiders, result.RunningRides, result.TotalCompleteRides);
                    return result;
                }
                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetDriverRiderRideCounts in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Monthly Rides Details
        public async Task<IEnumerable<MonthlyCompletedCanceledRides>> MonthlyRidesDetails()
        {
            try
            {

                _logger.LogInformation("{0} InSide  MonthlyRidesDetails in DashBoardRepository Method ", DateTime.UtcNow);
                List<MonthlyCompletedCanceledRides> result = new();
                result = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<MonthlyCompletedCanceledRides>("Sp_GetMonthlyCompletedCanceledRunningRides", new SqlParameter[] { });
                if (result.Count() > 0)
                {
                    _logger.LogInformation("{0} InSide after calling SP Sp_GetMonthlyCompletedCanceledRunningRides  MonthlyRidesDetails in DashBoardRepository Method  --", DateTime.UtcNow);
                    return result;
                }
                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside MonthlyRidesDetails in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Current Running Rides
        public async Task<IEnumerable<CurrentlyRunningRides>> CurrentlyRunningRides()
        {
            try
            {
                _logger.LogInformation("{0} InSide  CurrentlyRunningRides in DashBoardRepository Method ", DateTime.UtcNow);
                List<CurrentlyRunningRides> result = new();
                result = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<CurrentlyRunningRides>("Sp_GetCurrentlyRunningRides", new SqlParameter[] { });
                if (result.Count() > 0)
                {
                    _logger.LogInformation("{0} InSide after calling SP Sp_GetCurrentlyRunningRides  MonthlyRidesDetails in DashBoardRepository Method  --", DateTime.UtcNow);
                    //IEnumerable<CurrentlyRunningRides> obj = result;
                    return result;
                }
                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside MonthlyRidesDetails in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        public async Task<IEnumerable<DriversDataResponse>> GetDriversDataWithEarnings()
        {
            List<DriversDataResponse> driverdetails = new List<DriversDataResponse>();

            var user = await Task.Run(() => (from User in this.DbContextObj().Users
                                             join uroles in this.DbContextObj().UserRoles on User.Id equals uroles.UserId
                                             join role in this.DbContextObj().Roles on uroles.RoleId equals role.Id
                                             where role.Name == "Driver" && User.IsActive == true && User.StripeConnectedAccountId != null
                                             && User.StatusId == new Guid("57DEEADB-B1C5-4273-A830-ED8D3B001F70")
                                             select new
                                             {
                                                 User
                                             }).ToListAsync());



            foreach (var users in user)
            {
                DriversDataResponse driverDetailsResponse = new DriversDataResponse();
                string userAccountId = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(users.User!.Id)).Select(x => x.StripeConnectedAccountId).FirstOrDefault()!;
                if (!string.IsNullOrEmpty(userAccountId))
                {
                    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                    var options = new BalanceGetOptions();
                    var requestOptions = new RequestOptions
                    {
                        StripeAccount = userAccountId,
                    };
                    var service = new BalanceService();
                    var accountBalance = await service.GetAsync(options, requestOptions);
                    var response = JsonConvert.DeserializeObject<StripeAccountBalance>(accountBalance.ToJson())!;
                    if (response.Available!.Count() > 0)
                    {
                        if (response.Available![0]?.Currency == "usd")
                        {
                            driverDetailsResponse.TotalEarnings = (Convert.ToDouble(response.Available![0].Amount) / 100);
                        }
                    }
                }
                var rateingList = await this.DbContextObj().TblTipsReviews?.Where(x => x.DriverId!.Equals(users.User!.Id)).Select(c => c.RatingByRider).ToListAsync()!;
                var rateingSum = await this.DbContextObj().TblTipsReviews?.Where(x => x.DriverId!.Equals(users.User!.Id)).Select(c => c.RatingByRider).SumAsync()!;
                int countRating = rateingList.Count;
                if (countRating > 0 && rateingSum > 0.0)
                {
                    driverDetailsResponse.Rating = Math.Round((rateingSum / (double)countRating), 2, MidpointRounding.AwayFromZero);
                }
                driverDetailsResponse.User!.Id = users.User?.Id;
                driverDetailsResponse.User!.Comment = users.User?.Comment;
                driverDetailsResponse.User!.DocStatus = this.DbContextObj().TblStatus.Where(z => z.Id.Equals(users.User!.StatusId)).FirstOrDefault()!.Name;
                driverDetailsResponse.User!.Email = users.User?.Email;
                driverDetailsResponse.User!.Mobile = users.User?.PhoneNumber;
                driverDetailsResponse.User!.DriverName = users.User?.Name;
                driverDetailsResponse.User!.DOB = string.Concat("0", Convert.ToDateTime(users.User!.DOB).Month.ToString() + "/" + Convert.ToDateTime(users.User!.DOB).Day.ToString() + "/" + Convert.ToDateTime(users.User!.DOB).Year.ToString());
                driverDetailsResponse.User!.ProfilePhoto = _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + users.User?.ProfilePhoto;
                driverdetails.Add(driverDetailsResponse);
            }


            return driverdetails;
        }

        #region total Earnings
        public async Task<IEnumerable<TotalEarningByDate>> TotalEarningsDetails()
        {
            try
            {
                _logger.LogInformation("{0} InSide  TotalEarningsDetails in DashBoardRepository Method ", DateTime.UtcNow);
                List<TotalEarningByDate> result = new();

                result = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<TotalEarningByDate>("Sp_GetTotalEarningsByDate ", new SqlParameter[] { });
                if (result.Count() > 0)
                {
                    _logger.LogInformation("{0} InSide after calling SP Sp_GetTotalEarningsByDate  TotalEarningsDetails in DashBoardRepository Method  --", DateTime.UtcNow);
                    return result;
                }
                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside TotalEarningsDetails in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Order Percentage with status
        public async Task<IEnumerable<OrdersCountWithStatus>> GetOrdersCountsWithStatus()
        {
            try
            {
                List<OrdersCountWithStatus> result = new();
                _logger.LogInformation("{0} InSide  GetOrdersCountsWithStatus Sp_GetOrdersPercentagesWithStatus in DashBoardRepository Method ", DateTime.UtcNow);
                 result = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<OrdersCountWithStatus>("Sp_GetOrdersPercentagesWithStatus", new SqlParameter[] { });
                _logger.LogInformation("{0} InSide after execution of   GetOrdersCountsWithStatus Sp_GetOrdersPercentagesWithStatus in DashBoardRepository Method ", DateTime.UtcNow);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetOrdersCountsWithStatus in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }

        #endregion

        #region Top 10 Rated Drivers
        //*****************************************************************************************
        // Name                 :   Top 10 Rated Drivers
        // Return type          :   IEnumerable<DriverRating>
        // Input Parameter(s)   :   N/A
        // Purpose              :   To create a Graph at dashboard 
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  July 30 2024  -                            -  Saloni S
        //*****************************************************************************************
        public async Task<IEnumerable<DriverRating>> Top10RatedDrivers()
        {
            List<DriverRating> listOfDrivers = new();
            try
            {


                _logger.LogInformation("{0} Inside Top10RatedDrivers in DashBoardRepository Method -- Start", DateTime.UtcNow);
                var driverRating = await (from rating in DbContextObj().TblTipsReviews
                                          join user in DbContextObj().Users on rating.DriverId equals user.Id
                                          group new { rating, user } by rating.DriverId into ratingGroup
                                          select new DriverRating
                                          {
                                              DriverName = ratingGroup.FirstOrDefault()!.user.Name,
                                              Rating = ratingGroup.Average(X => X.rating.RatingByRider),
                                              ProfilePhoto = string.IsNullOrEmpty(ratingGroup.FirstOrDefault()!.user.ProfilePhoto)
                                              ? GlobalConstants.GlobalValues.personImage
                                              : $"/{GlobalResourceFile.ProfilePic}/{ratingGroup.FirstOrDefault()!.user!.ProfilePhoto}"
                                          })
                                    .OrderByDescending(rating => rating.Rating)
                                    .Take(10)
                                    .ToListAsync();

                _logger.LogInformation("{0} Inside Top10RatedDrivers in DashBoardRepository Method -- End", DateTime.UtcNow);
                _logger.LogInformation("{0} Inside Top10RatedDrivers in DashBoardRepository Method -- End", DateTime.UtcNow);
                return driverRating;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside TotalEarningsDetails in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }


        #endregion

        #region Top5EarnedDrivers
        //*****************************************************************************************
        // Name                 :   Top5EarnedDrivers
        // Return type          :   IEnumerable<DriverRating>
        // Input Parameter(s)   :   N/A
        // Purpose              :   To create a Graph at dashboard 
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0      - 05 Aug 2024   -                            -  Saloni S
        //*****************************************************************************************
        public async Task<IEnumerable<DriverEarning>> Top5EarnedDrivers()
        {
            List<DriverEarning> listOfDrivers = new();
            try
            {
                _logger.LogInformation("{0} Inside Top5EarnedDrivers in DashBoardRepository Method -- Start", DateTime.UtcNow);
               listOfDrivers = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<DriverEarning>("Sp_GetTop5EarnedDrivers", new SqlParameter[] { });
                _logger.LogInformation("{0} Inside Top5EarnedDrivers in DashBoardRepository Method -- End", DateTime.UtcNow);
                return listOfDrivers;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside Top5EarnedDrivers in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion
    }

}


