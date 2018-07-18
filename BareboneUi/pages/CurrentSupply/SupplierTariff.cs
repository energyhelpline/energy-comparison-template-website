using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.CurrentSupply
{
    public class SupplierTariff : INamedRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
    }
}