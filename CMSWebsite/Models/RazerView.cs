using Microsoft.Extensions.FileProviders;
using Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSWebsite.Models
{
    public class RazerView : IFileInfo
    {

        #region Fields 

        private byte[] _ContentBytes;
        private string _Content;

        private string _Html = "";
        private string _Css = "";
        private string _Js = "";


        #endregion // Fields

        public int RazerViewId { get; set; }
        public string Location { get; set; } = "";

        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public byte[] ContentBytes { get => Encoding.UTF8.GetBytes(Content); private set => _ContentBytes = value; }

        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public string Content { get => ((Model ?? "") + "<style>" + (_Css ?? "") + "</style>") + (_Html ?? "") + (_Js ?? ""); }

        public string Model { get; set; }
        public int HTMLContentId { get; set; }
        public string HTMLContent { get => _Html ?? ""; set => _Html = value; }
        public int CSSContentId { get; set; }
        public string CSSContent { get => _Css == null ? null : _Css.Contains("<style>") ? "<style>" + _Css + "</style>" : _Css; set => _Css = value; }
        public int JSContentId { get; set; }
        public string JSContent { get => _Js ?? ""; set => _Js = value; }
        [NotMapped]
        public string InsertBy { get; set; } = "NotSet";

        public DateTimeOffset LastModified { get; set; }

        [SqlQueryParameter(Ignore = true)]
        public DateTime? LastRequested { get; set; }

        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public bool Exists => Content == null ? false : true;
        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public bool IsDirectory => false;

        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public long Length
        {
            get
            {
                using (var stream = new MemoryStream(this.ContentBytes))
                {
                    return stream.Length;
                }
            }
        }
        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public string PhysicalPath => null;

        [NotMapped]
        [SqlQueryParameter(Ignore = true)]
        public string Name => Path.GetFileName(Location);

        public Stream CreateReadStream()
        {
            return new MemoryStream(ContentBytes);
        }
    }
}
