using System;
using System.Collections.Generic;
using System.Linq;
using BareboneUi.Common;

namespace BareboneUi.Pages
{
    public class Questions
    {
        private readonly DataTemplate _dataTemplate;

        public Questions(DataTemplate dataTemplate)
        {
            _dataTemplate = dataTemplate;
        }

        public Question this[string group, string item] => GetItem(group, item);

        public Question GetItem(string groupName, string itemName)
        {
            var group = FindGroup(groupName);

            if (group == null)
            {
                throw new KeyNotFoundException($"Could not find a group named [{groupName}] in the data-template");
            }

            var item = FindItem(group, itemName);

            if (item == null)
            {
                throw new KeyNotFoundException($"Could not find an item named [{itemName}] in group [{groupName} in the data-template");
            }

            return new Question(item);
        }

        public bool HasItem(string groupName, string itemName)
        {
            var group = FindGroup(groupName);
            var item = FindItem(group, itemName);

            return group != null && item != null;
        }

        private Group FindGroup(string name)
        {
            return _dataTemplate.Groups?.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private Item FindItem(Group group, string name)
        {
            return group?.Items?.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}