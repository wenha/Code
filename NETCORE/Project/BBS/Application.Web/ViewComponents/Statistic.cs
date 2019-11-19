using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Web.ViewComponents
{
    public class Statistic : ViewComponent
    {
        private readonly DataContext DataContext;
        private IMemoryCache MemoryCache;
        private string cachekey = "statistic";

        public Statistic(DataContext context, IMemoryCache memoryCache)
        {
            DataContext = context;
            MemoryCache = memoryCache;
        }

        public IViewComponentResult Invoke()
        {
            var allstatistics = new Tuple<int, int, int>(0, 0, 0);
            if (!MemoryCache.TryGetValue(cachekey, out allstatistics))
            {
                var usercount = DataContext.Users.Count();
                var topiccount = DataContext.Topics.Count();
                var replycount = DataContext.TopicReplys.Count();
                allstatistics = new Tuple<int, int, int>(usercount, topiccount, replycount);
                MemoryCache.Set(cachekey, allstatistics, TimeSpan.FromMinutes(1));
            }
            return View(allstatistics);
        }
    }
}
