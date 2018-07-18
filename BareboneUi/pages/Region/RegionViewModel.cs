using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.Region
{
    public class RegionViewModel: IRegionAnswer
    {
        public string RegionUri { get; set; }
        public string RegionId { get; set; }
        public IEnumerable<KeyPair> Regions { get; set; }
    }
}