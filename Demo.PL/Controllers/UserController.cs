using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
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
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationModel> _userManager;
		private readonly SignInManager<ApplicationModel> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationModel> userManager , SignInManager<ApplicationModel> signInManager , IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }

		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var users = await _userManager.Users.Select(U => new UserViewModel()
					{ 
					Id = U.Id,
					FName = U.Fname,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles =  _userManager.GetRolesAsync(U).Result
				}).ToListAsync(); 

				return View(users);
			}
			else
			{
				var User = await _userManager.FindByEmailAsync(SearchValue);
				var mappedUser = new UserViewModel()
				{
					Id = User.Id,
					FName = User.Fname,
					LName = User.LName,
					Email = User.Email,
					PhoneNumber = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result
				};

				return View(new List<UserViewModel>(){ mappedUser});

			}

		}


        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id == null)
                return BadRequest(); //400

            var User = await _userManager.FindByIdAsync(id);


            if (User == null)
                return NotFound();//404

            UserViewModel mappeduser = _mapper.Map<ApplicationModel, UserViewModel>(User);

            return View(viewName, mappeduser);

        }



        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel UpdatedUser)
        {
    

            if (id != UpdatedUser.Id)
                return BadRequest();


            if (ModelState.IsValid) //Server Side Validation
            {
                try
                {
                   var user = await _userManager.FindByIdAsync(id);
                    user.Fname = UpdatedUser.FName;
                    user.LName = UpdatedUser.LName;
                    user.PhoneNumber = UpdatedUser.PhoneNumber;
                    //user.Email = UpdatedUser.Email;
                    //user.SecurityStamp = Guid.NewGuid().ToString();


                    await _userManager.UpdateAsync(user);

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
            return  await Details(id, "Delete");

        }

        [HttpPost ]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CDelete(string id)
        {


            try
            {
                var user = await _userManager.FindByIdAsync(id);
            
                await _userManager.DeleteAsync(user);
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
