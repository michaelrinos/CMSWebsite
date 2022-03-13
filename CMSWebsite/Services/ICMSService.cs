using CMSWebsite.Models;
using Microsoft.AspNetCore.Mvc.Razor;

namespace CMSWebsite.Services
{
    public interface ICMSService
    {
        Task<RazerView> GetRazerView(string viewPath);
        Task<RazerView> GetRazerViewLikeLocation(string viewPath);
    }
}
