using Microsoft.Extensions.Options;

namespace Reflection {
    public class AppSettings {
        public string ConnectionString { get; internal set; }
        public IOptions<AppSettings> ConnectionStringCMS { get; internal set; }
    }
}