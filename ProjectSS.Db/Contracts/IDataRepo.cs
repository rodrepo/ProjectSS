﻿using Microsoft.AspNet.Identity;
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
        Task<User> GetUserByIdAsync(string id);
        Task<User> UpdateUserAsync(User user, UserManager<User> userManager, string role, string userId);
        void DeleteUser(User user, string userId);
        #endregion

        #region Roles
        IdentityRole GetRoleById(string Id);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<List<IdentityRole>> GetRolesByUserId(string userId);

        #endregion

        #region CRM
        Task<List<CRM>> GetCRM();
        Task<List<CRM>> GetCRMByRegion(string region);
        #endregion
    }
}
