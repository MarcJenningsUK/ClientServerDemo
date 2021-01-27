using Prism.DryIoc;
using Prism.Ioc;
using Refit;
using Demo.WpfClient.Services;
using Demo.WpfClient.Views;
using System;
using System.Windows;

namespace Demo.WpfClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : PrismApplication
    {
   
        static App()
        {
        }

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			throw new InvalidProgramException("Update the tenantId, clientId, scopes, and hostUrl values below and then remove this exception.");

			// Lookup tenant, client, scopes from environment.
			var tenantId = "";
			var clientId = "";
			var scopes = new string[] { "api://api1/somescope" };
			var hostUrl = "";

			containerRegistry.RegisterInstance<IAuthenticationService>(new MsalAuthenticationService(tenantId, clientId, scopes));

			// Should actually use a delegating handler and httpclient to handle auth.
			containerRegistry.RegisterInstance(RestService.For<IApi1Service>(hostUrl, new RefitSettings(new NewtonsoftJsonContentSerializer())));
		}

		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}
	}
}
