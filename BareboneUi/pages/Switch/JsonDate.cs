using System;
using System.Text.RegularExpressions;

namespace BareboneUi.Pages.Switch
{
    public class JsonDate
    {
        private readonly string _contractExpiryDate;

        public JsonDate(string contractExpiryDate)
        {
            _contractExpiryDate = contractExpiryDate;
        }

        public DateTime? ToDateTime()
        {
            if (string.IsNullOrEmpty(_contractExpiryDate)) return null;
            var milliseconds = ExtractMilliseconds(_contractExpiryDate);
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).ToLocalTime().Date;
        }

        private long ExtractMilliseconds(string contractExpiryDate)
        {
            var regex = new Regex(@"/Date\(([0-9]*)");
            var timeInMilliseconds = long.Parse(regex.Match(contractExpiryDate).Groups[1].Value);
            return timeInMilliseconds;
        }
    }
}