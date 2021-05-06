#nullable enable
using Newtonsoft.Json;
using System;
using System.Net;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WeatherApp
{
    public class DataProvider
    {
        public bool ResponseIsNull;
        private readonly string _token = new ResourceLoader().GetString("WeatherApiToken");
        private readonly Uri _uri;
        private readonly Rootobject? _data;
        private readonly WebClient _client;

        public DataProvider(string city)
        {
            var uriStr = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_token}";
            _client = new();
            _uri = new(uriStr);
            _data = DownloadData();

            if (_data is null)
                ResponseIsNull = true;
        }

        private Rootobject? DownloadData()
        {
            string? downloadString = default;

            try
            {
                downloadString = _client.DownloadString(_uri);
            }
            catch (WebException exception)
            {
                ExceptionsHandler.WebExceptionHandler(exception);
                return null;
            }

            return JsonConvert.DeserializeObject<Rootobject>(downloadString);
        }

        public ImageSource GetIcoSource()
        {
            string iconStr = _data.weather[0].icon;
            return new BitmapImage(
                new Uri($"http://openweathermap.org/img/wn/{iconStr}@2x.png", UriKind.Absolute));
        }

        public string GetTemp() => _data.main.temp.ToString();

        public string GetFeelsLikeTemp() => _data.main.feels_like.ToString();

        public string GetPressure() => _data.main.pressure.ToString();

        public string GetWindSpeed() => _data.wind.speed.ToString();

        public DateTime GetSunrise() => UnixTimeToDateTime(_data.sys.sunrise);

        public DateTime GetSunset() => UnixTimeToDateTime(_data.sys.sunset);

        public string GetDesc() => _data.weather[0].description;

        public string GetCountry() => _data.sys.country;

        public string GetCity() => _data.name;


        private DateTime UnixTimeToDateTime(double unixTimeStamp)
        {
            var dtDateTime =
                new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
