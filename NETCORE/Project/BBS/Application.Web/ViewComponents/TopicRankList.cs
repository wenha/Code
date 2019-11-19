using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Entity;
using Application.Entity;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Web.ViewComponents
{
    public class TopicRankList : ViewComponent
    {
        private readonly DataContext DataContext;
        private IMemoryCache MemoryCache;
        private string cachekey = "topicrank";

        public TopicRankList(DataContext context, IMemoryCache memoryCache)
        {
            DataContext = context;
            MemoryCache = memoryCache;
        }

        public IViewComponentResult Invoke(int days)
        {
            var items = new List<Topic>();
            if (!MemoryCache.TryGetValue(cachekey, out items))
            {
                items = GetRankTopics(10, days);
                MemoryCache.Set(cachekey, items, TimeSpan.FromMinutes(10));
            }
            return View(items);
        }
        /// <summary>
        /// 获取主题排行
        /// </summary>
        /// <param name="top"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        private List<Topic> GetRankTopics(int top, int days)
        {
            return DataContext.Topics.Where(r => r.CreateOn > DateTime.Now.AddDays(-days))
                .OrderByDescending(r => r.ViewCount).Take(top).ToList();
        }
    }
}
