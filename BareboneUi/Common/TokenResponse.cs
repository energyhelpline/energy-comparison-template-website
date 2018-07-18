using System.Runtime.Serialization;

namespace BareboneUi.Common
{
    [DataContract]
    public class TokenResponse
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
    }
}