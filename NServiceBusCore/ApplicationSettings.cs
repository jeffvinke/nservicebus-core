using System;
namespace NServiceBusCore
{
    public class ApplicationSettings
    {
        public string ServiceVersion { get; set; }
        public string ServiceUrl { get; set; }
        public string Queue { get; set; }
        public string ConnectionString { get; set; }

    }
}
