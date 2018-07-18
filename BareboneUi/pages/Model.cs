using BareboneUi.Common;

namespace BareboneUi.Pages
{
    public abstract class Model : ISaveable
    {
        private readonly Resource _resource;

        protected Model(Resource resource)
        {
            _resource = resource;
            Questions = new Questions(_resource.DataTemplate);
            LinkedData = new Links(resource.LinkedData);
        }

        protected Questions Questions { get; }
        protected Links LinkedData { get; }

        Resource ISaveable.Resource => _resource;
    }
}