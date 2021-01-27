using System.Threading.Tasks;

namespace Demo.WpfClient.Services
{
	public interface IAuthenticationService
	{
		Task<string> LoginAsync();
		Task LogoutAsync();
	}
}
