using System.Data;
using System.Data.SqlClient;
namespace GeoFlux.SqlConnectionFix
{
    public class SqlCommandWithHierarchyId : IDbCommand
    {
        readonly SqlCommand c;
        private IDbCommand i => c;
        public SqlCommandWithHierarchyId(SqlCommand inner)
        {
            this.c = inner;
        }
        public IDbConnection Connection { get => i.Connection; set => i.Connection = value; }
        public IDbTransaction Transaction { get => i.Transaction; set => i.Transaction = value; }
        public string CommandText { get => i.CommandText; set => i.CommandText = value; }
        public int CommandTimeout { get => i.CommandTimeout; set => i.CommandTimeout = value; }
        public CommandType CommandType { get => i.CommandType; set => i.CommandType = value; }

        public IDataParameterCollection Parameters => i.Parameters;

        public UpdateRowSource UpdatedRowSource { get => i.UpdatedRowSource; set => i.UpdatedRowSource = value; }

        public void Cancel()
        {
            i.Cancel();
        }

        public IDbDataParameter CreateParameter()
        {
            return i.CreateParameter();
        }

        public int ExecuteNonQuery()
        {
            return i.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader()
        {
            return new SqlDataReaderWithHierarchyId(c.ExecuteReader());
            //return new ErrorDataReader(new SqlDataReaderWithHierarchyId(c.ExecuteReader()));
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return new SqlDataReaderWithHierarchyId(c.ExecuteReader(behavior));
            //return new ErrorDataReader(new SqlDataReaderWithHierarchyId(c.ExecuteReader(behavior)));
        }

        public object ExecuteScalar()
        {
            return i.ExecuteScalar();
        }

        public void Prepare()
        {
            i.Prepare();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    c.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}