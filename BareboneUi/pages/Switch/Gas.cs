using BareboneUi.Common;

namespace BareboneUi.Pages.Switch
{
    public class Gas
    {
        public KeyPair PaymentMethod { get; set; }
        public KeyPair Supplier { get; set; }
        public KeyPair SupplierTariff { get; set; }
        public string ContractExpiryDate { get; set; }
    }
}