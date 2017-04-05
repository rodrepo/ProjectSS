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
        string GetUserName(string userId);
        #endregion

        #region Roles
        IdentityRole GetRoleById(string Id);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<List<IdentityRole>> GetRolesByUserId(string userId);
        Task<string> GetRoleNameByUserId(string userId);


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
        Task<ProposalExpense> GetProposalExpenseByIdAsync(int id);
        void AddProposalExpenses(ProposalExpense proposalExpense, string userId);
        Task DeleteProposalExpense(int id);
        #endregion

        #region Proposal Contractors/OutSource
        Task<List<ProposalContractor>> GetProposalContractorsByProposalIdAsync(int proposalId);

        Task<ProposalContractor> GetProposalContractorByIdAsync(int id);
        void AddProposalContractor(ProposalContractor proposalContractor, string userId);
        Task DeleteProposalContractor(int id);

        #endregion

        #region Proposal Equipment

        Task<List<ProposalEquipment>> GetProposalEquipmentsByProposalIdAsync(int proposalId);
        Task<ProposalEquipment> GetProposalEquipmentByIdAsync(int id);

        void AddProposalEquipment(ProposalEquipment proposalEquipment, string userId);
        Task DeleteProposalEquipment(int id);

        #endregion

        #region Proposal Laboratory
        Task<ProposalLaboratory> GetProposalLaboratoryByIdAync(int id);
        Task<List<ProposalLaboratory>> GetProposalLaboratoriesByProposalAsync(int proposalId);
        void AddProposalLaboratory(ProposalLaboratory proposalLaboratory, string userId);
        Task DeleteProposalLaboratory(int id);

        #endregion

        #region Proposal Commission
        Task<ProposalCommission> GetProposalCommissionByIdAsync(int id);
        Task<List<ProposalCommission>> GetProposalCommissionsByProposalIdAsync(int proposalId);
        void AddProposalCommission(ProposalCommission proposalCommission, string userId);
        Task DeleteProposalCommission(int id);
        #endregion

        #region Project
        Task<Project> AddProjectAsync(Project project, string userId);
        Task<List<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);

        #endregion

        #region Inventory
        Task<List<Inventory>> GetInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int id);
        void AddInventory(Inventory inventory, string userId);
        Task UpdateInventory(Inventory inventory, string userId);
        Task DeleteInventory(int id);

        #endregion

        #region BugetRequest

        void AddBudGetRequestItem(BudgetRequestItem budgetRequestItem, string userId);
        Task<BudgetRequest> AddBudGetRequest(BudgetRequest budgetRequest, string userId);
        Task<List<BudgetRequest>> GetBudGetRequestsByProjectIdAndUserIdAsync(int projectId, string userId);
        Task<List<BudgetRequest>> GetBudGetRequestsByProjectIdAsync(int projectId);
        Task<List<BudgetRequest>> GetBudGetRequestsForOMAsync();
        Task<List<BudgetRequest>> GetBudGetRequestsForAHAsync();
        Task<List<BudgetRequest>> GetBudGetRequestsForTHAsync(string userId);
        Task<int> GetToBeApprovedRequestsCountAsync(string role, string userId);
        #endregion
    }
}
