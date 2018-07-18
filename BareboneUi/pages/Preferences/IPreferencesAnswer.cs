namespace BareboneUi.Pages.Preferences
{
    public interface IPreferencesAnswer
    {
        string TariffFilterOption { get; }
        string ResultsOrder { get; }
        string PaymentMethod { get; }
    }
}