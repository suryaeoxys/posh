using Posh_TRPT_Domain.Interfaces;

namespace Posh_TRPT_Domain.DashBoard
{
    public interface IDashBoardRepository : IRepository<object>
    {
        Task<DriverRiderRideCounts> GetDriverRiderRideCounts();
        Task<IEnumerable<MonthlyCompletedCanceledRides>> MonthlyRidesDetails();
        Task<IEnumerable<CurrentlyRunningRides>> CurrentlyRunningRides();
        Task<IEnumerable<DriversDataResponse>> GetDriversDataWithEarnings();
        Task<IEnumerable<TotalEarningByDate>> TotalEarningsDetails();
        Task<IEnumerable<OrdersCountWithStatus>> GetOrdersCountsWithStatus();
        Task<IEnumerable<DriverRating>> Top10RatedDrivers();
        Task<IEnumerable<DriverEarning>> Top5EarnedDrivers();
    }
}
