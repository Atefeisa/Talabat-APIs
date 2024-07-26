using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Extenstions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;
using Talabat.Service;

namespace Talabat.APIs
{
    public class Program
    {
        // Entry Point
        public static async Task Main(string[] args)
        {



            #region  1      /-    Configure Services



            var webApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.




            webApplicationbuilder.Services.AddControllers(); // Register Web Api Bult-in Services at the container


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationbuilder.Services.AddEndpointsApiExplorer();



            webApplicationbuilder.Services.AddSwaggerGen();




            webApplicationbuilder.Services.AddDbContext<StoreDbContext>(options =>
            {

                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));

            });



			webApplicationbuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
			{

				options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("IdentityConnection"));

			});



			webApplicationbuilder.Services.AddSingleton<IConnectionMultiplexer>(  (serverProvider) =>
            {

                var connection = webApplicationbuilder.Configuration.GetConnectionString("Redis");

				return ConnectionMultiplexer.Connect(connection);
            });



			// App Services
            webApplicationbuilder.Services.AddAppLicationsServices();


			// Identity Services
			webApplicationbuilder.Services.AddIdentityServices(webApplicationbuilder.Configuration);
         


			webApplicationbuilder.Services.AddCors(options =>
			{

				options.AddPolicy("MyPolicy", config =>
				{
					config.AllowAnyHeader();
					config.AllowAnyMethod();
					config.WithOrigins(webApplicationbuilder.Configuration["FrontEndURL"]);


				});

			});



			#endregion



		   var app = webApplicationbuilder.Build();



			#region   2       /-       Update - Database






			using var scope = app.Services.CreateScope();        // Group Of Services

			var services = scope.ServiceProvider;


			// 1 -
			var _context = services.GetRequiredService<StoreDbContext>();
			//ASk CLR to Create object from StoreDbContext Explicitly


			// 2 -
			var _IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
			//ASk CLR to Create object from AppIdentityDbContext Explicitly


			var loggerFactory = services.GetRequiredService<ILoggerFactory>();



			try
			{
				// 1 - Business

				await _context.Database.MigrateAsync(); // Update Database  [ Business ]

				await StoreDbContextSeed.SeedAsync(_context); // Data Seeding



				// 2 - Identity

				await _IdentityDbContext.Database.MigrateAsync(); // Update Database [ Identity ]


				var _userManger = services.GetRequiredService<UserManager<AppUser>>();
				//ASk CLR to Create object from UserManager<AppUser> Explicitly

				await AppIdentityDbContextSeed.SeedUsersAsync(_userManger);



			}


			catch (Exception ex)
			{

				var logger = loggerFactory.CreateLogger<Program>();

				logger.LogError(ex, "An Error han been occured during appling the Migrations");


				//Console.WriteLine(ex.Message);
			}





			#endregion







			#region   3       /-      Configure Kestrel Pipline

			
            
            // Configure the HTTP request pipeline.
			
            
            app.UseMiddleware<ExceptionMiddleware>();


			if (app.Environment.IsDevelopment())
            { 
                app.UseSwagger();  // Middileware

                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");   // Redirect 

            app.UseHttpsRedirection();

            app.UseStaticFiles() ;


			app.UseCors("MyPolicy");


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion



            app.Run();


        }
    }
}
