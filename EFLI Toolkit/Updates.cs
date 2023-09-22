using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFLI_Toolkit
{
    class Updates
    {
        // create client, read string with version from my server and then when read
        // it will run the event function "versionReadComplete" and will have the read version
        // it's doing it like this with events so it could run in the background
        // and not freeze the app if server dies or user's internet dies
        public static void CheckForUpdates()
        {
            // using "try" in case connection drops and exception gets thrown
            try
            {
                // enabling TLS as it's required for connection with github
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                WebClient webCl = new WebClient();
                webCl.DownloadStringCompleted += new DownloadStringCompletedEventHandler(VersionReadComplete);
                webCl.DownloadStringAsync(new Uri("https://raw.githubusercontent.com/ru-mii/EFLI-Toolkit/main/VERSION"));
            }
            catch { }
        }

        // this launches when version number is finally read from the server
        static void VersionReadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            // using "try" in case connection drops and exception gets thrown
            try
            {
                // if current version is not the latest version
                if (e.Result != Main.softwareVersion)
                {
                    Main formWrapper = Application.OpenForms["Main"] as Main;
                    formWrapper.Text += " - update available!";
                }
            }
            catch { }
        }
    }
}
