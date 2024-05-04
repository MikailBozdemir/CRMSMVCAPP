
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllersWithViews();
		//builder.Services.AddMvc();

		//builder.Services.AddAuthentication(
		//	CookieAuthenticationDefaults.AuthenticationScheme)
		//	.AddCookie(x =>
		//	{
		//		x.LoginPath = "/Home/Index";
		//	});

		//builder.Services.AddMvc( config=>
		//{
		//	var policy =new AuthorizationPolicyBuilder()
		//	.RequireAuthenticatedUser()
		//	.Build();
		//	config.Filters.Add(new AuthorizeFilter(policy));

		//});




		builder.Services.AddSession();
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}


		app.UseSession();
		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();
		

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}