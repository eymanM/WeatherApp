using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherApp.Data
{
    public class CitiesDataBase
    {
        readonly SQLiteAsyncConnection database;

        public CitiesDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<City>().Wait();
        }

        public Task<List<City>> GetCitiesAsync()
        {
            //Get all cities.
            return database.Table<City>().ToListAsync();
        }

        public Task<int> SaveCityAsync(City city)
        {
            //if (city.Id != 0)
            //{
            //    // Update an existing city.
            //    return database.UpdateAsync(city);
            //}
            //else
            //{
            //    // Save a new city.
            //    return database.InsertAsync(city);
            //}

            return database.InsertAsync(city);
        }

        public Task<int> DeleteAllCitiesAsync()
        {
            // Delete all.
            return database.DeleteAllAsync<City>();
        }
    }
}
