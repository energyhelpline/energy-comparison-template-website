using System.Collections.Generic;
using BareboneUi.Pages.FutureSupply;
using NUnit.Framework;

namespace BareboneUi.Tests.Pages.FutureSupply
{
    public class ResultsModelTests
    {
        private ResultsModel _sut;
        private List<EnergySupply> _expectedDualFuels;
        private List<EnergySupply> _expectedSingleGas;

        [SetUp]
        public void SetUp()
        {
            _expectedSingleGas = new List<EnergySupply>();
            _expectedDualFuels = new List<EnergySupply>();

            var futureSupplies = new FutureSupplies
            {
                Results = new List<Result>
                {
                    new Result
                    {
                        SupplyType = new SupplyType
                        {
                            Id = "4"
                        },
                        EnergySupplies = _expectedDualFuels
                    },
                    new Result
                    {
                        SupplyType = new SupplyType
                        {
                            Id = "1"
                        },
                        EnergySupplies = _expectedSingleGas
                    }
                }
            };

            _sut = new ResultsModel(futureSupplies);
        }

        [Test]
        public void Returns_dual_fuel_energy_supplies()
        {
            Assert.That(_sut.GetDualFuelEnergySupplies(), Is.SameAs(_expectedDualFuels));
        }

        [Test]
        public void Returns_single_gas_fuel_energy_supplies()
        {
            Assert.That(_sut.GetSingleGasFuelEnergySupplies(), Is.SameAs(_expectedSingleGas));
        }
    }
}