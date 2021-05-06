using Microsoft.Toolkit.Uwp.Notifications;
using System.Net;
using System.Net.NetworkInformation;

namespace WeatherApp
{
    public class ExceptionsHandler
    {
        public static void WebExceptionHandler(WebException exception)
        {
            var response = exception.Response as HttpWebResponse;
            if (response?.StatusCode == HttpStatusCode.NotFound)
                NotificationHandler.NotFoundNotif();

            if (exception.Status == WebExceptionStatus.NameResolutionFailure)
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    NotificationHandler.NoInternetNotif();
            }
        }
    }
}
