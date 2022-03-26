using Microsoft.Extensions.Options;

namespace Reflection {
    public class AppSettings {
        public string ConnectionString { get; 
            internal set; }
        public string ConnectionStringCMS { get; internal set; } = "Data Source=192.168.1.49;Database=CMS;User ID=SA;Password=Orian1Mitchell2Police";
    }
}