using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class CurrentUsageDetailedEstimatorSwitch
    {
        [Test]
        public void Customer_can_enter_current_usage_by_detailed_estimator()
        {
            using (var customer = new Customer())
            {
                customer.EnterPostcode("SE1 0ES");
                customer.SubmitPostcode();

                customer.GasComparison();
                customer.EnterGasSupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "British Gas",
                        Tariff = "Standard"
                    });
                customer.ElectricityComparison();
                customer.EnterElectricitySupplier(
                    new CurrentSupplier
                    {
                        PaymentMethod = "Pay On Receipt Of Bill",
                        Name = "EDF Energy",
                        Tariff = "Standard (Variable)"
                    });
                customer.SubmitCurrentSupplier();

                customer.SelectGasUsageType("1");
                customer.SelectElectricityUsageType("1");
                customer.EnterUsageUsingDetailedEstimate(
                    new DetailedEstimate
                    {
                        HouseType = "detached bungalow",
                        NumberOfBedrooms = "3",
                        MainCookingSource = "gas",
                        CookingFrequency = "often",
                        CentralHeating = "Standard Electricity",
                        NumberOfOccupants = "3",
                        Insulation = "average",
                        EnergyUsage = "normal user (family house)"
                    });
                customer.SubmitCurrentUsage();

                customer.EnterTariffFilterOption("All");
                customer.EnterResultsOrder("price");
                customer.EnterPaymentMethod("Monthly Direct Debit");
                customer.SubmitPreferences();

                customer.SelectCheapestDualFuelTariff();

                customer.EnterThankUrl("http://thank.com");
                customer.EnterCallbackUrl("http://callback.com");
                customer.SubmitPrepareToTransfer();

                Assert.That(customer.GetPostCode(),                      Is.EqualTo("SE1 0ES"));
                Assert.That(customer.GasSupplier(),                      Is.EqualTo("British Gas"));
                Assert.That(customer.GasSupplierTariff(),                Is.EqualTo("Standard"));
                Assert.That(customer.GasSupplierPaymentMethod(),         Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.ElectricitySupplier(),              Is.EqualTo("EDF Energy"));
                Assert.That(customer.ElectricitySupplierTariff(),        Is.EqualTo("Standard (Variable)"));
                Assert.That(customer.ElectricitySupplierPaymentMethod(), Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(string.IsNullOrEmpty(customer.GetTransferUrl()), Is.False);

                Assert.That(customer.GasHouseType(),                     Is.EqualTo("detached bungalow"));
                Assert.That(customer.GasNumberOfBedrooms(),              Is.EqualTo("3"));
                Assert.That(customer.GasMainCookingSource(),             Is.EqualTo("gas"));
                Assert.That(customer.GasCookingFrequency(),              Is.EqualTo("often"));
                Assert.That(customer.GasCentralHeating(),                Is.EqualTo("Standard Electricity"));
                Assert.That(customer.GasNumberOfOccupants(),             Is.EqualTo("3"));
                Assert.That(customer.GasInsulation(),                    Is.EqualTo("average"));
                Assert.That(customer.GasEnergyUsage(),                   Is.EqualTo("normal user (family house)"));
                Assert.That(customer.ElectricityHouseType(),             Is.EqualTo("detached bungalow"));
                Assert.That(customer.ElectricityNumberOfBedrooms(),      Is.EqualTo("3"));
                Assert.That(customer.ElectricityMainCookingSource(),     Is.EqualTo("gas"));
                Assert.That(customer.ElectricityCookingFrequency(),      Is.EqualTo("often"));
                Assert.That(customer.ElectricityCentralHeating(),        Is.EqualTo("Standard Electricity"));
                Assert.That(customer.ElectricityNumberOfOccupants(),     Is.EqualTo("3"));
                Assert.That(customer.ElectricityInsulation(),            Is.EqualTo("average"));
                Assert.That(customer.ElectricityEnergyUsage(),           Is.EqualTo("normal user (family house)"));
            }
        }
    }
}