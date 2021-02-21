//References...
//https://www.thaicreate.com/community/asp-net-c-sharp-vb-net-line-notify.html
//https://jackrobotics.me/line-notify-%E0%B8%94%E0%B9%89%E0%B8%A7%E0%B8%A2-c-99099a38e922

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LineNoti
{
    public class LineNotify
    {
        private const string lineNotiRequestAPI = "https://notify-api.line.me/api/notify";
        private string lineToken;
        private bool notificationDisabled;

        public string LineToken { get => lineToken; set => lineToken = value; }
        public bool NotificationDisabled { get => notificationDisabled; set => notificationDisabled = value; }

        public LineNotify(string token = "", bool notificationdisabled = false)
        {
            if (string.IsNullOrWhiteSpace(token))
                token = "-- place your default token here --";
            lineToken = token;
            notificationDisabled = notificationdisabled;
        }

        public string SendMessage(string msg)
        {
            return this.Notify(msg);
        }
        public string SendSticker(string msg, int stickerPackageId, int stickerId)
        {
            return this.Notify(msg, null, stickerPackageId, stickerId);
        }
        public string SendPhotoFile(string msg, string imgPath)
        {
            return this.Notify(msg, imgPath);
        }
        public string Notify(string msg, string imgPath = null, int stickerPackageId = 0, int stickerId = 0)
        {
            if (!string.IsNullOrWhiteSpace(imgPath) && !File.Exists(imgPath))
                return string.Format("{0} not existed.", imgPath);

            string token = lineToken;
            try
            {
                //Method 3
                var formDataContent = new MultipartFormDataContent();
                formDataContent.Add(new StringContent(msg), "message");
                if (!string.IsNullOrWhiteSpace(imgPath))
                {
                    var contentType = MimeTypes.GetContentType(imgPath);
                    formDataContent.Add(imgPath.ToStreamContent(contentType), "imageFile", imgPath);
                }
                if ((stickerPackageId > 0) && (stickerId > 0))
                {
                    formDataContent.Add(new StringContent(stickerPackageId.ToString()), "stickerPackageId");
                    formDataContent.Add(new StringContent(stickerId.ToString()), "stickerId");
                }
                if (notificationDisabled)
                {
                    formDataContent.Add(new StringContent("true"), "notificationDisabled");
                }
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, lineNotiRequestAPI);
                    request.Content = formDataContent;
                    request.Headers.Add("Authorization", "Bearer " + token);
                    using (var response = client.SendAsync(request).Result)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        return responseString;
                    }
                }

                //Method 2
                //var dict = new Dictionary<string, string>();
                //dict.Add("message", msg);
                //dict.Add("imageFile", string.Format("@\"{0}\"", imgPath));
                //var request = new HttpRequestMessage(HttpMethod.Post, lineNotiRequestAPI);
                //request.Content = new FormUrlEncodedContent(dict);
                //request.Headers.Add("Authorization", "Bearer " + token);
                //using (var client = new HttpClient())
                //{
                //    var response = client.SendAsync(request).Result;
                //    var responseString = response.Content.ReadAsStringAsync().Result;
                //    return responseString;
                //}

                //Method 1
                //var request = (HttpWebRequest)WebRequest.Create(lineNotiRequestAPI);
                //var messageData = string.Format("message=\n{0}", msg);
                //var imageData = string.Format("imageFile=@{0}", imgPath);
                //var postData = string.Format("{0}&{1}", messageData, imageData);
                //var data = Encoding.UTF8.GetBytes(postData);
                //request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = data.Length;
                //request.Headers.Add("Authorization", "Bearer " + token);
                //using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                //var response = (HttpWebResponse)request.GetResponse();
                //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
