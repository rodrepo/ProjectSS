﻿using Microsoft.AspNet.Identity;
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

        public string GetUserName(string userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId).DisplayName;

        }
        #endregion

        #region Users


        public async Task<List<User>> GetUsersAsync()
        {
            return await _db.Users.Where(u => !u.IsDeleted && u.IsMaster == false).ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _db.Users.Where(u => u.Id == id && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<string> GetUserNameByIdAsync(string id)
        {
            return await _db.Users.Where(u => u.Id == id && !u.IsDeleted).Select(u => u.FirstName + " " + u.MiddleName + " " + u.LastName).FirstOrDefaultAsync();
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
                usr.Rate = user.Rate;
                usr.Email = user.Email;
                usr.UserName = user.Email;
                var result = await userManager.UpdateAsync(usr);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(usr.Id, role);
                    return usr;
                }
            }
            return null;
        }

        public async Task<bool> CanUserBeDeleted(string userId, string role)
        {
            bool result = true;
            if (role == "TH")
            {
                var th = await _db.Projects.Where(p => p.Proposal.THId == userId && p.IsClosed == false && p.RemainingBudget > 0).ToListAsync();
                if (th.Count > 0)
                {
                    result = false;
                }
            }
            else if (role == "BD")
            {
                var bd = await _db.Projects.Where(p => p.Proposal.BDId == userId && p.IsClosed == false && p.RemainingBudget > 0).ToListAsync();
                if (bd.Count > 0)
                {
                    result = false;
                }
            }
            else if (role == "TS")
            {
                var ts = await _db.Projects.Where(p => p.Proposal.TSId == userId && p.IsClosed == false && p.RemainingBudget > 0).ToListAsync();
                if (ts.Count > 0)
                {
                    result = false;
                }
            }
            return result;
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
        public async Task<string> GetRoleNameByUserId(string userId)
        {
            return await (from ur in _db.UserRoles
                          join r in _db.Roles on ur.RoleId equals r.Id
                          where ur.UserId == userId
                          select r.Name
                          ).FirstOrDefaultAsync();
        }
        #endregion

        #region CRM

        public async Task<List<CRM>> GetCRM()
        {
            var crm = await _db.CRMs.Where(m => !m.IsDeleted).OrderByDescending(m => m.CreatedDate).ToListAsync();
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
            crm.Number = result.Number.ToString();
            _db.CRMs.Add(crm);
            return crm;
        }

        private ReferenceModel GenerateCRMReference()
        {
            var cRMReference = new ReferenceModel();

            var latestReference = _db.CRMs.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefault();
            if (latestReference != null && latestReference.Number != null)
            {
                int multi = 1;
                multi += int.Parse(latestReference.Number);
                cRMReference.Reference = "SCII-" + multi.ToString();
                cRMReference.Number = multi;
            }
            else
            {
                cRMReference.Reference = "SCII-1";
                cRMReference.Number = 1;

            }
            return cRMReference;
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

        #region Proposal
        public async Task<List<Proposal>> GetProposalAsync(string userRole,string userId)
        {
            if (userRole == "TH")
            {
                return await _db.Proposals.Where(p => !p.IsDeleted && p.THId == userId).OrderByDescending(p => p.CreatedDate).ToListAsync();
            }
            else
            {
                return await _db.Proposals.Where(p => !p.IsDeleted).OrderByDescending(p => p.CreatedDate).ToListAsync();

            }

        }

        public async Task<Proposal> AddPorposalAsync(Proposal proposal, string userId)
        {
            var crm = await GetCRMById(proposal.CRMId);
            proposal.CompanyName = crm.CompanyName;
            proposal.ContactPerson = crm.Name;
            proposal.ContactPersonPosition = crm.Position;
            proposal.Industry = crm.Industry;
            proposal.Location = crm.Region;

            var key = await GenerateProposalNumber();
            proposal.ProposalNumber = key.Reference;
            proposal.PPNumber = key.Number;

            _db.UserId = userId;
            var result = _db.Proposals.Add(proposal);
            return result;
        }

        public async Task<Proposal> GetProposalByIdAsync(int id)
        {
            return await _db.Proposals.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateProposal(Proposal proposal, string userId)
        {
            _db.UserId = userId;
            var result = await MappPropsal(proposal);
            _db.Entry(result).State = EntityState.Modified;
        }

        private async Task<Proposal> MappPropsal(Proposal proposal)
        {
            var latestProposal = await GetProposalByIdAsync(proposal.Id);
            if (latestProposal != null && latestProposal.RVNumber > 0)
            {
                int multi = 1;
                multi += latestProposal.RVNumber;
                latestProposal.RevisionNumber = "REV-" + multi.ToString();
                latestProposal.RVNumber = multi;
            }
            else
            {
                latestProposal.RevisionNumber = "REV-1";
                latestProposal.RVNumber = 1;
            }
            latestProposal.ProjectType = proposal.ProjectType;
            latestProposal.Title = proposal.Title;
            latestProposal.Cost = proposal.Cost;
            latestProposal.Description = proposal.Description;
            latestProposal.BDId = proposal.BDId;
            latestProposal.THId = proposal.THId;
            latestProposal.TSId = proposal.TSId;
            latestProposal.Status = proposal.Status;
            latestProposal.NegotiationAllowance = proposal.NegotiationAllowance;
            latestProposal.ProjectNumber = proposal.ProjectNumber;
            return latestProposal;
        }

        private async Task<ReferenceModel> GenerateProposalNumber()
        {
            var keys = new ReferenceModel();

            var latestProposal = await _db.Proposals.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefaultAsync();
            if (latestProposal != null && latestProposal.PPNumber > 0)
            {
                int multi = 1;
                multi += latestProposal.PPNumber;
                keys.Reference = "PROPOSAL No. " + multi.ToString();
                keys.Number = multi;
            }
            else
            {
                keys.Reference = "PROPOSAL No. 1";
                keys.Number = 1;
            }
            return keys;
        }

        public async Task DeleteProposal(int id)
        {
            var staff = await GetProposalByIdAsync(id);
            staff.IsDeleted = true;
            _db.Entry(staff).State = EntityState.Modified;
        }

        #endregion

        #region Proposal Staff
        public async Task<List<ProposalStaff>> GetProposalStaffsByProposalIdAsync(int proposalId)
        {
            return await _db.ProposalStaffs.Where(p => !p.IsDeleted && p.ProposalId == proposalId).ToListAsync();
        }

        public async Task<ProposalStaff> GetProposalStaffByIdAsync(int id)
        {
            return await _db.ProposalStaffs.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        }


        public void AddProposalStaff(ProposalStaff proposalStaff, string userId)
        {
            _db.UserId = userId;
            _db.ProposalStaffs.Add(proposalStaff);
        }

        public async Task DeleteProposalStaff(int id)
        {
            var staff = await GetProposalStaffByIdAsync(id);
            staff.IsDeleted = true;
            _db.Entry(staff).State = EntityState.Modified;
        }

        #endregion

        #region Proposal Operationg Expenses
        public async Task<List<ProposalExpense>> GetProposalExpensesByProposalIdAsync(int proposalId)
        {
            return await _db.ProposalExpensess.Where(p => !p.IsDeleted && p.ProposalId == proposalId).ToListAsync();
        }

        public void AddProposalExpenses(ProposalExpense proposalExpense, string userId)
        {
            _db.UserId = userId;
            _db.ProposalExpensess.Add(proposalExpense);
        }

        public async Task<ProposalExpense> GetProposalExpenseByIdAsync(int id)
        {
            return await _db.ProposalExpensess.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteProposalExpense(int id)
        {
            var expense = await GetProposalExpenseByIdAsync(id);
            expense.IsDeleted = true;
            _db.Entry(expense).State = EntityState.Modified;
        }

        #endregion

        #region Proposal Contractors/OutSource
        public async Task<List<ProposalContractor>> GetProposalContractorsByProposalIdAsync(int proposalId)
        {
            return await _db.ProposalContractors.Where(p => !p.IsDeleted && p.ProposalId == proposalId).ToListAsync();
        }

        public void AddProposalContractor(ProposalContractor proposalContractor, string userId)
        {
            _db.UserId = userId;
            _db.ProposalContractors.Add(proposalContractor);
        }

        public async Task<ProposalContractor> GetProposalContractorByIdAsync(int id)
        {
            return await _db.ProposalContractors.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteProposalContractor(int id)
        {
            var contractor = await GetProposalContractorByIdAsync(id);
            contractor.IsDeleted = true;
            _db.Entry(contractor).State = EntityState.Modified;
        }

        #endregion

        #region Proposal Equipment

        public async Task<List<ProposalEquipment>> GetProposalEquipmentsByProposalIdAsync(int proposalId)
        {
            return await _db.ProposalEquipments.Where(p => !p.IsDeleted && p.ProposalId == proposalId).ToListAsync();
        }

        public void AddProposalEquipment(ProposalEquipment proposalEquipment, string userId)
        {
            _db.UserId = userId;
            _db.ProposalEquipments.Add(proposalEquipment);
        }

        public async Task<ProposalEquipment> GetProposalEquipmentByIdAsync(int id)
        {
            return await _db.ProposalEquipments.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteProposalEquipment(int id)
        {
            var equipment = await GetProposalEquipmentByIdAsync(id);
            equipment.IsDeleted = true;
            _db.Entry(equipment).State = EntityState.Modified;
        }


        #endregion

        #region Proposal Laboratory
        public async Task<List<ProposalLaboratory>> GetProposalLaboratoriesByProposalAsync(int proposalId)
        {
            return await _db.ProposalLaboratories.Where(p => !p.IsDeleted && p.Id == proposalId).ToListAsync();
        }

        public void AddProposalLaboratory(ProposalLaboratory proposalLaboratory, string userId)
        {
            _db.UserId = userId;
            _db.ProposalLaboratories.Add(proposalLaboratory);
        }

        public async Task<ProposalLaboratory> GetProposalLaboratoryByIdAync(int id)
        {
            return await _db.ProposalLaboratories.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();

        }

        public async Task DeleteProposalLaboratory(int id)
        {
            var laboratory = await GetProposalLaboratoryByIdAync(id);
            laboratory.IsDeleted = true;
            _db.Entry(laboratory).State = EntityState.Modified;
        }
        #endregion

        #region Commission
        public async Task<List<ProposalCommission>> GetProposalCommissionsByProposalIdAsync(int proposalId)
        {
            return await _db.ProposalCommissions.Where(c => c.ProposalId == proposalId && !c.IsDeleted).ToListAsync();
        }

        public void AddProposalCommission(ProposalCommission proposalCommission, string userId)
        {
            _db.UserId = userId;
            _db.ProposalCommissions.Add(proposalCommission);
        }

        public async Task<ProposalCommission> GetProposalCommissionByIdAsync(int id)
        {
            return await _db.ProposalCommissions.Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task DeleteProposalCommission(int id)
        {
            var commission = await GetProposalCommissionByIdAsync(id);
            commission.IsDeleted = true;
            _db.Entry(commission).State = EntityState.Modified;
        }
        #endregion

        #region Inventory

        public async Task<List<Inventory>> GetInventoriesAsync()
        {
            return await _db.Inventories.Where(p => !p.IsDeleted).OrderByDescending(p => p.CreatedDate).ToListAsync();
        }
        public async Task<int> GetInventoriesAssignedCountAsync(string userId)
        {
            return await _db.Inventories.Where(p => p.UserId == userId).CountAsync();
        }
        public async Task<Inventory> GetInventoryByIdAsync(int id)
        {
            return await _db.Inventories.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddInventory(Inventory inventory, string userId)
        {
            var key = await GenerateInventoryNumber();
            inventory.InventoryNumber = key.Reference;
            inventory.InvNumber = key.Number;
            _db.UserId = userId;
            _db.Inventories.Add(inventory);
        }

        public async Task UpdateInventory(Inventory inventory, string userId)
        {
            _db.UserId = userId;
            var result = await MapInventory(inventory);
            _db.Entry(result).State = EntityState.Modified;
        }

        public async Task DeleteInventory(int id)
        {
            var inventory = await GetInventoryByIdAsync(id);
            inventory.IsDeleted = true;
            _db.Entry(inventory).State = EntityState.Modified;
        }

        private async Task<ReferenceModel> GenerateInventoryNumber()
        {
            var keys = new ReferenceModel();

            var latestProposal = await _db.Inventories.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefaultAsync();
            if (latestProposal != null && latestProposal.InvNumber > 0)
            {
                int multi = 1;
                multi += latestProposal.InvNumber;
                keys.Reference = "PROPERTY No. " + multi.ToString();
                keys.Number = multi;
            }
            else
            {
                keys.Reference = "PROPERTY No. 1";
                keys.Number = 1;
            }
            return keys;
        }

        private async Task<Inventory> MapInventory(Inventory inventory)
        {
            var storedInventory = await GetInventoryByIdAsync(inventory.Id);
            storedInventory.Name = inventory.Name;
            storedInventory.Brand = inventory.Brand;
            storedInventory.ItemModel = inventory.ItemModel;
            storedInventory.SerialNo = inventory.SerialNo;
            storedInventory.Location = inventory.Location;
            storedInventory.UserId = inventory.UserId;
            storedInventory.Quantity = inventory.Quantity;
            return storedInventory;
        }
        #endregion

        #region Project

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.Where(p => !p.IsDeleted).OrderByDescending(p => p.CreatedDate).ToListAsync();
        }
        public async Task<int> GetProjectsCountAsync()
        {
            return await _db.Projects.Where(p => !p.IsDeleted).CountAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _db.Projects.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CloseProject(int id)
        {
            var project = await GetProjectByIdAsync(id);
            project.IsClosed = true;
            _db.Entry(project).State = EntityState.Modified;
        }

        public async Task<Project> AddProjectAsync(Project project, string userId)
        {
            var key = await GenerateProjectNumber();
            project.ProjectNo = key.Reference;
            project.PJNumber = key.Number;

            _db.UserId = userId;
            var result = _db.Projects.Add(project);
            return result;
        }

        private async Task<ReferenceModel> GenerateProjectNumber()
        {
            var keys = new ReferenceModel();

            var latestProject = await _db.Projects.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefaultAsync();
            if (latestProject != null && latestProject.PJNumber > 0)
            {
                int multi = 1;
                multi += latestProject.PJNumber;
                keys.Reference = "PROJECT No. " + multi.ToString();
                keys.Number = multi;
            }
            else
            {
                keys.Reference = "PROJECT No. 1";
                keys.Number = 1;
            }
            return keys;
        }

        public async Task<List<BudgetRequest>> GetBudGetRequestsByProjectIdAndUserIdAsync(int projectid, string userId)
        {
            return await _db.BudgetRequests.Where(p => p.ProjectId == projectid && p.CreatedBy == userId && p.IsDeleted == false).ToListAsync();
        }

        public async Task<List<BudgetRequest>> GetBudGetRequestsByUserIdAsync(string userId)
        {
            return await _db.BudgetRequests.Where(p => p.CreatedBy == userId && p.IsDeleted == false).OrderByDescending(p => p.CreatedDate).ToListAsync();
        }

        public async Task<int> GetBudGetPeddingRequestsCountByUserIdAsync(string userId)
        {
            return await _db.BudgetRequests.Where(p => p.CreatedBy == userId && p.IsDeleted == false && (p.StatusRecommendingApproval == false || p.StatusApproval == false || p.StatusRelease == false) && p.IsDisapproved == false).CountAsync();
        }
        public async Task<List<BudgetRequest>> GetBudGetRequestsApprovedNotificationByUserIdAsync(string userId)
        {
            return await _db.BudgetRequests.Where(p => p.CreatedBy == userId && p.IsDeleted == false && p.NotifyRequestor == true).ToListAsync();
        }

        public async Task<List<BudgetRequest>> GetBudGetRequestsByProjectIdAsync(int projectid)
        {
            return await _db.BudgetRequests.Where(p => p.ProjectId == projectid && p.IsDeleted == false).ToListAsync();
        }

        public async Task<List<BudgetRequest>> GetBudGetRequestsForOMAsync()
        {
            var project = await _db.BudgetRequests.Where(p => p.IsDeleted == false && p.StatusRecommendingApproval == true && p.StatusApproval == false && p.StatusRelease == false && p.IsDisapproved == false).OrderByDescending(p => p.CreatedDate).ToListAsync();
            project.AddRange(await GetAdminBudgetRequestByIdAsync());
            project = project.OrderByDescending(p => p.CreatedDate).ToList();
            return project;
        }

        public async Task<List<BudgetRequest>> GetBudGetRequestsForTHAsync(string userId)
        {
            var result = (from pro in _db.Proposals.Where(p => p.THId == userId)
                          join prj in _db.Projects on pro.Id equals prj.ProposalId
                          join bud in _db.BudgetRequests on prj.Id equals bud.ProjectId
                          where bud.StatusRecommendingApproval == false && bud.StatusApproval == false && bud.StatusRelease == false && bud.IsDisapproved == false
                          select bud
                          ).OrderByDescending(p => p.CreatedDate).ToListAsync();

            return await result;
        }

        public async Task<List<BudgetRequest>> GetBudGetRequestsForAHAsync()
        {
            return await _db.BudgetRequests.Where(p => p.IsDeleted == false && p.StatusRecommendingApproval == true && p.StatusApproval == true && p.StatusRelease == false && p.IsDisapproved == false).OrderByDescending(p => p.CreatedDate).ToListAsync();
        }

        public async Task<BudgetRequest> GetBudgetRequestByIdAsync(int id)
        {
            return await _db.BudgetRequests.Where(b => b.Id == id && b.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<List<BudgetRequest>> GetAdminBudgetRequestByIdAsync()
        {
            return await _db.BudgetRequests.Where(b => b.ProjectNumber == "ADMIN" && b.IsDeleted == false && b.StatusRecommendingApproval == false && b.StatusApproval == false && b.StatusRelease == false && b.IsDisapproved == false).OrderByDescending(b => b.CreatedDate).ToListAsync();
        }

        public async Task<int> GetToBeApprovedRequestsCountAsync(string role, string userId)
        {
            int count = 0;
            if (role == "OM")
            {
                var om = await GetBudGetRequestsForOMAsync();
                count = om.Count;
            }
            else if (role == "TH")
            {
                var th = await GetBudGetRequestsForTHAsync(userId);
                count = th.Count;
            }
            else if (role == "AH")
            {
                var ah = await GetBudGetRequestsForAHAsync();
                count = ah.Count;
            }
            return count;
        }

        public async Task ApprovedBudgetRequest(int id, string role)
        {
            var request = await GetBudgetRequestByIdAsync(id);
            if (role == "OM")
            {
                if (request.ProjectNumber == "ADMIN")
                {
                    request.StatusRecommendingApproval = true;
                    request.StatusApproval = true;
                    request.NotifyRequestor = true;
                }

                request.StatusApproval = true;
                request.NotifyRequestor = true;
            }
            else if (role == "TH")
            {
                request.StatusRecommendingApproval = true;
                request.NotifyRequestor = true;
            }
            else if (role == "AH")
            {
                request.StatusRelease = true;
                request.NotifyRequestor = true;
                if (request.ProjectNumber != "ADMIN")
                {
                    await ProcessApprovedRequest(request);
                }
            }
            _db.Entry(request).State = EntityState.Modified;
        }

        public async Task RequestorNotified(int id)
        {
            var request = await GetBudgetRequestByIdAsync(id);
            request.NotifyRequestor = false;
            _db.Entry(request).State = EntityState.Modified;
        }

        private async Task ProcessApprovedRequest(BudgetRequest request)
        {
            var project = await GetProjectByIdAsync(request.ProjectId.Value);
            project.RemainingBudget = project.RemainingBudget - request.TotalAmount;
            if (request.BudgetRequestItems.Count > 0)
            {
                foreach (var item in request.BudgetRequestItems)
                {
                    if (item.Amount > 0)
                    {
                        if (item.Category == "CONTRACTORS/OUTSOURCE")
                        {
                            var result = await GetProposalContractorByIdAsync(item.ItemId);
                            if (result != null)
                            {
                                result.RemainingBudget = result.RemainingBudget - item.Amount;
                                _db.Entry(result).State = EntityState.Modified;
                            }
                        }
                        else if (item.Category == "OPERATING EXPENSES")
                        {
                            var result = await GetProposalExpenseByIdAsync(item.ItemId);
                            if (result != null)
                            {
                                result.RemainingBudget = result.RemainingBudget - item.Amount;
                                _db.Entry(result).State = EntityState.Modified;
                            }
                        }
                        else if (item.Category == "EQUIPMENT")
                        {
                            var pEquip = await GetProposalEquipmentByIdAsync(item.ItemId);
                            var result = await GetInventoryByIdAsync(pEquip.InventoryId);
                            if (result != null)
                            {
                                pEquip.RemainingBudget = pEquip.RemainingBudget - item.Amount;
                                _db.Entry(result).State = EntityState.Modified;
                            }
                        }
                        else if (item.Category == "LABORATORY ANALYSIS")
                        {
                            var result = await GetProposalLaboratoryByIdAync(item.ItemId);
                            if (result != null)
                            {
                                result.RemainingBudget = result.RemainingBudget - item.Amount;
                                _db.Entry(result).State = EntityState.Modified;
                            }
                        }
                        else if (item.Category == "COMMISSIONS/REPRESENTATIONS")
                        {
                            var result = await GetProposalCommissionByIdAsync(item.ItemId);
                            if (result != null)
                            {
                                result.RemainingBudget = result.RemainingBudget - item.Amount;
                                _db.Entry(result).State = EntityState.Modified;
                            }
                        }
                    }
                }
            }
            _db.Entry(project).State = EntityState.Modified;
        }
        #endregion

        #region Private Class
        private class ReferenceModel
        {
            public string Reference { get; set; }
            public int Number { get; set; }
        }
        #endregion

        #region BudgetRequest
        public async Task<List<BudgetRequest>> GetBudGetRequests()
        {
            return await _db.BudgetRequests.Where(x => x.IsDeleted == false).OrderByDescending(c => c.CreatedDate).ToListAsync();
        }
        public void AddBudGetRequestItem(BudgetRequestItem budgetRequestItem, string userId)
        {
            _db.UserId = userId;
            var result = _db.BudgetRequestItems.Add(budgetRequestItem);
        }

        public async Task<BudgetRequest> AddBudGetRequest(BudgetRequest budgetRequest, string userId)
        {
            if (budgetRequest.ProjectNumber == "ADMIN")
            {
                budgetRequest.ProjectId = null;
            }
            var key = await GenerateBudgetRequestNumber();
            budgetRequest.RequestNumber = key.Reference;
            budgetRequest.RNumber = key.Number;

            _db.UserId = userId;
            var result = _db.BudgetRequests.Add(budgetRequest);
            return result;
        }

        private async Task<ReferenceModel> GenerateBudgetRequestNumber()
        {
            var keys = new ReferenceModel();

            var latestBR = await _db.BudgetRequests.Where(m => !m.IsDeleted).OrderByDescending(d => d.CreatedDate).FirstOrDefaultAsync();
            if (latestBR != null && latestBR.RNumber > 0)
            {
                int multi = 1;
                multi += latestBR.RNumber;
                keys.Reference = "BR-" + multi.ToString();
                keys.Number = multi;
            }
            else
            {
                keys.Reference = "BR-1";
                keys.Number = 1;
            }
            return keys;
        }

        public async Task DisapprovedBudgetRequest(int id, string note, string userName, string userRole)
        {
            var request = await GetBudgetRequestByIdAsync(id);
            request.IsDisapproved = true;
            request.DisapprovedNote = note;
            request.DisapprovedBy = userName;
            request.DisapproverRole = userRole;
            _db.Entry(request).State = EntityState.Modified;
        }
        #endregion

        #region Comment
        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _db.Comments.Where(x => x.IsDeleted == false).OrderByDescending(c => c.CreatedDate).Take(100).ToListAsync();
        }
        public async Task<List<SubComment>> GetAllSubCommentsByCommentIdAsync(int commentId)
        {
            return await _db.SubComments.Where(x => x.IsDeleted == false).OrderByDescending(c => c.CreatedDate).Take(100).ToListAsync();
        }
        public void AddComments(Comment comment, string userId)
        {
            _db.UserId = userId;
            var result = _db.Comments.Add(comment);
        }
        public void AddSubComments(SubComment comment, string userId)
        {
            _db.UserId = userId;
            var result = _db.SubComments.Add(comment);
        }
        public async Task DeleteComments(List<Comment> comments, string userId)
        {
            foreach(var comment in comments)
            {
                var result = await _db.Comments.Where(c => c.Id == comment.Id).FirstOrDefaultAsync();
                _db.UserId = userId;
                _db.Comments.Remove(result);
            }
        }
        public void DeleteSubComments(List<SubComment> comments, string userId)
        {
            foreach (var comment in comments)
            {
                _db.UserId = userId;
                _db.SubComments.Remove(comment);
            }
        }
        public void DeleteComment(Comment comment, string userId)
        {
            _db.UserId = userId;
            _db.Comments.Remove(comment);
        }
        public void DeleteSubComment(SubComment comment, string userId)
        {
            _db.UserId = userId;
            _db.SubComments.Remove(comment);
        }
        #endregion
    }
}
