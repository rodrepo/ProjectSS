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

        public async Task<CRM> GetCRMById(int id)
        {
            var crm = await _db.CRMs.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
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
            var result = GenerateCRMReference();
            crm.Reference = result.Reference;
            crm.Number = result.Number;
            _db.CRMs.Add(crm);
            return crm;
        }

        private CRMReferenceModel GenerateCRMReference()
        {
            var cRMReference = new CRMReferenceModel();

            var latestReference = _db.CRMs.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefault();
            if (latestReference != null && latestReference.Number != null)
            {
                int multi = 1;
                multi += int.Parse(latestReference.Number);
                cRMReference.Reference = "SCII-" + multi.ToString();
                cRMReference.Number = multi.ToString();
            }
            else
            {
                cRMReference.Reference = "SCII-1";
                cRMReference.Number = "1";

            }
            return cRMReference;
        }

        //Used to follow SCII number
        private string GenerateCRMNumbering()
        {
            string number = "";
            var latestReference = _db.CRMs.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefault();
            if (latestReference != null && latestReference.Id > 0 && latestReference != null)
            {
                number = latestReference.Id.ToString();
            }
            else
            {
                number = "1";
            }
            return number;
        }
        public void DeleteCRM(CRM crm, string userId)
        {
            crm.IsDeleted = true;
            UpdateCRM(crm, userId);
        }


        public void AddCRMEmailHistory(CRMEmailHistory cRMEmailHistory, string userId)
        {
            _db.UserId = userId;
            _db.CRMEmailHistorys.Add(cRMEmailHistory);
        }

        public async Task<List<CRMEmailHistory>> GetCRMEmailHistoryByCRMId(int CRMId)
        {
            var emails = await _db.CRMEmailHistorys.Where(m => !m.IsDeleted && m.CRMId == CRMId).ToListAsync();
            return emails;
        }

        public void AddCRMCallHistory(CRMCallHistory cRMCallHistory, string userId)
        {
            _db.UserId = userId;
            _db.CRMCallHistorys.Add(cRMCallHistory);
        }

        public void AddCRMRevisionHistory(CRMRevisionHistory cRMRevisionHistory, string userId)
        {
            _db.UserId = userId;
            _db.CRMRevisionHistorys.Add(cRMRevisionHistory);
        }

        public async Task<List<CRMCallHistory>> GetCRMCallHistoryByCRMId(int CRMId)
        {
            var calls = await _db.CRMCallHistorys.Where(m => !m.IsDeleted && m.CRMId == CRMId).ToListAsync();
            return calls;
        }
        public async Task<List<CRMRevisionHistory>> GetCRMRevisionHistoryByCRMId(int CRMId)
        {
            var revision = await _db.CRMRevisionHistorys.Where(m => !m.IsDeleted && m.CRMId == CRMId).ToListAsync();
            return revision;
        }

        #endregion

        #region Private Class
        private class CRMReferenceModel
        {
            public string Reference { get; set; }
            public string Number { get; set; }
        }
        #endregion
    }
}
