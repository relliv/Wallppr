using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Wallppr.Dialogs;
using Wallppr.ViewModel.App;
using static Wallppr.DI.DI;

namespace Wallppr.Helpers
{
    public static class HttpHelpers
    {
        public static string Get(this string uri)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);

                return reader.ReadToEnd();
            }
            catch (Exception exp)
            {
                var dialog = new MessageDialog();
                dialog.ShowDialogWindow(new MessageDialogViewModel(
                    dialog, 
                    ViewModelApplication.LanguageResourceDictionary["Error"].ToString(), 
                    exp.ToString()));

            }

            return null;
        }

        public static async Task Download(this string uri, string fileName)
        {
            using var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            await webClient.DownloadFileTaskAsync(new Uri(uri), fileName).ConfigureAwait(false);
        }
    }
}