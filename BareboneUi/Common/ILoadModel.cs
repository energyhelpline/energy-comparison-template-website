using System.Threading.Tasks;

namespace BareboneUi.Common
{
    public interface ILoadModel
    {
        Task<TModel> Load<TResource, TModel>(string url);
    }
}