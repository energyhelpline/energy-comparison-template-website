using BareboneUi.Common;

namespace BareboneUi.Pages.CurrentSupply
{
    public class CurrentSupplyModel : Model
    {
        public CurrentSupplyModel(Resource resource)
            : base(resource)
        {}

        public string GetCurrentSuppliesUri() => LinkedData["/rels/domestic/current-supplies"];

        public void Update(ICurrentSupplyAnswer answer)
        {
            IsGasComparison.SetAnswer(answer.IsGasComparison);

            if (answer.IsGasComparison)
            {
                GasSupplier.SetAnswer(answer.GasSupplier);
                GasSupplierTariff.SetAnswer(answer.GasSupplierTariff);
                GasSupplierPaymentMethod.SetAnswer(answer.GasSupplierPaymentMethod);
            }

            IsElectricityComparison.SetAnswer(answer.IsElectricityComparison);

            if (answer.IsElectricityComparison)
            {
                ElectricitySupplier.SetAnswer(answer.ElectricitySupplier);
                ElectricitySupplierTariff.SetAnswer(answer.ElectricitySupplierTariff);
                ElectricitySupplierPaymentMethod.SetAnswer(answer.ElectricitySupplierPaymentMethod);
                Economy7.SetAnswer(answer.Economy7);
            }
        }

        private Question IsGasComparison => Questions["includedFuels", "compareGas"];

        private Question GasSupplier => Questions["gasTariff", "supplier"];

        private Question GasSupplierTariff => Questions["gasTariff", "supplierTariff"];

        private Question GasSupplierPaymentMethod => Questions["gasTariff", "paymentMethod"];

        private Question IsElectricityComparison => Questions["includedFuels", "compareElec"];

        private Question ElectricitySupplier => Questions["elecTariff", "supplier"];

        private Question ElectricitySupplierTariff => Questions["elecTariff", "supplierTariff"];

        private Question ElectricitySupplierPaymentMethod => Questions["elecTariff", "paymentMethod"];

        private Question Economy7 => Questions["elecTariff", "economy7"];
    }
}