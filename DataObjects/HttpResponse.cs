using CustomObjs;

namespace DataObjects
{
    public class HttpResponse
    {
        public bool isSuccess { get; private set; }

        public string value { get; private set; }

        public string url { get; private set; }

        public HttpResponse(bool isSuccess, string value, string url)
        {
            this.isSuccess = isSuccess;
            this.value = value;
            this.url = url;
        }
    }
}
