using System;
using BareboneUi.Common;

namespace BareboneUi.Pages.FutureTariffDetails
{
    public class FutureTariffDetailsSupply
    {
        public string Fuel { get; set; }
        public FutureTariffDetailsSupplier Supplier { get; set; }
        public FutureTariffDetailsSupplierTariff SupplierTariff { get; set; }
    }

    public class FutureTariffDetailsSupplierTariff
    {
        public string Name { get; set; }
        public string StandingCharge { get; set; }
        public string TariffType { get; set; }
        public KeyPair PaymentMethod { get; set; }
        public string UnitCharge { get; set; }
        public DateTime TariffEndDate { get; set; }
        public DateTime PriceGuaranteedUntil { get; set; }
        public decimal ExitFees { get; set; }
        public string Discounts { get; set; }
        public string OtherServices { get; set; }
    }

    public class FutureTariffDetailsSupplier
    {
        public string Description { get; set; }
        public FutureTariffDetailsSupplierEnvironmental Environmental { get; set; }
        public string Name { get; set; }
    }

    public class FutureTariffDetailsSupplierEnvironmental
    {
        public decimal CO2EmissionPerKwh { get; set; }
        public decimal NuclearWastePerKwh { get; set; }
    }
}