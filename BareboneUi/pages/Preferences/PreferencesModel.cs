using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.Preferences
{
    public class PreferencesModel : Model
    {
        public PreferencesModel(Resource resource) : base(resource) { }

        public void Update(IPreferencesAnswer preferencesAnswers)
        {
            TariffFilterOptions.SetAnswer(preferencesAnswers.TariffFilterOption);
            ResultsOrders.SetAnswer(preferencesAnswers.ResultsOrder);
            PaymentMethods.SetAnswer(preferencesAnswers.PaymentMethod);
        }

        public IEnumerable<KeyPair> GetTariffFilterOptions() => TariffFilterOptions.DropDownValues;
        public IEnumerable<KeyPair> GetResultsOrders() => ResultsOrders.DropDownValues;
        public IEnumerable<KeyPair> GetPaymentMethods() => PaymentMethods.DropDownValues;

        private Question TariffFilterOptions => Questions["tariffFilterOptions", "tariffFilterOption"];
        private Question ResultsOrders => Questions["resultsOrder", "resultsOrder"];
        private Question PaymentMethods => Questions["limitToPaymentType", "paymentMethod"];
    }
}