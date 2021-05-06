using SQLite;

namespace WeatherApp
{
    public class City
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
