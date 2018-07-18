using System.Collections.Generic;
using System.Linq;

namespace BareboneUi.Pages.FutureSupply
{
    public class FutureSuppliesViewModel
    {
        private readonly FutureSupplies _futureSupplies;

        public FutureSuppliesViewModel(FutureSupplies futureSupplies, string url)
        {
            _futureSupplies = futureSupplies;
            FutureSupplyUri = url;
        }

        public string FutureSupplyUri { get; set; }

        public IEnumerable<EnergySupply> DualFuelEnergySupplies => GetDualFuelEnergySupplies();

        public IEnumerable<EnergySupply> SingleGasEnergySupplies => GetSingleGasEnergySupplies();

        private IEnumerable<EnergySupply> GetDualFuelEnergySupplies()
        {
            return _futureSupplies.Results.Single(result => result.SupplyType.Id == "4").EnergySupplies;
        }

        private IEnumerable<EnergySupply> GetSingleGasEnergySupplies()
        {
            return _futureSupplies.Results.Single(result => result.SupplyType.Id == "1").EnergySupplies;
        }

        public string FormatAnnualSavings(decimal expectedAnnualSavings)
        {
            return string.Format($"{expectedAnnualSavings:C}");
        }

        public string GetPaymentMethodDescription(string paymentMethodId)
        {
            return _futureSupplies.PaymentMethods.Single(paymentMethod => paymentMethod.Id == paymentMethodId).Name;
        }
    }
}