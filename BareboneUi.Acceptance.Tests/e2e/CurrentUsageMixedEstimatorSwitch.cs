using BareboneUi.Acceptance.Tests.Common;
using NUnit.Framework;

namespace BareboneUi.Acceptance.Tests.e2e
{
    [TestFixture]
    public class CurrentUsageMixedEstimatorSwitch
    {
        [Test]
        public void Customer_can_enter_current_usage_by_detailed_estimator_for_one_fuel_and_simple_estimator_for_the_other_fuel()
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

                customer.SelectGasUsageType("2");
                customer.EnterGasUsageUsingSimpleEstimator("Medium (house or large flat)");
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
                Assert.That(customer.GetTransferUrl(),                   Contains.Substring("https://refresh.staging.energyhelpline.com/domestic/energy/signup/"));

                Assert.That(customer.GetGasUsageForSimpleEstimator(),    Is.EqualTo("Medium (house or large flat)"));
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

        [Test]
        public void Customer_can_enter_current_usage_by_simple_estimator_for_one_fuel_and_spend_for_the_other_fuel()
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

                customer.SelectGasUsageType("4");
                customer.EnterGasSpend("999");
                customer.SelectElectricityUsageType("2");
                customer.EnterElectricityUsageUsingSimpleEstimator("Medium (house or large flat)");
                customer.SubmitCurrentUsage();

                customer.EnterTariffFilterOption("All");
                customer.EnterResultsOrder("price");
                customer.EnterPaymentMethod("Monthly Direct Debit");
                customer.SubmitPreferences();

                customer.SelectCheapestDualFuelTariff();

                customer.EnterThankUrl("http://thank.com");
                customer.EnterCallbackUrl("http://callback.com");
                customer.SubmitPrepareToTransfer();

                Assert.That(customer.GetPostCode(),                           Is.EqualTo("SE1 0ES"));
                Assert.That(customer.GasSupplier(),                           Is.EqualTo("British Gas"));
                Assert.That(customer.GasSupplierTariff(),                     Is.EqualTo("Standard"));
                Assert.That(customer.GasSupplierPaymentMethod(),              Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.ElectricitySupplier(),                   Is.EqualTo("EDF Energy"));
                Assert.That(customer.ElectricitySupplierTariff(),             Is.EqualTo("Standard (Variable)"));
                Assert.That(customer.ElectricitySupplierPaymentMethod(),      Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.GetTransferUrl(),                        Contains.Substring("https://refresh.staging.energyhelpline.com/domestic/energy/signup/"));

                Assert.That(customer.GetGasCurrentSpend(),                    Is.EqualTo("999"));
                Assert.That(customer.GetElectricityUsageForSimpleEstimator(), Is.EqualTo("Medium (house or large flat)"));
            }
        }

        [Test]
        public void Customer_can_enter_current_usage_by_spend_for_one_fuel_and_usage_as_kwh_for_the_other_fuel()
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

                customer.SelectGasUsageType("3");
                customer.EnterGasUsageAsKwh("1234");
                customer.SelectElectricityUsageType("4");
                customer.EnterElectricitySpend("555");
                customer.SubmitCurrentUsage();

                customer.EnterTariffFilterOption("All");
                customer.EnterResultsOrder("price");
                customer.EnterPaymentMethod("Monthly Direct Debit");
                customer.SubmitPreferences();

                customer.SelectCheapestDualFuelTariff();

                customer.EnterThankUrl("http://thank.com");
                customer.EnterCallbackUrl("http://callback.com");
                customer.SubmitPrepareToTransfer();

                Assert.That(customer.GetPostCode(),                           Is.EqualTo("SE1 0ES"));
                Assert.That(customer.GasSupplier(),                           Is.EqualTo("British Gas"));
                Assert.That(customer.GasSupplierTariff(),                     Is.EqualTo("Standard"));
                Assert.That(customer.GasSupplierPaymentMethod(),              Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.ElectricitySupplier(),                   Is.EqualTo("EDF Energy"));
                Assert.That(customer.ElectricitySupplierTariff(),             Is.EqualTo("Standard (Variable)"));
                Assert.That(customer.ElectricitySupplierPaymentMethod(),      Is.EqualTo("Pay On Receipt Of Bill"));
                Assert.That(customer.GetTransferUrl(),                        Contains.Substring("https://refresh.staging.energyhelpline.com/domestic/energy/signup/"));

                Assert.That(customer.GetGasCurrentUsageAsKwh(),               Is.EqualTo("1234"));
                Assert.That(customer.GetElectricityCurrentSpend(),            Is.EqualTo("555"));
            }
        }
    }
}