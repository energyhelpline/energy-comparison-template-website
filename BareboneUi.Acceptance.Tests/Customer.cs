using System;
using System.Collections.Generic;
using System.Linq;
using BareboneUi.Acceptance.Tests.Common;
using BareboneUi.Acceptance.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace BareboneUi.Acceptance.Tests
{
    public class Customer : IDisposable
    {
        private readonly RemoteWebDriver _driver;

        public Customer()
        {
            _driver = Chrome.CreateDriver();

            _driver.Navigate().GoToUrl(@"http://localhost:61336");
        }

        public void EnterPostcode(string postcode)
        {
             _driver.FindElementById("Postcode").SendKeys(postcode);
        }

        public void SubmitPostcode()
        {
            _driver.FindElementById("submitPostcode").Click();
        }

        public void EnterRegion(string regionId)
        {
            SelectOptionFromDropdownByText("RegionId", regionId);
        }

        public void SubmitRegion()
        {
            _driver.FindElementById("submitRegion").Click();
        }

        public void GasComparison()
        {
            _driver.FindElementById("IsGasComparison").Click();
        }

        public void ElectricityComparison()
        {
            _driver.FindElementById("IsElectricityComparison").Click();
        }

        public void Ecomony7()
        {
            _driver.FindElementById("Economy7").Click();
        }

        public void EnterGasSupplier(CurrentSupplier gasSupplier)
        {
            SelectOptionFromDropdownByText("GasSupplier", gasSupplier.Name);
            SelectOptionFromDropdownByText("GasSupplierTariff", gasSupplier.Tariff);
            SelectOptionFromDropdownByText("GasSupplierPaymentMethod", gasSupplier.PaymentMethod);
        }

        private void SelectOptionFromDropdownByText(string dropdownId, string textToSelect)
        {
            var dropdown = _driver.FindElementById(dropdownId);
            var selectElement = new SelectElement(dropdown);
            selectElement.SelectByText(textToSelect);
        }

        public void EnterElectricitySupplier(CurrentSupplier electricitySupplier)
        {
            SelectOptionFromDropdownByText("ElectricitySupplier", electricitySupplier.Name);
            SelectOptionFromDropdownByText("ElectricitySupplierTariff", electricitySupplier.Tariff);
            SelectOptionFromDropdownByText("ElectricitySupplierPaymentMethod", electricitySupplier.PaymentMethod);
        }

        public void NightPercentageUsage(string percentage)
        {
            _driver.FindElementById("NightPercentageUsage").SendKeys(percentage);
        }

        public void SubmitCurrentSupplier()
        {
            _driver.FindElementById("submitCurrentSupply").Click();
        }

        public void ContractExpiryDateForGas(string date)
        {
            _driver.FindElementById("GasContractExpiryDate").SendKeys(date);
        }

        public void ContractExpiryDateForElectricity(string date)
        {
            _driver.FindElementById("ElectricityContractExpiryDate").SendKeys(date);
        }

        public void SubmitContractExpiryDate()
        {
            _driver.FindElementById("submitContractExpiryDate").Click();
        }

        public void EnterGasUsageUsingSimpleEstimator(string mediumHouseOrLargeFlat)
        {
            SelectOptionFromDropdownByText("GasUsageSimpleEstimator", mediumHouseOrLargeFlat);
        }

        public void EnterGasSpend(string spend)
        {
            _driver.FindElementById("GasCurrentSpend").SendKeys(spend);
        }

        public void EnterGasUsageAsKwh(string usage)
        {
            _driver.FindElementById("GasCurrentUsageAsKwh").SendKeys(usage);
        }

        public void EnterElectricityUsageUsingSimpleEstimator(string mediumHouseOrLargeFlat)
        {
            SelectOptionFromDropdownByText("ElectricityUsageSimpleEstimator", mediumHouseOrLargeFlat);
        }

        public void EnterElectricitySpend(string spend)
        {
            _driver.FindElementById("ElectricityCurrentSpend").SendKeys(spend);
        }

        public void EnterElectricityUsageAsKwh(string usage)
        {
            _driver.FindElementById("ElectricityCurrentUsageAsKwh").SendKeys(usage);
        }

        public void SelectGasUsageType(string usageType)
        {
            SelectOptionFromRadioByText("GasUsageType", usageType);
        }

        public void SelectElectricityUsageType(string usageType)
        {
            SelectOptionFromRadioByText("ElectricityUsageType", usageType);
        }

        public void EnterUsageUsingDetailedEstimate(DetailedEstimate detailedEstimate)
        {
            SelectOptionFromDropdownByText("HouseType", detailedEstimate.HouseType);
            SelectOptionFromDropdownByText("NumberOfBedrooms", detailedEstimate.NumberOfBedrooms);
            SelectOptionFromDropdownByText("MainCookingSource", detailedEstimate.MainCookingSource);
            SelectOptionFromDropdownByText("CookingFrequency", detailedEstimate.CookingFrequency);
            SelectOptionFromDropdownByText("CentralHeating", detailedEstimate.CentralHeating);
            SelectOptionFromDropdownByText("NumberOfOccupants", detailedEstimate.NumberOfOccupants);
            SelectOptionFromDropdownByText("Insulation", detailedEstimate.Insulation);
            SelectOptionFromDropdownByText("EnergyUsage", detailedEstimate.EnergyUsage);
        }

        public void SubmitCurrentUsage()
        {
            _driver.FindElementById("submitCurrentUsage").Click();
        }

        public void EnterTariffFilterOption(string tariffFilterOption)
        {
            SelectOptionFromDropdownByText("TariffFilterOption", tariffFilterOption);
        }

        public void EnterResultsOrder(string resultsOrder)
        {
            SelectOptionFromDropdownByText("ResultsOrder", resultsOrder);
        }

        public void EnterPaymentMethod(string paymentMethod)
        {
            SelectOptionFromDropdownByText("PaymentMethod", paymentMethod);
        }

        public void SubmitPreferences()
        {
            _driver.FindElementById("submitPreferences").Click();
        }

        public void SelectCheapestDualFuelTariff()
        {
            var row = _driver.FindElementByXPath("//*[@data-can-apply='True']");
            row.FindElement(By.TagName("button")).Click();
        }

        public void SelectFirstDualFuelTariff()
        {
            _driver.FindElementByXPath("//table//tr[1]//button").Click();
        }

        public void SelectCheapestFutureTariffDetails()
        {
            _driver.FindElementByXPath("//table//tr[1]//a").Click();
        }

        public void EnterThankUrl(string thankyouUrl)
        {
            _driver.FindElementById("ThankYouUri").SendKeys(thankyouUrl);
        }

        public void EnterCallbackUrl(string callbackUrl)
        {
            _driver.FindElementById("CallbackUri").SendKeys(callbackUrl);
        }

        public void SubmitPrepareToTransfer()
        {
            _driver.FindElementById("submitPrepareForTransfer").Click();
        }

        public string GetPostCode()
        {
            return _driver.FindElementById("customerPostcode").Text;
        }

        public string GasSupplier()
        {
            return _driver.FindElementById("customerGasSupplyName").Text;
        }

        public string GasSupplierTariff()
        {
            return _driver.FindElementById("customerGasTariff").Text;
        }

        public string GasSupplierPaymentMethod()
        {
            return _driver.FindElementById("customerGasPaymentMethod").Text;
        }

        public string ElectricitySupplier()
        {
            return _driver.FindElementById("customerElectricitySupplyName").Text;
        }

        public string ElectricitySupplierTariff()
        {
            return _driver.FindElementById("customerElectricityTariff").Text;
        }

        public string ElectricitySupplierPaymentMethod()
        {
            return _driver.FindElementById("customerElectricityPaymentMethod").Text;
        }

        public string GetGasUsageForSimpleEstimator()
        {
            return _driver.FindElementById("customerGasUsageSimpleEstimator").Text;
        }

        public string GetGasCurrentSpend()
        {
            return _driver.FindElementById("customerGasCurrentSpend").Text;
        }

        public string GetGasCurrentUsageAsKwh()
        {
            return _driver.FindElementById("customerGasCurrentUsageAsKwh").Text;
        }

        public string GetElectricityUsageForSimpleEstimator()
        {
            return _driver.FindElementById("customerElectricityUsageSimpleEstimator").Text;
        }

        public string GetElectricityCurrentSpend()
        {
            return _driver.FindElementById("customerElectricityCurrentSpend").Text;
        }

        public string GetElectricityCurrentUsageAsKwh()
        {
            return _driver.FindElementById("customerElectricityCurrentUsageAsKwh").Text;
        }

        public string GetGasContractExpiryDate()
        {
            return _driver.FindElementById("customerGasContractExpiryDate").Text;
        }

        public string GetElectricityContractExpiryDate()
        {
            return _driver.FindElementById("customerElectricityContractExpiryDate").Text;
        }

        public string NightPercentUsage()
        {
            return _driver.FindElementById("customerNightPercentUsage").Text;
        }

        public bool ElectricityEcomony7()
        {
            return bool.Parse(_driver.FindElementById("customerElectricityEcomony7").Text);
        }

        public string GetTransferUrl()
        {
            return _driver.FindElementById("customerTransferUrl").Text;
        }

        public string GasHouseType()
        {
            return _driver.FindElementById("customerGasHouseType").Text;
        }

        public string GasNumberOfBedrooms()
        {
            return _driver.FindElementById("customerGasNumberOfBedrooms").Text;
        }

        public string GasMainCookingSource()
        {
            return _driver.FindElementById("customerGasMainCookingSource").Text;
        }

        public string GasCookingFrequency()
        {
            return _driver.FindElementById("customerGasCookingFrequency").Text;
        }

        public string GasCentralHeating()
        {
            return _driver.FindElementById("customerGasCentralHeating").Text;
        }

        public string GasNumberOfOccupants()
        {
            return _driver.FindElementById("customerGasNumberOfOccupants").Text;
        }

        public string GasInsulation()
        {
            return _driver.FindElementById("customerGasInsulation").Text;
        }

        public string GasEnergyUsage()
        {
            return _driver.FindElementById("customerGasEnergyUsage").Text;
        }

        public string ElectricityHouseType()
        {
            return _driver.FindElementById("customerElectricityHouseType").Text;
        }

        public string ElectricityNumberOfBedrooms()
        {
            return _driver.FindElementById("customerElectricityNumberOfBedrooms").Text;
        }

        public string ElectricityMainCookingSource()
        {
            return _driver.FindElementById("customerElectricityMainCookingSource").Text;
        }

        public string ElectricityCookingFrequency()
        {
            return _driver.FindElementById("customerElectricityCookingFrequency").Text;
        }

        public string ElectricityCentralHeating()
        {
            return _driver.FindElementById("customerElectricityCentralHeating").Text;
        }

        public string ElectricityNumberOfOccupants()
        {
            return _driver.FindElementById("customerElectricityNumberOfOccupants").Text;
        }

        public string ElectricityInsulation()
        {
            return _driver.FindElementById("customerElectricityInsulation").Text;
        }

        public string ElectricityEnergyUsage()
        {
            return _driver.FindElementById("customerElectricityEnergyUsage").Text;
        }

        public IEnumerable<string> GetErrors()
        {
            return _driver.FindElementById("errors").FindElements(By.Id("error")).Select(ele => ele.Text);
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        public bool HasElectricityTariffDetails()
        {
            var row = _driver.FindElementByXPath("//*[@data-fuel='electricity']");
            return row != null;
        }

        public bool HasGasTariffDetails()
        {
            var row = _driver.FindElementByXPath("//*[@data-fuel='gas']");
            return row != null;
        }

        private void SelectOptionFromRadioByText(string radioGroupName, string valueToSelect)
        {
            var radioGroup = _driver.FindElementsByName(radioGroupName);
            var radioButton = radioGroup.Single(element => element.GetAttribute("value") == valueToSelect);
            radioButton.Click();
        }
    }
}