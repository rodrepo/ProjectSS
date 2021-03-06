﻿using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using ProjectSS.Web.Models.admin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace ProjectSS.Web.Controllers.Admin
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ActionResult> Index()
        {
            await RunNotifications();
            var users = await _repo.GetUsersAsync();
            var model = _mapper.Map<List<UserViewModel>>(users);
            if (model != null)
            {
                foreach (var user in model)
                {
                    var roles = _mapper.Map<List<RoleViewModel>>(await _repo.GetRolesByUserId(user.Id));
                    if (roles != null)
                    {
                        user.Roles = roles;
                    }
                    foreach (var role in user.Roles)
                    {
                        role.Name = GetProperRoleName(role.Name);
                    }
                    if (user.Id == CurrentUser.Id)
                    {
                        user.IsCurrentUser = true;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await RunNotifications();
            var model = new UserViewModel();
            await SetListItemsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            try
            {
                var vuser = _mapper.Map<List<UserViewModel>>(await _repo.GetUsersAsync());
                if (vuser.Any(u => u.Email == model.Email))
                {
                    TempData["Error"] = "Error email address is already taken";
                    await SetListItemsAsync(model);
                    return View(model);
                }
                if (ModelState.IsValid)
                {
                    var user = MapUserViewModelToEntity(model);
                    var role = _repo.GetRoleById(model.RoleId);
                    string defaultPassword = "P@ssw0rd";
                    var result = await UserManager.CreateAsync(user, defaultPassword);
                    await UserManager.AddToRoleAsync(user.Id, role.Name);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = string.Format("Success create a User default password is P@ssw0rd");
                        return RedirectToAction("Index");
                    }
                }
                foreach (var v in ModelState.Values)
                {
                    foreach (var e in v.Errors)
                    {
                        TempData["Error"] = e.ErrorMessage;
                    }

                }
                await SetListItemsAsync(model);
                return View(model);
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                await RunNotifications();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await _repo.GetUserByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var model = _mapper.Map<UserViewModel>(user);
                var roles = _mapper.Map<List<RoleViewModel>>(await _repo.GetRolesByUserId(user.Id));
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        model.RoleId = role.Id;
                        break;
                    }
                }
                if (model.Id == CurrentUser.Id)
                {
                    model.IsCurrentUser = true;
                }
                await SetListItemsAsync(model);
                return View(model);
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
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var roles = _mapper.Map<List<RoleViewModel>>(await _repo.GetRolesByUserId(model.Id));
                    if (roles != null)
                    {
                        //Remove all roles in user
                        foreach (var ro in roles)
                        {
                            await UserManager.RemoveFromRolesAsync(model.Id, ro.Name);
                        }
                    }
                    var role = _repo.GetRoleById(model.RoleId);
                    var user = await _repo.UpdateUserAsync(_mapper.Map<User>(model), UserManager, role.Name.ToString(), GetUserId());
                    await _repo.SaveAllAsync();
                    TempData["Success"] = string.Format("User successfully updated");
                    if (model.AccountId != 0)
                    {
                        return RedirectToAction("AccountUsers", new { @accountId = model.AccountId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;

                    }
                }
                await SetListItemsAsync(model);
                return View(model);
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ActivateAndDeactivate(string id, bool isActivated)
        {
            try
            {
                await RunNotifications();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Get User
                var user = await _repo.GetUserByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    //Map user to model
                    var model = _mapper.Map<UserViewModel>(user);
                    var roles = _mapper.Map<List<RoleViewModel>>(await _repo.GetRolesByUserId(user.Id));
                    model.RoleId = roles.FirstOrDefault().Id;
                    if (model.Id == CurrentUser.Id)
                    {
                        model.IsCurrentUser = true;
                    }
                    //Set if its for Deactivation or Activation
                    if (isActivated == true)
                    {
                        model.IsActive = true;
                    }
                    else
                    {
                        model.IsActive = false;
                    }

                    if (roles != null)
                    {
                        //Remove all roles in user
                        foreach (var ro in roles)
                        {
                            await UserManager.RemoveFromRolesAsync(model.Id, ro.Name);
                        }
                    }
                    var role = _repo.GetRoleById(model.RoleId);
                    var resultUser = await _repo.UpdateUserAsync(_mapper.Map<User>(model), UserManager, role.Name.ToString(), GetUserId());
                    await _repo.SaveAllAsync();
                    if (isActivated == true)
                    {
                        TempData["Success"] = string.Format("User successfully Activated");
                    }
                    else
                    {
                        TempData["Success"] = string.Format("User successfully Deactivated");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await RunNotifications();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await _repo.GetUserByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var model = _mapper.Map<UserViewModel>(user);
                return View(model);
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(UserViewModel umodel)
        {
            try
            {
                var roles = _mapper.Map<List<RoleViewModel>>(await _repo.GetRolesByUserId(umodel.Id));
                string role = roles.FirstOrDefault().Name;
                if (await _repo.CanUserBeDeleted(umodel.Id, role))
                {

                    var model = await _repo.GetUserByIdAsync(umodel.Id);
                    _repo.DeleteUser(model, GetUserId());
                    if (await _repo.SaveAllAsync())
                    {
                        TempData["Success"] = $"Successfully deleted user";
                    }
                    else
                    {
                        TempData["Error"] = "Unable to delete user due to some internal issues.";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Unable to delete user, User is involved in an ongoing project";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        #region Helper

        private User MapUserViewModelToEntity(UserViewModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Birthday = model.Birthday,
                Mobile = model.Mobile,
                Rate = model.Rate,
                IsActive = model.IsActive,
            };
            return (user);
        }

        #endregion

        #region SetListItems
        private async Task SetListItemsAsync(UserViewModel model)
        {
            ViewBag.Roles = ViewBag.Roles ?? await GetRolesAsync(model.RoleId);
        }
        #endregion
    }
}