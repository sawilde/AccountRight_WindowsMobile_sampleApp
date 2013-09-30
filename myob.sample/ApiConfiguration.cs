using MYOB.AccountRight.SDK;

namespace MYOB.Sample
{
    public class ApiConfiguration : IApiConfiguration
    {
        public string ClientId { get { return Configuration.ClientId; } }
        public string ClientSecret { get { return Configuration.ClientSecret; } }
        public string RedirectUrl { get { return Configuration.RedirectUrl; } }
        public string ApiBaseUrl { get { return "https://api.myob.com/accountright"; } }
        //public string ApiBaseUrl { get { return "http://10.0.0.8:8080/accountright"; } }
    }
}