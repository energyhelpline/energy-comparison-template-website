using System.Collections.Generic;
using BareboneUi.Common;

namespace BareboneUi.Pages.Preferences
{
    public class PreferencesViewModel : IPreferencesAnswer
    {
        public string PreferencesUri { get; set; }
        public string TariffFilterOption { get; set; }
        public string ResultsOrder { get; set; }
        public string PaymentMethod { get; set; }

        public IEnumerable<KeyPair> TariffFilterOptions { get; set; }
        public IEnumerable<KeyPair> ResultsOrders { get; set; }
        public IEnumerable<KeyPair> PaymentMethods { get; set; }
    }
}
