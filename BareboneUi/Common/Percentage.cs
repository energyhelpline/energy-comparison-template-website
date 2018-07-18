using System.Globalization;

namespace BareboneUi.Common
{
    public class Percentage
    {
        private readonly decimal _value;

        public Percentage(decimal value)
        {
            _value = value;
        }

        public Percentage(string value) : this(decimal.Parse(value.Replace("%", ""))) { }

        public override string ToString()
        {
            return (_value / 100m).ToString(CultureInfo.InvariantCulture);
        }
    }
}