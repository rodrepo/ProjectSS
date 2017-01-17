using Microsoft.AspNet.Identity.EntityFramework;
using ProjectSS.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Contracts
{
    public interface IDataRepo : IDisposable
    {
        bool SaveAll();
        Task<bool> SaveAllAsync();
        User GetUser(string userId);

        #region Users
        Task<List<User>> GetUsersAsync();
        #endregion

        #region Roles
        IdentityRole GetRoleById(string Id);
        #endregion
    }
}
