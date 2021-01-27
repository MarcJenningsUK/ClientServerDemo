using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Demo.Api1.Middleware;

namespace DemoApi1
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API1");
					c.RoutePrefix = string.Empty;
				});
			}

			app.UseSwagger(c =>
			{
				c.SerializeAsV2 = true;
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			// Only look for mutual TLS auth in higher level environments?
			if (!env.IsDevelopment())
			{
				app.UseMutualTlsAuthentication(c =>
				{
					c.CertHeader = Configuration.GetValue<string>("CertHeader");
					c.Thumbprint = Configuration.GetValue<string>("Thumbprint");
				});
			}
			
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
