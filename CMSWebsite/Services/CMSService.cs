using CMSWebsite.Models;
using Microsoft.Extensions.Options;
using Reflection;
using Reflection.Services;

namespace CMSWebsite.Services
{
    public class CMSService : TransactionalService,  ICMSService
    {
        private readonly IOptions<AppSettings> appSettings;
		public CMSService() : base(null) { }

		public CMSService(
			ISqlDataProviderCMS dataProvider,
			IOptions<AppSettings> appSettings

		) : base(dataProvider)
		{
			this.appSettings = appSettings;
		}

        public Task CreateRazerView(RazerView view)
        {
            throw new NotImplementedException();
        }

        public async Task<RazerView> GetRazerView(string viewPath)
        {
            throw new NotImplementedException();
        }

        public int GetRazerViewCount()
        {
            throw new NotImplementedException();
        }

        public async Task<RazerView> GetRazerViewLikeLocation(string viewPath)
        {
            throw new NotImplementedException();
        }

        public void UpdateRazerView(object fromDb)
        {
            throw new NotImplementedException();
        }
    }
}
