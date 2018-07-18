using System.Collections.Generic;

namespace BareboneUi.Pages.FutureSupply
{
    public class Result
    {
        public IEnumerable<EnergySupply> EnergySupplies { get; set; }
        public string ResultCount { get; set; }
        public SupplyType SupplyType { get; set; }
    }
}