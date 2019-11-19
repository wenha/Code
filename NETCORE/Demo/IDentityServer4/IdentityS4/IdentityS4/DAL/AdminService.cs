using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityS4.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityS4.DAL
{
    public class AdminService : IAdminService
    {
        public EFContext db;

        public AdminService(EFContext _efContext)
        {
            db = _efContext;
        }

        public Task<Admin> GetByStr(string userName, string pwd)
        {
            return db.Admin.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == pwd);
        }
    }
}
