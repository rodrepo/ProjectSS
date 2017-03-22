using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Common;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using ProjectSS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace ProjectSS.Web.Controllers.Admin
{
    [Authorize]
    public class ProposalController : BaseController
    {
        public ProposalController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        #region Get
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = _mapper.Map<List<ProposalViewModel>>(await _repo.GetProposalAsync());
            await SetListItemsAsync(new ProposalViewModel());
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Manage(int id, string from)
        {
            var getValues = await _repo.GetProposalByIdAsync(id);
            var proposal = _mapper.Map<ProposalViewModel>(getValues);
            if (proposal != null)
            {
                if (getValues.ProposalStaffs?.Any() ?? false)
                {
                    proposal.Staffs = _mapper.Map<List<ProposalStaffModel>>(getValues.ProposalStaffs);
                }
                if (getValues.ProposalContractors?.Any() ?? false)
                {
                    proposal.Contractors = _mapper.Map<List<ProposalContractorModel>>(getValues.ProposalContractors);
                }
                if (getValues.ProposalExpenses?.Any() ?? false)
                {
                    proposal.Expenses = _mapper.Map<List<ProposalExpenseModel>>(getValues.ProposalExpenses);
                }
                if (getValues.ProposalEquipments?.Any() ?? false)
                {
                    proposal.Equipments = _mapper.Map<List<ProposalEquipmentModel>>(getValues.ProposalEquipments);
                }
                if(getValues.ProposalLaboratories?.Any() ?? false)
                {
                    proposal.Laboratories = _mapper.Map<List<ProposalLaboratoryModel>>(getValues.ProposalLaboratories);
                }
                if (getValues.ProposalCommissions?.Any() ?? false)
                {
                    proposal.Commissions = _mapper.Map<List<ProposalCommissionModel>>(getValues.ProposalCommissions);
                }
                var mergeValues = await MergeToProposal(proposal);
                await DropdownListForUsers();
                await SetListItemsAsync(mergeValues);
                mergeValues.From = from;
                return View(mergeValues);
            }
            proposal.From = from;
            return View(proposal);
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteStaff(int id , int proposalId)
        {
            try
            {
                await _repo.DeleteProposalStaff(id);
                if (await _repo.SaveAllAsync())
                {   
                    TempData["Success"] = $"Successfully deleted staff";
                }
                else
                {
                    TempData["Error"] = "Unable to delete staff due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId, @from = "staff" });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLaboratory(int id, int proposalId)
        {
            try
            {
                await _repo.DeleteProposalLaboratory(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted laboratory";
                }
                else
                {
                    TempData["Error"] = "Unable to delete laboratory due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId , @from = "laboratory" });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _repo.DeleteProposal(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted proposal";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Unable to delete proposal due to some internal issues.";
                    return RedirectToAction("Manage", new { @id = id });
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContractor(int id , int proposalId)
        {
            try
            {
                await _repo.DeleteProposalContractor(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted contactor/outsource";
                }
                else
                {
                    TempData["Error"] = "Unable to delete contactor/outsource due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId , @from = "contractor" });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteExpense(int id, int proposalId)
        {
            try
            {
                await _repo.DeleteProposalExpense(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted operating expense";
                }
                else
                {
                    TempData["Error"] = "Unable to delete operating expense due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId , @from = "operating" });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteEquipment(int id, int proposalId)
        {
            try
            {
                await _repo.DeleteProposalEquipment(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted equipment";
                }
                else
                {
                    TempData["Error"] = "Unable to delete equipment due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId , @from = "equipment" });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCommission(int id, int proposalId)
        {
            try
            {
                await _repo.DeleteProposalCommission(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted commission/representation";
                }
                else
                {
                    TempData["Error"] = "Unable to delete commission/representation due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId ,@from = "commissions" });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        #endregion

        #region Create

        [HttpPost]
        public async Task<ActionResult> Manage(ProposalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region String Convertion
                    model.Cost = ConvertStringToDecimal(model.SCost);
                    model.NegotiationAllowance = ConvertStringToDecimal(model.SNegotiationAllowance);
                    #endregion
                    await _repo.UpdateProposal(_mapper.Map<Proposal>(model), CurrentUser.Id);
                    if (await _repo.SaveAllAsync())
                    {
                        TempData["Success"] = string.Format("CRM has been successfully Updated");
                        return RedirectToAction("Manage", new { id = model.Id });
                    }
                }
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;
                        return RedirectToAction("Manage", new { id = model.Id });
                    }
                }
                return RedirectToAction("Manage", new { id = model.Id });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateStaff(ProposalStaffModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalStaff(_mapper.Map<ProposalStaff>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {

                    TempData["Success"] = string.Format("Staff has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId , @from ="staff"});
                }
                TempData["Error"] = "Unable to create Staff due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateContractor(ProposalContractorModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalContractor(_mapper.Map<ProposalContractor>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Contractor/Outsource has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId , @from = "contractor" });
                }
                TempData["Error"] = "Unable to create Contractor/Outsource due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateExpense(ProposalExpenseModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalExpenses(_mapper.Map<ProposalExpense>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Operating expense has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId , @from = "operating" });
                }
                TempData["Error"] = "Unable to create operating expense due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateEquipment(ProposalEquipmentModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalEquipment(_mapper.Map<ProposalEquipment>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Equipment has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId ,@from = "equipment" });
                }
                TempData["Error"] = "Unable to create equipment due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateCommission(ProposalCommissionModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalCommission(_mapper.Map<ProposalCommission>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Cmmission/Representations has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId ,@from = "commissions" });
                }
                TempData["Error"] = "Unable to create Cmmission/Representations due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateLaboratory(ProposalLaboratoryModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalLaboratory(_mapper.Map<ProposalLaboratory>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Laboratory has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId , @from = "laboratory" });
                }
                TempData["Error"] = "Unable to create laboratory due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProposalViewModel model)
        {
            if (model.CRMId != 0)
            {
                var proposal = await _repo.AddPorposalAsync(_mapper.Map<Proposal>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {

                    TempData["Success"] = string.Format("Proposal has been successfully Created");
                    return RedirectToAction("Manage", new { @id = proposal.Id });
                }
                TempData["Error"] = "Unable to create Proposal due to some internal issues.";
            }
            return RedirectToAction("Index");

        }
        #endregion

        #region Private Helpers

        #region Mapping
        private async Task<ProposalViewModel> MergeToProposal(ProposalViewModel model)
        {
            if (model != null)
            {
                if (model.Cost > 0)
                {
                    model.MangementFeeBilledToClient = model.Cost * decimal.Parse("0.04") ;
                }
                model.SCost = ConvertDecimalToPesos(model.Cost);
                model.SNegotiationAllowance = ConvertDecimalToPesos(model.NegotiationAllowance);

                #region Staff
                var result = model.Staffs.Where(s => !s.IsDeleted).ToList();
                if (result?.Any() ?? false)
                {
                    result = await MapNeedFieldForProposalStaff(result);
                    if (result?.Any() ?? false)
                    {
                        model.TotalStaffBilledToClient = result.Select(m => m.BilledToClient).Sum();
                        model.TotalStaffDirectCost = result.Select(m => m.DirectCost).Sum();
                    }
                }
                model.Staffs = result;
                #endregion

                #region Contractor
                var contractorResult = model.Contractors.Where(c => !c.IsDeleted).ToList();
                if (contractorResult?.Any() ?? false)
                {
                    contractorResult = MapNeedFieldForProposalContractor(contractorResult);
                    model.TotalContractorBilledToClient = contractorResult.Select(m => m.BilledToClient).Sum();
                    model.TotalContractorDirectCost = contractorResult.Select(m => m.DirectCost).Sum();
                }
                model.Contractors = contractorResult;
                #endregion

                #region Expense
                var expenseResult = model.Expenses.Where(c => !c.IsDeleted).ToList();
                if (expenseResult?.Any() ?? false)
                {
                    expenseResult = MapNeedFieldForProposalExpense(expenseResult);
                    model.TotalExpenseBilledToClient = expenseResult.Select(m => m.BilledToClient).Sum();
                    model.TotalExpenseDirectCost = expenseResult.Select(m => m.DirectCost).Sum();
                }
                model.Expenses = expenseResult;
                #endregion

                #region Laboratory
                var laboratoryResult = model.Laboratories.Where(l => !l.IsDeleted).ToList();
                if(laboratoryResult?.Any() ?? false)
                {
                    laboratoryResult = MapNeedFieldForLaboratoryModel(laboratoryResult);
                    model.TotalLaboratoryBilledToClient = laboratoryResult.Select(l => l.BilledToClient).Sum();
                    model.TotalLaboratoryDirectCost = laboratoryResult.Select(l => l.DirectCost).Sum();
                }
                #endregion

                #region Commision
                var commissionResult = model.Commissions.Where(l => !l.IsDeleted).ToList();
                if (commissionResult?.Any() ?? false)
                {
                    commissionResult = MapNeedFieldForCommissionModel(commissionResult);
                    model.TotalCommissionBilledToClient = commissionResult.Select(l => l.BilledToClient).Sum();
                    model.TotalCommissionDirectCost = commissionResult.Select(l => l.Cost).Sum();
                }
                #endregion

                #region Equipment
                var equipmentResult = model.Equipments.Where(c => !c.IsDeleted).ToList();
                if (equipmentResult?.Any() ?? false)
                {
                    equipmentResult = await MapNeedFieldForEquipmentModel(equipmentResult);
                    model.TotalEquipmentBilledToClient = equipmentResult.Select(m => m.BilledToClient).Sum();
                    model.TotalEquipmentDirectCost = equipmentResult.Select(m => m.DirectCost).Sum();
                }
                model.Expenses = expenseResult;
                #endregion

                #region GrandTotal
                model.DirectCost = model.TotalLaboratoryDirectCost + model.TotalExpenseDirectCost + model.TotalContractorDirectCost + model.TotalStaffDirectCost + model.TotalCommissionDirectCost + model.TotalEquipmentDirectCost;
                model.CostWithFactor = model.TotalLaboratoryBilledToClient + model.TotalExpenseBilledToClient + model.TotalContractorBilledToClient + model.MangementFeeBilledToClient + model.TotalStaffBilledToClient + model.TotalCommissionBilledToClient + model.TotalEquipmentBilledToClient;
                model.OtherRevenues = model.Cost - model.CostWithFactor - model.MangementFeeBilledToClient;
                model.TotalBilledToClient = model.CostWithFactor + model.NegotiationAllowance + model.OtherRevenues + model.MangementFeeBilledToClient;
                model.Vat = model.Cost * decimal.Parse("0.12");
                model.TotalRevenue = model.TotalBilledToClient - model.DirectCost;
                if (model.TotalBilledToClient > 0 && model.DirectCost > 0)
                {
                    model.NetFactor = model.TotalBilledToClient / model.DirectCost;
                }
                var user = await _repo.GetUserByIdAsync(model.CreatedBy);
                if(user != null)
                {
                    model.CreatedByName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                }
                var usermodified = await _repo.GetUserByIdAsync(model.ModifiedBy);
                if (user != null)
                {
                    model.ModifiedByName = usermodified.FirstName + " " + usermodified.MiddleName + " " + usermodified.LastName;
                }
                else
                {
                    model.ModifiedByName = "Never been modified";
                }
                #endregion
            }
            return model;
        }

        private List<ProposalCommissionModel> MapNeedFieldForCommissionModel(List<ProposalCommissionModel> commissions)
        {
            if (commissions?.Any() ?? false)
            {
                foreach (var commission in commissions)
                {
                    if (commission != null)
                    {
                        commission.BilledToClient = commission.Cost * commission.Factor;
                    }
                }
            }
            return commissions;
        }

        private List<ProposalLaboratoryModel> MapNeedFieldForLaboratoryModel(List<ProposalLaboratoryModel> laboratories)
        {
            if (laboratories?.Any() ?? false)
            {
                foreach (var laboratory in laboratories)
                {
                    if (laboratory != null)
                    {
                        laboratory.DirectCost = laboratory.Cost * laboratory.NoOfStations * int.Parse(laboratory.Replicate);
                        laboratory.BilledToClient = laboratory.DirectCost * laboratory.Factor;
                    }
                }
            }
            return laboratories;
        }

        private async Task<List<ProposalEquipmentModel>> MapNeedFieldForEquipmentModel(List<ProposalEquipmentModel> equipments)
        {
            if (equipments?.Any() ?? false)
            {
                foreach (var equipment in equipments)
                {
                    if (equipment != null)
                    {
                        if(equipment.InventoryId > 0)
                        {
                            var inventory = await _repo.GetInventoryByIdAsync(equipment.InventoryId);
                            equipment.Name = inventory.Name;
                        }
                        equipment.TotalHours = equipment.Hours * equipment.Months;
                        equipment.DirectCost = equipment.Rate * equipment.TotalHours;
                        equipment.BilledToClient = equipment.DirectCost * equipment.Factor;
                    }
                }
            }
            return equipments;
        }

        private List<ProposalExpenseModel> MapNeedFieldForProposalExpense(List<ProposalExpenseModel> expenses)
        {
            if (expenses?.Any() ?? false)
            {
                foreach (var expense in expenses)
                {
                    if (expense != null)
                    {
                        expense.BilledToClient = expense.Quantity * expense.NoOfDay * expense.DirectCost * expense.Factor;
                    }
                }
            }
            return expenses;
        }

        private List<ProposalContractorModel> MapNeedFieldForProposalContractor(List<ProposalContractorModel> contractors)
        {
            if (contractors?.Any() ?? false)
            {
                foreach (var contractor in contractors)
                {
                    if (contractor != null)
                    {
                        contractor.DirectCost = contractor.NoOfDay * contractor.Rate;
                        contractor.BilledToClient = contractor.DirectCost * contractor.Factor;
                    }
                }
            }
            return contractors;
        }

        private async Task<List<ProposalStaffModel>> MapNeedFieldForProposalStaff(List<ProposalStaffModel> staffs)
        {
            if (staffs?.Any() ?? false)
            {
                foreach (var staff in staffs)
                {
                    var user = await _repo.GetUserByIdAsync(staff.UserId);
                    if (user != null)
                    {
                        if (staff != null && !staff.UserId.IsEmpty())
                        {

                            staff.Name = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                            staff.BillingRate = user.Rate;
                            staff.TotalManHours = staff.ManHours * staff.ManMonths;
                            staff.DirectCost = user.Rate * staff.TotalManHours;
                            staff.BilledToClient = staff.DirectCost * staff.Factor;
                        }
                    }
                    else
                    {
                        if (staff.Id > 0)
                        {
                            await _repo.DeleteProposalStaff(staff.Id);
                            await _repo.SaveAllAsync();
                        }
                    }
                }
            }
            return staffs;
        }

        #endregion

        #region Approved
        public async Task<ActionResult> Approved(int id)
        {
            if (id > 0)
            {
                var model = await _repo.GetProposalByIdAsync(id);
                if(model.Cost <= 0)
                {
                    TempData["Error"] = "Please add Project Cost";
                    return RedirectToAction("Manage", new { @id = id });
                }
                if (User.IsInRole(RoleType.OM.ToString()))
                {
                    model.Status = "Approved";
                    var project = new Project
                    {
                        CRMId = model.CRMId,
                        ProposalId = id
                    };
                    await _repo.AddProjectAsync(project, CurrentUser.Id);
                    await _repo.UpdateProposal(_mapper.Map<Proposal>(model), CurrentUser.Id);
                    if (await _repo.SaveAllAsync())
                    {
                        TempData["Success"] = string.Format("Proposal has been successfully Updated");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Unable to update due to some internal issues.";
                        return RedirectToAction("Manage", new { @id = id });
                    }
                }

                else
                {
                    TempData["Error"] = "Access denied";
                    return RedirectToAction("Manage", new { @id = id });
                }
            }
            else
            {
                TempData["Error"] = "Unable to update due to some internal issues.";
                return RedirectToAction("Manage", new { @id = id });
            }
      
        }
        #endregion

        #endregion

        #region SetListItems
        private async Task SetListItemsAsync(ProposalViewModel model)
        {
            ViewBag.CRMs = ViewBag.CRMs ?? await GetCRMsAsync(model.CRMId);
            ViewBag.BDs = ViewBag.BDs ?? await GetBDUsersAsync(model.BDId);
            ViewBag.TSs = ViewBag.TSs ?? await GetTSUsersAsync(model.TSId);
            ViewBag.THs = ViewBag.THs ?? await GetTHUsersAsync(model.THId);
        }

        private async Task DropdownListForUsers()
        {
            ViewBag.Users = ViewBag.Users ?? await GetUsersAsync();
            ViewBag.Inventories = ViewBag.Inventories ?? await GetInventoriesAsync();
        }
        #endregion

    }
}