using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interfaces;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extenstions
{
    public static class IdentityServicesExtension
	{


		public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration configuration)
		{



			//   1   - Service [ TOKEN ]

			Services.AddScoped<ITokenService, TokenService>();




			////    2   - Identity
			

			Services.AddIdentity<AppUser, IdentityRole>()
				    .AddEntityFrameworkStores<AppIdentityDbContext>();




			//Services.AddIdentity<AppUser, IdentityRole>(options =>
			//{
			//	 options.Password.RequiredUniqueChars = 2;
			//	 options.Password.RequireDigit = true;

			//}).AddEntityFrameworkStores<AppIdentityDbContext>();




		// 	====>>  Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

			Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			})
				.AddJwtBearer(options =>
				{

					options.TokenValidationParameters = new TokenValidationParameters()
					{

						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],

						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],

						ValidateLifetime = true,

						ValidateIssuerSigningKey = true,

						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))


					};



				});

			return Services;

		}


	}
}
