using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BareboneUi.Common
{
    [DataContract]
    public class Item
    {
        [DataMember(Name = "acceptableValues")]
        public IEnumerable<KeyPair> AcceptableValues { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "data")]
        public string Data { get; set; }
    }
}