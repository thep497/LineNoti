//References...
//https://www.thaicreate.com/community/asp-net-c-sharp-vb-net-line-notify.html
//https://jackrobotics.me/line-notify-%E0%B8%94%E0%B9%89%E0%B8%A7%E0%B8%A2-c-99099a38e922

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace LineNoti
{
    public class LineNotify
    {
        private const string lineNotiRequestAPI = "https://notify-api.line.me/api/notify";
        private string lineToken { get; set; }

        public LineNotify(string token="")
        {
            if (string.IsNullOrWhiteSpace(token))
                token = "-- place your default token here --";
            lineToken = token;
        }

        public void Notify(string msg)
        {
            string token = lineToken;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(lineNotiRequestAPI);
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
