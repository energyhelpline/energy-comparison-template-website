using System.Collections.Generic;

namespace BareboneUi.Common
{
    public interface IResponse
    {
        string GetNextUrl();
        string SwitchUrl { get; }
        bool ContainsRel(string rel);
        IEnumerable<Error> Errors { get; }
    }
}