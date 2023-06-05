using Microsoft.Extensions.Hosting;

namespace DataTransfer.Api.Configurations
{
    public class GatewayConfiguration
    {
        private string scheme;
        private string host;

        public GatewayConfiguration()
        {
            scheme = "http";
            host = "localhost";
            Port = 80;
        }

        public string Scheme { get => scheme; set => scheme = value; }

        public string Host { get => host; set => host = value; }

        public int Port { get; set; }

        public string Url
        {
            get
            {
                if (Port == 80)
                {
                    return $"{Scheme}://{Host}";
                }

                return $"{Scheme}://{Host}:{Port}";
            }
        }
    }
}
