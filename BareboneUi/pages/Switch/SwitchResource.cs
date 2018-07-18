using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.Switch
{
    public class SwitchResource
    {
        public IEnumerable<Link> Links { get; set; }
        public SupplyLocation SupplyLocation { get; set; }
        public CurrentSupply CurrentSupply { get; set; }
        public CurrentUsage CurrentUsage { get; set; }

    }
}