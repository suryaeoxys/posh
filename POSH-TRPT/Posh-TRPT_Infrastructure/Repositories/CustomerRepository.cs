using iTextSharp.text.log;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Posh_TRPT_Domain.Customer;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.Resources;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Posh_TRPT_Utility.ConstantStrings.AuthorizationLevel;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class CustomerRepository : Repository<UserMobileData>, ICustomerRepository
    {
        private readonly StripeSettings _stripeSettings;
		private readonly IHttpContextAccessor _context;
        private readonly ILogger<CustomerRepository> _logger;
		public CustomerRepository(DbFactory dbFactory, IHttpContextAccessor context,IOptions<StripeSettings> _stripeSetting, ILogger<CustomerRepository> _logger) : base(dbFactory)
        {
            _stripeSettings = _stripeSetting.Value;
            _context = context;
            this._logger = _logger;
        }

		#region GetCustomerDetails
		/// <summary>
		/// method to GetCustomerDetails
		/// </summary>
		/// <returns></returns>
		public async Task<List<UserMobileData>> GetCustomerDetails()
        {
            try
            {


                var userData = await (

                    from UR in this.DbContextObj().Roles
                    where UR.Name == "Customer"
                    join r in this.DbContextObj().UserRoles on UR.Id equals r.RoleId
                    join u in this.DbContextObj().Users on r.UserId equals u.Id
                    where u.IsDeleted == false && u.IsActive == true
                    orderby u.UpdatedDate descending, u.CreatedDate descending
                    select new UserMobileData
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email??"N/A",
                        MobileNumber = u.PhoneNumber,

                    }).ToListAsync();



                return userData;
            }
            catch (Exception)
            {

                throw;
            }
        }
		#endregion

		#region GetCustomerBasicDetails
		/// <summary>
		/// method to GetCustomerBasicDetails
		/// </summary>
		/// <returns></returns>
		public async Task<UserMobileData> GetCustomerBasicDetails()
        {
			try
			{
				UserMobileData data = new UserMobileData();
				var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
				var record = await this.DbContextObj().Users.Where(s => s.Id == userId).FirstOrDefaultAsync();
                var promoMoney = await this.DbContextObj().TblDigitalWallet?.Where(X => X.UserId!.Equals(userId)).Select(y => y.Balance).FirstOrDefaultAsync()!;
				if (record != null)
				{
					data.Id = record.Id;
					data.MobileNumber = record.PhoneNumber;
					data.Name = record.Name;
					data.Email = record.Email;
                    data.DeviceId = record.DeviceId;
                    data.ProfilePic = string.IsNullOrEmpty(record?.ProfilePhoto) ? "/" + "Images" + "/person.png" : "/" + GlobalResourceFile.ProfilePic + "/" + record!.ProfilePhoto;
                    data.Promotion = promoMoney;

                    return data;
				}
				return data;
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion

		#region GetNearestDriversDetail
		/// <summary>
		/// method to GetNearestDriversDetail
		/// </summary>
		/// <param name="nearestDriver"></param>
		/// <returns></returns>
		public async Task<DriverMobileData> GetNearestDriversDetail(NearestDriverRequest nearestDriver)
        {
            try
            {
                            
                List<DriverMobileData> record = await (from categoryPrice in this.DbContextObj().TblCategoryPrices where categoryPrice.StateId.ToString()!.Equals(nearestDriver.StateId)&&categoryPrice.RideCategoryId.ToString()!.Equals(nearestDriver.CategoryId)
                                    join vehicleDetails in this.DbContextObj().TblVehicleDetails on categoryPrice.RideCategoryId.ToString() equals vehicleDetails.RideCategoryId.ToString()
                                    join drivers in this.DbContextObj().Users on vehicleDetails.UserId equals drivers.Id                          
                                    orderby drivers.Latitude ascending, drivers.Longitude ascending
                                    where drivers.IsDeleted==false && drivers.IsActive==true
                                    //where drivers.Latitude <= nearestDriver.Latitude && drivers.Longitude <= nearestDriver.Longitude
                                    select new DriverMobileData
                                       {
                                       DeviceId = drivers.DeviceId,
                                        Email = drivers.Email,
                                        Id = drivers.Id,
                                        MobileNumber = drivers.PhoneNumber,
                                        Latitude = drivers.Latitude,
                                        Longitude =drivers.Longitude

                                        }).ToListAsync();
                if(record.Count>0)
                {
                    return record[0];
                }

               return record.FirstOrDefault()!;
            }
            catch (Exception)
            {

                throw;
            }
        }
		#endregion

		#region GetCustomerById
		/// <summary>
		/// method to GetCustomerById
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public async Task<UserMobileData> GetCustomerById(string Id)
        {
            try
            {
                UserMobileData data = new UserMobileData();
                var Record = await this.DbContextObj().Users.Where(s => s.Id == Id).FirstOrDefaultAsync();
                if (Record != null)
                {
                    data.Id = Record.Id;
                    data.MobileNumber = Record.PhoneNumber;
                    data.Name = Record.Name;
                    data.Email = Record.Email;
                    return data;
                }
                return new UserMobileData();
            }
            catch (Exception)
            {

                throw;
            }
        }
		#endregion

		#region UpdateCustomerByAdmin
		/// <summary>
		/// method to UpdateCustomerByAdmin
		/// </summary>
		/// <param name="Details"></param>
		/// <returns></returns>
		public async Task<int> UpdateCustomerByAdmin(UserMobileData Details)
        {
            try
            {
                int obj = 0;
                var Record = await this.DbContextObj().Users.Where(s => s.Id == Details.Id).FirstOrDefaultAsync();
                if (Record != null)
                {
                    Record.Name = Details.Name;
                    Record.Email = Details.Email;
                    Record.PhoneNumber = Details.MobileNumber;
                    Record.UpdatedDate = DateTime.UtcNow;
                    this.DbContextObj().Users.Update(Record);
                    obj = await this.DbContextObj().SaveChangesAsync();
                }
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region DeleteCustomerByAdmin
        /// <summary>
        /// method to DeleteCustomerByAdmin
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteCustomerByAdmin(string Id)
        {

            try
            {
                _logger.LogInformation("{0} Inside DeleteCustomerByAdmin method in CustomerRepositoty for id = {1}", DateTime.UtcNow, Id);
                int obj = 0;
                var Record = await this.DbContextObj().Users.Where(s => s.Id == Id).FirstOrDefaultAsync();
                bool isStripeCustumerDeleted = false;
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                if (Record != null)
                {
                    if (Record.StripeCustomerId != null)
                    {
                        try
                        {
                            var service = new CustomerService();
                            var data = await service.DeleteAsync(Record.StripeCustomerId).ConfigureAwait(false);
                            StripeAccountDeleteResponse deleteResponse = JsonConvert.DeserializeObject<StripeAccountDeleteResponse>(data.ToJson())!;
                            if (deleteResponse.Deleted)
                            {
                                isStripeCustumerDeleted = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                    }
                    if (isStripeCustumerDeleted)
                    {
                        Record.PhoneNumber = string.Concat(Id, "@removed.com");
                        Record.PhoneNumberConfirmed = false;
                        Record.EmailConfirmed = false;
                        Record.Email = string.Concat(Id, "@removed.com");
                        Record.NormalizedEmail = string.Concat(Id, "@removed.com");
                        Record.IsLoggedIn = false;
                        Record.IsActive = false;
                        Record.IsDeleted = true;
                        Record.UpdatedDate = DateTime.UtcNow;
                        Record.UpdatedBy = Id;
                        this.DbContextObj().Entry(Record).State = EntityState.Modified;
                        obj = await this.DbContextObj().SaveChangesAsync();
                        return obj;
                    }
                    else
                    {
                        Record.PhoneNumber = string.Concat(Id, "@removed.com");
                        Record.PhoneNumberConfirmed = false;
                        Record.EmailConfirmed = false;
                        Record.Email = string.Concat(Id, "@removed.com");
                        Record.NormalizedEmail = string.Concat(Id, "@removed.com");
                        Record.IsLoggedIn = false;
                        Record.IsActive = false;
                        Record.IsDeleted = true;
                        Record.UpdatedDate = DateTime.UtcNow;
                        Record.UpdatedBy = Id;
                        this.DbContextObj().Entry(Record).State = EntityState.Modified;
                        obj = await this.DbContextObj().SaveChangesAsync();
                        return obj;
                    }

                    //Record.IsDeleted = true;
                    //Record.IsActive = false;
                    //this.DbContextObj().Entry(Record).State = EntityState.Modified;
                    //obj = await this.DbContextObj().SaveChangesAsync();
                }
                return obj;
             
            }
            catch (Exception ex)
            {
                _logger.LogInformation("{0} Inside DeleteCustomerByAdmin method in CustomerRepositoty Errors : {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

    }
}
