using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Demo.Api1.Middleware
{
	public class MutualTlsAuthenticationMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<MutualTlsAuthenticationMiddleware> logger;
		private readonly MutualTlsAuthenticationOptions options;

		public MutualTlsAuthenticationMiddleware(RequestDelegate next, MutualTlsAuthenticationOptions options, ILoggerFactory loggerFactory)
		{
			_ = options ?? throw new ArgumentNullException(nameof(options));
			this.options = options;
			ValidateOptions();

			this.next = next ?? throw new ArgumentNullException(nameof(next));
			logger = loggerFactory?.CreateLogger<MutualTlsAuthenticationMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (ValidateMutualTlsAuthentication(context))
			{
				await next(context);
			}
			else
			{
				context.Response.StatusCode = 401;
			}
		}

		private void ValidateOptions()
		{
			if (string.IsNullOrWhiteSpace(options.CertHeader))
			{
				throw new InvalidOperationException($"{nameof(MutualTlsAuthenticationOptions.CertHeader)} is required.");
			}

			if (string.IsNullOrWhiteSpace(options.Thumbprint))
			{
				throw new InvalidOperationException($"{nameof(MutualTlsAuthenticationOptions.Thumbprint)} is required.");
			}
		}

		private bool ValidateMutualTlsAuthentication(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue(options.CertHeader, out var value))
			{
				byte[] clientCertBytes = Convert.FromBase64String(value[0]);
				var certificate = new X509Certificate2(clientCertBytes);

				if (string.Compare(options.Thumbprint, certificate.Thumbprint, true) != 0)
				{
					return false;
				}

				if (!string.IsNullOrWhiteSpace(options.Subject) && string.Compare(options.Subject, certificate.Subject, true) != 0)
				{
					return false;
				}

			
				if (!string.IsNullOrWhiteSpace(options.Issuer) && string.Compare(options.Issuer, certificate.Issuer, true) != 0)
				{
					return false;
				}

				if (options.ValidateIssuanceDates && (DateTime.UtcNow > certificate.NotAfter || DateTime.UtcNow < certificate.NotBefore))
				{
					return false;
				}

				return true;
			}

			return false;
		}
	}
}
