using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.CurrentSupply
{
    public class Supplier : INamedRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DefaultSupplierTariff DefaultSupplierTariff { get; set; }
        public List<SupplierTariff> SupplierTariffs { get; set; }
    }
}