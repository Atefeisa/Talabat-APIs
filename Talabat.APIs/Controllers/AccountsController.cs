using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extenstions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : ApiBaseController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountsController(
								  UserManager<AppUser> userManager,
								  SignInManager<AppUser> signInManager,
								  ITokenService tokenService,
								  IMapper mapper
								 )
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}




		#region  1 -  REGISTER

		// 1 -   Register 


		[HttpPost("Register")]   //POST : /api/accounts/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{


			if (CheckEmailExists(model.Email).Result.Value)
			{
				return BadRequest(new ApiResponse(400, "Email Is Already Exists !"));
			}



			var user = new AppUser()
			{
				DispalyName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.PhoneNumber,

			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
				return BadRequest(new ApiResponse(400));


			var returnedUser = new UserDto()
			{
				DisplayName = user.DispalyName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};

			return Ok(returnedUser);
		}



		#endregion




		#region 2 -  LOG IN


		//  2 - Log In


		[HttpPost("Login")]   //POST : /api/accounts/login

		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null)
				return Unauthorized(new ApiResponse(401));

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded)
				return Unauthorized(new ApiResponse(401));

			var returnedUser = new UserDto()
			{
				DisplayName = user.DispalyName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};

			return Ok(returnedUser);

		}

		#endregion







		#region  3   -   Get Current User 


		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

		[Authorize]
		[HttpGet("GetCurrentUser")]                       //GET :  /api/accounts/GetCurrentUser
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{

			var Email = User.FindFirstValue(ClaimTypes.Email);



			var user = await _userManager.FindByEmailAsync(Email);


			var returnUser = new UserDto()
			{

				DisplayName = user.DispalyName,

				Email = user.Email,

				Token = await _tokenService.CreateTokenAsync(user, _userManager)

			};

			return Ok(returnUser);



		}




		#endregion





		#region  4   -   Get Current UserAddress


		[Authorize]
		[HttpGet("CurrentUserAddress")]          //GET :  /api/accounts/CurrentUserAddress
		public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
		{
			var user = await _userManager.FindUserWithAddressAsync(User);

			var mappedAddress = _mapper.Map<Address, AddressDto>(user.Address);


			return Ok(mappedAddress);

		}



		#endregion





		#region  5   -   Update User Address



		[Authorize]
		[HttpPut("address")]            // PUT :  /api/accounts/address
		public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto model)
		{
			var user = await _userManager.FindUserWithAddressAsync(User);

			var address = _mapper.Map<AddressDto, Address>(model);

			user.Address = address;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
				return BadRequest(new ApiResponse(400));

			return Ok(model);

		}





		#endregion




		#region   6  -  Check User ? 




		[HttpGet("emailExists")]          // GET : /api/accounts/emailexists?email=
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			//var user = await _userManager.FindByEmailAsync(email);
			//if (user is null) return false;
			//return true;

			return await _userManager.FindByEmailAsync(email) is not null;

		}


		#endregion





















	}
}
