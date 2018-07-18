using System.Collections.Generic;
using BareboneUi.Pages;

namespace BareboneUi.Common
{
    public class Response : IResponse
    {
        private readonly Links _links;

        public Response(Resource resource)
        {
            _links = new Links(resource.Links);
            Errors = resource.Errors;
        }

        public string GetNextUrl() => _links["/rels/next"];

        public string SwitchUrl => _links["/rels/domestic/switch"];

        public bool ContainsRel(string rel) => _links.ContainRel(rel);

        public IEnumerable<Error> Errors { get; }
    }
}