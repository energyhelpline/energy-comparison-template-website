using BareboneUi.Common;

namespace BareboneUi.Pages.Switch
{
    public class Electricity
    {
        public KeyPair PaymentMethod { get; set; }
        public KeyPair Supplier { get; set; }
        public KeyPair SupplierTariff { get; set; }
        public string ContractExpiryDate { get; set; }
        public bool? Economy7 { get; set; }
    }
}