using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityS4.Models;

namespace IdentityS4.DAL
{
    public interface IAdminService
    {
        Task<Admin> GetByStr(string userName, string pwd);
    }
}
