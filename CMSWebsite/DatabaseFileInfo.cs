using CMSWebsite.Models;
using CMSWebsite.Services;
using Microsoft.Extensions.FileProviders;

namespace CMSWebsite
{
    public class DatabaseFileInfo : IFileInfo
    {
        private string _viewPath;
        private string _contents;
        private byte[] _viewContent;
        private DateTimeOffset _lastModified;
        private bool _exists;

        private readonly ICMSService cMSService;

        public DatabaseFileInfo(string viewPath, ICMSService cMSService)
        {
            _viewPath = viewPath;
            this.cMSService = cMSService;
            GetView(viewPath);
            
        }
        public bool Exists => _exists;

        public bool IsDirectory => false;

        public DateTimeOffset LastModified => _lastModified;

        public long Length
        {
            get
            {
                using (var stream = new MemoryStream(_viewContent))
                {
                    return stream.Length;
                }
            }
        }

        public string Name => Path.GetFileName(_viewPath);

        public string PhysicalPath => null;

        public Stream CreateReadStream()
        {
            return new MemoryStream(_viewContent);
        }

        private async void GetView(string viewPath)
        {
            
            
            var view = await (this.cMSService.GetRazerView(viewPath) ?? cMSService.GetRazerViewLikeLocation(viewPath));
            if (view == default(RazerView))
                return;

            _exists = true;
            _contents = view.Content;
            _viewContent = view.ContentBytes;
            _lastModified = view.LastModified;

            // */
            /*
            if (viewPath.Contains("Views/Dynamic")) {
                var li = viewPath.LastIndexOf('/') + 1;
                viewPath = viewPath.Substring(li, viewPath.Length - li);
            }
            var query = @"SELECT rv.*, js.JSContent, css.CSSContent, html.HTMLContent from [dbo].[RazerViews] rv
                inner join [dbo].[JSContent] js on js.JSContentId = rv.JSContentId
                inner join [dbo].[CSSContent] css on css.CSSContentId = rv.CSSContentId
                inner join [dbo].[HTMLContent] html on html.HTMLContentId = rv.HTMLContentId
            where Location = @Location";
            //var query = @"SELECT Content, LastModified FROM Views WHERE Location = @Path;
             //       UPDATE Views SET LastRequested = GetUtcDate() WHERE Location = @Path";
            try {
                using (var conn = new SqlConnection(connection))
                using (var cmd = new SqlCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@Location", viewPath);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader()) {
                        _exists = reader.HasRows;
                        if (_exists) {
                            reader.Read();
                            _contents = reader["HTMLContent"].ToString();
                            _viewContent = Encoding.UTF8.GetBytes(reader["HTMLContent"].ToString());
                            _lastModified = Convert.ToDateTime(reader["LastModified"]);
                        }
                    }
                }
            } catch (Exception ) {

                // if something went wrong, Exists will be false
            }
            // */
        }
    }
}
