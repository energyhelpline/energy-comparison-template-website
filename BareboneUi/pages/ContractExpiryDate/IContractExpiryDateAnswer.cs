using System;

namespace BareboneUi.Pages.ContractExpiryDate
{
    public interface IContractExpiryDateAnswer
    {
        DateTime GasContractExpiryDate { get; }
        DateTime ElectricityContractExpiryDate { get; }
    }
}