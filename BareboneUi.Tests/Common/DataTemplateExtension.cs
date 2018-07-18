using BareboneUi.Common;
using BareboneUi.Pages;

namespace BareboneUi.Tests.Common
{
    public static class DataTemplateExtension
    {
        public static Item GetItem(this DataTemplate dataTemplate, string groupName, string itemName)
        {
            return (Item) new Questions(dataTemplate).GetItem(groupName, itemName);
        }

        public static bool HasItem(this DataTemplate dataTemplate, string groupName, string itemName)
        {
            return new Questions(dataTemplate).HasItem(groupName, itemName);
        }
    }
}