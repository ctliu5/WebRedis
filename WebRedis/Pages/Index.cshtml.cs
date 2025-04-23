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

                _redisCache.StringSet<TemplateCachedFile>("TestKey", templateCached, new TimeSpan(0,0,3,0));
            }
            else
            {
                TemplateCachedFile templateCached = sessionRedis.CachedFile;
                templateCached.ContentType += "_CF";
                sessionRedis.CachedFile = templateCached;
                _redisCache.StringSet<TemplateCachedFile>("TestKey", templateCached, new TimeSpan(0, 0, 3, 0));
            }
        }
    }
}
