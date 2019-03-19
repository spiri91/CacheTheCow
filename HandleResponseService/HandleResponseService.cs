using System;
using DataObjects;
using Obligations;
using System.Collections.Generic;

namespace Services
{
    public class HandleResponseService : IHandleResponse
    {
        public IList<T> HandleArrayResult<T>(HttpResponse response, dynamic returnObjInCaseOfFail)
        {
            if (null == response) return returnObjInCaseOfFail;
            if (false == response.isSuccess) return returnObjInCaseOfFail;

            try
            {
                var movieArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(response.value);

                return movieArray;
            }
            catch (Exception ex)
            {
                // log or retry
                return returnObjInCaseOfFail;
            }
        }
    }
}
