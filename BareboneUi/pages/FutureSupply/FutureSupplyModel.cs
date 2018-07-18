using BareboneUi.Common;

namespace BareboneUi.Pages.FutureSupply
{
    public class FutureSupplyModel: Model
    {
        public FutureSupplyModel(Resource resource)
            :base(resource)
        {}

        public void Update(string tariffId) => Questions["futureSupply", "id"].SetAnswer(tariffId);

        public string ResultsUri => LinkedData["/rels/domestic/future-supplies"];
    }
}