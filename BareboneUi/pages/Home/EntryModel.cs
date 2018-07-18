using BareboneUi.Common;

namespace BareboneUi.Pages.Home
{
    public class EntryModel
    {
        private readonly Links _links;

        public EntryModel(Resource resource)
        {
            _links = new Links(resource.Links);
        }

        public string StartSwitchUrl => _links["/rels/domestic/switches"];
    }
}