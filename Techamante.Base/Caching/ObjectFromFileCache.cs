using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Techamante.Base;
using System.Xml.Serialization;

namespace Techamante.Base.Caching
{
    public class ObjectFromFileCache : Singleton<ObjectFromFileCache>
    {
        //TODO: Think about move cache to the Application Cache in ASP.NET
        private readonly ConcurrentDictionary<string, object> _internalCache = new ConcurrentDictionary<string, object>();

        public T RetriveJSONObject<T>(string filename) where T : class
        {
            if (_internalCache.ContainsKey(filename) && _internalCache[filename] is T)
                return _internalCache[filename] as T;

            object objToRemove;

            if (_internalCache.ContainsKey(filename))
                _internalCache.TryRemove(filename, out objToRemove);

            var serializer = new JsonSerializer();

            using (var stream = new StreamReader(filename))
            {
                var obj = serializer.Deserialize(stream, typeof(T)) as T;
                if (obj != null)
                    _internalCache.TryAdd(filename, obj);

                return obj;
            }
        }

        public T RetriveXmlObject<T>(String filename) where T : class
        {
            if (_internalCache.ContainsKey(filename) && _internalCache[filename] is T)
                return _internalCache[filename] as T;

            object objToRemove;

            if (_internalCache.ContainsKey(filename))
                _internalCache.TryRemove(filename, out objToRemove);

            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new StreamReader(filename))
            {
                var obj = serializer.Deserialize(stream) as T;
                if (obj != null)
                    _internalCache.TryAdd(filename, obj);

                return obj;
            }
        }

        public T RetriveBinaryObject<T>(String filename) where T : class
        {
            if (_internalCache.ContainsKey(filename) && _internalCache[filename] is T)
                return _internalCache[filename] as T;

            object objToRemove;

            if (_internalCache.ContainsKey(filename))
                _internalCache.TryRemove(filename, out objToRemove);

            var serializer = new BinaryFormatter();

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var obj = serializer.Deserialize(stream) as T;
                if (obj != null)
                    _internalCache.TryAdd(filename, obj);

                return obj;
            }
        }
    }
}
