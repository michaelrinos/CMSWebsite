using CMSWebsite.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace CMSWebsite
{
    public class DatabaseFileProvider : IFileProvider
    {
        #region Fields

        private string _connection;

        #endregion // Fields

        #region Methods

        #region Public
        public DatabaseFileProvider(string connection)
        {
            this._connection = connection;
        }
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            Console.WriteLine();
            return new NotFoundDirectoryContents();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (IsVirtualPath(subpath))
            {
                var li = subpath.LastIndexOf('/');
                var ns = subpath.Substring(li == -1 ? 0 : li + 1);

                var result = new DatabaseFileInfo(_connection, ns);
                return result.Exists ? result as IFileInfo : new NotFoundFileInfo(subpath);
            }
            else
            {
                return new NotFoundFileInfo(subpath);
            }
        }


        public IChangeToken Watch(string filter)
        {
            return new DatabaseChangeToken(_connection, filter);
        }

        #endregion // Public 

        #region Private

        /// <summary>
        /// Determines if a file is a virtual (db) file or a real file      
        /// by looking at the path
        /// </summary>
        /// <param name="v"></param>
        /// <param name="subpath"></param>
        /// <returns></returns>
        private bool IsVirtualPath(string subpath)
        {
            return subpath.Contains("/Views/Dynamic") || subpath.Contains("/Views/Shared/");
        }

        #endregion // Private 

        #endregion //Methods

    }

}
