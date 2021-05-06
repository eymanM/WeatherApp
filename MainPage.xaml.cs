using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeatherApp
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<string> CitiesList { get; set; } = new();

        public MainPage()
        {
            this.InitializeComponent();

            CitiesListUI.ItemsSource = CitiesList;
            MapService.ServiceToken = new ResourceLoader().GetString("mapToken");

        }

        private void CityInput_Enter(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var city = CityInput.Text;
                var data = new DataProvider(city);
                if (data.ResponseIsNull) return;

                if (!CitiesList.Contains(city)) SaveCity(city);

                SetData(data);
            }
        }

        private async void SaveCity(string city)
        {
            await App.Database.SaveCityAsync(new City() { Name = city });
            CitiesList.Add(city);
        }

        private void SetData(DataProvider data)
        {
            TemperatureUI.Text = $"{data.GetTemp()} \u00B0C";
            FeelTemperatureUI.Text = $"{data.GetFeelsLikeTemp()} \u00B0C";
            PressureUI.Text = data.GetPressure() + " hPa";
            WindSpeedUI.Text = data.GetWindSpeed() + " m/s";
            SunriseUI.Text = (data.GetSunrise().Hour + " : " + data.GetSunrise().Minute).ToString() + " PL Time";
            SunsetUI.Text = (data.GetSunset().Hour + " : " + data.GetSunset().Minute).ToString() + " PL Time";

            WeatherIco.Source = data.GetIcoSource();
            CityCountryUI.Text = data.GetCity() + "  " + data.GetCountry();
            DescUI.Text = char.ToUpper(data.GetDesc()[0]).ToString() + data.GetDesc().Substring(1);
        }

        private async void CitiesListUILoading(object sender, RoutedEventArgs e)
        {
            var cities = await App.Database.GetCitiesAsync();

            foreach (City city in cities) CitiesList.Add(city.Name);
        }

        private void CitiesListUITapped(object sender, TappedRoutedEventArgs e)
        {
            if (CitiesListUI.SelectedItem is string item)
            {
                var data = new DataProvider(item);
                SetData(data);
                CityInput.Text = item;
                CitiesListUI.SelectedIndex = -1; //deselect
            }
        }

        private void ClearCitiesBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Database.DeleteAllCitiesAsync();
            CitiesList.Clear();
        }

        private async void MyLocationWeather_Click(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            string city = null;

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    city = await Position.GetPositionNameAsync();
                    break;

                case GeolocationAccessStatus.Denied:
                    NotificationHandler.LocationDeniedNotif();
                    break;
            }

            if (city is null) return;

            var data = new DataProvider(city);
            if (data.ResponseIsNull) return;

            SetData(data);
            CityInput.Text = string.Empty;
        }
    }
}
