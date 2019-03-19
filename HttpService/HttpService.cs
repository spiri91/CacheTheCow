using CustomObjs;
using DataObjects;
using Obligations;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    public class HttpService : IMakeHttpRequest
    {
        public HttpResponse Get(string url)
        {
            string getRequestResponse = string.Empty;
            HttpResponse response = new HttpResponse(false, getRequestResponse, url);

            if (String.IsNullOrEmpty(url)) return response;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                using (Stream stream = webResponse.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) getRequestResponse = reader.ReadToEnd();

                return new HttpResponse(true, getRequestResponse, url);
            }
            catch (Exception ex)
            {
                // log or retry... 
                return response;
            }
        }

        public async Task<HttpResponse> GetAsync(string url) => await Task.Run(() => Get(url));

        public async Task<string> GetImageAsBase64url(string url)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(url)) return result;

            var client = new HttpClient();

            try
            {
                var bytes = await client.GetByteArrayAsync(url);
                var base64EncodedImage = "image/jpg;base64, " + Convert.ToBase64String(bytes);

               result = base64EncodedImage;
            }
            catch (Exception ex)
            {
                // log or retry
                return String.Empty;
            }
            finally
            {
                client.Dispose();
            }

            return result;
        }
    }
}
