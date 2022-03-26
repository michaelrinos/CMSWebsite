using CMSWebsite.Models;
using Microsoft.Extensions.Options;
using Reflection;
using Reflection.Services;

namespace CMSWebsite.Services
{
    public class CMSService : TransactionalService, ICMSService
    {
        private readonly IOptions<AppSettings> appSettings;

		public CMSService(
			ISqlDataProviderCMS dataProvider,
			IOptions<AppSettings> appSettings

		) : base(dataProvider)
		{
			this.appSettings = appSettings;
		}

        public void CreateRazerView(RazerView view)
        {
            this.DataProvider.ExecuteProcAsync<RazerView>("[dbo].[CreateRazerView]", view).ConfigureAwait(false);

        }

        public async Task<RazerView> GetRazerView(int razerViewId)
        {
            var result = await this.DataProvider.ExecuteProcAsync<RazerView>("[dbo].[GetRazerView]", 
                new { RazerViewId = razerViewId }).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public Task<RazerView> GetRazerView(string viewPath)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetRazerViewCount()
        {
            var result = await this.DataProvider.ExecuteProcAsync<int>("[dbo].[GetRazerViewCount]");
            return result.FirstOrDefault();
        }

        public async Task<RazerView> GetRazerViewLikeLocation(string viewPath)
        {
            var result = await this.DataProvider.ExecuteProcAsync<RazerView>("[dbo].[GetRazerViewLikeLocation]", new { Location = viewPath })
                .ConfigureAwait(false);
            return result.FirstOrDefault();

        }

        public void UpdateRazerView(RazerView view)
        {
            this.DataProvider.ExecuteProcAsync<RazerView>("[dbo].[UpdateRazerView]", view);
        }
    }
}
