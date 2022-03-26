namespace CMSWebsite.Models
{
    public class EFViewsRepository : IViewRepository
    {
        private ApplicationDbContext context;

        public EFViewsRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<RazerView> Views => context.Views;

        public void SaveView(RazerView view)
        {
            context.Views.Add(view);
            context.SaveChanges();
        }
    }
}
