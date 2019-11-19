using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Entity;
using Application.Core.Interface;
using Application.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ITopicRepository _topic;

        private ITopicReplyRepository _reply;

        private UserManager<User> UserManager;

        private IWebHostEnvironment _env;

        public UserController(ITopicRepository topic, ITopicReplyRepository reply, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _topic = topic;
            _reply = reply;
            UserManager = userManager;
            _env = env;
        }

        public IActionResult Index()
        {
            var u = UserManager.GetUserAsync(User).Result;
            var topics = _topic.GetList(r => r.UserId == u.Id).ToList();
            var replys = _reply.GetList(r => r.ReplyUserId == u.Id).ToList();

            ViewBag.Topics = topics;
            ViewBag.Replys = replys;
            return View(u);
        }

        public ActionResult Edit()
        {
            return View(UserManager.GetUserAsync(User).Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel userModel)
        {
            var user = UserManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {
                if (userModel.Avatar != null)
                {
                    var avatar = userModel.Avatar;
                    if (avatar.Length / 1024 > 100)
                    {
                        return Content("头像大小超过100KB");
                    }

                    var ext = Path.GetExtension(avatar.FileName);
                    var avatarFile = user.Id + ext;
                    var avatarPath = Path.Combine(_env.WebRootPath, "images", "avatar");
                    if (!Directory.Exists(avatarPath))
                    {
                        Directory.CreateDirectory(avatarPath);
                    }

                    var filePath = Path.Combine(avatarPath, avatarFile);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        avatar.CopyTo(fs);
                        fs.Flush();
                    }

                    user.Avatar = $"/images/avatar/{avatarFile}";
                }

                user.Email = userModel.Email;
                user.Url = userModel.Url;
                user.GitHub = userModel.GitHub;
                user.Profile = userModel.Profile;
                await UserManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }
    }
}