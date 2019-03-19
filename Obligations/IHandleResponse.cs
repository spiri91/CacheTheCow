using DataObjects;
using System.Collections.Generic;

namespace Obligations
{
    public interface IHandleResponse : IService
    {
        IList<T> HandleArrayResult<T>(HttpResponse response, dynamic returnObjInCaseOfFail);
    }
}
