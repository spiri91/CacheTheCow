using DataObjects;
using Obligations;
using System;
using System.Collections.Generic;

namespace Units.mocks
{
    public class MockedHandleHttpResultService : IHandleResponse
    {
        public Func<HttpResponse, dynamic, IList<MovieDto>> HandleFunc;

        public IList<T> HandleArrayResult<T>(HttpResponse response, dynamic returnObjInCaseOfFail)
        {
            return HandleFunc(response, returnObjInCaseOfFail);
        }
    }
}
