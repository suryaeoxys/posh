using Microsoft.EntityFrameworkCore;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.MasterTable;
using Posh_TRPT_Domain.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class MasterTableRepository : Repository<MasterTable>, IMasterTableRepository
    {
        public MasterTableRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

		#region GetCountryList
		/// <summary>
		/// method to get GetCountryList
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<Country>> GetCountryList()
        {
            var countryList = Task.Run(() => this.DbContextObj().Tbl_Countries.ToList());

            IEnumerable<Country> obj = await countryList;


            return obj;
        }
		#endregion

		#region AddCountry
		/// <summary>
		/// method to AddCountry
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> AddCountry(Country model)
        {
            model.CountryCode = string.Empty;
            int obj = 0;
            var countryRecord = await this.DbContextObj().Tbl_Countries.Where(s => s.Name == model.Name || s.Id == model.Id).FirstOrDefaultAsync();

            if (countryRecord != null && countryRecord.Id == model.Id)
            {
                countryRecord.Name = model.Name;
                this.DbContextObj().Update(countryRecord);
                await this.DbContextObj().SaveChangesAsync();
                obj = 2;
            }
            else if (countryRecord != null && countryRecord.Id != model.Id)
            {
                obj = 3;
            }
            else
            {
                await this.DbContextObj().Tbl_Countries.AddAsync(model);
                await this.DbContextObj().SaveChangesAsync();
                obj = 1;
            }
            return obj;
        }
		#endregion

		#region UpdateCountry
		/// <summary>
		/// method to UpdateCountry
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> UpdateCountry(Country model)
        {
            model.CountryCode = string.Empty;
            int obj = 0;
            var countryRecord = await this.DbContextObj().Tbl_Countries.Where(s => s.Id == model.Id).FirstOrDefaultAsync();

            if (countryRecord != null)
            {
                this.DbContextObj().Entry(model).State = EntityState.Modified;
                obj = await this.DbContextObj().SaveChangesAsync();
            }

            return obj;
        }
		#endregion

		#region DeleteCountry
		/// <summary>
		/// method to DeleteCountry
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public async Task<int> DeleteCountry(Guid Id)
        {

            try
            {
                int obj = 0;
                var countryRecord = await this.DbContextObj().Tbl_Countries.Where(s => s.Id == Id).FirstOrDefaultAsync();

                if (countryRecord != null)
                {
                    this.DbContextObj().Remove(countryRecord);
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



		#region GetStateList
		/// <summary>
		/// method to GetStateList
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<State>> GetStateList()
        {
            var countryList = Task.Run(() => (from s in this.DbContextObj().Tbl_States
                                              join c in this.DbContextObj().Tbl_Countries
                                              on s.Country!.Id equals c.Id
                                              select new State { Name = s.Name, Country = c, Id = s.Id }).ToList());
            IEnumerable<State> obj = await countryList;


            return obj;
        }
		#endregion


		#region DeleteState
		/// <summary>
		/// method to DeleteState
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public async Task<int> DeleteState(Guid Id)
        {

            int obj = 0;
            var countryRecord = await this.DbContextObj().Tbl_States.Where(s => s.Id == Id).FirstOrDefaultAsync();

            if (countryRecord != null)
            {
                this.DbContextObj().Remove(countryRecord);
                obj = await this.DbContextObj().SaveChangesAsync();
            }

            return obj;
        }
		#endregion


		#region AddState
		/// <summary>
		/// method to AddState
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> AddState(State model)
        {
            int obj = 0;
            var stateRecord = await this.DbContextObj().Tbl_States.Where(s => s.Name == model.Name || s.Id == model.Id).FirstOrDefaultAsync();

            if (stateRecord != null && stateRecord.Id == model.Id)
            {
                stateRecord.Name = model.Name;
                stateRecord.StateCode = model.StateCode;
                this.DbContextObj().Update(stateRecord);
                await this.DbContextObj().SaveChangesAsync();
                obj = 2;
            }
            else if (stateRecord != null && stateRecord.Id != model.Id)
            {
                obj = 3;
            }
            else
            {
                await this.DbContextObj().Tbl_States.AddAsync(model);
                await this.DbContextObj().SaveChangesAsync();
                obj = 1;
            }
            return obj;

        }
		#endregion

		#region GetStateList
		/// <summary>
		/// method to GetStateList
		/// </summary>
		/// <param name="countryId"></param>
		/// <returns></returns>
		public async Task<IEnumerable<State>> GetStateList(Guid countryId)
        {
            var stateList = Task.Run(() => (from s in this.DbContextObj().Tbl_States where s.Country!.Id == countryId select s).ToList());

            IEnumerable<State> obj = await stateList;
            var d = Convert.ToString(countryId);

            return obj;
        }
		#endregion

		#region GetStateDetailById
		/// <summary>
		/// method to GetStateDetailById
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public async Task<State> GetStateDetailById(Guid stateId)
        {
            var stateList = Task.Run(() => (from s in this.DbContextObj().Tbl_States where s.Id == stateId select s).FirstAsync());

            State obj = await stateList;

            return obj;
        }
		#endregion


		#region GetCityDetailById
		/// <summary>
		/// method to GetCityDetailById
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public async Task<City> GetCityDetailById(Guid stateId)
        {
            var cityList = Task.Run(() => (from s in this.DbContextObj().Tbl_Cities where s.Id == stateId select s).FirstAsync());

            City obj = await cityList;

            return obj;
        }

		#endregion


		#region GetCityByStateId
		/// <summary>
		/// method to GetCityByStateId
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		public async Task<List<City>> GetCityByStateId(Guid stateId)
        {
            var cityList = Task.Run(() => (from s in this.DbContextObj().Tbl_Cities where s.StateId == stateId select s).ToListAsync());

            List<City> obj = await cityList;

            return obj;
        }
		#endregion

		#region DeleteCity
		/// <summary>
		/// method to DeleteCity
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public async Task<int> DeleteCity(Guid Id)
        {

            int obj = 0;
            var countryRecord = await this.DbContextObj().Tbl_Cities.Where(s => s.Id == Id).FirstOrDefaultAsync();

            if (countryRecord != null)
            {
                this.DbContextObj().Remove(countryRecord);
                obj = await this.DbContextObj().SaveChangesAsync();
            }

            return obj;
        }
		#endregion

		#region AddCity

		/// <summary>
		/// method to AddCity
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> AddCity(City model)
        {
            int obj = 0;
            var cityRecord = await this.DbContextObj().Tbl_Cities.Where(s => s.Name == model.Name || s.Id == model.Id).FirstOrDefaultAsync();

            if (cityRecord != null && cityRecord.Id == model.Id)
            {
                cityRecord.Name = model.Name;
                this.DbContextObj().Update(cityRecord);
                await this.DbContextObj().SaveChangesAsync();
                obj = 2;
            }
            else if (cityRecord != null && cityRecord.Id != model.Id)
            {
                obj = 3;
            }
            else
            {
                await this.DbContextObj().Tbl_Cities.AddAsync(model);
                await this.DbContextObj().SaveChangesAsync();
                obj = 1;
            }
            return obj;

        }
		#endregion


		#region GetCityList
		/// <summary>
		/// method to GetCityList
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<City>> GetCityList()
        {
            var countryList = Task.Run(() => (from s in this.DbContextObj().Tbl_States
                                              join c in this.DbContextObj().Tbl_Countries
                                              on s.Country!.Id equals c.Id
                                              join city in this.DbContextObj().Tbl_Cities
                                              on s.Id equals city.StateId
                                              select new City { Name = city.Name, State = s, Id = city.Id, }).ToList());
            IEnumerable<City> obj = await countryList;


            return obj;
        }
		#endregion


		#region GetRideCategoryList
		/// <summary>
		/// method to GetRideCategoryList
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<RideCategory>> GetRideCategoryList()
        {

            var countryList = Task.Run(() => this.DbContextObj().TblRideCategory.ToList());

            IEnumerable<RideCategory> obj = await countryList;


            return obj;
        }
		#endregion


		#region AddRideCategory
		/// <summary>
		/// method to AddRideCategory
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> AddRideCategory(RideCategory model)
        {
            int obj = 0;
            var countryRecord = await this.DbContextObj().TblRideCategory.Where(s => s.Name == model.Name || s.Id == model.Id).FirstOrDefaultAsync();

            if (countryRecord != null && countryRecord.Id == model.Id)
            {
                countryRecord.Name = model.Name;
                this.DbContextObj().Update(countryRecord);
                await this.DbContextObj().SaveChangesAsync();
                obj = 2;
            }
            else if (countryRecord != null && countryRecord.Id != model.Id)
            {
                obj = 3;
            }
            else
            {
                await this.DbContextObj().TblRideCategory.AddAsync(model);
                await this.DbContextObj().SaveChangesAsync();
                obj = 1;
            }
            return obj;
        }
        #endregion


        #region MyRegion
        /// <summary>
        /// method to UpdateRideCategory
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<int> UpdateRideCategory(RideCategory country)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteRideCategory(Guid Id)
        {
            int obj = 0;
            var countryRecord = await this.DbContextObj().TblRideCategory.Where(s => s.Id == Id).FirstOrDefaultAsync();

            if (countryRecord != null)
            {
                this.DbContextObj().Remove(countryRecord);
                obj = await this.DbContextObj().SaveChangesAsync();
            }

            return obj;
        }
		#endregion


		#region GetCategoryPriceList
		/// <summary>
		/// method to GetCategoryPriceList
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<CategoryPrice>> GetCategoryPriceList()
        {
            var countryList = Task.Run(() => (from cp in this.DbContextObj().TblCategoryPrices
                                              join s in this.DbContextObj().Tbl_States
                                              on cp.State!.Id equals s.Id
                                              join c in this.DbContextObj().Tbl_Countries
                                              on s.CountryId equals c.Id
                                              join r in this.DbContextObj().TblRideCategory
                                              on cp.RideCategory!.Id equals r.Id
                                              join city in this.DbContextObj().Tbl_Cities
                                              on cp.City!.Id equals city.Id                                             
                                              
                                              select new CategoryPrice
                                              {
                                                  Id = cp.Id,
                                                  BaseFare = cp.BaseFare,
                                                  Cost_Per_Mile = cp.Cost_Per_Mile,
                                                  Cost_Per_Minute = cp.Cost_Per_Minute,
                                                  State = cp.State,
                                                  RideCategory = r,
                                                  City = cp.City,
                                              }).ToList());
            IEnumerable<CategoryPrice> obj = await countryList;
            return obj;
        }
		#endregion

		#region AddCategoryPrice
		/// <summary>
		/// method to AddCategoryPrice
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> AddCategoryPrice(CategoryPrice model)
        {

            int obj = 0;
            var CategoryPriceRecord = await this.DbContextObj().TblCategoryPrices.AsNoTracking().Where(s => (s.RideCategoryId == model.RideCategoryId && s.StateId == model.StateId) || s.Id == model.Id).FirstOrDefaultAsync();

            if (CategoryPriceRecord != null && CategoryPriceRecord.Id == model.Id)
            {
                this.DbContextObj().Update(model);
                await this.DbContextObj().SaveChangesAsync();
                obj = 2;
            }
            else if (CategoryPriceRecord != null && CategoryPriceRecord.StateId == model.StateId && CategoryPriceRecord.RideCategoryId == model.RideCategoryId)
            {
                obj = 3;
            }
            else
            {
          
                await this.DbContextObj().TblCategoryPrices.AddAsync(model);
                await this.DbContextObj().SaveChangesAsync();
                obj = 1;
            }
            return obj;

        }
		#endregion


		#region DeleteCategoryPrice
		/// <summary>
		/// method to DeleteCategoryPrice
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public async Task<int> DeleteCategoryPrice(Guid categoryId)
        {
            int obj = 0;
            var countryRecord = await this.DbContextObj().TblCategoryPrices.Where(s => s.Id == categoryId).FirstOrDefaultAsync();

            if (countryRecord != null)
            {
                this.DbContextObj().Remove(countryRecord);
                obj = await this.DbContextObj().SaveChangesAsync();
            }

            return obj;
        }
		#endregion

		#region GetCategoryPriceDetailById
		/// <summary>
		/// method to GetCategoryPriceDetailById
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public async Task<CategoryPrice> GetCategoryPriceDetailById(Guid categoryId)
        {
            var record = Task.Run(() => (from s in this.DbContextObj().TblCategoryPrices where s.Id == categoryId select s).FirstAsync());

            CategoryPrice obj = await record;

            return obj;
        }
		#endregion

		#region GetStatusList
		/// <summary>
		/// method to GetStatusList
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<Status>> GetStatusList()
        {
            var status = Task.Run(() => this.DbContextObj().TblStatus.ToList());

            IEnumerable<Status> obj = await status;
            return obj;
        } 
        #endregion
    }
}
