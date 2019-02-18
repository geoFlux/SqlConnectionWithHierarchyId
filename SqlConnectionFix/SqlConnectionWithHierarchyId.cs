using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Types;

namespace GeoFlux.SqlConnectionFix
{
    public class SqlConnectionWithHierarchyId : IDbConnection
    {
        readonly SqlConnection conn;
        public SqlConnectionWithHierarchyId(SqlConnection conn)
        {
            this.conn = conn;
        }

        public static void AddAddDapperSupport(){
            Dapper.SqlMapper.AddTypeHandler<SqlHierarchyId>(new SqlHierarchyIdTypeHandler());
        }
        public IDbTransaction BeginTransaction()
        {
            return conn.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return conn.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            conn.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            conn.Close();
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommandWithHierarchyId(conn.CreateCommand());
        }

        public void Open()
        {
            conn.Open();
        }
        public string ConnectionString { get => conn.ConnectionString; set => conn.ConnectionString = value; }

        public int ConnectionTimeout => conn.ConnectionTimeout;

        public string Database => conn.Database;

        public ConnectionState State => conn.State;
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.conn.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }
}