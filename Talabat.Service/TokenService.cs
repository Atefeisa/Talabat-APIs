using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interfaces;

namespace Talabat.Service
{
    public class TokenService : ITokenService
	{

		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
        {
			_configuration = configuration;
		}


        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
		{


			// 1 - Header 



			// 2 - Payload

			// 2 - 1 )  Private Claims ( name , id , email , ..... )

			var AuthClaims = new List<Claim>()
			{
					new Claim( ClaimTypes.GivenName,user.DispalyName ),

					new Claim( ClaimTypes.Email,user.Email )

			};



			var userRoles = await _userManager.GetRolesAsync(user);


			foreach (var Role in userRoles)
			{

			   AuthClaims.Add(new Claim(ClaimTypes.Role,Role));
			
			}





			//	// 3 - Signature

			//	// 3 - 1 )  KEY



			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));



			var token = new JwtSecurityToken(

             		issuer: _configuration["JWT:ValidIssuer"],

					audience: _configuration["JWT:ValidAudience"],

					expires: DateTime.Now.AddDays(double.Parse( _configuration["JWT:DurationInDays"] )),

					claims : AuthClaims ,
					signingCredentials : new SigningCredentials( AuthKey , SecurityAlgorithms.HmacSha256Signature ) 

				) ;


				return new JwtSecurityTokenHandler().WriteToken(token) ;	

        }












		










	}
}

