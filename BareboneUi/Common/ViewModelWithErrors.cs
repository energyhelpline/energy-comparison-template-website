using System.Collections.Generic;
using System.Linq;

namespace BareboneUi.Common
{
    public class ViewModelWithErrors
    {
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
}