using System.Collections.Generic;
using BareboneUi.Common;
using BareboneUi.Pages.Home;

namespace BareboneUi.Pages.FutureSupply
{
    public class FutureSupplyViewModel : ViewModelWithErrors
    {
        public string FutureSupplyUri { get; set; }
        public string FutureTariffId { get; set; }

        public IEnumerable<EnergySupply> DualFuelEnergySupplies { get; set; }
        public Dictionary<string, string> PaymentMethods { get; set; }
    }
}