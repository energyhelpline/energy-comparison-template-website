using System.Collections.Generic;
using System.Linq;
using BareboneUi.Common;

namespace BareboneUi.Tests.Common
{
    public class ResourceBuilder
    {
        private DataTemplate _dataTemplate;
        private Dictionary<string, List<Item>> _groups;
        private readonly List<Link> _links = new List<Link>();
        private readonly List<Link> _linkedData = new List<Link>();
        private readonly Dictionary<Item, List<KeyPair>> _acceptableValues = new Dictionary<Item, List<KeyPair>>();
        private readonly List<Error> _errors = new List<Error>();
        private string _method = "PUT";

        public ResourceBuilder WithDataTemplate()
        {
            _dataTemplate = new DataTemplate();
            return this;
        }

        public ResourceBuilder WithGroup(string groupName)
        {
            if (_groups == null)
                _groups = new Dictionary<string, List<Item>>();

            _groups.Add(groupName, new List<Item>());

            return this;
        }

        public ResourceBuilder WithItem(string itemName)
        {
            _groups.Last().Value.Add(new Item{ Name = itemName });
            return this;
        }

        public ResourceBuilder WithData(string value)
        {
            _groups.Last().Value.Last().Data = value;
            return this;
        }

        public ResourceBuilder WithLink(string rel, string uri)
        {
            _links.Add(CreateLink(rel, uri));
            return this;
        }

        public ResourceBuilder WithLinkedData(string rel, string uri)
        {
            _linkedData.Add(CreateLink(rel, uri));
            return this;
        }

        public ResourceBuilder WithAcceptableValue(string id, string name)
        {
            var item = _groups.Last().Value.Last();

            if (!_acceptableValues.ContainsKey(item))
            {
                _acceptableValues[item] = new List<KeyPair>();
            }

            var acceptableValue = new KeyPair { Id = id, Name = name };
            _acceptableValues[item].Add(acceptableValue);

            return this;
        }

        private static Link CreateLink(string rel, string uri)
        {
            return new Link
            {
                Uri = uri,
                Rel = rel
            };
        }

        public ResourceBuilder WithMethod(string method)
        {
            _method = method;
            return this;
        }

        public ResourceBuilder WithError(string messageText)
        {
            _errors.Add(new Error
            {
                Message = new Message
                {
                    Text = messageText
                }
            });

            return this;
        }

        public Resource Build()
        {
            if (_dataTemplate != null)
            {
                _dataTemplate.Groups =
                    _groups?.Select(group => new Group
                    {
                        Name = group.Key,
                        Items = group.Value.Select(AddAcceptableValues).ToList()
                    }).ToList();

                _dataTemplate.Methods = new[] {_method};
            }

            return new Resource
            {
                DataTemplate = _dataTemplate,
                Links = _links,
                LinkedData = _linkedData,
                Errors = _errors
            };
        }

        private Item AddAcceptableValues(Item item)
        {
            if (_acceptableValues.ContainsKey(item))
            {
                item.AcceptableValues = _acceptableValues[item];
            }

            return item;
        }
    }
}