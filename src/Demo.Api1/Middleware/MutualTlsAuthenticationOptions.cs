namespace Demo.Api1.Middleware
{
	public class MutualTlsAuthenticationOptions
	{
		public string CertHeader { get; set; }
		public string Thumbprint { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public bool ValidateIssuanceDates { get; set; }
    }
}
