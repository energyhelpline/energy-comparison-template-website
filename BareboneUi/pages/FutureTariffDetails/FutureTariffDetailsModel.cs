using System.Collections.Generic;

namespace BareboneUi.Pages.FutureTariffDetails
{
    public class FutureTariffDetailsModel
    {
        private readonly FutureTariffDetail _resource;

        public FutureTariffDetailsModel(FutureTariffDetail resource)
        {
            _resource = resource;
        }

        public IEnumerable<FutureTariffDetailsSupply> GetSupplies()
        {
            return _resource.Supplies;
        }
    }
}