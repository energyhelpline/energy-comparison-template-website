using BareboneUi.Common;
using System.Collections.Generic;

namespace BareboneUi.Pages.CurrentSupply
{
    public class CurrentSupplyViewModel : ViewModelWithErrors, ICurrentSupplyAnswer
    {
        public bool IsGasComparison { get; set; }
        public bool IsElectricityComparison { get; set; }
        public bool Economy7 { get; set; }
        public string CurrentSupplyUrl { get; set; }
        public string GasSupplier { get; set; }
        public string GasSupplierTariff { get; set; }
        public string GasSupplierPaymentMethod { get; set; }
        public string ElectricitySupplier { get; set; }
        public string ElectricitySupplierTariff { get; set; }
        public string ElectricitySupplierPaymentMethod { get; set; }
        public string CurrentSupplies { get; set; }
    }
}
