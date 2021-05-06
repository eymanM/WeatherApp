using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

namespace WeatherApp
{
    public class Position
    {
        public static async Task<string> GetPositionNameAsync()
        {
            //new Position();
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 20 };
            Geoposition pos = await geolocator.GetGeopositionAsync();

            var myPoint = new Geopoint(new BasicGeoposition()
                { Latitude = pos.Coordinate.Latitude, Longitude = pos.Coordinate.Longitude });

            var result =
                await MapLocationFinder.FindLocationsAtAsync(myPoint);

            return result.Status == MapLocationFinderStatus.Success ? result.Locations[0].Address.Town : null;
        }
    }
}
