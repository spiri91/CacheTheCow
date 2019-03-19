using CustomObjs;
using DataObjects;
using System.Threading.Tasks;

namespace Obligations
{
    public interface IMakeHttpRequest : IService
    {
        HttpResponse Get(string url);

        Task<HttpResponse> GetAsync(string url);

        Task<string> GetImageAsBase64url(string url);
    }
}
