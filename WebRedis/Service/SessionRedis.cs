using AiritiUtility.Session.Redis.Contracts;
using AiritiUtility.Session.Redis.POCO;

namespace WebRedis.Service
{
    public class SessionRedis(IHttpContextAccessor httpContextAccessor, IRedisCache typedRedisCache) : AbstractRedisSession(httpContextAccessor, typedRedisCache)
    {
        public TemplateCachedFile? CachedFile { get { return GetOrDefault<TemplateCachedFile>(); } set { SetOrRemove(value); } }

    }
}
