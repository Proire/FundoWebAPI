using Microsoft.EntityFrameworkCore.Storage.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserRLL.Utilities
{
    public class CacheService : ICacheService   
    {
        private readonly IDatabase _db;
        public CacheService(Lazy<ConnectionMultiplexer> lazyConnection) 
        { 
            _db = lazyConnection.Value.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key,JsonConvert.SerializeObject(value),expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}
