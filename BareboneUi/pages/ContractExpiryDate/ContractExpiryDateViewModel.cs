using System;

namespace BareboneUi.Pages.ContractExpiryDate
{
    public class ContractExpiryDateViewModel : IContractExpiryDateAnswer
    {
        public string ContractExpiryDateUri { get; set; }
        public DateTime GasContractExpiryDate { get; set; }
        public DateTime ElectricityContractExpiryDate { get; set; }
    }
}