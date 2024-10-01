using Posh_TRPT_Domain.Entity;

namespace Posh_TRPT_Domain.Report
{
    public interface IReportRepository
    {
        Task<IEnumerable<BookingStatus>> GetAllStatusForReport();
        Task<ReportData> GetFilteredDataOfOrders(string startDate, string endDate, int statusType, Guid? driverId);
    }
}
