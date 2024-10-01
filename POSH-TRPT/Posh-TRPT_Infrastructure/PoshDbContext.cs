
using Posh_TRPT_Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Posh_TRPT_Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Posh_TRPT_Domain.Token;
using System.Runtime.CompilerServices;
using Posh_TRPT_Domain.Register;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Posh_TRPT_Domain.Entity.Posh_TRPT_Domain.StripePayment;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.DashBoard;
using Microsoft.Extensions.Configuration;

namespace Posh_TRPT_Infrastructure
{
    public class PoshDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PoshDbContext(DbContextOptions<PoshDbContext> options, IConfiguration configuration) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
        }
		
		public DbSet<Country> Tbl_Countries { get; set; }
        public DbSet<State> Tbl_States { get; set; }
		public DbSet<StripeCustomers> TblStripeCustomers { get; set; }
        public DbSet<StripeCustomersPaymentIntent> TblStripeCustomersPaymentIntent { get; set; }
        public DbSet<TipsReviews> TblTipsReviews { get; set; }
        public DbSet<DigitalWallet> TblDigitalWallet { get; set; }
        public DbSet<RideBonusHistory> TblRideBonusHistory { get; set; }
        public DbSet<StripeConnectAccountUsers> TblStripeConnectAccountUsers { get; set; }
		public DbSet<StripeCustomersSetupIntent> TblStripeCustomersSetupIntent { get; set; }
	    public DbSet<StripeCustomerIntent> TblStripeCustomersIntent { get; set; }
		public DbSet<Status> TblStatus { get; set; }
		public DbSet<StripeDriverPaymentTransferDetails> TblStripeDriverPaymentTransferDetails { get; set; }
		public DbSet<BookingDetail> TblBookingDetail { get; set; }
		public DbSet<PaymentIntentCapture> TblPaymentIntentCapture { get; set; }
		public DbSet<PaymentIntentConfirm> TblPaymentIntentConfirm { get; set; }
		public DbSet<City> Tbl_Cities { get; set; }
        public DbSet<VehicleDetail> TblVehicleDetails { get; set; }
        public DbSet<DriverDocuments> TblDriverDocuments { get; set; }
        public DbSet<GeneralAddress> TblGeneralAddress { get; set; }
        public DbSet<TokenInfoUser> TblTokenInfoUsers { get; set; }
        public DbSet<MenuMaster> TblMenuMaster { get; set; }
        private void SeedUsers(ModelBuilder builder)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                UserName = "temitope@yopmail.com",
                Name= "Temitope Fabiyi",
                Email = "temitope@yopmail.com",
                NormalizedEmail = "temitope@yopmail.com".ToUpper(),
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                UpdatedBy = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                SecurityStamp = Guid.NewGuid().ToString(),
                StatusId =Guid.Parse("57DEEADB-B1C5-4273-A830-ED8D3B001F70"),
                IsActive = true,
                IsDeleted = false
            };
            PasswordHasher<ApplicationUser> _passwordHasher=new PasswordHasher<ApplicationUser>();
            string password = _passwordHasher.HashPassword(user,"SuperAdmin@007!");
            user.PasswordHash = password;
            builder.Entity<ApplicationUser>().HasData(user);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id= "1c5e174e-3b0e-446f-86af-483d56fd7211",Name="SuperAdmin",ConcurrencyStamp="1",NormalizedName="SUPER ADMIN" },
                new IdentityRole() { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = "Driver", ConcurrencyStamp = "2", NormalizedName = "DRIVER" },
                new IdentityRole() { Id = "3c5e174e-3b0e-446f-86af-483d56fd7213", Name = "Customer", ConcurrencyStamp = "3", NormalizedName = "CUSTOMER" }

                );
        }
        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId= "1c5e174e-3b0e-446f-86af-483d56fd7211",UserId= "8e445865-a24d-4543-a6c6-9443d048cdb9" }
                );
        }
        public DbSet<TokenInfoUser> TblTokenInfos {  get; set; }
        public DbSet<RideCategory> TblRideCategory { get; set; }
        public DbSet<CategoryPrice> TblCategoryPrices { get; set; }
        public DbSet<BookingStatus> TblBookingStatus { get; set; }



        public async Task<List<T>> GetListOfRecordExecuteProcedureAsync<T>(string procedureName, SqlParameter[] sqlParameters, CancellationToken cancellationToken = default) where T : new()
        {
            using (var cmd = this.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    List<T> list = new List<T>();

                    cmd.CommandText = procedureName;
                    switch (cmd.CommandText)
                    {
                        case "Sp_NotifyNearestDriverBooking":
                            cmd.CommandTimeout = 2700;
                            break;
                    }

                    await cmd.Connection!.OpenAsync(cancellationToken);

                    foreach (SqlParameter value in sqlParameters)
                    {
                        cmd.Parameters.Add(value);
                    }

                    var result = await cmd.ExecuteReaderAsync(cancellationToken);

                    while (await result.ReadAsync(cancellationToken))
                    {
                        T obj = Activator.CreateInstance<T>()!;

                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!result.IsDBNull(result.GetOrdinal(prop.Name)))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }

                        list.Add(obj);
                    }

                    return list;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (cmd.Connection!.State == ConnectionState.Open)
                    {
                        await cmd.Connection.CloseAsync();
                    }
                }
            }
        }

        public List<T> GetListOfRecordExecuteProcedure<T>(string procedureName, SqlParameter[] sqlParameters) where T : new()
        {
            using (var cmd = this.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    List<T> list = new List<T>();
                    T obj = default(T)!;

                    {
                        cmd.CommandText = procedureName;
                        switch (cmd.CommandText)
                        {
                            case "Sp_NotifyNearestDriverBooking":
                                {
                                    cmd.CommandTimeout = 2700;
                                    break;
                                }

                        }
                        cmd.Connection!.Open();
                        foreach (SqlParameter value in sqlParameters)
                        {
                            cmd.Parameters.Add(value);
                        }



                        var result = cmd.ExecuteReader();

                        while (result.Read())
                        {
                            obj = Activator.CreateInstance<T>();
                            foreach (PropertyInfo prop in obj!.GetType().GetProperties())
                            {
                                if (!object.Equals(result[prop.Name], DBNull.Value))
                                {
                                    prop.SetValue(obj, result[prop.Name], null);
                                }
                            }
                            list.Add(obj);
                        }
                    }
                    return list;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    cmd.Connection!.Close();
                }
            }
            
        }

        public T GetRecordExecuteProcedure<T>(string procedureName, SqlParameter[] sqlParameters) where T : new()
        {
            var cmd = this.Database.GetDbConnection().CreateCommand();
            try
            {
                T list = new T();
                T obj = default(T)!;

                {
                    cmd.CommandText = procedureName;
                    var procedure= procedureName.Split("@")[0].Trim();
                    switch (procedure)
                    {
                        case "Sp_NotifyNearestDriverBooking":
                        case "Sp_GetDriverInfoAfterBookingStatusUpdate":
                            {
                                cmd.CommandTimeout = 2700;
                                break;
                            }
                        
                    }

                    cmd.Connection!.Open();

                    foreach (SqlParameter value in sqlParameters)
                    {
                        cmd.Parameters.Add(value);
                    }



                    var result = cmd.ExecuteReader();

                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj!.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list=obj;
                    }
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cmd.Connection!.Close();
            }
        }

        public async Task<T> GetRecordExecuteProcedureAsync<T>(string procedureName, SqlParameter[] sqlParameters, CancellationToken cancellationToken = default) where T : new()
        {
            var cmd = this.Database.GetDbConnection().CreateCommand();
            try
            {
                T list = new T();
                T obj = default(T)!;

                {
                    cmd.CommandText = procedureName;
                    var procedure = procedureName.Split("@")[0].Trim();
                    switch (procedure)
                    {
                        case "Sp_NotifyNearestDriverBooking":
                        case "Sp_GetDriverInfoAfterBookingStatusUpdate":
                            {
                                cmd.CommandTimeout = 2700;
                                break;
                            }

                    }

                    await cmd.Connection!.OpenAsync(cancellationToken);


                    foreach (SqlParameter value in sqlParameters)
                    {
                        cmd.Parameters.Add(value);
                    }



                    //var result = cmd.ExecuteReader();

                    using (DbDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                    {

                        while (await reader.ReadAsync(cancellationToken))
                        {

                            obj = Activator.CreateInstance<T>();
                            foreach (PropertyInfo prop in obj!.GetType().GetProperties())
                            {
                                if (!object.Equals(reader[prop.Name], DBNull.Value))
                                {
                                    prop.SetValue(obj, reader[prop.Name], null);
                                }
                            }
                            list = obj;
                        }
                    }

                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await cmd.Connection!.CloseAsync();
            }
        }
        public async Task<List<T>> CallRestAPI<T>(string apiUrl) where T : new()
        {
            using (var client = new HttpClient())
            {

                List<T> list = new List<T>();

                client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                var response = await client.GetAsync(client.BaseAddress + apiUrl);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<APIResponse<List<T>>>(result);
                    
                    if (data!.Data != null)
                    {
                        list = data.Data;
                    }
                    return list;
                }
                return null!;
            }
        }
    }
}