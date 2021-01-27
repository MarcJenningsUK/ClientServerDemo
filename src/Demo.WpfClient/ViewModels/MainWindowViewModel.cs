using Prism.Commands;
using Prism.Mvvm;
using Demo.WpfClient.Models;
using Demo.WpfClient.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace Demo.WpfClient.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		private readonly IAuthenticationService authenticationService;
		private readonly IApi1Service api1Service;

		private string accessToken;

		private bool isAuthenticated;
		public bool IsAuthenticated
		{
			get => isAuthenticated;
			set => SetProperty(ref isAuthenticated, value);
		}

		private ObservableCollection<ForecastModel> forecastItems;
		public ObservableCollection<ForecastModel> ForecastItems
		{
			get => forecastItems;
			set => SetProperty(ref forecastItems, value);
		}

		public MainWindowViewModel(IAuthenticationService authenticationService, IApi1Service api1Service)
		{
			this.authenticationService = authenticationService;
			this.api1Service = api1Service;

			ForecastItems = new ObservableCollection<ForecastModel>();

			LoginCommand = new DelegateCommand(ExecuteLoginCommand, CanExecuteLoginCommand).ObservesProperty(() => IsAuthenticated);
			LogoutCommand = new DelegateCommand(ExecuteLogoutCommand, CanExecuteLogoutCommand).ObservesProperty(() => IsAuthenticated);
			GetWeatherCommand = new DelegateCommand(ExecuteGetWeatherCommand, CanExecuteGetWeatherCommand).ObservesProperty(() => IsAuthenticated);

			IsAuthenticated = false;
		}

		public ICommand LoginCommand { get; private set; }

		private async void ExecuteLoginCommand()
		{
			accessToken = await authenticationService.LoginAsync();
			Dispatcher.CurrentDispatcher.Invoke(() =>
			{
				IsAuthenticated = !string.IsNullOrEmpty(accessToken);
			});
		}

		private bool CanExecuteLoginCommand()
		{
			return !IsAuthenticated;
		}

		public ICommand LogoutCommand { get; private set; }

		private async void ExecuteLogoutCommand()
		{
			await authenticationService.LogoutAsync();
			accessToken = string.Empty;
			IsAuthenticated = false;
		}

		private bool CanExecuteLogoutCommand()
		{
			return IsAuthenticated;
		}

		public ICommand GetWeatherCommand { get; private set; }

		private async void ExecuteGetWeatherCommand()
		{
			var items = await api1Service.GetWeatherForecastAsync($"Bearer {accessToken}");

			Dispatcher.CurrentDispatcher.BeginInvoke(() =>
			{
				ForecastItems.Clear();
				foreach (var item in items)
				{
					ForecastItems.Add(item);
				}
			});
		}

		private bool CanExecuteGetWeatherCommand()
		{
			return IsAuthenticated;
		}
	}
}
