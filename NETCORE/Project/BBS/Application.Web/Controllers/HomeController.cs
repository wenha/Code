using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Core.Entity;
using Application.Core.Interface;
using Application.Web.Services;
using Application.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITopicRepository TopicRepository;
        private IRepository<TopicNode> TopicNodeRepository;
        public UserManager<User> UserManager { get; }
        public HomeController(ITopicRepository topicRepository, IRepository<TopicNode> topicNodeRepository, UserManager<User> userManager)
        {
            TopicRepository = topicRepository;
            TopicNodeRepository = topicNodeRepository;
            UserManager = userManager;
        }

        public IActionResult Index([FromServices]IUserServices userServices)
        {
            var pagesize = 20;
            var pageindex = 1;
            Page<Topic> result = null;
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            if (!string.IsNullOrEmpty(Request.Query["s"]))
                result = TopicRepository.PageList(r => r.Title.Contains(Request.Query["s"]), pagesize, pageindex);
            else
                result = TopicRepository.PageList(pagesize, pageindex);
            ViewBag.Topics = result.List.Select(r => new TopicViewModel
            {
                Id = r.Id,
                NodeId = r.Node.Id,
                NodeName = r.Node.Name,
                UserName = r.User.UserName,
                Avatar = r.User.Avatar,
                Title = r.Title,
                Top = r.Top,
                Type = r.Type,
                ReplyCount = r.ReplyCount,
                LastReplyTime = r.LastReplyTime,
                CreateOn = r.CreateOn
            }).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = result.GetPageCount();
            ViewBag.User = userServices.User.Result;
            var nodes = TopicNodeRepository.GetList().ToList();
            ViewBag.Nodes = nodes;
            ViewBag.NodeListItem = nodes.Where(r => r.ParentId != 0).Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.CreateOn = DateTime.Now;
                topic.Type = TopicType.Normal;
                TopicRepository.Add(topic);
            }
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = ".NET Core 版轻论坛";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}