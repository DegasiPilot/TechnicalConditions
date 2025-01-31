using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechnicalConditions.Components;
using TechnicalConditions.Components.Account;
using TechnicalConditions.Data;
using TechnicalConditions.Services;

namespace TechnicalConditions
{
	public class Program
	{
		private const string Admin = "Administrator";

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();

			builder.Services.AddCascadingAuthenticationState();
			builder.Services.AddScoped<IdentityUserAccessor>();
			builder.Services.AddScoped<IdentityRedirectManager>();
			builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

			builder.Services.AddAuthentication(options =>
				{
					options.DefaultScheme = IdentityConstants.ApplicationScheme;
					options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
				})
				.AddIdentityCookies();

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddIdentityCore<ApplicationUser>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireNonAlphanumeric = false;
			}
			)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddSignInManager()
				.AddDefaultTokenProviders();

			builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IDocumentService, DBDocumentService>();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			// Add additional endpoints required by the Identity /Account Razor components.
			app.MapAdditionalIdentityEndpoints();

			app.MapGet("/files/{id:int}", DownloadFile).RequireAuthorization();

			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

				// Создаем роль Администратор
				if (!roleManager.RoleExistsAsync(Admin).Result)
				{
					roleManager.CreateAsync(new IdentityRole(Admin)).Wait();
				}

				// Назначаем роль администратору
				var adminUser = userManager.FindByEmailAsync("timurnig2@gmail.com").Result;
				if (adminUser != null && !userManager.IsInRoleAsync(adminUser, Admin).Result)
				{
					userManager.AddToRoleAsync(adminUser, Admin).Wait();
				}
			}

			app.Run();
		}

		private static async Task<IResult> DownloadFile(int id, ApplicationDbContext dbContext, ClaimsPrincipal userClaims, IDocumentService documentService, UserManager<ApplicationUser> userManager)
		{
			if (userClaims.Identity == null || !userClaims.Identity.IsAuthenticated)
			{
				return Results.Unauthorized();
			}
			ApplicationUser? user = await userManager.GetUserAsync(userClaims);
			if(user == null)
			{
				return Results.Unauthorized();
			}

			var file = await dbContext.Documents.FindAsync(id);
			if (file == null)
			{
				return Results.NotFound("Файл не найден");
			}

			if(await userManager.IsInRoleAsync(user, Admin) == false)
			{
				Appeal? appeal = await dbContext.Appeals.FindAsync(file.AppealId);
				if (appeal == null || appeal.UserId != user.Id)
				{
					return Results.Forbid();
				}
			}

			if(file.ContentBytes.Length == 0)
			{
				return Results.BadRequest("Неправильный способ загрузки файла");
			}
			else
			{
				return documentService.SuccesDownloadFileResponse(file);
			}
		}
	}
}