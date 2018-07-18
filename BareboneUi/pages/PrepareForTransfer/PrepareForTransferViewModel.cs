using BareboneUi.Common;

namespace BareboneUi.Pages.PrepareForTransfer
{
    public class PrepareForTransferViewModel : ViewModelWithErrors, IPrepareForTransferAnswer
    {
        public string PrepareForTransferUri { get; set; }
        public string ThankYouUri { get; set; }
        public string CallbackUri { get; set; }
    }
}