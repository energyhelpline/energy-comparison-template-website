using System.Threading.Tasks;
using BareboneUi.Pages;

namespace BareboneUi.Common
{
    public interface ISaveModel
    {
        Task<IResponse> Save(ISaveable model);
    }
}