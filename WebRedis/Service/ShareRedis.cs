using AiritiUtility.Session.Redis.Contracts;
using AiritiUtility.Session.Redis.POCO;

namespace WebRedis.Service
{
    public class ShareRedis(IRedisCache redisCache)
    {
        const string keyForCachedFile = "CachedFile"; //請自行確保這個key不會重複而碰撞
        public TemplateCachedFile? CachedFile
        {
            get
            {
                redisCache.SelectDatabase(1);
                return redisCache.StringGet<TemplateCachedFile>(keyForCachedFile);
            }
            set
            {
                redisCache.SelectDatabase(1);
                if (value is not null)
                    redisCache.StringSet(keyForCachedFile, value); // 敏感內容，請自行加密
                else
                    redisCache.StringDelete<TemplateCachedFile>(keyForCachedFile);
            }
        }

    }
}
