using System.Threading.Tasks;

namespace BareboneUi.Common
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string url);
        Task<string> GetAsync(string url);
        Task<Resource> SendAsync(Resource resource);
    }
}