using AiritiUtility.Session.Redis.Contracts;
using AiritiUtility.Session.Redis.POCO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRedis.Service;

namespace WebRedis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SessionRedis sessionRedis;
        private readonly ILogger<IndexModel> _logger;
        private readonly IRedisCache _redisCache;

        public IndexModel(ILogger<IndexModel> logger, SessionRedis session, IRedisCache redisCache)
        {
            _logger = logger;
            sessionRedis = session;
            _redisCache = redisCache;
        }
        const string UniqueKey = "UniqueKey";

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserSession") is not string jsonStr)
            {
                HttpContext.Session.SetString("UserSession", "");
            }
            else
                HttpContext.Session.SetString("UserSession", jsonStr + "_U");

            if (sessionRedis.CachedFile is null)
            {
                TemplateCachedFile templateCached = new TemplateCachedFile()
                {
                    FileName = "",
                    ContentType = "",
                    FileContent = System.Text.Encoding.UTF8.GetBytes("Hello World")
                };
                sessionRedis.CachedFile = templateCached;
            }
            else
            {
                TemplateCachedFile templateCached = sessionRedis.CachedFile;
                templateCached.ContentType += "_CF";
                sessionRedis.CachedFile = templateCached;
            }

            _redisCache.SelectDatabase(1); // Select database 1

            if (!_redisCache.StringExists<TemplateCachedFile>(UniqueKey)) // Check if the key not exists
            {
                TemplateCachedFile templateCached = new()
                {
                    FileName = "",
                    ContentType = "",
                    FileContent = System.Text.Encoding.UTF8.GetBytes("Hello World")
                };
                _redisCache.StringSet<TemplateCachedFile>(UniqueKey, templateCached, TimeSpan.FromMinutes(10)); // Set
            }
            else if (_redisCache.StringGet<TemplateCachedFile>(UniqueKey) is TemplateCachedFile templateCached) // Get
            {
                _redisCache.StringDelete<TemplateCachedFile>(UniqueKey); // Remove
                templateCached.ContentType += "_CF";
                _redisCache.StringSet<TemplateCachedFile>(UniqueKey, templateCached, TimeSpan.FromMinutes(10)); // Set
            }
        }
    }
}
