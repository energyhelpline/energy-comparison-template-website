using BareboneUi.Common;

namespace BareboneUi.Pages.ContractExpiryDate
{
    public class ContractExpiryDateModel : Model
    {
        public ContractExpiryDateModel(Resource resource): base(resource)
        {}

        private Question ElectricityContractExpiryDate => Questions["elecContractDetails", "expiryDate"];
        private Question GasContractExpiryDate => Questions["gasContractDetails", "expiryDate"];

        public void Update(IContractExpiryDateAnswer answer)
        {
            GasContractExpiryDate.SetAnswer(answer.GasContractExpiryDate);
            ElectricityContractExpiryDate.SetAnswer(answer.ElectricityContractExpiryDate);
        }
    }
}