using BareboneUi.Common;

namespace BareboneUi.Pages.PrepareForTransfer
{
    public class PrepareForTransferModel : Model
    {
        public PrepareForTransferModel(Resource resource) : base(resource) { }

        public void Update(IPrepareForTransferAnswer answer)
        {
            Questions["returnURLs", "thankYou"].SetAnswer(answer.ThankYouUri);
            Questions["returnURLs", "callback"].SetAnswer(answer.CallbackUri);
        }
    }
}