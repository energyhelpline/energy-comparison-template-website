using BareboneUi.Common;
using Microsoft.Extensions.Configuration;

namespace BareboneUi.Pages.Home
{
    public class StartSwitchModel : Model
    {
        private readonly string _apiKey;
        private readonly string _partnerReference;

        public StartSwitchModel(Resource resource, IConfiguration configuration)
            :base(resource)
        {
            _apiKey = configuration.GetValue<string>("ApiKey");
            _partnerReference = configuration.GetValue<string>("ApiPartnerReference");
        }

        private Question Postcode => Questions["supplyPostcode", "postcode"];
        private Question PartnerReference => Questions["references", "partnerReference"];
        private Question ApiKey => Questions["references", "apiKey"];

        public void Update(string postcode)
        {
            Postcode.SetAnswer(postcode);
            PartnerReference.SetAnswer(_partnerReference);
            ApiKey.SetAnswer(_apiKey);
        }
    }
}