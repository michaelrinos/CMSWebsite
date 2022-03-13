namespace CMSWebsite.Models
{
    public interface IViewRepository
    {
        IQueryable<RazerView> Views { get; }
        void SaveView(RazerView view);

    }
}
