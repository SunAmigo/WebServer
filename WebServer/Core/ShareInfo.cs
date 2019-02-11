using System.Collections.Generic;

namespace WebServer.Core
{
    public class ShareInfo
    {
        private readonly Dictionary<dynamic, dynamic> _dictionary;

        public ShareInfo()
        {
            _dictionary = new Dictionary<dynamic, dynamic>();
        }

        public dynamic this[object obj]
        {
            get => _dictionary[obj];
            set
            {
                if (!_dictionary.ContainsKey(obj))
                {
                    _dictionary.Add(obj, value);
                }

                _dictionary[obj] = value;
            }
        }
    }
}
