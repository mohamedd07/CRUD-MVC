using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var roles = await _roleManager.Roles.Select(U => new RoleViewModeL()
                {
                    Id = U.Id,
                   RoleName=U.Name,
                }).ToListAsync();

                return View(roles);
            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(SearchValue);
               if(Role is not null)
                {
                    var mappedRole = new RoleViewModeL()
                    {
                        Id = Role.Id,
                        RoleName = Role.Name,
                    };

                    return View(new List<RoleViewModeL>() { mappedRole });
                }

                return View(Enumerable.Empty<RoleViewModeL>());
            }

        }


        public IActionResult Create()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModeL roleVM)
        {
           if(ModelState.IsValid)
            {
                var mappRole = _mapper.Map<RoleViewModeL, IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mappRole);
                return RedirectToAction(nameof(Index));
            }
            return View();

        }




        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id == null)
                return BadRequest(); //400

            var User = await _roleManager.FindByIdAsync(id);


            if (User == null)
                return NotFound();//404

            RoleViewModeL mappeduser = _mapper.Map<IdentityRole, RoleViewModeL>(User);

            return View(viewName, mappeduser);

        }



        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModeL UpdatedUser)
        {


            if (id != UpdatedUser.Id)
                return BadRequest();


            if (ModelState.IsValid) //Server Side Validation
            {
                try
                {
                    var user = await _roleManager.FindByIdAsync(id);
                    user.Name = UpdatedUser.RoleName;
               
                    //user.Email = UpdatedUser.Email;
                    //user.SecurityStamp = Guid.NewGuid().ToString();


                    await _roleManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1.Log Exception
                    //2.Friend Message
                    ModelState.AddModelError(string.Empty, ex.Message);

                }

            }
            return View(UpdatedUser);
        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CDelete(string id)
        {


            try
            {
                var user = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");

            }

        }

    }
}
