using System.Collections.Generic;
using System.Threading.Tasks;

namespace BareboneUi.Common
{
    public interface IAuthenticate
    {
        Task RenewToken(IDictionary<string, string> headers, string tokenUri = null);
    }
}