using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BareboneUi.Common
{
    [DataContract]
    public class DataTemplate
    {
        [DataMember(Name = "groups", EmitDefaultValue = false)]
        public IEnumerable<Group> Groups { get; set; }

        [DataMember(Name = "methods", EmitDefaultValue = false)]
        public string[] Methods { get; set; }
    }
}