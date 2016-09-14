using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisDriver.RedisClient
{
    public class RedisCache 
    {
        private static readonly ConnectionMultiplexer redisConnections = ConnectionMultiplexer.Connect("localhost");
               
        public static void Set<T>(string key, T objectToCache) where T : class
        {
            var db = redisConnections.GetDatabase();

            db.StringSet(key, JsonConvert.SerializeObject(objectToCache
                        , Formatting.Indented
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        }));
        }


        public static T Get<T>(string key) where T : class
        {
            var db = redisConnections.GetDatabase();

            var redisObject = db.StringGet(key);
            if (redisObject.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(redisObject
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        });
            }
            else
            {
                return null;
            }
        }        
    }
}
