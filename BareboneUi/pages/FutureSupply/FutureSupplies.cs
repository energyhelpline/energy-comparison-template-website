using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.FutureSupply
{
    public class FutureSupplies
    {
        public IEnumerable<KeyPair> PaymentMethods { get; set; }
        public IEnumerable<Result> Results { get; set; }
        public IEnumerable<KeyPair> SupplyTypes { get; set; }
    }
}