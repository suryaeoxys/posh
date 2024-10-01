using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Data.SqlClient;
using Posh_TRPT_Domain.DashBoard;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Report;
using Posh_TRPT_Utility.ConstantStrings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class ReportRepository : Repository<object>, IReportRepository
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<DashBoardRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;

        private static IWebHostEnvironment _environment => (IWebHostEnvironment)_httpContext.RequestServices.GetService(typeof(IWebHostEnvironment))!;
        public ReportRepository(DbFactory dbFactory, IHttpContextAccessor context, ILogger<DashBoardRepository> logger,
            UserManager<ApplicationUser> userManager ,IConfiguration config) : base(dbFactory)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _config = config;
        }
        #region GetFilteredDataOfOrders
        //*****************************************************************************************
        // Name                 :   GetFilteredDataOfOrdered
        // Return type          :   IEnumerable<ReportOrderData>
        // Input Parameter(s)   :   N/A
        // Purpose              :   To create a Report of orders 
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  July 31 2024  -                            -  Saloni S
        //*****************************************************************************************
        public async Task<ReportData> GetFilteredDataOfOrders(string startDate, string endDate, int statusType, Guid? driverId)
        {
            try
            {
                ReportData data = new();
                _logger.LogInformation("{0} InSide  GetFilteredDataOfOrders in DashBoardRepository Method ", DateTime.UtcNow);
                List<ReportOrderData> result = new();
                SqlParameter[] sqlParameter = new SqlParameter[]
                {
                     new SqlParameter { ParameterName ="@StartDate", Value = startDate},
                     new SqlParameter { ParameterName ="@EndDate", Value = endDate},
                     new SqlParameter { ParameterName ="@StatusType", Value = statusType},
                     new SqlParameter { ParameterName = "@DriverId", Value = (driverId.HasValue ? driverId.Value : (object)DBNull.Value)}
                };
                result = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<ReportOrderData>("Sp_GetRidesDetailsForReport @StartDate, @EndDate, @StatusType, @DriverId", sqlParameter);
                if (result.Count() > 0)
                {
                    string url = await GeneratePdfForReport(result).ConfigureAwait(false);
                    string Excelurl = await GenerateExecelForReport(result).ConfigureAwait(false);
                    data.ReportOrderData = result;
                    data.PdfURL = url;
                    data.ExeclURL = Excelurl;
                    _logger.LogInformation("{0} InSide after calling SP Sp_GetTotalEarningsByDate  GetFilteredDataOfOrders in DashBoardRepository Method  --", DateTime.UtcNow);
                    return data;
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetFilteredDataOfOrders in DashBoardRepository Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }

        #endregion

        #region GetAllStatusForReport
        public async Task<IEnumerable<BookingStatus>> GetAllStatusForReport()
        {
            var specificStatuses = new List<string> {
                    GlobalConstants.GlobalValues.BookingStatus.COMPLETED,
                    GlobalConstants.GlobalValues.BookingStatus.CANCELLED,
                     GlobalConstants.GlobalValues.BookingStatus.DECLINED,
                     GlobalConstants.GlobalValues.BookingStatus.AUTOMATICDECLINE,
                };
                    var data = await (from status in this.DbContextObj().TblBookingStatus
                              where specificStatuses.Contains(status.Id.ToString())
                              select new BookingStatus
                              {
                                  Id = status.Id,
                                  Name = status.Name,
                              })
                          .ToListAsync();
            return data;
        }
        #endregion

        #region GeneratePdfForReport
        public async Task<string> GeneratePdfForReport(List<ReportOrderData> data)
        {
            string baseUrl = _config["Request:Url"];
            string webRootPath = _environment.WebRootPath;
            string htmlContent = string.Empty;

            string logoPath = $"{webRootPath}{GlobalConstants.GlobalValues.poshlogopath}";
            string coinPath = $"{baseUrl}{GlobalConstants.GlobalValues.coinlogopath}";
            string reportPath = System.IO.Path.Combine(webRootPath, "Reports", "Report.PDF");

            //var data = data.Select(s => s.OrderStatus.Replace("NotifyPaymentSuccess", "COMPLETED")).ToList();
            data.Where(w => w.OrderStatus == "NotifyPaymentSuccess").ToList().ForEach(s => s.OrderStatus = "COMPLETED");

            // Define the directory where PDF files will be stored
            string pdfDirectory = System.IO.Path.Combine(webRootPath, "Reports");

            if (System.IO.Directory.Exists(pdfDirectory))
            {
                foreach ( var item in  System.IO.Directory.GetFiles(pdfDirectory))
                {
                    System.IO.File.Delete(item);
                }
                System.IO.Directory.Delete(pdfDirectory);
                Directory.CreateDirectory(pdfDirectory);
            }
            else
            {
                Directory.CreateDirectory(pdfDirectory);
            }
            string fileName = $"Report{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(pdfDirectory, fileName);
            Rectangle pageSize = new Rectangle(PageSize.A4.Width, PageSize.A4.Height);
            Document document = new Document(pageSize.Rotate());
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;

                    if (System.IO.File.Exists(logoPath))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                        float maxWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                        float maxHeight = 40; // Adjust the height as needed
                        logo.ScaleToFit(maxWidth, maxHeight);
                        float logoY = document.PageSize.Height - logo.ScaledHeight - 10;
                        float logoX = (document.PageSize.Width - logo.ScaledWidth) / 2;
                        logo.SetAbsolutePosition(logoX, logoY);
                        cb.AddImage(logo);
                        document.Add(new iTextSharp.text.Paragraph("\n"));
                        document.Add(new iTextSharp.text.Paragraph("\n"));
                        document.Add(new iTextSharp.text.Paragraph("\n"));
                        document.Add(new iTextSharp.text.Paragraph("\n"));
                        document.Add(new iTextSharp.text.Paragraph("\n"));
                    }

                    BaseColor headerTextColor = BaseColor.WHITE;
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD.ToUpper(), 15, headerTextColor);
                    PdfPTable table1 = new PdfPTable(2);
                    table1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.WidthPercentage = 50;
                    //table1.get
                    AddHeaderCell(table1, "Name", "#7a5e8b", headerFont);
                    AddHeaderCell(table1, "Value", "#7a5e8b", headerFont);

                    OrderCountsForPDF orderCountsForPDF = new OrderCountsForPDF()
                    {
                        TOTAL_RIDES = data.Count(),
                        TOTAL_DRIVERS = data.GroupBy(x => x.Driver).Count(),
                        TOTAL_RIDERS = data.GroupBy(X => X.Rider).Count(),
                        COMPLETED_RIDES = data.Where(x => x.OrderStatus == "COMPLETED").Count(),
                        CANCELLED_RIDES = data.Where(x => x.OrderStatus == "CANCELLED").Count(),
                        DECLINED_RIDES = data.Where(x => x.OrderStatus == "DECLINED").Count(),
                        SYSTEMDECLINED_RIDES = data.Where(x => x.OrderStatus == "AutomaticDecline").Count(),
                    };
                    table1.AddCell("TOTAL_RIDES");
                    table1.AddCell(orderCountsForPDF.TOTAL_RIDES.ToString());
                    table1.AddCell("TOTAL_DRIVERS");
                    table1.AddCell(orderCountsForPDF.TOTAL_DRIVERS.ToString());
                    table1.AddCell("TOTAL_RIDERS");
                    table1.AddCell(orderCountsForPDF.TOTAL_RIDERS.ToString());
                    table1.AddCell("COMPLETED_RIDES");
                    table1.AddCell(orderCountsForPDF.COMPLETED_RIDES.ToString());
                    table1.AddCell("CANCELLED_RIDES");
                    table1.AddCell(orderCountsForPDF.CANCELLED_RIDES.ToString());
                    table1.AddCell("DECLINED_RIDES");
                    table1.AddCell(orderCountsForPDF.DECLINED_RIDES.ToString());
                    table1.AddCell("SYSTEMDECLINED_RIDES");
                    table1.AddCell(orderCountsForPDF.SYSTEMDECLINED_RIDES.ToString());
                    document.Add(table1);

                    document.Add(new iTextSharp.text.Paragraph("\n"));
                    document.Add(new iTextSharp.text.Paragraph("\n"));
                    var tripsByStatus = data.GroupBy(t => t.OrderStatus);
                    foreach (var group in tripsByStatus)
                    {
                        var heading = new iTextSharp.text.Paragraph(group.Key, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                        heading.Alignment = Element.ALIGN_CENTER;
                        document.Add(heading);
                        document.Add(new iTextSharp.text.Paragraph("\n"));
                        // Create a table with 9 columns
                        PdfPTable table = new PdfPTable(10);
                        table.WidthPercentage = 100;
                        float[] columnWidths = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 2f, 3f, 3f, 1f };
                        table.SetWidths(columnWidths);

                        // Create header cells with custom background and text color
                        AddHeaderCell(table, "Date", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Driver", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Rider", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Category", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Pick Up Time", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Drop Off Time", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Price", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Source Address", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "Destination Address", "#7a5e8b", headerFont);
                        AddHeaderCell(table, "TollFees", "#7a5e8b", headerFont);

                        // Add trips to the table
                        foreach (var trip in group)
                        {
                            table.AddCell(trip.Date.ToString("MM/dd/yyyy"));
                            table.AddCell(trip.Driver);
                            table.AddCell(trip.Rider);
                            table.AddCell(trip.Category);
                            table.AddCell(trip.PickUpTime.ToString("hh\\:mm") ?? "00:00");
                            table.AddCell(trip.DropTime.ToString("hh\\:mm") ?? "00:00" );
                            table.AddCell(trip.RideTotalPrice ?? "0:00");
                            table.AddCell(trip.RiderSourceName);
                            table.AddCell(trip.DestinationPlaceName);
                            table.AddCell(trip.TollFees.ToString() ?? "0:00");
                        }

                        // Add the table to the document
                        document.Add(table);

                        // Add a new page after each status group, if needed
                        document.NewPage();
                    }
                    document.Close();
                }
                string pdfUrl = $"/Reports/{fileName}";
                return pdfUrl;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Inside GeneratePdfForReport into DashBoard Repository Exception {0}", ex.Message);
                throw;
            }
           
        }

        #region Header and Cell Color Configuration
        private static void AddHeaderCell(PdfPTable table, string text, string backgroundColorHex, Font font)
        {
            // Convert the hex color string to a BaseColor
            BaseColor backgroundColor = GetBaseColorFromHex(backgroundColorHex);

            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = backgroundColor;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5; // Add some padding if needed
            table.AddCell(cell);
        }

        private static BaseColor GetBaseColorFromHex(string hexColor)
        {
            // Remove the # if it's present
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }

            // Convert the hex string to a byte array
            byte[] bytes = Enumerable.Range(0, hexColor.Length)
                                     .Where(x => x % 2 == 0)
                                     .Select(x => Convert.ToByte(hexColor.Substring(x, 2), 16))
                                     .ToArray();

            // Create a BaseColor from the byte array
            switch (bytes.Length)
            {
                case 3:
                    return new BaseColor(bytes[0], bytes[1], bytes[2]);
                case 4:
                    return new BaseColor(bytes[0], bytes[1], bytes[2], bytes[3]);
                default:
                    throw new ArgumentException("Invalid hex color format. It should be a 3-digit or 4-digit hex value.");
            }
        }

        #endregion

        #endregion

        #region GenerateExecelForReport
        public async Task<string> GenerateExecelForReport(List<ReportOrderData> data)
        {
            string baseUrl = _config["Request:Url"];
            string webRootPath = _environment.WebRootPath;
            string htmlContent = string.Empty;

            string logoPath = $"{webRootPath}{GlobalConstants.GlobalValues.poshlogopath}";
            string coinPath = $"{baseUrl}{GlobalConstants.GlobalValues.coinlogopath}";

            //var data = data.Select(s => s.OrderStatus.Replace("NotifyPaymentSuccess", "COMPLETED")).ToList();
            data.Where(w => w.OrderStatus == "NotifyPaymentSuccess").ToList().ForEach(s => s.OrderStatus = "COMPLETED");
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    // Group orders by status
                    var ordersByStatus = data.GroupBy(o => o.OrderStatus);

                    // Create a worksheet for each status
                    foreach (var group in ordersByStatus)
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Add(group.Key);

                        // Add headers
                        worksheet.Cell(1, 1).Value = "Date";
                        worksheet.Cell(1, 2).Value = "Driver";
                        worksheet.Cell(1, 3).Value = "Rider";
                        worksheet.Cell(1, 4).Value = "Source Address";
                        worksheet.Cell(1, 5).Value = "PickUpTime";
                        worksheet.Cell(1, 6).Value = "DropOffTime";
                        worksheet.Cell(1, 7).Value = "Price";
                        worksheet.Cell(1, 8).Value = "Toll Fees";
                        worksheet.Cell(1, 9).Value = "Destination Address";
                        worksheet.Cell(1, 10).Value = "Category";

                        worksheet.Column(1).Width = 16;
                        worksheet.Column(2).Width = 20;
                        worksheet.Column(3).Width = 20;
                        worksheet.Column(4).Width = 30;
                        worksheet.Column(5).Width = 15;
                        worksheet.Column(6).Width = 15;
                        worksheet.Column(7).Width = 15;
                        worksheet.Column(8).Width = 15;
                        worksheet.Column(9).Width = 40;
                        worksheet.Column(10).Width = 15;

                        string hexColorCode = "#7a5e8b"; // Replace with your desired hex color code

                        XLColor headerColor = XLColor.FromHtml(hexColorCode);

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Fill.SetBackgroundColor(headerColor);
                        headerRow.Style.Font.Bold = true; // Optional: make the header text bold
                        headerRow.Style.Font.FontColor = XLColor.White;
                        int row = 2;
                        foreach (var order in group)
                        {
                            worksheet.Cell(row, 1).Value = order.Date.ToString("MM-dd-yyyy");
                            worksheet.Cell(row, 2).Value = order.Driver;
                            worksheet.Cell(row, 3).Value = order.Rider;
                            worksheet.Cell(row, 4).Value = order.RiderSourceName;
                            worksheet.Cell(row, 5).Value = order.PickUpTime.ToString("hh\\:mm") ?? "00:00";
                            worksheet.Cell(row, 6).Value = order.DropTime.ToString("hh\\:mm") ?? "00:00";
                            worksheet.Cell(row, 7).Value = order.RideTotalPrice?.ToString() ?? "0.00";
                            worksheet.Cell(row, 8).Value = order.TollFees.ToString() ?? "0.00";
                            worksheet.Cell(row, 9).Value = order.DestinationPlaceName;
                            worksheet.Cell(row, 10).Value = order.Category;
                            row++;
                        }
                    }
                    var filePath = Path.Combine(webRootPath, "ExcelFile", "OrdersReportInExcel.xlsx");
                    var directoryPath = Path.GetDirectoryName(filePath);

                    if (System.IO.Directory.Exists(directoryPath))
                    {
                        foreach (var item in System.IO.Directory.GetFiles(directoryPath))
                        {
                            System.IO.File.Delete(item);
                        }
                        System.IO.Directory.Delete(directoryPath);
                        Directory.CreateDirectory(directoryPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(directoryPath!);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.SaveAs(stream);
                    }

                    return "/ExcelFile/OrdersReportInExcel.xlsx";
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        #endregion
    }
}

