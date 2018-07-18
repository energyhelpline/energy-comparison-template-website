using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.Region
{
    public class RegionModel : Model
    {
        public RegionModel(Resource resource): base(resource)
        {}

        private Question RegionId => Questions["electricityRegion", "region"];

        public void Update(IRegionAnswer regionAnswer)
        {
            RegionId.SetAnswer(regionAnswer.RegionId);
        }

        public IEnumerable<KeyPair> GetRegions()
        {
            return RegionId.DropDownValues;
        }
    }
}