using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CustomObjs;
using DataObjects;
using Obligations;

namespace Units.mocks
{
    public class MockedHttpService : IMakeHttpRequest
    {
        public Func<NotEmptyString, HttpResponse> GetFunc;
        public Func<NotEmptyString, HttpResponse> GetAsyncFunc;
        public Func<NotEmptyString, string> GetImageAsBase64Func;

        public HttpResponse Get(string url)
        {
            return GetFunc(url);
        }

        public Task<HttpResponse> GetAsync(string url)
        {
            return Task.FromResult(GetAsyncFunc(url));
        }

        public Task<string> GetImageAsBase64url(string url)
        {
            return Task.FromResult(GetImageAsBase64Func(url));
        }
    }
}
