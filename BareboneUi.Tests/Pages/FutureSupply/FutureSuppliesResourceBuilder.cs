using System.Collections.Generic;
using BareboneUi.Common;
using BareboneUi.Pages.FutureSupply;

namespace BareboneUi.Tests.Pages.FutureSupply
{
    public class FutureSuppliesResourceBuilder
    {
        private readonly List<Result> _results = new List<Result>();

        public FutureSuppliesResourceBuilder WithResult(Result result)
        {
            _results.Add(result);

            return this;
        }

        public FutureSupplies Build()
        {
            return new FutureSupplies
            {
                PaymentMethods = new KeyPair[] { },
                Results = _results
            };
        }
    }
}