using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Demo.Api1.Middleware
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseMutualTlsAuthentication(this IApplicationBuilder app, MutualTlsAuthenticationOptions options)
		{
			return app.UseMiddleware<MutualTlsAuthenticationMiddleware>(options);
		}

		public static IApplicationBuilder UseMutualTlsAuthentication(this IApplicationBuilder app, Action<MutualTlsAuthenticationOptions> setupAction)
		{
			var options = app.ApplicationServices.GetRequiredService<IOptions<MutualTlsAuthenticationOptions>>().Value;

			setupAction?.Invoke(options);

			return app.UseMutualTlsAuthentication(options);
		}
	}
}
