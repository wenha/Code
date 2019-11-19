using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Web.Services
{
    public interface IUserServices
    {
        Task<User> User { get; }
    }

    public class UserServices : IUserServices
    {
        public UserManager<User> UserManager { get; }

        private IHttpContextAccessor Context;

        public UserServices(UserManager<User> userManager, IHttpContextAccessor context)
        {
            UserManager = userManager;
            Context = context;
        }

        public Task<User> User => UserManager.GetUserAsync(Context.HttpContext.User);
    }
}
