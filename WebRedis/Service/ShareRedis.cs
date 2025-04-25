using AiritiUtility.Session.Redis.Contracts;
using AiritiUtility.Session.Redis.POCO;

namespace WebRedis.Service
{
    public class ShareRedis(IRedisCache redisCache)
    {
        /// <summary>
        /// RedisDB enum for database selection.
        /// </summary>
        enum RedisDatabase
        {
            Default = 0,
            JSON_Data = 1,
            PdfViewer = 2,
        }
        private void changeBD(RedisDatabase redisDB)
        {
            redisCache.SelectDatabase((int)redisDB);
        }

        #region Database(1)
        const string keyForCachedFile = "CachedFile"; //請自行確保這個key不會重複而碰撞
        public TemplateCachedFile? CachedFile
        {
            get
            {
                changeBD(RedisDatabase.JSON_Data);
                return redisCache.StringGet<TemplateCachedFile>(keyForCachedFile);
            }
            set
            {
                changeBD(RedisDatabase.JSON_Data);
                if (value is not null)
                    redisCache.StringSet(keyForCachedFile, value); // 敏感內容，請自行加密
                else
                    redisCache.StringDelete<TemplateCachedFile>(keyForCachedFile);
            }
        }
        #endregion

    }
}
