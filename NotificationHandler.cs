using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class NotificationHandler
    {
        public static void LocationDeniedNotif()
        {
            new ToastContentBuilder()
                .AddText("Location denied")
                .AddButton(new ToastButton()
                    .SetContent("Go to Setings")
                    .AddArgument("action", "Settings"))
                .Show();
        }
        
        public static void NotFoundNotif()
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddText("Not found this city")
                .AddText("Please check the name")
                .Show();
        }
        
        public static void NoInternetNotif()
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddText("No internet connection")
                .AddText("Please contact your ISP")
                .Show();
        }
    }
}
