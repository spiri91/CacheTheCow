using CustomObjs;
using DataObjects;
using Obligations;
using System.Threading.Tasks;

namespace Acceptance.wrappers
{
    public class HttpServiceWrapper : IMakeHttpRequest
    {
        public int numberOfTimesGetOrGetAsyncIsCalled { get; private set; }
        public int numberOfTimesTheImageUrlsAreCalled { get; private set; }

        private IMakeHttpRequest httpService;

        public HttpServiceWrapper()
        {
            numberOfTimesGetOrGetAsyncIsCalled = numberOfTimesTheImageUrlsAreCalled = 0;
            httpService = new Dealer().GiveMeA<IMakeHttpRequest>();
        }

        public HttpResponse Get(string url)
        {
            numberOfTimesGetOrGetAsyncIsCalled++;
            return httpService.Get(url);
        }

        public Task<HttpResponse> GetAsync(string url)
        {
            numberOfTimesGetOrGetAsyncIsCalled++;
            return httpService.GetAsync(url);
        }

        public Task<string> GetImageAsBase64url(string url)
        {
            numberOfTimesTheImageUrlsAreCalled++;

            return httpService.GetImageAsBase64url(url);
        }

        public void ResetNumberOfTimesTheImageUrlsAreCalled() => numberOfTimesTheImageUrlsAreCalled = 0;
    }
}
