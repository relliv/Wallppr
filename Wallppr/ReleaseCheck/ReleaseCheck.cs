using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Wallppr.ReleaseCheck
{
    public static class ReleaseCheck
    {
        public static string GithubUsername { get; set; }
        public static string GithubRepo { get; set; }
        public static string UserAgentHeader { get; set; }


        public static string GetData(this string uri)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.UserAgent = UserAgentHeader;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static List<Release> GetReleases()
        {
            var apiUrl = $"https://api.github.com/repos/{GithubUsername}/{GithubRepo}/releases";

            Debug.WriteLine(apiUrl);
            var releasesData = apiUrl.GetData();

            if (releasesData != null)
            {
                return Release.FromJson(releasesData);
            }

            return null;
        }
    }
}
