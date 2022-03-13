﻿using Microsoft.Extensions.Primitives;
using System.Data.SqlClient;

namespace CMSWebsite
{
    public class DatabaseChangeToken : IChangeToken
    {
        private string _connection;
        private string _viewPath;

        public DatabaseChangeToken(string connection, string viewPath)
        {
            _connection = connection;
            _viewPath = viewPath;
        }

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged
        {
            get
            {

                var query = "SELECT LastRequested, LastModified FROM RazerViews WHERE Location = @Path;";
                try
                {
                    using (var conn = new SqlConnection(_connection))
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Path", _viewPath);
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                if (reader["LastRequested"] == DBNull.Value)
                                {
                                    return false;
                                }
                                else
                                {
                                    var lastModified = ((DateTimeOffset)reader["lastModified"]).DateTime;
                                    var lastRequested = Convert.ToDateTime(reader["LastRequested"]);
                                    return lastModified > lastRequested;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => EmptyDisposable.Instance;
    }

    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();
        private EmptyDisposable() { }
        public void Dispose() { }
    }

}
