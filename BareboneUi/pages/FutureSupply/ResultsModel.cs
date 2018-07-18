using System.Collections.Generic;
using System.Linq;

namespace BareboneUi.Pages.FutureSupply
{
    public class ResultsModel
    {
        private readonly FutureSupplies _data;

        public ResultsModel(FutureSupplies futureSupplies)
        {
            _data = futureSupplies;
        }

        public IEnumerable<EnergySupply> GetDualFuelEnergySupplies()
        {
            return _data.Results.Single(energySupply => energySupply.SupplyType.Id == "4").EnergySupplies;
        }

        public IEnumerable<EnergySupply> GetSingleGasFuelEnergySupplies()
        {
            return _data.Results.Single(x => x.SupplyType.Id == "1").EnergySupplies;
        }

        public Dictionary<string, string> GetPaymentMethods()
        {
            return _data.PaymentMethods.ToDictionary(x => x.Id, x => x.Name);
        }
    }
}