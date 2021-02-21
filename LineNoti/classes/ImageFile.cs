using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LineNoti
{
    public static class ImageFile
    {
        public static StreamContent ToStreamContent(this string imagename, string contentType = "image/jpeg")
        {
            byte[] bytesImage = ImageToByteArray(imagename);
            var fileContent = new StreamContent(new MemoryStream(bytesImage))
            {
                Headers =
                {
                   ContentLength = bytesImage.Length,
                   ContentType = new MediaTypeHeaderValue(contentType)
                }
            };
            return fileContent;
        }
        static byte[] ImageToByteArray(String path)
        {
            return File.ReadAllBytes(path);
        }
    }
}