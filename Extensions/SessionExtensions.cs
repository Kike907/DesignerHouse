using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DesignerHouse.Extensions
{
    //We add session and session extension so that we can handle complex 
    //object types like a list or an object and put that into session.
    public static class SessionExtensions
    {
        //This class works on a generic object which is T, so we can store any object which could be a list or even an object of any class.
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : 
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}