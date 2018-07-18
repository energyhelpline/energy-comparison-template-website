using System;
using System.Collections.Generic;
using System.Linq;
using BareboneUi.Common;

namespace BareboneUi.Pages
{
    public class Links
    {
        private readonly IEnumerable<LinkLookup> _links;

        public Links(IEnumerable<Link> links)
        {
            _links = links?.Select(link => new LinkLookup(link)) ?? Enumerable.Empty<LinkLookup>();
        }

        public string this[string rel] => GetUriForRel(rel);

        public bool ContainRel(string rel) => !MatchedLink(rel).IsEmpty;

        private string GetUriForRel(string rel)
        {
            var matchedLink = MatchedLink(rel);

            if (matchedLink.IsEmpty)
            {
                throw new KeyNotFoundException($"Cannot find a rel named [{rel}]");
            }

            return matchedLink.Uri;
        }

        private LinkLookup MatchedLink(string rel) => _links.SingleOrDefault(x => x.Rels.Contains(rel));

        private struct LinkLookup
        {
            public LinkLookup(Link link)
            {
                Rels = link.Rel.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Uri = link.Uri;
            }

            public IEnumerable<string> Rels { get; }
            public string Uri { get; }

            public bool IsEmpty => Rels == null;
        }
    }
}
