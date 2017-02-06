using Microsoft.AspNet.Identity;
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
        #region Default
        bool SaveAll();
        Task<bool> SaveAllAsync();
        User GetUser(string userId);
        #endregion

        #region Users
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> UpdateUserAsync(User user, UserManager<User> userManager, string role, string userId);
        void DeleteUser(User user, string userId);
        Task<string> GetUserNameByIdAsync(string id);
        #endregion

        #region Roles
        IdentityRole GetRoleById(string Id);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<List<IdentityRole>> GetRolesByUserId(string userId);

        #endregion

        #region CRM
        Task<List<CRM>> GetCRM();
        Task<List<CRM>> GetCRMByRegion(string region);
        CRM AddCRM(CRM crm, string userId);
        void UpdateCRM(CRM crm, string userId);
        void DeleteCRM(CRM crm, string userId);
        Task<CRM> GetCRMById(int id);
        void AddCRMEmailHistory(CRMEmailHistory cRMEmailHistory, string userId);
        Task<List<CRMEmailHistory>> GetCRMEmailHistoryByCRMId(int CRMid);
        void AddCRMCallHistory(CRMCallHistory cRMCallHistory, string userId);
        void AddCRMRevisionHistory(CRMRevisionHistory cRMRevisionHistory, string userId);
        Task<List<CRMCallHistory>> GetCRMCallHistoryByCRMId(int id);
        Task<List<CRMRevisionHistory>> GetCRMRevisionHistoryByCRMId(int id);


        #endregion

        #region Proposal
        Task<List<Proposal>> GetProposalAsync();
        Task<Proposal> AddPorposalAsync(Proposal proposal, string UserId);
        Task<Proposal> GetProposalByIdAsync(int id);
        Task UpdateProposal(Proposal proposal, string userId);
        Task DeleteProposal(int id);
        #endregion

        #region Proposal Staff
        Task<List<ProposalStaff>> GetProposalStaffsByProposalIdAsync(int proposalId);
        void AddProposalStaff(ProposalStaff proposalStaff, string userId);
        Task DeleteProposalStaff(int id);
        #endregion

        #region Proposal Operationg Expenses
        Task<List<ProposalExpense>> GetProposalExpensesByProposalIdAsync(int proposalId);
        void AddProposalExpenses(ProposalExpense proposalExpense, string userId);
        Task DeleteProposalExpense(int id);
        #endregion

        #region Proposal Contractors/OutSource
        Task<List<ProposalContractor>> GetProposalContractorsByProposalIdAsync(int proposalId);
        void AddProposalContractor(ProposalContractor proposalContractor, string userId);
        Task DeleteProposalContractor(int id);

        #endregion

        #region Inventory
        Task<List<Inventory>> GetInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int id);
        void AddInventory(Inventory inventory, string userId);
        Task UpdateInventory(Inventory inventory, string userId);
        Task DeleteInventory(int id);

        #endregion
    }
}
