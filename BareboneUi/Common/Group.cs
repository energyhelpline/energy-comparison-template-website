using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BareboneUi.Common
{
    [DataContract]
    public class Group
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "items", EmitDefaultValue = false)]
        public IEnumerable<Item> Items { get; set; }
    }
}