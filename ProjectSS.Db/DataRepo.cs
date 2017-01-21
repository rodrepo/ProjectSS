using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectSS.Common;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db
{
    public class DataRepo : BaseRepo, IDataRepo
    {
        #region Default
        private DataContext _db;

        public DataRepo(DataContext db)
        {
            _db = db;
        }

        public static DataRepo Create()
        {
            return new DataRepo(new DataContext());
        }

        public bool SaveAll()
        {
            return _db.SaveChanges() > 0;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public User GetUser(string userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId);
        }

        #endregion

        #region Users


        public async Task<List<User>> GetUsersAsync()
        {
            return await _db.Users.Where(u => !u.IsDeleted).ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _db.Users.Where(u => u.Id == id && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(User user, UserManager<User> userManager, string role, string userId)
        {
            var usr = await userManager.FindByIdAsync(user.Id);
            var rol = _db.Roles.Find(role);
            if (usr != null)
            {
                usr.FirstName = user.FirstName;
                usr.MiddleName = user.MiddleName;
                usr.LastName = user.LastName;
                usr.Gender = user.Gender;
                usr.Birthday = user.Birthday;
                usr.Mobile = user.Mobile;
                usr.IsActive = user.IsActive;
                var result = await userManager.UpdateAsync(usr);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(usr.Id, role);
                    return usr;
                }
            }
            return null;
        }

        public void DeleteUser(User user, string userId)
        {
            user.IsDeleted = true;
            UpdateUser(user, userId);
        }

        private void UpdateUser(User user, string userId)
        {
            _db.UserId = userId;
            _db.Entry(user).State = EntityState.Modified;
        }
        public async Task<bool> ActivateUserAsync(string id, DateTime bday)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null && user.Birthday == bday)
            {
                _db.UserId = user.Id;
                user.IsActive = true;
                user.EmailConfirmed = true;
                return await SaveAllAsync();
            }
            return false;
        }

        #endregion

        #region Roles

        public IdentityRole GetRoleById(string Id)
        {
            return _db.Roles.FirstOrDefault(u => u.Id == Id);
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<List<IdentityRole>> GetRolesByUserId(string userId)
        {
            return await (from ur in _db.UserRoles
                          join r in _db.Roles on ur.RoleId equals r.Id
                          where ur.UserId == userId
                          select r
                          ).ToListAsync();
        }
        #endregion

        #region CRM

        public async Task<List<CRM>> GetCRM()
        {
            var crm = await _db.CRMs.Where(m => !m.IsDeleted).ToListAsync();
            return crm;
        }

        public async Task<List<CRM>> GetCRMByRegion(string region)
        {
            var crm = await _db.CRMs.Where(m => !m.IsDeleted && m.Region == region).ToListAsync();
            return crm;
        }

        public void UpdateCRM(CRM crm, string userId)
        {
            _db.UserId = userId;
            _db.Entry(crm).State = EntityState.Modified;
        }

        public CRM AddCRM(CRM crm, string userId)
        {
            _db.UserId = userId;
            _db.CRMs.Add(crm);
            return crm;
        }

        public void DeleteCRM(CRM crm, string userId)
        {
            crm.IsDeleted = true;
            UpdateCRM(crm, userId);
        }
        #endregion
    }
}
