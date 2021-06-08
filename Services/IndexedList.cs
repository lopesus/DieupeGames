using System.Collections.Generic;
using System.Linq;

namespace LabirunServer.Services
{
    public class IndexedList
    {

        private class Index : Dictionary<string, List<string>>
        {

            private int indexLength;

            public Index(int indexLength)
            {
                this.indexLength = indexLength;
            }

            public void Add(string value)
            {
                if (value.Length >= indexLength)
                {
                    string key = value.Substring(0, indexLength);
                    List<string> list;
                    if (!this.TryGetValue(key, out list))
                    {
                        Add(key, list = new List<string>());
                    }
                    list.Add(value);
                }
            }

            public IEnumerable<string> Find(string query, int limit)
            {
                return
                    this[query.Substring(0, indexLength)]
                        .Where(s => s.Length > query.Length && s.StartsWith(query))
                        .Take(limit);
            }

        }

        private Index index1;
        private Index index2;
        private Index index4;
        private Index index8;

        public IndexedList(IEnumerable<string> values)
        {
            index1 = new Index(1);
            index2 = new Index(2);
            index4 = new Index(4);
            index8 = new Index(8);
            foreach (string value in values)
            {
                index1.Add(value);
                index2.Add(value);
                index4.Add(value);
                index8.Add(value);
            }
        }

        public IEnumerable<string> Find(string query, int limit)
        {
            if (query.Length >= 8) return index8.Find(query, limit);
            if (query.Length >= 4) return index4.Find(query, limit);
            if (query.Length >= 2) return index2.Find(query, limit);
            return index1.Find(query, limit);
        }

    }
}