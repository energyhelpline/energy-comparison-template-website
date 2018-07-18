using System.Collections.Generic;
using System.Runtime.Serialization;
using BareboneUi.Pages;
using Newtonsoft.Json;

namespace BareboneUi.Common
{
    [DataContract]
    public class Resource
    {
        [DataMember(Name = "data-template")]
        public DataTemplate DataTemplate { get; set; }

        [DataMember(Name = "links", EmitDefaultValue = false)]
        public IEnumerable<Link> Links { get; set; }

        [DataMember(Name = "linked-data", EmitDefaultValue = false)]
        public IEnumerable<Link> LinkedData { get; set; }

        [DataMember(Name = "errors", EmitDefaultValue = false)]
        public IEnumerable<Error> Errors { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public string GetUriForRel(string rel) => new Links(Links)[rel];
    }
}