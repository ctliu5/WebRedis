using AiritiUtility.Session.Redis.Contracts;
using AiritiUtility.Session.Redis.POCO;

namespace WebRedis.Service
{
    public class ShareRedis(IRedisCache typedRedisCache)
    {
        const string keyForCachedFile = "CachedFile";
        public TemplateCachedFile? CachedFile
        {
            get
            {
                return typedRedisCache.StringGet<TemplateCachedFile>(keyForCachedFile);
            }
            set
            {
                if (value is not null)
                    typedRedisCache.StringSet(keyForCachedFile, value);
                else
                    typedRedisCache.StringDelete<TemplateCachedFile>(keyForCachedFile);
            }
        }

    }
}
