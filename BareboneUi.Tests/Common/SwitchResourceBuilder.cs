using System;
using System.Collections.Generic;
using System.Linq;
using BareboneUi.Common;
using BareboneUi.Pages.Switch;

namespace BareboneUi.Tests.Common
{
    public class SwitchResourceBuilder
    {
        private List<Item> _gasUsageItems;
        private List<Item> _electricityUsageItems;

        private IEnumerable<Link> _links;

        private string _postCode;
        private string _electricityPaymentMethod;
        private string _electrcitySupplierName;
        private string _electrcitySupplierTariff;
        private string _electricityUsageProfileUsageTypeId;
        private string _electricityContractExpiryDate;
        private bool _economy7;

        private string _gasPaymentMethod;
        private string _gasSupplierName;
        private string _gasSupplierTariff;
        private string _gasUsageProfileUsageTypeId;
        private string _gasContractExpiryDate;

        public SwitchResourceBuilder WithDefaultGasDetailedEstimateItems()
        {
            _gasUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "houseType"
                },
                new Item
                {
                    Name = "numberOfBedrooms"
                },
                new Item
                {
                    Name = "mainCookingSource"
                },
                new Item
                {
                    Name = "cookingFrequency"
                },
                new Item
                {
                    Name = "centralHeating"
                },
                new Item
                {
                    Name = "numberOfOccupants"
                },
                new Item
                {
                    Name = "insulation"
                },
                new Item
                {
                    Name = "energyUsage"
                }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultElectricityDetailedEstimateItems()
        {
            _electricityUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "houseType"
                },
                new Item
                {
                    Name = "numberOfBedrooms"
                },
                new Item
                {
                    Name = "mainCookingSource"
                },
                new Item
                {
                    Name = "cookingFrequency"
                },
                new Item
                {
                    Name = "centralHeating"
                },
                new Item
                {
                    Name = "numberOfOccupants"
                },
                new Item
                {
                    Name = "insulation"
                },
                new Item
                {
                    Name = "energyUsage"
                }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultGasSimpleEstimateItems()
        {
            _gasUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "simpleEstimate"
                }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultElectricitySimpleEstimateItems()
        {
            _electricityUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "simpleEstimate"
                }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultGasSpendItems()
        {
            _gasUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "usageAsSpend"
,               }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultElectricitySpendItems()
        {
            _electricityUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "usageAsSpend"
,               }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultGasUsageAsKwhItems()
        {
            _gasUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "usageAsKWh"
                }
            };

            return this;
        }

        public SwitchResourceBuilder WithDefaultElectricityUsageAsKwhItems()
        {
            _electricityUsageItems = new List<Item>
            {
                new Item
                {
                    Name = "usageAsKWh"
                }
            };

            return this;
        }

        public SwitchResourceBuilder WithLinks(IEnumerable<Link> links)
        {
            _links = links;

            return this;
        }

        public SwitchResourceBuilder WithPostCode(string postcode)
        {
            _postCode = postcode;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyElectricityContractExpiryDate(string jsonDate)
        {
            _electricityContractExpiryDate = jsonDate;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyElectricityEconomy7(bool eco7)
        {
            _economy7 = eco7;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyElectricityPaymentMethodName(string theElecPaymentMethod)
        {
            _electricityPaymentMethod = theElecPaymentMethod;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyElectricitySupplierName(string electrcitySupplierName)
        {
            _electrcitySupplierName = electrcitySupplierName;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyElectricitySupplierTariffName(string electrcitySupplierTariff)
        {
            _electrcitySupplierTariff = electrcitySupplierTariff;

            return this;
        }

        public SwitchResourceBuilder WithCurrentUsageElecUsageProfileUsageTypeid(string usageProfileUsageTypeId)
        {
            _electricityUsageProfileUsageTypeId = usageProfileUsageTypeId;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyGasContractExpiryDate(string jsonDate)
        {
            _gasContractExpiryDate = jsonDate;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyGasPaymentMethodName(string gasPaymentMethod)
        {
            _gasPaymentMethod = gasPaymentMethod;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyGasSupplierName(string gasSupplierName)
        {
            _gasSupplierName = gasSupplierName;

            return this;
        }

        public SwitchResourceBuilder WithCurrentSupplyGasSupplierTariffName(string gasSupplierTariff)
        {
            _gasSupplierTariff = gasSupplierTariff;

            return this;
        }

        public SwitchResourceBuilder WithCurrentUsageGasUsageProfileUsageTypeid(string usageProfileUsageTypeId)
        {
            _gasUsageProfileUsageTypeId = usageProfileUsageTypeId;

            return this;
        }

        public SwitchResourceBuilder WithGasUsageItem(Func<Item, bool> usageItemSelector, string data)
        {
            _gasUsageItems.Single(usageItemSelector).Data = data;

            return this;
        }

        public SwitchResourceBuilder WithElectricityUsageItem(Func<Item, bool> usageItemSelector, string data)
        {
            _electricityUsageItems.Single(usageItemSelector).Data = data;

            return this;
        }

        public SwitchResource Build()
        {
            return new SwitchResource
            {
                SupplyLocation = new SupplyLocation
                {
                    SupplyPostcode = _postCode
                },
                CurrentSupply = new CurrentSupply
                {
                    Gas = new Gas
                    {
                        Supplier = new KeyPair { Name = _gasSupplierName },
                        SupplierTariff = new KeyPair { Name = _gasSupplierTariff },
                        PaymentMethod = new KeyPair { Name = _gasPaymentMethod },
                        ContractExpiryDate = _gasContractExpiryDate
                    },
                    Electricity = new Electricity
                    {
                        Supplier = new KeyPair { Name = _electrcitySupplierName },
                        SupplierTariff = new KeyPair { Name = _electrcitySupplierTariff },
                        PaymentMethod = new KeyPair { Name = _electricityPaymentMethod },
                        ContractExpiryDate = _electricityContractExpiryDate,
                        Economy7 = _economy7
                    }
                },
                CurrentUsage = new CurrentUsage
                {
                    Gas = new Utility
                    {
                        UsageProfile = new UsageProfile
                        {
                            Usage = new Usage { items = _gasUsageItems },
                            UsageType = new UsageType { id = _gasUsageProfileUsageTypeId }
                        }
                    },
                    Elec = new Utility
                    {
                        UsageProfile = new UsageProfile
                        {
                            Usage = new Usage { items = _electricityUsageItems },
                            UsageType = new UsageType { id = _electricityUsageProfileUsageTypeId }
                        }
                    }
                },
                Links = _links
            };
        }
    }
}