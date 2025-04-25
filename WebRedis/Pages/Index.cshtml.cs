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
        private readonly ShareRedis _shareRedis;

        public IndexModel(ILogger<IndexModel> logger, SessionRedis session, ShareRedis shareRedis)
        {
            _logger = logger;
            sessionRedis = session;
            _shareRedis = shareRedis;
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

            _shareRedis.CachedFile = null; // Remove the key from Redis.

            /*
            _shareRedis.SelectDatabase(1); // Don't do this in here.

            if (!_shareRedis.StringExists<TemplateCachedFile>(UniqueKey)) // Check if the key not exists
            {
                TemplateCachedFile templateCached = new()
                {
                    FileName = "",
                    ContentType = "",
                    FileContent = System.Text.Encoding.UTF8.GetBytes("Hello World")
                };
                _shareRedis.StringSet<TemplateCachedFile>(UniqueKey, templateCached, TimeSpan.FromMinutes(10)); // Set
            }
            else if (_shareRedis.StringGet<TemplateCachedFile>(UniqueKey) is TemplateCachedFile templateCached) // Get
            {
                _shareRedis.StringDelete<TemplateCachedFile>(UniqueKey); // Remove
                templateCached.ContentType += "_CF";
                _shareRedis.StringSet<TemplateCachedFile>(UniqueKey, templateCached, TimeSpan.FromMinutes(10)); // Set
            }
            */
        }
    }
}
