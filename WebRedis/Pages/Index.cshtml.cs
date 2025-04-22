using AiritiUtility.Session.Redis.POCO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRedis.Service;

namespace WebRedis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SessionRedis sessionRedis;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, SessionRedis session)
        {
            _logger = logger;
            sessionRedis = session;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserSession") is not string jsonStr)
            {
                HttpContext.Session.SetString("UserSession", "Somebody");
            }
            else
                HttpContext.Session.SetString("UserSession", jsonStr + "_A");

            if (sessionRedis.CachedFile is null)
            {
                TemplateCachedFile templateCached = new TemplateCachedFile()
                {
                    FileName = "test.txt",
                    ContentType = "txt",
                    FileContent = System.Text.Encoding.UTF8.GetBytes("Hello World")
                };
                sessionRedis.CachedFile = templateCached;
            }
            else
            {
                TemplateCachedFile templateCached = sessionRedis.CachedFile;
                templateCached.ContentType += "_A";
                sessionRedis.CachedFile = templateCached;
            }
        }
    }
}
