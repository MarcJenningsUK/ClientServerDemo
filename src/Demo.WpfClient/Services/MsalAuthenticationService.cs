using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WpfClient.Services
{
	public class MsalAuthenticationService : IAuthenticationService
	{
		private const string Instance = "https://login.microsoftonline.com/";
		private readonly string[] scopes;

		private IPublicClientApplication publicClientApp;

		public MsalAuthenticationService(string tenantId, string clientId, string[] scopes)
		{
			this.scopes = scopes;

			publicClientApp = PublicClientApplicationBuilder.Create(clientId)
				.WithAuthority($"{Instance}{tenantId}")
				.WithRedirectUri("http://localhost")
				.Build();
		}

		public async Task<string> LoginAsync()
		{
			AuthenticationResult result = null;
			var accounts = await publicClientApp.GetAccountsAsync();

			try
			{
				result = await publicClientApp
					.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
					.ExecuteAsync();
			}
			catch (MsalUiRequiredException ex)
			{
				// A MsalUiRequiredException happened on AcquireTokenSilent.
				// This indicates you need to call AcquireTokenInteractive to acquire a token
				System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

				try
				{
					result = await publicClientApp
						.AcquireTokenInteractive(scopes)
						.ExecuteAsync();
				}
				catch (MsalException msalex)
				{
					System.Diagnostics.Debug.WriteLine($"Error Acquiring Token: {msalex.Message}");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error Acquiring Token: {ex.Message}");
			}

			if (result != null)
			{
				return result.AccessToken;
				// Use the token
			}

			return null;
		}

		public async Task LogoutAsync()
		{
			var accounts = await publicClientApp.GetAccountsAsync();

			foreach (var account in accounts)
			{
				await publicClientApp.RemoveAsync(account);
			}
		}
	}
}
